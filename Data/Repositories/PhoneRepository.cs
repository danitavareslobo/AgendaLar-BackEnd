using AgendaLarAPI.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model = AgendaLarAPI.Models.People;

namespace AgendaLarAPI.Data.Repositories
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly ApplicationDbContext _context;

        public PhoneRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Model.Phone> AddAsync(Model.Phone phone)
        {
            await _context.Phone.AddAsync(phone);
            await _context.SaveChangesAsync();

            return phone;
        }

        public async Task<Model.Phone> UpdateAsync(Model.Phone phone)
        {
            _context.Phone.Update(phone);
            await _context.SaveChangesAsync();

            return phone;
        }

        public async Task<Model.Phone> GetByIdAsync(string loggedUserId, Guid id)
        {
            var phone = await _context.Phone.FirstOrDefaultAsync(p => p.UserId == loggedUserId
                                                                            && p.Id == id);

            return await _context.Phone.FindAsync(id) ?? new Model.Phone();
        }

        public async Task<List<Model.Phone>> GetAllAsync(string loggedUserId)
        {
            return await _context.Phone.AsNoTracking()
                                        .Where(p => p.UserId == loggedUserId
                                            && !p.IsDeleted)
                                        .ToListAsync();
        }

        public async Task<List<Model.Phone>> GetPagedAsync(string loggedUserId, int pageSize, int pageIndex)
        {
            return await _context.Phone
                                    .AsNoTracking()
                                    .Where(p => p.UserId == loggedUserId
                                            && !p.IsDeleted)
                                    .Skip(pageSize * pageIndex)
                                    .Take(pageSize)
                                    .ToListAsync();
        }

        public async Task<List<Model.Phone>> GetAllByPersonIdAsync(string loggedUserId, Guid personId)
        {
            return await _context.Phone
                .AsNoTracking()
                .Where(p => p.UserId == loggedUserId
                            && p.PersonId == personId)
                .ToListAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
