using Microsoft.EntityFrameworkCore;
using TweetsService.Models;

namespace TweetsService.Data
{
    public class TweeterServiceDbContext : DbContext
    {

        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }

        public TweeterServiceDbContext(DbContextOptions<TweeterServiceDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tweet>()
                .HasMany(t => t.Comments)
                .WithOne()
                .HasForeignKey(c => c.TweetId)
                .IsRequired();

            modelBuilder.Entity<Tweet>()
                .HasMany(t => t.Reactions)
                .WithOne()
                .HasForeignKey(r => r.TweetId)
                .IsRequired();


            modelBuilder.Entity<Reaction>()
                .Property(r => r.ReactionType)
                .HasConversion(
                rt => rt.ToString(),
                rtdb => (ReactionType)Enum.Parse(typeof(ReactionType), rtdb));

            modelBuilder.Entity<Reaction>()
                .HasKey(r => new { r.TweetId, r.UserId });

            modelBuilder.Entity<Reaction>()
                .HasAlternateKey(r => r.Id);
        }
    }
}
