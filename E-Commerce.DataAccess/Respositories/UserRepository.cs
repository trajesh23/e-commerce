using AutoMapper;
using E_Commerce.DataAccess.Context;
using E_Commerce.DataAccess.Respositories.Interfaces;
using E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DataAccess.Respositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EcommerceContext _context;

        public UserRepository(EcommerceContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(User entity)
        {
            // Throw null if entity is null
            ArgumentNullException.ThrowIfNull(entity);

            // Password hashing
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(entity.Password);

            // Assign hashed password to new user
            entity.Password = hashedPassword;

            // Add to the database
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            // Find the user to be deleted
            var user = await _context.Users.FindAsync(id);

            // If not found, throw and exception
            ArgumentNullException.ThrowIfNull(user);

            // Remove user
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync(); // Get all users in a list
        }

        public async Task<User> GetByIdAsync(int id)
        {
            // Find requested user
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            // Throw an exception if not found
            ArgumentNullException.ThrowIfNull(user);

            return user;
        }

        public async Task UpdateAsync(User entity)
        {
            // Throw an exception if not found
            ArgumentNullException.ThrowIfNull(entity);

            // Update user
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
