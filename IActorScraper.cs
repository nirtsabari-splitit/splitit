using System.Collections.Generic;

public interface IActorScraper
{
    IAsyncEnumerable<ActorModel> ScrapeActorsAsync();
}
