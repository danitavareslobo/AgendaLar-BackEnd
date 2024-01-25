using AgendaLarAPI.Data.Repositories.Interfaces;
using AgendaLarAPI.Services.Interfaces;

namespace AgendaLarAPI.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly NotificationContext _notificationContext;

        public PersonService(
            IPersonRepository personRepository,
            NotificationContext notificationContext)
        {
            _personRepository = personRepository;
            _notificationContext = notificationContext;
        }

        public Task<Model.Person?> GetByIdAsync(Guid id)
        {
            return _personRepository.GetByIdAsync(id);
        }

        public Task<List<Model.Person>> GetAllAsync()
        {
            return _personRepository.GetAllAsync();
        }

        public Task<List<Model.Person>> GetPagedAsync(int pageSize, int pageIndex)
        {
            return _personRepository.GetPagedAsync(pageSize, pageIndex);
        }

        public async Task<Model.Person?> AddAsync(Model.Person entity)
        {
            if (!entity.IsValid)
            {
                _notificationContext.AddNotifications(entity.ValidationResult);
                return null;
            }

            var result = await _personRepository.AddAsync(entity);

            if (result == null || result.Id == Guid.Empty)
                _notificationContext.AddNotification("Person", "Não foi possível adicionar a pessoa");

            return result;
        }

        public async Task<Model.Person?> UpdateAsync(Model.Person entity)
        {
            if (entity.IsValid) return await _personRepository.UpdateAsync(entity);

            _notificationContext.AddNotifications(entity.ValidationResult);
            return null;
        }

        public async Task<bool> DeleteAsync(Model.Person entity)
        {
            var person = await _personRepository.GetByIdAsync(entity.Id);

            if (person == null)
            {
                _notificationContext.AddNotification("Person", "Não foi possível encontrar a pessoa");
                return false;
            }

            entity.IsDeleted = true;
            entity.IsActive = false;
            var result = await _personRepository.UpdateAsync(entity);

            return result?.IsDeleted ?? false;
        }

        public void Dispose()
        {
            _personRepository?.Dispose();
        }
    }
}
