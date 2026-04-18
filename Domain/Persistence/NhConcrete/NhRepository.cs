using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate;

namespace Domain.Persistence.NhConcrete
{
    public abstract class NhRepository : IDisposable
    {
        private readonly ISessionFactory _sessionFactory;
        private ISession _session;
        private ITransaction _transaction;
        private bool _disposed;

        protected NhRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
        }

        protected ISession Session => _session ??= _sessionFactory.OpenSession();

        protected async Task<T> ReadAsync<T>(Func<ISession, Task<T>> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (_session != null)
                return await action(_session);

            using var session = _sessionFactory.OpenSession();
            return await action(session);
        }

        protected Task BeginTransactionAsync()
        {
            if (_transaction?.IsActive == true)
                return Task.CompletedTask;

            _transaction = Session.BeginTransaction();
            return Task.CompletedTask;
        }

        protected async Task CommitAsync()
        {
            if (_transaction is null)
                throw new InvalidOperationException("No active transaction.");

            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                if (_transaction.IsActive)
                    await _transaction.RollbackAsync();

                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
                _session?.Dispose();
                _session = null;
            }
        }

        protected async Task RollbackAsync()
        {
            if (_transaction is null)
                return;

            try
            {
                if (_transaction.IsActive)
                    await _transaction.RollbackAsync();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
                _session?.Dispose();
                _session = null;
            }
        }

        protected Task ClearSessionAsync()
        {
            _session?.Clear();
            return Task.CompletedTask;
        }

        protected async Task<T> SaveOrUpdateAsync<T>(T entity) where T : class
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (_transaction?.IsActive != true)
                await BeginTransactionAsync();

            try
            {
                await Session.SaveOrUpdateAsync(entity);
                return entity;
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        protected async Task BulkInsertAsync<T>(IEnumerable<T> entities) where T : class
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            using var statelessSession = _sessionFactory.OpenStatelessSession();
            using var transaction = statelessSession.BeginTransaction();

            foreach (var entity in entities)
                await statelessSession.InsertAsync(entity);

            await transaction.CommitAsync();
        }

        protected async Task DeleteAsync<T>(T entity) where T : class
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (_transaction?.IsActive != true)
                await BeginTransactionAsync();

            try
            {
                await Session.DeleteAsync(entity);
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _transaction?.Dispose();
            _session?.Dispose();
            _transaction = null;
            _session = null;
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
