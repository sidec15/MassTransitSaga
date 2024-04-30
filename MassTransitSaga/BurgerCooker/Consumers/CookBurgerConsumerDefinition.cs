namespace MassTransitSaga.BurgerCooker.Consumers
{
  using MassTransit;

  public class CookBurgerConsumerDefinition :
        ConsumerDefinition<CookBurgerConsumer>
  {
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CookBurgerConsumer> consumerConfigurator, IRegistrationContext registrationContext)
    {
      endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
  }
}