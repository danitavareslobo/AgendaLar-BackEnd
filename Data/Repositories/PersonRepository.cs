using AgendaLarAPI.Data.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

using Model = AgendaLarAPI.Models.People;

namespace AgendaLarAPI.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Model.Person?> GetByIdAsync(string loggedUserId, Guid id)
        {
            return await _context.Person
                .Include(p => p.Phones.Where(ph => !ph.IsDeleted))
                .FirstAsync(p => p.Id.Equals(id)
                                 && p.UserId == loggedUserId);
        }

        public async Task<List<Model.Person>> GetAllAsync(string loggedUserId)
        {
            return await _context.Person
                .AsNoTracking()
                .Include(p => p.Phones.Where(ph => !ph.IsDeleted))
                .Where(p => p.UserId == loggedUserId
                            && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Model.Person>> GetPagedAsync(string loggedUserId, int pageSize, int pageIndex)
        {
            return await _context.Person.AsNoTracking()
                .Include(p => p.Phones.Where(ph => !ph.IsDeleted))
                .Where(p => p.UserId == loggedUserId
                            && !p.IsDeleted)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Model.Person?> AddAsync(Model.Person entity)
        {
            entity.Phones ??= new List<Model.Phone>();
            foreach (var phone in entity.Phones)
            {
                phone.UserId = entity.UserId;
                phone.PersonId = entity.Id;
                _context.Add(phone);
            }

            _context.Person.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Model.Person?> UpdateAsync(Model.Person entity)
        {
            var originalEntity = await _context.Person
                .Include(p => p.Phones.Where(ph => !ph.IsDeleted))
                .FirstOrDefaultAsync(p => p.Id == entity.Id);

            if (originalEntity == null)
                return null;

            _context.Entry(originalEntity).CurrentValues.SetValues(entity);

            foreach (var originalPhone in originalEntity.Phones.ToList())
            {
                if (!entity.Phones.Any(updatedPhone => updatedPhone.Id == originalPhone.Id))
                {
                    _context.Phone.Remove(originalPhone);
                }
            }

            foreach (var updatedPhone in entity.Phones)
            {
                var originalPhone = originalEntity.Phones.FirstOrDefault(p => p.Id == updatedPhone.Id);

                updatedPhone.PersonId = entity.Id;
                updatedPhone.UserId = entity.UserId;
                if (originalPhone != null)
                    _context.Entry(originalPhone).CurrentValues.SetValues(updatedPhone);
                else
                    originalEntity.Phones.Add(updatedPhone);
            }

            originalEntity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return originalEntity;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
