using Domain.Persistence;
using Domain.Persistence.Abstract;
using Domain.Persistence.NhConcrete;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using Xunit;

namespace Domain.Tests;

public class NhServiceFactoryTests
{
    [Fact]
    public void AddNHibernate_RegistersExpectedServices()
    {
        var services = new ServiceCollection();

        services.AddNHibernate("Server=localhost,1433;Database=RecipeDb;User ID=intern;Password=secret;TrustServerCertificate=True;");

        Assert.Contains(services, descriptor =>
            descriptor.ServiceType == typeof(ISessionFactory) &&
            descriptor.Lifetime == ServiceLifetime.Singleton);
        Assert.Contains(services, descriptor =>
            descriptor.ServiceType == typeof(IRecipeRepository) &&
            descriptor.ImplementationType == typeof(NhRecipeRepository) &&
            descriptor.Lifetime == ServiceLifetime.Scoped);
        Assert.DoesNotContain(services, descriptor => descriptor.ServiceType == typeof(ISession));
    }
}
