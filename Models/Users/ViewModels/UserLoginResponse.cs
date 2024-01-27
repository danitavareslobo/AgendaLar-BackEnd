namespace AgendaLarAPI.Models.User.ViewModels
{
    public class UserLoginResponse
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public UserToken UserToken { get; set; }
    }
}
