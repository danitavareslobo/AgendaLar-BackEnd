using AgendaLarAPI.Data.Repositories.Interfaces;
using AgendaLarAPI.Services.Interfaces;

using Model = AgendaLarAPI.Models.People;

namespace AgendaLarAPI.Services
{
    public class PhoneService : IPhoneService
    {
        private readonly IPhoneRepository _phoneRepository;
        private readonly NotificationService _notificationService;

        public PhoneService(
            IPhoneRepository phoneRepository,
            NotificationService notificationService)
        {
            _phoneRepository = phoneRepository;
            _notificationService = notificationService;
        }

        public Task<Model.Phone?> GetByIdAsync(string loggedUserId, Guid id)
        {
            return _phoneRepository.GetByIdAsync(loggedUserId, id);
        }

        public Task<List<Model.Phone>> GetAllAsync(string loggedUserId)
        {
            return _phoneRepository.GetAllAsync(loggedUserId);
        }

        public Task<List<Model.Phone>> GetPagedAsync(string loggedUserId, int pageSize, int pageIndex)
        {
            return _phoneRepository.GetPagedAsync(loggedUserId, pageSize, pageIndex);
        }

        public async Task<Model.Phone?> AddAsync(Model.Phone entity)
        {
            if (!entity.IsValid)
            {
                _notificationService.AddNotifications(entity.ValidationResult);
                return null;
            }

            var result = await _phoneRepository.AddAsync(entity);

            if (result == null || result.Id == Guid.Empty)
                _notificationService.AddNotification("Telefone", "Não foi possível adicionar o telefone");

            return result;
        }

        public async Task<Model.Phone?> UpdateAsync(Model.Phone entity)
        {
            if (entity.IsValid) return await _phoneRepository.UpdateAsync(entity);

            _notificationService.AddNotifications(entity.ValidationResult);
            return null;
        }

        public async Task<bool> DeleteAsync(string loggedUserId, Guid id)
        {
            var phone = await _phoneRepository.GetByIdAsync(loggedUserId, id);

            if (phone == null)
            {
                _notificationService.AddNotification("Phone", "Não foi possível encontrar o telefone");
                return false;
            }

            phone.IsDeleted = true;
            phone.IsActive = false;
            var result = await _phoneRepository.UpdateAsync(phone);

            return result?.IsDeleted ?? false;
        }

        public Task<List<Model.Phone>> GetAllByPersonIdAsync(string loggedUserId, Guid personId)
        {
            return _phoneRepository.GetAllByPersonIdAsync(loggedUserId, personId);
        }

        public void Dispose()
        {
            _phoneRepository?.Dispose();
        }
    }

}
