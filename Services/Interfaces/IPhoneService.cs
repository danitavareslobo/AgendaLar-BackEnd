using AgendaLarAPI.Models.People;

namespace AgendaLarAPI.Services.Interfaces
{
    public interface IPhoneService : IService<Models.People.Phone>
    {
        Task<List<Phone>> GetAllByPersonIdAsync(string loggedUserId, Guid personId);
    }
}
