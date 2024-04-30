namespace MassTransitSaga.BurgerCooker.Consumers
{
  using MassTransit;

  public class BurgerCookerFallbackConsumerDefinition :
        ConsumerDefinition<BurgerCookerFallbackConsumer>
  {
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<BurgerCookerFallbackConsumer> consumerConfigurator, IRegistrationContext registrationContext)
    {
      //endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
  }
}