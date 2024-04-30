using System;

namespace BurgerCooker.Contracts
{
  public interface IBurgerMessage
  {
    public Guid CorrelationId { get; init; }
  }
}
