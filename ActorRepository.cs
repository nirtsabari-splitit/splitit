using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class ActorRepository(DatabaseContext databaseContext) : IActorRepository
{
    private DatabaseContext _databaseContext = databaseContext;

    public Task<List<ActorModel>> GetActorsAsync()
    {
        return _databaseContext.Actors.ToListAsync();
    }

    public async Task DeleteActorAsync(string id)
    {
        var actor = await _databaseContext.Actors.FirstOrDefaultAsync(a => a.Id == id);

        if (actor != null)
        {
            _databaseContext.Actors.Remove(actor);

            await _databaseContext.SaveChangesAsync();
        }
    }

    public async Task<ActorModel> GetActorAsync(string id)
    {
        return await _databaseContext.Actors.FirstOrDefaultAsync(a => a.Id.Contains(id)); // TODO: Should be an exact match, but for now, this will do.
    }

    public async Task SaveBulkAsync(List<ActorModel> actors)
    {
        await _databaseContext.Actors.AddRangeAsync(actors);

        await _databaseContext.SaveChangesAsync();
    }

    public async Task UpsertActorAsync(ActorModel actor)
    {
        var existingActor = await _databaseContext.Actors.FirstOrDefaultAsync(
            a => a.Id == actor.Id
        );

        if (existingActor is not null)
            _databaseContext.Actors.Update(actor);
        else
            await _databaseContext.Actors.AddAsync(actor);

        await _databaseContext.SaveChangesAsync();
    }
}
