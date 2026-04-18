using System.Text.Json.Serialization;
using Domain.Persistence;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>(optional: true);

var sqlConnectionStringTemplate = builder.Configuration.GetConnectionString("MsSqlConnection")
    ?? throw new InvalidOperationException("Connection string 'MsSqlConnection' is missing.");
var sqlUsername = builder.Configuration["SQLUSERNAME"]
    ?? throw new InvalidOperationException("User secret 'SQLUSERNAME' is missing.");
var sqlUserPassword = builder.Configuration["SQLUSERPASSWORD"]
    ?? throw new InvalidOperationException("User secret 'SQLUSERPASSWORD' is missing.");
var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(sqlConnectionStringTemplate)
{
    UserID = sqlUsername,
    Password = sqlUserPassword
};

builder.Services.AddNHibernate(sqlConnectionStringBuilder.ConnectionString, builder.Environment.IsDevelopment());

builder.Services.AddControllers();
builder.Services.AddMvc().AddJsonOptions(jo => jo.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
