using BurgerCooker.Contracts;
using MassTransit;

namespace MassTransitSaga.BurgerCooker.Consumers
{
  public class BurgerCookerFallbackConsumer(ILogger<BurgerCookerFallbackConsumer> _logger, IBus _bus) : IConsumer<Fault<BurgerCookerFinishedCookingEvent>>
  {
    async Task IConsumer<Fault<BurgerCookerFinishedCookingEvent>>.Consume(ConsumeContext<Fault<BurgerCookerFinishedCookingEvent>> context)
    {
      // Specific code to run after all retries fail
      var retryLoop = false;
      var delay = "00:00:10";
      _logger.LogError("Error processing message with correlation id: {m}.{r}", context.Message.Message.CorrelationId, retryLoop ? $"Retrying in {delay}" : "");
      if (retryLoop)
      {
        await Task.Delay((int)TimeSpan.Parse(delay).TotalMilliseconds);
        _logger.LogInformation("Republish failed event");
        await _bus.Publish(context.Message.Message);
      }
      
    }
  }
}
