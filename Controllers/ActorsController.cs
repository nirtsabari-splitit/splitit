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
    public class ActorsController : ControllerBase
    {
        private readonly IActorScraper _actorScraper;
        private readonly IActorRepository _actorRepository;

        public ActorsController(IActorScraper actorScraper, IActorRepository actorRepository)
        {
            _actorScraper = actorScraper;
            _actorRepository = actorRepository;
        }

        [HttpGet]
        public async Task<ActorsResponse> GetActors()
        {
            var actors = await _actorRepository.GetActorsAsync();

            return new ActorsResponse
            {
                IsSuccess = true,
                Actors = actors,
                Errors = [],
                StatusCode = 200,
                TraceId = Guid.NewGuid().ToString()
            };
        }

        [HttpGet("{id}")]
        public async Task<ActorResponse> GetActor(string id)
        {
            var actor = await _actorRepository.GetActorAsync(id);

            return new ActorResponse
            {
                IsSuccess = true,
                Actor = actor,
                Errors = [],
                StatusCode = 200,
                TraceId = Guid.NewGuid().ToString()
            };
        }

        [HttpGet("populate")]
        public async Task<Response> PopulateActors()
        {
            var actors = new List<ActorModel>();

            await foreach (var actor in _actorScraper.ScrapeActorsAsync())
                actors.Add(actor);

            await _actorRepository.SaveBulkAsync(actors);

            return new Response
            {
                IsSuccess = true,
                Errors = [],
                StatusCode = 200,
                TraceId = Guid.NewGuid().ToString()
            };
        }
    }
}
