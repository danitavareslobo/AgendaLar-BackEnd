﻿using AgendaLarAPI.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model = AgendaLarAPI.Models.Person;

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

        public async Task<Model.Phone> GetByIdAsync(Guid id)
        {
            return await _context.Phone.FindAsync(id) ?? new Model.Phone();
        }

        public async Task<List<Model.Phone>> GetAllAsync()
        {
            return await _context.Phone.AsNoTracking().Where(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<List<Model.Phone>> GetPagedAsync(int pageSize, int pageIndex)
        {
            return await _context.Phone
                                    .AsNoTracking()
                                    .Where(p => !p.IsDeleted)
                                    .Skip(pageSize * pageIndex)
                                    .Take(pageSize)
                                    .ToListAsync();
        }

        public async Task<List<Model.Phone>> GetAllByPersonIdAsync(Guid personId)
        {
            return await _context.Phone
                .AsNoTracking()
                .Where(p => p.PersonId == personId)
                .ToListAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
