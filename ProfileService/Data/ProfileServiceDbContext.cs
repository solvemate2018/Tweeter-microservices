using Microsoft.EntityFrameworkCore;
using ProfileService.Models;

namespace ProfileService.Data
{
    public class ProfileServiceDbContext : DbContext
    {
        public virtual DbSet<UserProfile> Profiles { get; set; }

        public ProfileServiceDbContext(DbContextOptions<ProfileServiceDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public ProfileServiceDbContext() : base()
        {
        }

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
    }
}
