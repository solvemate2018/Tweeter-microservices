using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileService.Models
{
    public class UserProfile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserBio { get; set; }
        public List<UserProfile> Followers { get; set; }
        public List<UserProfile> Following { get; set; }

        [NotMapped]
        public List<int> FollowersIds { get; set; }
        [NotMapped]
        public List<int> FollowingIds { get; set; }
    }
}
