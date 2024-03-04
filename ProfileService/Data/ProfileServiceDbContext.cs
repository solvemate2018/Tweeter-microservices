using Microsoft.EntityFrameworkCore;
using ProfileService.Models;

namespace ProfileService.Data
{
    public class ProfileServiceDbContext : DbContext
    {
        public DbSet<UserProfile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>().HasKey(p => p.Id);

            modelBuilder.Entity<UserProfile>()
                .HasMany(profile => profile.Followers)
                .WithMany(profile => profile.Following)
                .UsingEntity<FollowersJoinTable>(
                l => l.HasOne<UserProfile>().WithMany().HasForeignKey(p => p.ProfileId),
                r => r.HasOne<UserProfile>().WithMany().HasForeignKey(p => p.FollowingId)
                );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=HACKERBOT;Database=profile_service_db;Trusted_Connection=True;Encrypt=no");
        }
    }
}
