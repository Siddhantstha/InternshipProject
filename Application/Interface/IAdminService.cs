using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using static Application.DTOs.UserDTOs;

namespace Application.Interface
{
    public interface IAdminService
    {
        Task<ExecuteResult<bool>> AddUserAsync(AddUserDTO entity);
        Task<ExecuteResult<bool>> ChangeUserRoleAsync(UserRoleDto dto);
        Task<ExecuteResult<bool>> ChangeUserStatusAsync(int adminId, UserStatusDtos entity);
        Task<ExecuteResult<bool>> DeleteUserAsync(DeleteUserDto dto);
    }
}
