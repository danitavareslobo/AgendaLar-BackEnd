using AgendaLarAPI.Data.Repositories.Interfaces;
using AgendaLarAPI.Services.Interfaces;

using Model = AgendaLarAPI.Models.Person;

namespace AgendaLarAPI.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly NotificationService _notificationService;

        public PersonService(
            IPersonRepository personRepository,
            NotificationService notificationService)
        {
            _personRepository = personRepository;
            _notificationService = notificationService;
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
                _notificationService.AddNotifications(entity.ValidationResult);
                return null;
            }

            var result = await _personRepository.AddAsync(entity);

            if (result == null || result.Id == Guid.Empty)
                _notificationService.AddNotification("Person", "Não foi possível adicionar a pessoa");

            return result;
        }

        public async Task<Model.Person?> UpdateAsync(Model.Person entity)
        {
            if (entity.IsValid) return await _personRepository.UpdateAsync(entity);

            _notificationService.AddNotifications(entity.ValidationResult);
            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var person = await _personRepository.GetByIdAsync(id);

            if (person == null)
            {
                _notificationService.AddNotification("Person", "Não foi possível encontrar a pessoa");
                return false;
            }

            person.IsDeleted = true;
            person.IsActive = false;
            var result = await _personRepository.UpdateAsync(person);

            return result?.IsDeleted ?? false;
        }

        public void Dispose()
        {
            _personRepository?.Dispose();
        }
    }
}
