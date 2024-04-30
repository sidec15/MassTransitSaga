namespace BurgerCooker.StateMachines
{
  using System;
  using Contracts;
  using MassTransit;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Logging;

  public class BurgerCookerStateMachine :
        MassTransitStateMachine<BurgerCookerState>
  {
    public BurgerCookerStateMachine(IServiceProvider _context)
    //public BurgerCookerStateMachine()
    {
      InstanceState(x => x.CurrentState, Created);

      Event(() => BurgerCookerOrderedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
      Event(() => BurgerCookerBeginCookingEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
      Event(() => BurgerCookerFinishedCookingEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

      Initially(
        When(BurgerCookerOrderedEvent)
          .Then(context =>
          {
            context.Saga.CustomerName = context.Message.CustomerName;
            context.Saga.CookTemp = context.Message.CookTemp;
            var bcEvent = new BurgerCookerBeginCookingEvent()
            {
              CorrelationId = context.Message.CorrelationId,
            };
            context.Publish(bcEvent);

          }).TransitionTo(BeginCooking)
      );

      During(BeginCooking,
        When(BurgerCookerBeginCookingEvent)
          .Then(context =>
          {
            var cb = new CookBurger()
            {
              CorrelationId = context.Saga.CorrelationId, // needed to say that this burger belongs to this sage
              CookTemp = context.Saga.CookTemp,
              CustomerName = context.Saga.CustomerName
            };
            context.Publish(cb);

          }).TransitionTo(FinishedCooking)
      );

      During(FinishedCooking,
        When(BurgerCookerFinishedCookingEvent)
          .Then(context =>
          {
            var logger = _context.GetService<ILogger<BurgerCookerStateMachine>>();
            logger.LogInformation("Order up for: {n}, Cook temp: {t}", context.Saga.CustomerName, context.Saga.CookTemp);
            //throw new Exception("^_^");

          }).TransitionTo(Completed)
      );



      SetCompletedWhenFinalized();
    }

    public State Created { get; private set; }
    public State BeginCooking { get; private set; }
    public State FinishedCooking { get; private set; }
    public State Completed { get; private set; }

    public Event<BurgerCookerOrderedEvent> BurgerCookerOrderedEvent { get; private set; }
    public Event<BurgerCookerBeginCookingEvent> BurgerCookerBeginCookingEvent { get; private set; }
    public Event<BurgerCookerFinishedCookingEvent> BurgerCookerFinishedCookingEvent { get; private set; }
  }
}