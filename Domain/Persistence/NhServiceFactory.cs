using System;
using System.Reflection;
using Domain.Persistence.Abstract;
using Domain.Persistence.NhConcrete;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;

namespace Domain.Persistence
{
    public static class NhServiceFactory
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string connectionString, bool logSql = false)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("A SQL connection string is required.", nameof(connectionString));

            services.AddSingleton<ISessionFactory>(_ => BuildSessionFactory(connectionString, logSql));
            services.AddScoped<IRecipeRepository, NhRecipeRepository>();

            return services;
        }

        private static ISessionFactory BuildSessionFactory(string connectionString, bool logSql)
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(Assembly.GetAssembly(typeof(NhMapping))?.GetExportedTypes()
                ?? throw new InvalidOperationException("NHibernate mappings could not be loaded."));

            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            var configuration = new Configuration();
            configuration.DataBaseIntegration(dbi =>
            {
                dbi.Dialect<MsSql2012Dialect>();
                dbi.Driver<MicrosoftDataSqlClientDriver>();
                dbi.ConnectionString = connectionString;
                dbi.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                dbi.SchemaAction = SchemaAutoAction.Validate;
                dbi.LogFormattedSql = logSql;
                dbi.LogSqlInConsole = logSql;
            });

            configuration.AddMapping(mapping);

            return configuration.BuildSessionFactory();
        }
    }
}
