using Microsoft.EntityFrameworkCore;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<ActorModel> Actors { get; set; }
}
