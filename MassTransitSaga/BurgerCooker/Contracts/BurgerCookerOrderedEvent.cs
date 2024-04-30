namespace BurgerCooker.Contracts
{
  using System;

  public record BurgerCookerOrderedEvent : IBurgerMessage
  {
    public Guid CorrelationId { get; init; }
    public string CustomerName { get; init; }
    public string CookTemp { get; set; }
  }
}