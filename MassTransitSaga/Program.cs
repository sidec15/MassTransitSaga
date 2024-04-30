using BurgerCooker.Contracts;
using BurgerCooker.StateMachines;
using MassTransit;
using MassTransitSaga.Models;
using MassTransitSaga.Settings;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var isTestEnvironment = builder.Environment.IsEnvironment("Test");
var services = builder.Services;

var databaseConfiguration = builder.Configuration.GetSection(nameof(DatabaseSettings));
services.Configure<DatabaseSettings>(databaseConfiguration);
IDatabaseSettings databaseSettings = databaseConfiguration.Get<DatabaseSettings>();
//services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

services.AddOptions<SqlTransportOptions>().Configure(options =>
{
  //var connStr = builder.Configuration.GetConnectionString("Local");
  //var builderConnStr = new NpgsqlConnectionStringBuilder(connStr);
  options.Host = databaseSettings.Host;
  options.Port = databaseSettings.Port;
  options.Database = databaseSettings.Database;
  options.Schema = "transport"; // the schema for the transport-related tables, etc. 
  options.Role = "transport";   // the role to assign for all created tables, functions, etc.
  options.Username = databaseSettings.Username;  // the application-level credentials to use
  options.Password = databaseSettings.Password;
  //options.AdminUsername = "postgres"; // the admin credentials to create the tables, etc.
  //options.AdminPassword = "postgres";
});

services.AddMassTransit(configurator =>
{
  configurator.SetKebabCaseEndpointNameFormatter();
  configurator.AddConsumers(typeof(Program).Assembly);
  configurator.AddSagaStateMachine<BurgerCookerStateMachine, BurgerCookerState>(cfg =>
  {

  })
  .InMemoryRepository();

  //configurator.UsingInMemory((context, config) =>
  //{
  //  config.ConfigureEndpoints(context);
  //});
  configurator.UsingPostgres((context, cfg) =>
  {
    cfg.UseDbMessageScheduler();

    cfg.ConfigureEndpoints(context);
    //cfg.UseMessageRetry(r =>
    //{
    //  r.Exponential(retryLimit: 5,                          // Number of retry attempts
    //                minInterval: TimeSpan.FromSeconds(1),   // Minimum time to wait between retries
    //                maxInterval: TimeSpan.FromMinutes(2),   // Maximum time to wait between retries
    //                intervalDelta: TimeSpan.FromSeconds(0)  // Time added to the wait each retry
    //  );
    //  //r.Immediate(2);
    //});
    // Fallback configuration

  });
});
// To automatically create the tables, roles, functions, and other related database elements, a hosted service is available.
// Specifying delete: true is only recommended for unit tests!
services.AddPostgresMigrationHostedService(create: true, delete: isTestEnvironment);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/burger-cooker/order", async (
  [FromBody] BurgerOrder burgerOrder,
  [FromServices] ILogger<Program> logger,
  [FromServices] IBus bus
  ) =>
{
  logger.LogInformation("Hey request arrived! Wow!");

  BurgerCookerOrderedEvent orderedEvent = new()
  {
    CookTemp = burgerOrder.CookTemp,
    CustomerName = burgerOrder.CustomerName,
    CorrelationId = Guid.NewGuid(),
  };

  await bus.Publish(orderedEvent);

})
//.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

