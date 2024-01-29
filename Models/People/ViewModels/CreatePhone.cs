namespace AgendaLarAPI.Models.People.ViewModels
{
    public class CreatePhone
    {
        public string? PersonId { get; set; }

        public PhoneType Type { get; set; }

        public string Number { get; set; }
    }
}
