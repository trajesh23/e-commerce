using E_Commerce.DataAccess.Context;
using E_Commerce.DataAccess.Respositories.Interfaces;
using E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DataAccess.Respositories
{
    public class SettingRepository : ISettingRepository
    {
        private readonly EcommerceContext _context;

        public SettingRepository(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<Setting> GetByIdAsync(int id)
        {
            return await _context.Settings.FirstOrDefaultAsync(o => o.Id == id)!;
        }

        public async Task UpdateByIdAsync(int id)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(o => o.Id == id);

            _context.Settings.Update(setting!);
            _context.SaveChanges();
        }
    }
}
