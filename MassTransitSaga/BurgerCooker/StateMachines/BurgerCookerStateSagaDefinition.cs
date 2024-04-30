namespace BurgerCooker.StateMachines
{
  using MassTransit;
  using BurgerCooker.Contracts;
  using System;
  using System.Threading.Tasks;

  public class BurgerCookerStateSagaDefinition :
        SagaDefinition<BurgerCookerState>
  {
    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<BurgerCookerState> sagaConfigurator, IRegistrationContext context)
    {
      endpointConfigurator.UseMessageRetry(r => r.Intervals(2000, 4000, 8000));
      endpointConfigurator.UseInMemoryOutbox(context);
      //endpointConfigurator.Handler<Fault<BurgerCookerFinishedCookingEvent>>((context) =>
      //{
      //  Console.WriteLine($"Error processing message: {context.Message}");
      //  return Task.CompletedTask;
      //});
    }
  }
}