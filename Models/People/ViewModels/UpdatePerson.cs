namespace AgendaLarAPI.Models.People.ViewModels
{
    public class UpdatePerson
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string SocialNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public bool Ativo { get; set; }

        public List<CreatePhone> NewPhones { get; set; } = new List<CreatePhone>();

        public List<UpdatePhone> UpdatedPhones { get; set; } = new List<UpdatePhone>();
    }
}
