namespace MassTransitSaga.BurgerCooker.Consumers
{
  using global::BurgerCooker.Contracts;
  using MassTransit;
  using Microsoft.Extensions.Logging;
  using System;
  using System.Threading.Tasks;

  public class CookBurgerConsumer(ILogger<CookBurgerConsumer> logger) :
        IConsumer<CookBurger>
  {


    private readonly ILogger<CookBurgerConsumer> _logger = logger;

    public async Task Consume(ConsumeContext<CookBurger> context)
    {

      var cookingSeconds = 5;

      if (context.Message.CookTemp == "Rare")
      {
        cookingSeconds = 2;
      }
      else if (context.Message.CookTemp == "Med")
      {
        cookingSeconds = 5;
      }
      else if (context.Message.CookTemp == "Burned")
      {
        cookingSeconds = 10;
      }
      _logger.LogInformation("Cooking burger for customer {n}, for {t}", context.Message.CustomerName, TimeSpan.FromSeconds(cookingSeconds));

      await Task.Delay(cookingSeconds * 1000);

      //debug_sdc
      throw new Exception("^_^");

      await context.Publish(new BurgerCookerFinishedCookingEvent()
      {
        CorrelationId = context.Message.CorrelationId,
      });
    }
  }


}