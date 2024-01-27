using FluentValidation;
using FluentValidation.Results;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgendaLarAPI.Models.Base
{
    public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsActive = true;
            IsDeleted = false;
        }

        public Guid Id { get; set; }
        public string UserId { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }

        [JsonIgnore]
        public bool IsActive { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }

        [NotMapped]
        public abstract bool IsValid { get; }

        [JsonIgnore]
        [NotMapped]
        public ValidationResult ValidationResult { get; private set; }

        protected void Validate<TModel>(TModel model, AbstractValidator<TModel> validator)
        {
            ValidationResult = validator.Validate(model);
        }
    }
}

