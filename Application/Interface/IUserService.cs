using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static Application.DTOs.UserDTOs;

namespace Application.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<ViewUserDto>> GetAllUserAsync();
        Task<ExecuteResult<bool>> UpdateUserAsync(int userId,UpdateUserDto entity);
        Task<LoginResponseDto> LoginAsync(LoginUserDto entity);
        //Task<ExecuteResult<bool>> ActivateUserAysnc(int id);
        //Task<ExecuteResult<bool>> LockedUserAysnc(int id);
    }
}
