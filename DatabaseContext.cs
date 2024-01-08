using Microsoft.EntityFrameworkCore;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) { }

    public DbSet<Actor> Actors { get; set; }
}
