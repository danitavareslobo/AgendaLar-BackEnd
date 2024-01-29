using AgendaLarAPI.Data.Repositories.Interfaces;
using AgendaLarAPI.Models.People;
using AgendaLarAPI.Services.Interfaces;

using FluentValidation.Results;

using Model = AgendaLarAPI.Models.People;

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

        public Task<Models.People.Person?> GetByIdAsync(string loggedUserId, Guid id)
        {
            return _repository.GetByIdAsync(loggedUserId, id);
        }

        public Task<List<Models.People.Person>> GetAllAsync(string loggedUserId)
        {
            return _repository.GetAllAsync(loggedUserId);
        }

        public Task<List<Models.People.Person>> GetPagedAsync(string loggedUserId, int pageSize, int pageIndex)
        {
            return _repository.GetPagedAsync(loggedUserId, pageSize, pageIndex);
        }

        public async Task<Models.People.Person?> AddAsync(Models.People.Person entity)
        {
            if (!entity.IsValid)
            {
                _notificationService.AddNotifications(entity.ValidationResult);
                return null;
            }

            entity.Phones ??= new List<Phone>();
            if(entity.Phones.Count > 0) 
            {
                foreach (var phone in entity.Phones)
                {
                    if (!phone.IsValid)
                    {
                        var validationResult = new ValidationResult(entity.ValidationResult.Errors.Where(e => e.ErrorCode != nameof(Phone.PersonId)));
                        
                        if(validationResult.Errors.Count > 0)
                            _notificationService.AddNotifications(validationResult);
                    }
                }

                if (_notificationService.HasNotifications)
                    return null;
            }

            var result = await _repository.AddAsync(entity);

            if (result == null || result.Id == Guid.Empty)
            {
                _notificationService.AddNotification("Pessoa", "Não foi possível adicionar a pessoa");
            }

            return result;
        }

        public async Task<Person?> UpdateAsync(Person entity)
        {
            if (!entity.IsValid)
            {
                _notificationService.AddNotifications(entity.ValidationResult);
                return null;
            }

            if (entity.Phones.Count > 0)
            {
                foreach (var phone in entity.Phones)
                {
                    if (!phone.IsValid)
                        _notificationService.AddNotifications(entity.ValidationResult);
                }

                if (_notificationService.HasNotifications)
                    return null;
            }

            var result = await _repository.UpdateAsync(entity);

            if (result == null || result.Id == Guid.Empty)
            {
                _notificationService.AddNotification("Pessoa", "Não foi possível atualizar a pessoa");
            }

            return result;
        }

        public async Task<bool> DeleteAsync(string loggedUserId, Guid id)
        {
            var phone = await _repository.GetByIdAsync(loggedUserId, id);

            if (phone == null)
            {
                _notificationService.AddNotification("Pessoa", "Não foi possível encontrar a pessoa");
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
