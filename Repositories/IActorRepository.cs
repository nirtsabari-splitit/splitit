using System.Collections.Generic;
using System.Threading.Tasks;

public interface IActorRepository
{
    Task<List<ActorModel>> GetActorsAsync(GetActorsOptions options); // TODO: Don't use the same type as the DTO
    Task<ActorModel> GetActorAsync(string id);
    Task UpsertActorAsync(ActorModel actor);
    Task DeleteActorAsync(string id);
    Task SaveManyAsync(List<ActorModel> actors);
}

public class GetActorsOptions
{
    public string ActorName { get; set; }
    public int? MinRank { get; set; }
    public int? MaxRank { get; set; }
    public string Provider { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; }
}
