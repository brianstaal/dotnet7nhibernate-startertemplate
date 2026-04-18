using Domain.Persistence.NhConcrete;
using Moq;
using NHibernate;
using Xunit;

namespace Domain.Tests;

public class NhRepositoryTests
{
    [Fact]
    public async Task ReadAsync_UsesTemporarySession_WhenNoTransactionIsOpen()
    {
        var sessionFactory = new Mock<ISessionFactory>(MockBehavior.Strict);
        var session = new Mock<ISession>(MockBehavior.Strict);

        session.Setup(x => x.Dispose());
        sessionFactory.Setup(x => x.OpenSession()).Returns(session.Object);

        using var repository = new TestRepository(sessionFactory.Object);

        var usedExpectedSession = await repository.ReadWithCurrentSessionAsync(currentSession =>
            Task.FromResult(ReferenceEquals(currentSession, session.Object)));

        Assert.True(usedExpectedSession);
        sessionFactory.Verify(x => x.OpenSession(), Times.Once);
        session.Verify(x => x.Dispose(), Times.Once);
    }

    [Fact]
    public async Task CommitAsync_DisposesTransactionalSession()
    {
        var sessionFactory = new Mock<ISessionFactory>(MockBehavior.Strict);
        var session = new Mock<ISession>(MockBehavior.Strict);
        var transaction = new Mock<ITransaction>(MockBehavior.Strict);

        sessionFactory.Setup(x => x.OpenSession()).Returns(session.Object);
        session.Setup(x => x.BeginTransaction()).Returns(transaction.Object);
        session.Setup(x => x.Dispose());

        transaction.SetupGet(x => x.IsActive).Returns(true);
        transaction.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        transaction.Setup(x => x.Dispose());

        using var repository = new TestRepository(sessionFactory.Object);

        await repository.BeginTransactionForTestAsync();
        var usedExpectedSession = await repository.ReadWithCurrentSessionAsync(currentSession =>
            Task.FromResult(ReferenceEquals(currentSession, session.Object)));
        await repository.CommitTransactionForTestAsync();

        Assert.True(usedExpectedSession);
        sessionFactory.Verify(x => x.OpenSession(), Times.Once);
        session.Verify(x => x.BeginTransaction(), Times.Once);
        transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        session.Verify(x => x.Dispose(), Times.Once);
    }

    private sealed class TestRepository : NhRepository
    {
        public TestRepository(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        public Task<T> ReadWithCurrentSessionAsync<T>(Func<ISession, Task<T>> action)
        {
            return ReadAsync(action);
        }

        public Task BeginTransactionForTestAsync()
        {
            return BeginTransactionAsync();
        }

        public Task CommitTransactionForTestAsync()
        {
            return CommitAsync();
        }
    }
}
