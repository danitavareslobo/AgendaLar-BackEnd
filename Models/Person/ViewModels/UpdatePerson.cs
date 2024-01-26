﻿using System.Text.Json.Serialization;

namespace AgendaLarAPI.Models.Person.ViewModels
{
    public class UpdatePerson
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("nome")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("cpf")]
        public string SocialNumber { get; set; }

        [JsonPropertyName("dataNascimento")]
        public DateTime BirthDate { get; set; }

        [JsonPropertyName("ativo")]
        public bool Ativo { get; set; }

        [JsonPropertyName("telefones-novos")]
        public List<CreatePhone> NewPhones { get; set; } = new List<CreatePhone>();

        [JsonPropertyName("telefones-atualizados")]
        public List<UpdatePhone> UpdatedPhones { get; set; } = new List<UpdatePhone>();
    }
}
