using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// TODO: Filtering, Pagination, Error Handling (Middleware), Validation (FluentValidation)
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
        public async Task<ActorsResponse> GetActors([FromQuery] GetActorsRequest request)
        {
            var actors = await _actorRepository.GetActorsAsync(request);

            var entries = actors
                .Select(a => new ActorsResponseEntry { Id = a.Id, Name = a.Name })
                .ToList();

            return new ActorsResponse
            {
                IsSuccess = true,
                Actors = entries,
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

        [HttpPost]
        public async Task<Response> UpsertActor(UpsertActorRequest request)
        {
            var actor = new ActorModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Details = request.Details,
                Type = request.Type,
                Rank = request.Rank,
                Source = request.Source
            };

            await _actorRepository.UpsertActorAsync(actor);

            return new Response
            {
                IsSuccess = true,
                Errors = [],
                StatusCode = 200,
                TraceId = Guid.NewGuid().ToString()
            };
        }

        [HttpPut("{id}")]
        public async Task<Response> UpdateActor(string id, [FromBody] UpsertActorRequest request)
        {
            var actor = new ActorModel
            {
                Id = id,
                Name = request.Name,
                Details = request.Details,
                Type = request.Type,
                Rank = request.Rank,
                Source = request.Source
            };

            await _actorRepository.UpsertActorAsync(actor);

            return new Response
            {
                IsSuccess = true,
                Errors = [],
                StatusCode = 200,
                TraceId = Guid.NewGuid().ToString()
            };
        }

        [HttpDelete("{id}")]
        public async Task<Response> DeleteActor(string id)
        {
            await _actorRepository.DeleteActorAsync(id);

            return new Response
            {
                IsSuccess = true,
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

            await _actorRepository.SaveManyAsync(actors);

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
