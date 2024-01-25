using AgendaLarAPI.Data.Repositories.Interfaces;
using AgendaLarAPI.Services.Interfaces;
using Model = AgendaLarAPI.Models.Person;

namespace AgendaLarAPI.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repository;
        private readonly NotificationService _notificationService;

        public PersonService(
            IPersonRepository repository,
            NotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }

        public Task<Models.Person.Person?> GetByIdAsync(Guid id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<List<Models.Person.Person>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<List<Models.Person.Person>> GetPagedAsync(int pageSize, int pageIndex)
        {
            return _repository.GetPagedAsync(pageSize, pageIndex);
        }

        public Task<Models.Person.Person?> AddAsync(Models.Person.Person entity)
        {
            return _repository.AddAsync(entity);
        }

        public Task<Models.Person.Person?> UpdateAsync(Models.Person.Person entity)
        {
            return _repository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var phone = await _repository.GetByIdAsync(id);

            if (phone == null)
            {
                _notificationService.AddNotification("Phone", "Não foi possível encontrar a pessoa");
                return false;
            }

            phone.IsDeleted = true;
            phone.IsActive = false;
            var result = await _repository.UpdateAsync(phone);

            return result?.IsDeleted ?? false;
        }

        public void Dispose()
        {
            _repository?.Dispose();
        }
    }
}
