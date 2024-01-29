namespace AgendaLarAPI.Models.User.ViewModels
{
    public class UserToken
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string? Name { get; set; }
        public IEnumerable<UserClaim> Claims { get; set; }
    }
}
