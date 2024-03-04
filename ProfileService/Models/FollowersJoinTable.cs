namespace ProfileService.Models
{
    public class FollowersJoinTable
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public int FollowingId { get; set; }
    }
}
