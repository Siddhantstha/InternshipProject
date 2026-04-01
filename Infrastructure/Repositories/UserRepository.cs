using Dapper;
using Domain.Entities;
using Domain.Interface;
using Infrastructure.DBconnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using static Application.DTOs.UserDTOs;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBconnect _dbconnect;
        private readonly string _connectionString;

        public UserRepository(AppDBconnect dbconnect, IConfiguration configuration)
        {
            _dbconnect = dbconnect;
            _connectionString = configuration.GetConnectionString("DefaultConnection"); 
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
		public async Task<User?> GetCustomerByIdAsync(int customerId)
		{
			return await _dbconnect.Users.FirstOrDefaultAsync(x => x.Id == customerId && x.Role == "Customer");
		}

        public async Task<IEnumerable<User>> GetAllUserNameAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            string sql = @"SELECT name,email FROM ""Users""";
            var result = await connection.QueryAsync<User>(sql);
            return result;
        }
    }
}