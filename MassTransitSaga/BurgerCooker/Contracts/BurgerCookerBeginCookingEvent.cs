namespace BurgerCooker.Contracts
{
  using System;

  public record BurgerCookerBeginCookingEvent : IBurgerMessage
  {
    public Guid CorrelationId { get; init; }
  }
}