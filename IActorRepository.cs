using System.Collections.Generic;
using System.Threading.Tasks;

public interface IActorRepository
{
    Task<List<ActorModel>> GetActorsAsync();
    Task<ActorModel> GetActorAsync(string id);
    Task UpsertActorAsync(ActorModel actor);
    Task DeleteActorAsync(string id);
    Task SaveBulkAsync(List<ActorModel> actors);
}
