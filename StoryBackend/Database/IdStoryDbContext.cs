using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoryBackend.Models;

namespace StoryBackend.Database
{
    public class IdStoryDbContext : IdentityDbContext
    {
        //public DbSet<StoryEntry> StoryEntries { get; set; }

        public IdStoryDbContext(DbContextOptions<IdStoryDbContext> dbContextOptions) : base(dbContextOptions) {}
    }
}
