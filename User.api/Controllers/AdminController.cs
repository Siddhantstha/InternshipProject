using Application.DTOs;
using Application.Interface;
using Application.Validator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using static Application.DTOs.UserDTOs;

namespace User.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =Roles.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationActionFilterAttribute))]
        public async Task<ExecuteResult<bool>> Create(AddUserDTO entity)
        {
            return await _adminService.AddUserAsync(entity);
        }

        [HttpPut("RoleUpdate")]
        public async Task<ExecuteResult<bool>> ChangeRole(UserRoleDto dto)
        {
            return await _adminService.ChangeUserRoleAsync(dto);
        }

        [HttpPut("{adminId}")]
        public async Task<ExecuteResult<bool>> ChangeStatus (int adminId , UserStatusDtos dto)
        {
            return await _adminService.ChangeUserStatusAsync(adminId, dto);
        }

        [HttpDelete("DeleteUser")]
        public async Task<ExecuteResult<bool>> DeleteUser (DeleteUserDto dto) 
        {
            return await _adminService.DeleteUserAsync(dto);
        }
    }
}
