using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SplitIt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActorController : ControllerBase
    {
        private readonly ILogger<ActorController> _logger;
        private readonly IActorScraper _actorScraper;

        public ActorController(ILogger<ActorController> logger, IActorScraper actorScraper)
        {
            _logger = logger;
            _actorScraper = actorScraper;
        }

        [HttpGet]
        public async Task<IEnumerable<ActorModel>> GetActors()
        {
            var actors = new List<ActorModel>();

            await foreach (var actor in _actorScraper.ScrapeActorsAsync())
                actors.Add(actor);

            return actors;
        }

        [HttpGet("{id}")]
        public async Task<ActorModel> GetActor(string id)
        {
            var actors = new List<ActorModel>();

            await foreach (var actor in _actorScraper.ScrapeActorsAsync())
                actors.Add(actor);

            return actors.FirstOrDefault(a => a.Id == id);
        }
    }
}
