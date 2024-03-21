using Microsoft.EntityFrameworkCore;

namespace SuperheroQuoter.Server.Models
{
    public class QuoteContext : DbContext
    {
        public QuoteContext(DbContextOptions<QuoteContext> options) : base(options)
        {
            
        }

        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany(a => a.Quotes)
                .WithOne(q => q.Author)
                .HasForeignKey(q => q.AuthorId)
                .HasPrincipalKey(a => a.Id);
        }
    }
}
