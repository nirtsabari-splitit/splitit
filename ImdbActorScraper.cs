using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

public class ImdbActorScraper(IHttpClientFactory httpClientFactory) : IActorScraper
{
    private readonly IHttpClientFactory httpClientFactory = httpClientFactory;

    public async IAsyncEnumerable<Actor> ScrapeActorsAsync()
    {
        HtmlDocument document = await GetDocument();

        var listItems = document.DocumentNode.QuerySelectorAll(".lister-item.mode-detail");

        foreach (var item in listItems)
        {
            var actor = new Actor();

            // Everything here may not exist / be nullable, we'll ignore that for brevity.
            var header = item.QuerySelector(".lister-item-header a");
            var id = header?.GetAttributeValue("href", null);
            var rank = item?.QuerySelector(".lister-item-index")?.InnerText?.TrimEnd(['.', ' ']);
            var name = header?.InnerText?.Trim();
            var description = item?.QuerySelector(".list-description")?.InnerText?.Trim();
            var type = item?.QuerySelector("p")?.GetDirectInnerText()?.Trim();

            actor.Id = id;
            actor.Rank = int.Parse(rank); // Should be a TryParse, but I'm skipping it for brevity.
            actor.Name = name;
            actor.Type = type;
            actor.Source = "imdb";
            actor.Details = description;

            yield return actor;
        }
    }

    private async Task<HtmlDocument> GetDocument()
    {
        var html = await GetHtmlAsync();

        var document = new HtmlDocument();

        document.LoadHtml(html);

        return document;
    }

    private async Task<string> GetHtmlAsync()
    {
        var httpClient = httpClientFactory.CreateClient();

        var response = await httpClient.GetAsync("https://www.imdb.com/list/ls054840033/");

        var html = await response.Content.ReadAsStringAsync();

        return html;
    }
}
