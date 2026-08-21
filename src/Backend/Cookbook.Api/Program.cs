using Cookbook.Api.Extensions;
using Cookbook.Api.Filters;
using Cookbook.Application.Extensions;
using Cookbook.Infrastructure.Extensions;
using Cookbook.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>());
builder.Services.AddOpenApi();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services
    .AddApplicationLocalization()
    .AddApplicationAuthentication(builder.Configuration);

builder.Services
    .AddInfrastructure()
    .AddApplication();

var app = builder.Build();

app.UseApplicationLocalization();

app.Services.MigrateDatabase();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

public sealed partial class Program
{
    private Program() { }
}
