using System.Data.SqlClient;
using System.Reflection;
using System.Text.Json.Serialization;
using Domain.Persistence;
using Domain.Persistence.Abstract;
using Domain.Persistence.NhConcrete;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;

var builder = WebApplication.CreateBuilder(args);

// Secure the MsSql Username & Password from git (dotnet user-secrets init)
var connStrBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("MsSqlConnection"))
{
    UserID = builder.Configuration["SQLUSERNAME"],
    Password = builder.Configuration["SQLUSERPASSWORD"]
};
var connectionString = connStrBuilder.ConnectionString;

// Setup Nhibernate
var nhMapper = new ModelMapper();
nhMapper.AddMappings(Assembly.GetAssembly(typeof(NhMapping))?.GetExportedTypes());
var nhMapping = nhMapper.CompileMappingForAllExplicitlyAddedEntities();

var nhConfig = new Configuration();
nhConfig.DataBaseIntegration(dbi =>
{
    dbi.Dialect<MsSql2012Dialect>();
    dbi.ConnectionString = connectionString;
    dbi.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
    dbi.SchemaAction = SchemaAutoAction.Validate;
    dbi.LogFormattedSql = true;
    dbi.LogSqlInConsole = true;
});
nhConfig.AddMapping(nhMapping);

var sessionFactory = nhConfig.BuildSessionFactory();
builder.Services.AddSingleton(sessionFactory);
builder.Services.AddScoped(factory => sessionFactory.OpenSession());

// Repository Mappings
builder.Services.AddScoped<IRecipeRepository, NhRecipeRepository>();

// General Webapp setup
builder.Services.AddControllers();
builder.Services.AddMvc().AddJsonOptions(jo => jo.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Not Yet Implemented
//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();