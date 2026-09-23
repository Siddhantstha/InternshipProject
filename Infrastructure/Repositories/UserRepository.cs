using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using Domain.Entities;
using Domain.Interface;
using Infrastructure.DapperConnects;
using Infrastructure.DBconnect;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBconnect _dbconnect;
        private readonly DapperContext _dapperContext;
		public UserRepository(AppDBconnect dbconnect,DapperContext dapperContext)
        {
            _dbconnect = dbconnect;
            _dapperContext = dapperContext;
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
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var sql= @"SELECT * FROM ""Users"" WHERE ""Email"" = @Email
			AND ""isDeleted"" = false";

			using var connection = _dapperContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
		}
	}
}
