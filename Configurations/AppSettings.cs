namespace AgendaLarAPI.Configurations
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string ValidIn { get; set; }
        public int ExpirationHours { get; set; }
    }
}
