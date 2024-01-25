using AgendaLarAPI.Data.Repositories.Interfaces;
using AgendaLarAPI.Services.Interfaces;

using Model = AgendaLarAPI.Models.Person;

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

        public Task<Model.Phone?> GetByIdAsync(Guid id)
        {
            return _phoneRepository.GetByIdAsync(id);
        }

        public Task<List<Model.Phone>> GetAllAsync()
        {
            return _phoneRepository.GetAllAsync();
        }

        public Task<List<Model.Phone>> GetPagedAsync(int pageSize, int pageIndex)
        {
            return _phoneRepository.GetPagedAsync(pageSize, pageIndex);
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
                _notificationService.AddNotification("Phone", "Não foi possível adicionar a pessoa");

            return result;
        }

        public async Task<Model.Phone?> UpdateAsync(Model.Phone entity)
        {
            if (entity.IsValid) return await _phoneRepository.UpdateAsync(entity);

            _notificationService.AddNotifications(entity.ValidationResult);
            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var phone = await _phoneRepository.GetByIdAsync(id);

            if (phone == null)
            {
                _notificationService.AddNotification("Phone", "Não foi possível encontrar a pessoa");
                return false;
            }

            phone.IsDeleted = true;
            phone.IsActive = false;
            var result = await _phoneRepository.UpdateAsync(phone);

            return result?.IsDeleted ?? false;
        }

        public void Dispose()
        {
            _phoneRepository?.Dispose();
        }
    }
}
