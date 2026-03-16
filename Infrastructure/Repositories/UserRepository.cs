using Domain.Entities;
using Domain.Interface;
using Infrastructure.DBconnect;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBconnect _dbconnect;
        public UserRepository(AppDBconnect dbconnect)
        {
            _dbconnect = dbconnect;
        }
        public async Task AddUserAsync(User entity)
        {
            await _dbconnect.Users.AddAsync(entity);
            await _dbconnect.SaveChangesAsync();
            
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
           return await _dbconnect.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await _dbconnect.Users.ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbconnect.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
           return await _dbconnect.Users.FindAsync(id);
        }

        public async Task<User> UpdateUserAsync(User entity)
        {
            var update = _dbconnect.Users.Update(entity);
            await _dbconnect.SaveChangesAsync();
            return entity;
        }
    }
}
