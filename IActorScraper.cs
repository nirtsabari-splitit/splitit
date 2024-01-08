using System.Collections.Generic;

public interface IActorScraper
{
    IAsyncEnumerable<Actor> ScrapeActorsAsync();
}