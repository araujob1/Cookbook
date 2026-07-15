using Cookbook.Api.Extensions;
using Cookbook.Api.Filters;
using Cookbook.Infrastructure.Extensions;
using Cookbook.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddApplicationLocalization();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseApplicationLocalization();

builder.Services.AddMvc(options => options.Filters.Add<ExceptionFilter>());

app.Services.MigrateDatabase();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
