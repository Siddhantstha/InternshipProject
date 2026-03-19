using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task AddUserAsync(User entity);
        Task <User> UpdateUserAsync(User entity);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
		Task<User?> GetCustomerByIdAsync(int customerId);

	}
}
