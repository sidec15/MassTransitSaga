using System;

namespace BurgerCooker.Contracts
{
  public record CookBurger : IBurgerMessage
  {
    public Guid CorrelationId { get; init; }
    public string CookTemp { get; init; }
    public string CustomerName { get; init; }
  }
}