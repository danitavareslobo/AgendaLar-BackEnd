using System.ComponentModel.DataAnnotations;

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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public abstract bool IsValid { get; }
        public ValidationResult ValidationResult { get; private set; }

        protected void Validate<TModel>(TModel model, AbstractValidator<TModel> validator)
        {
            ValidationResult = validator.Validate(model);
        }
    }
}
