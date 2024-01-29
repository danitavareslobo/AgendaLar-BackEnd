namespace AgendaLarAPI.Models.People.ViewModels
{
    public class UpdatePhone
    {
        public string Id { get; set; }

        public string PersonId { get; set; }

        public PhoneType Type { get; set; }

        public string Number { get; set; }
    }
}
