using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

// TODO: Validation (FluentValidation)
// Ideally we would be using a service, wrapping the repository and the business logic
// but since this is such a small / simple project, we'll just use the repository directly.
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
        public async Task<Response> GetActors([FromQuery] GetActorsRequest request)
        {
            var validator = new GetActorsRequestValidator();

            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
                return ValidationResultToResponse(validationResult);

            var actors = await _actorRepository.GetActorsAsync(
                new GetActorsOptions
                {
                    ActorName = request.ActorName,
                    MinRank = request.MinRank,
                    MaxRank = request.MaxRank,
                    Provider = request.Provider,
                    Skip = request.Skip,
                    Take = request.Take
                }
            );

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
        public async Task<Response> AddActor(UpsertActorRequest request)
        {
            var validator = new InsertActorRequestValidator();

            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
                return ValidationResultToResponse(validationResult);

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
            var validator = new UpdateActorRequestValidator();

            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
                return ValidationResultToResponse(validationResult);

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

        private static Response ValidationResultToResponse(ValidationResult validationResult)
        {
            return new Response
            {
                IsSuccess = false,
                Errors = validationResult
                    .Errors.Select(
                        e =>
                            new Error
                            {
                                Code = e.ErrorCode,
                                Message = e.ErrorMessage,
                                AdditionalInfo = e.ErrorMessage
                            }
                    )
                    .ToList(),
                StatusCode = 400,
                TraceId = Guid.NewGuid().ToString()
            };
        }
    }
}
