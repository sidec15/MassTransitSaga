using MassTransit;
using Microsoft.Extensions.Logging;
using BurgerCooker.Contracts;
using System.Threading.Tasks;

namespace MassTransitSaga.BurgerCooker.Consumers
{
  public class BurgerCookerFallbackConsumer(ILogger<BurgerCookerFallbackConsumer> _logger) : IConsumer<Fault<BurgerCookerFinishedCookingEvent>>
  {
    Task IConsumer<Fault<BurgerCookerFinishedCookingEvent>>.Consume(ConsumeContext<Fault<BurgerCookerFinishedCookingEvent>> context)
    {
      // Specific code to run after all retries fail
      _logger.LogError("Error processing message: {m}", context.Message);

      return Task.CompletedTask;
    }
  }
}
