using AgendaLarAPI.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model = AgendaLarAPI.Models.Person;

namespace AgendaLarAPI.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Model.Person?> GetByIdAsync(Guid id)
        {
            return await _context.Person.FindAsync(id);
        }

        public async Task<List<Model.Person>> GetAllAsync()
        {
            return await _context.Person.AsNoTracking().Where(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<List<Model.Person>> GetPagedAsync(int pageSize, int pageIndex)
        {
            return await _context.Person.AsNoTracking()
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Model.Person?> AddAsync(Model.Person entity)
        {
            _context.Person.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Model.Person?> UpdateAsync(Model.Person entity)
        {
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
