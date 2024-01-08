using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class ActorRepository(DatabaseContext databaseContext) : IActorRepository
{
    private DatabaseContext _databaseContext = databaseContext;

    public async Task<List<ActorModel>> GetActorsAsync(GetActorsRequest options)
    {
        var query = _databaseContext.Actors.AsQueryable();

        if (!string.IsNullOrEmpty(options.ActorName))
            query = query.Where(actor => actor.Name.Contains(options.ActorName));

        if (options.MinRank.HasValue)
            query = query.Where(actor => actor.Rank >= options.MinRank.Value);

        if (options.MaxRank.HasValue)
            query = query.Where(actor => actor.Rank <= options.MaxRank.Value);

        if (!string.IsNullOrEmpty(options.Provider))
            query = query.Where(actor => actor.Source == options.Provider);

        query = query.Skip(options.Skip).Take(options.Take);

        return await query.ToListAsync();
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

    public async Task SaveManyAsync(List<ActorModel> actors)
    {
        await _databaseContext.Actors.AddRangeAsync(actors);

        await _databaseContext.SaveChangesAsync();
    }

    public async Task UpsertActorAsync(ActorModel actor)
    {
        // Both of these can be a simple UNIQUE constraint on the database, but I'm skipping that for brevity.
        var existingActor = await _databaseContext.Actors.FirstOrDefaultAsync(
            a => a.Id == actor.Id || a.Rank == actor.Rank
        );

        if (existingActor is not null)
            _databaseContext.Actors.Update(actor);
        else
            await _databaseContext.Actors.AddAsync(actor);

        await _databaseContext.SaveChangesAsync();
    }
}
