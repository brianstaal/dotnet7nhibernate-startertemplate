using System.Reflection;
using Domain.Persistence.Abstract;
using Domain.Persistence.NhConcrete;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;

namespace Domain.Persistence
{
    public static class NhServiceFactory
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string connectionString)
        {
            // Setup Nhibernate
            var nhMapper = new ModelMapper();
            nhMapper.AddMappings(Assembly.GetAssembly(typeof(NhMapping))?.GetExportedTypes());
            var nhMapping = nhMapper.CompileMappingForAllExplicitlyAddedEntities();

            var nhConfig = new NHibernate.Cfg.Configuration();
            nhConfig.DataBaseIntegration(dbi =>
            {
                dbi.Dialect<MsSql2012Dialect>();
                dbi.Driver<MicrosoftDataSqlClientDriver>();
                dbi.ConnectionString = connectionString;
                dbi.KeywordsAutoImport = NHibernate.Cfg.Hbm2DDLKeyWords.AutoQuote;
                dbi.SchemaAction = NHibernate.Cfg.SchemaAutoAction.Validate;
                dbi.LogFormattedSql = true;
                dbi.LogSqlInConsole = true;
            });
            nhConfig.AddMapping(nhMapping);

            var sessionFactory = nhConfig.BuildSessionFactory();
            services.AddSingleton(sessionFactory);
            services.AddScoped(_ => sessionFactory.OpenSession());

            // Repository Mappings
            services.AddScoped<IRecipeRepository, NhRecipeRepository>();

            return services;
        }
    }
}
