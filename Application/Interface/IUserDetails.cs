using System;
using System.Collections.Generic;
using System.Text;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interface
{
	public interface IUserDetails
	{
		public Task<UserDetailsDto> GetUserAsync(int userId);
	}
}
