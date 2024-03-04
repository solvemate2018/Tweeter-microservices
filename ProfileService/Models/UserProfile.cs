namespace ProfileService.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserTag {  get; set; }
        public string UserBio { get; set; }
        public string ProfileUrl { get; set; }
        public List<UserProfile> Followers { get; set; }
        public List<UserProfile> Following { get; set; }
    }
}
