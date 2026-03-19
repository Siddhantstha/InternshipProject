using System;
using System.Collections.Generic;
using System.Text;
using Application.DTOs;
using Application.Interface;
using Domain.Interface;

namespace Application.Service
{
	public class UserDetailsService: IUserDetails
	{
		private readonly IUserRepository _userrepo;

		public UserDetailsService(IUserRepository userrepo)
		{
			_userrepo = userrepo;
		}
	public async Task<UserDetailsDto> GetUserAsync(int UserId)
		{
			var userId = await _userrepo.GetCustomerByIdAsync(UserId);
			if (userId == null)
			{
				return null;
			}
			var result = new UserDetailsDto
			{
		     	Id = userId.Id,
				Name = userId.Name,
				Email = userId.Email,
				Role = userId.Role,
				Phone = userId.Phone,
						
			};
			return result;
		}




	}
}
