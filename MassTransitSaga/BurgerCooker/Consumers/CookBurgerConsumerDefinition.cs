namespace MassTransitSaga.BurgerCooker.Consumers
{
  using MassTransit;

  public class CookBurgerConsumerDefinition :
        ConsumerDefinition<CookBurgerConsumer>
  {
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CookBurgerConsumer> consumerConfigurator, IRegistrationContext registrationContext)
    {
      endpointConfigurator.UseMessageRetry(r =>
      {
        //r.Exponential(retryLimit: 5,                          // Number of retry attempts
        //              minInterval: TimeSpan.FromSeconds(1),   // Minimum time to wait between retries
        //              maxInterval: TimeSpan.FromMinutes(1),   // Maximum time to wait between retries
        //              intervalDelta: TimeSpan.FromSeconds(2)  // Time added to the wait each retry
        //);
        r.Intervals(5000);

      });

      // use the outbox to prevent duplicate events from being published
      //endpointConfigurator.UseInMemoryOutbox(registrationContext);
    }
  }
}