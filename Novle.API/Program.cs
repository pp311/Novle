using System.Text.Json.Serialization;
using Carter;
using FluentValidation;
using Novel.API.Extensions;
using Novel.API.Middlewares;
using Novle.Application.Services;
using Novle.Application.ViewModels;
using Novle.Infrastructure.Repositories.Base;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging();
builder.AddSettings();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddCarter()
    .ConfigureCors()
    .ConfigureDbContext(builder.Configuration)
    .ConfigureIdentity()
    .AddRepositories()
    .AddServices()
    .AddCurrentUser()
    .AddAutoMapper(typeof(BaseService).Assembly, typeof(NovleDbContext).Assembly)
    .AddProblemDetails()
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddAuthentication(builder.Configuration)
    .AddValidatorsFromAssemblyContaining<IRequest>(ServiceLifetime.Singleton);

// builder.Services.ConfigureHttpJsonOptions(options =>
// {
//     options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
// });
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

await app.MigrateDatabaseAsync();

app.UseSwagger();
app.UseSwaggerUI();

app.UseSerilogRequestLogging();
app.UseExceptionHandler();
app.UseCors("CorsPolicy");
// app.UseAuthentication();
// app.UseAuthorization();
app.UseHttpsRedirection();
app.MapCarter();
app.Run();
