using Application.DTOs;
using Application.Interface;
using Domain.Entities;
using Domain.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Text;
using static Application.DTOs.UserDTOs;

namespace Application.Service
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userrepo;
        private readonly IPasswordService _passwordService;
        
        private readonly IValidator<AddUserDTO> _validator;
        private readonly IValidator<UserStatusDtos> _statusValidator;
        private readonly IValidator<UserRoleDto> _roleValidator;
        private readonly IValidator<DeleteUserDto> _deleteValidator;
        public AdminService(IUserRepository userrepo, IPasswordService passwordService, IValidator<AddUserDTO> validator , IValidator<UserStatusDtos> statusValidator, IValidator<UserRoleDto> roleValidator, IValidator<DeleteUserDto> deleteValidator)
        {
            _userrepo = userrepo;
            _passwordService = passwordService;
            _validator = validator;
            _statusValidator = statusValidator;
            _roleValidator = roleValidator;
            _deleteValidator = deleteValidator;
        }
        public async Task<ExecuteResult<bool>> AddUserAsync(AddUserDTO entity)
        {
            var result = await _validator.ValidateAsync(entity);

            if (!result.IsValid)
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = result.Errors.First().ToString(),
                    Status = ResponseStatus.BadRequest
                };
            }
            var add = new User
            {
                Name = entity.Name,
                Email = entity.Email,
                Password = _passwordService.Hash(entity.Password),
                Role = entity.Role,
                Phone = entity.Phone,
                Address = entity.Address
            };
            await _userrepo.AddUserAsync(add);
            return new ExecuteResult<bool>
            {
                Data = true,
                Message = "Added Success by Admin",
                Status = ResponseStatus.Ok
            };
        }
        public async Task<ExecuteResult<bool>> ChangeUserRoleAsync(UserRoleDto dto)
        {
            var result = await _roleValidator.ValidateAsync(dto);

            if (!result.IsValid)
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = result.Errors.First().ToString(),
                    Status = ResponseStatus.BadRequest
                };
            }
            if (dto.Role != Roles.Admin && dto.Role != Roles.Customer)
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = "Invalid Role",
                    Status = ResponseStatus.BadRequest
                };
            }
            var user = await _userrepo.GetUserByIdAsync(dto.UserId);
            if (user == null)
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = "User not Found",
                    Status = ResponseStatus.BadRequest
                };
            }
            user.Role = dto.Role;
            await _userrepo.UpdateUserAsync(user);
            return new ExecuteResult<bool>
            {
                Data = true,
                Message = "User Status Changed Sucessfully",
                Status = ResponseStatus.Ok
            };
        }

        public async Task<ExecuteResult<bool>> ChangeUserStatusAsync(int adminId, UserStatusDtos entity)
        {
            var result = await _statusValidator.ValidateAsync(entity);

            if (!result.IsValid)
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = result.Errors.First().ToString(),
                    Status = ResponseStatus.BadRequest
                };
            }
            var admin = await _userrepo.GetUserByIdAsync(adminId);
            if (admin == null)
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = "User not Found",
                    Status = ResponseStatus.BadRequest
                };
            }
            if (admin.Role != "Admin")
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = "Only Admin is Authorized",
                    Status = ResponseStatus.BadRequest
                };
            }
            if(entity.isActive && entity.isLocked)
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = "Locked user cannot be active",
                    Status = ResponseStatus.BadRequest
                };
            }
            var user = await _userrepo.GetUserByIdAsync(entity.UserId);
            if (user == null)
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = "User not Found",
                    Status = ResponseStatus.BadRequest
                };
            }
            user.isActive = entity.isActive;
            user.isLocked = entity.isLocked;
            await _userrepo.UpdateUserAsync(user);
            return new ExecuteResult<bool>
            {
                Data = true,
                Message = "User Status Changed Sucessfully",
                Status = ResponseStatus.Ok
            };
        }

        public async Task<ExecuteResult<bool>> DeleteUserAsync(DeleteUserDto dto)
        {
            var response = await _deleteValidator.ValidateAsync(dto);

            if (!response.IsValid)
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = response.Errors.First().ToString(),
                    Status = ResponseStatus.BadRequest
                };
            }
            if (dto.UserId == dto.AdminId)
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = "Admin Cannot delete themself",
                    Status = ResponseStatus.BadRequest
                };
            }

            var result = await _userrepo.GetUserByIdAsync(dto.UserId);
            if (result == null || result.isDeleted)
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = "User not found",
                    Status = ResponseStatus.BadRequest
                };
            }
            result.isDeleted = true;
            result.isActive = false;
            result.isLocked = true;
            await _userrepo.UpdateUserAsync(result);
            return new ExecuteResult<bool>
            {
                Data = true,
                Message = "User Deleted sucessfully",
                Status = ResponseStatus.Ok
            };
        }
    }
}
