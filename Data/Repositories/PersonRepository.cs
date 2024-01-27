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
                .Include(p => p.Phones)
                .FirstAsync(p => p.Id.Equals(id)
                                 && p.UserId == loggedUserId);
        }

        public async Task<List<Model.Person>> GetAllAsync(string loggedUserId)
        {
            return await _context.Person
                .AsNoTracking()
                .Include(p => p.Phones)
                .Where(p => p.UserId == loggedUserId
                            && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Model.Person>> GetPagedAsync(string loggedUserId, int pageSize, int pageIndex)
        {
            return await _context.Person.AsNoTracking()
                .Include(p => p.Phones)
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
            }

            _context.Person.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Model.Person?> UpdateAsync(Model.Person entity)
        {
            foreach (var phone in entity.Phones)
            {
                phone.UserId = entity.UserId;
                phone.PersonId = entity.Id;

                if (entity.IsDeleted)
                    phone.IsDeleted = true;
            }

            _context.Person.Update(entity);
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Entry(entity).Property(p => p.CreatedAt).IsModified = false;
            await _context.SaveChangesAsync();
            return entity;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
