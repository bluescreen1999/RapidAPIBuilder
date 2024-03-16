using Microsoft.EntityFrameworkCore;
using RapidAPIBuilder.Models.Entties;

namespace RapidAPIBuilder.Models;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tag>()
            .Property(_ => _.Title)
            .HasMaxLength(150);
    }
}

