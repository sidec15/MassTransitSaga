namespace BurgerCooker.Contracts
{
  using System;

  public record BurgerCookerFinishedCookingEvent : IBurgerMessage
  {
    public Guid CorrelationId { get; init; }
  }
}