using System;
using System.Collections.Generic;

public class RottenTomatoesScraper : IActorScraper
{
    public async IAsyncEnumerable<ActorModel> ScrapeActorsAsync()
    {
        yield return new ActorModel
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Rotten Tomatoes Mock Actor",
            Rank = int.MaxValue,
            Source = "rotten-tomatoes",
            Type = "actor",
            Details = "This is a mock actor."
        };
    }
}
