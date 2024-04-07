using Microsoft.EntityFrameworkCore;
using ProfileService.Data;
using ProfileService.Models;

namespace ProfileService.Services
{
    public class ProfileService
    {
        private readonly ProfileServiceDbContext _dbContext;

        public ProfileService(ProfileServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserProfile> GetUserById(int id)
        {
            var user = await _dbContext.Profiles.Select(p => new UserProfile { 
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                UserBio = p.UserBio,
                FollowersIds = p.Followers.Select(p=>p.Id).ToList(),
                FollowingIds = p.Following.Select(p => p.Id).ToList(),
            })
        .FirstOrDefaultAsync(u => u.Id == id);


            if (user != null)
            {
                return user;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public async Task<UserProfile> CreateProfileIfNotExists(UserProfile profile)
        {
            if (!await _dbContext.Profiles.ContainsAsync(profile))
            {
                if (profile.Followers == null) { profile.Followers = new List<UserProfile>(); }
                if (profile.Following == null) { profile.Following = new List<UserProfile>(); }
                if (profile.UserBio == null) { profile.UserBio = "This is your bio, please change it when you have time.";  }

                var dbProfile = await _dbContext.Profiles.AddAsync(profile);

                await _dbContext.SaveChangesAsync();

                return dbProfile.Entity;
            }
            else
            {
                return null;
            }
        }

        public async Task<UserProfile> UpdateProfileBio(int userId, string profileBio)
        {
            var user = await _dbContext.Profiles.FirstOrDefaultAsync(profile => profile.Id == userId);

            if (user != null)
            {
                user.UserBio = profileBio;

                _dbContext.SaveChangesAsync();
                return await _dbContext.Profiles.Select(p => new UserProfile
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    UserBio = p.UserBio,
                    FollowersIds = p.Followers.Select(p => p.Id).ToList(),
                    FollowingIds = p.Following.Select(p => p.Id).ToList(),
                })
        .FirstOrDefaultAsync(u => u.Id == userId);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public async Task Follow(int userId, int followUserId)
        {
            var user = await _dbContext.Profiles.Include(p => p.Following).FirstOrDefaultAsync(profile => profile.Id == userId);

            if (user != null)
            {
                var followUser = await _dbContext.Profiles.Include(p => p.Followers).FirstOrDefaultAsync(profile => profile.Id == followUserId);

                if (followUser != null)
                {
                    if (user.Following == null)
                    {
                        user.Following = new List<UserProfile>();
                    }
                    if (followUser.Followers == null)
                    {
                        followUser.Followers = new List<UserProfile>();
                    }

                    user.Following.Add(followUser);

                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public async Task Unfollow(int userId, int unfollowUserId)
        {
            var user = await _dbContext.Profiles.Include(p => p.Following)
        .FirstOrDefaultAsync(profile => profile.Id == userId);

            if (user != null)
            {
                var followUser = await _dbContext.Profiles.Include(p => p.Followers)
        .FirstOrDefaultAsync(profile => profile.Id == unfollowUserId);

                if (followUser != null)
                {
                    user.Following.Remove(user.Following.FirstOrDefault(p => p.Id == followUser.Id));

                    followUser.Followers.Remove(followUser.Followers.FirstOrDefault(p => p.Id == user.Id));

                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public async Task<List<UserProfile>> GetFollowers(int userId)
        {
            var followers = await _dbContext.Profiles
        .Where(p => p.Following.Any(f => f.Id == userId))
        .Select(p => new UserProfile
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            UserBio = p.UserBio,
            FollowersIds = p.Followers.Select(p => p.Id).ToList(),
            FollowingIds = p.Following.Select(p => p.Id).ToList(),
        })
        .ToListAsync();


            return followers;
        }

        public async Task<List<UserProfile>> GetFollowing(int userId)
        {
            var following = await _dbContext.Profiles
                    .Where(p => p.Followers.Any(f => f.Id == userId))
                    .Select(p => new UserProfile
                    {
                        Id = p.Id,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        UserBio = p.UserBio,
                        FollowersIds = p.Followers.Select(p => p.Id).ToList(),
                        FollowingIds = p.Following.Select(p => p.Id).ToList(),
                    })
                    .ToListAsync();


            return following;
        }
    }
}
