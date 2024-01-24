namespace AgendaLarAPI.Models.User
{
    public class UserLoginResponse
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public UserToken UserToken { get; set; }
    }
}
