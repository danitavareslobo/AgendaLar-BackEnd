namespace AgendaLarAPI.Data.Repositories.Interfaces
{
    public interface IPhoneRepository : IRepository<Model.Phone>
    {
        Task<List<Model.Phone>> GetAllByPersonIdAsync(Guid personId);
    }
}
