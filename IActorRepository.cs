using System.Collections.Generic;
using System.Threading.Tasks;

public interface IActorRepository
{
    Task<List<ActorModel>> GetActorsAsync(GetActorsRequest options); // TODO: Don't use the same type as the DTO
    Task<ActorModel> GetActorAsync(string id);
    Task UpsertActorAsync(ActorModel actor);
    Task DeleteActorAsync(string id);
    Task SaveManyAsync(List<ActorModel> actors);
}
