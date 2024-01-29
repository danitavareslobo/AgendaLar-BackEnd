using Model = AgendaLarAPI.Models.People;

namespace AgendaLarAPI.Data.Repositories.Interfaces;

    public interface IPhoneRepository : IRepository<Model.Phone>
    {
        Task<List<Model.Phone>> GetAllByPersonIdAsync(string loggedUserId, Guid personId);
        //Task<bool> DeletePersonPhone(Guid personId, Guid id);
    }

