using Application.DTOs;
using Application.Interface;
using Domain.Entities;
using Domain.Interface;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using static Application.DTOs.UserDTOs;

namespace Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userrepo;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IValidator<UpdateUserDto> _validator;
        private readonly IValidator<LoginUserDto> _loginValidator;
        public UserService(IUserRepository userrepo, IPasswordService passwordService, ITokenService tokenService, IValidator<UpdateUserDto> validator, IValidator<LoginUserDto> loginValidator)
        {
            _userrepo = userrepo;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _validator = validator;
            _loginValidator = loginValidator;
        }

        public async Task<IEnumerable<ViewUserDto>> GetAllUserAsync()
        {
           var all = await _userrepo.GetAllUserAsync();
            var result = all.Select(s => new ViewUserDto
            {
                Id=s.Id,
                Name=s.Name,
                Email=s.Email,
                Role = s.Role,
                Phone=s.Phone,
                Address=s.Address,
                IsDeleted = s.isDeleted,
                IsActive = s.isActive,
                IsLocked = s.isLocked
            }).ToList();
            return result;
        }
        public async Task<LoginResponseDto> LoginAsync(LoginUserDto entity)
        {
            var result = await _loginValidator.ValidateAsync(entity);

            if (!result.IsValid)
            {
                return new LoginResponseDto
                {
                    message = result.Errors.First().ToString(),
                };
            }
            var user= await _userrepo.GetByEmailAsync(entity.Email);
            if(user == null)
            {
                return new LoginResponseDto
                {
                    message = "Sorry the Email is invalid or doesn't match"
                };
            }
            var isPasswordValid= _passwordService.Verify(entity.Password,user.Password);
            if (!isPasswordValid)
            {
                return new LoginResponseDto
                {
                    message = "Sorry The password doesn't match"
                };
            }
            if(user.isDeleted || user.isLocked)
            {
                return new LoginResponseDto
                {
                    message = "Sorry the deleted or locked user cannot login"
                };
            }
            if(!user.isActive)
            {
                return new LoginResponseDto
                {
                    message = "The user is currently not active"
                };
            }

            var token = _tokenService.TokenGenerate(user);

            return new LoginResponseDto
            {
                message = "Login Sucessfull",
                AccessToken = token,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<ExecuteResult<bool>> UpdateUserAsync(int userId, UpdateUserDto entity)
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
            if(userId != entity.Id)
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = "Sorry you are not allowed to update ",
                    Status = ResponseStatus.BadRequest
                };
            }
            var update = await _userrepo.GetUserByIdAsync(entity.Id);
            if(update == null)
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = "User not Found",
                    Status = ResponseStatus.BadRequest
                };
            }
            if(update.isDeleted || update.isLocked)
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = "Deleted or Locked user cannot be updated",
                    Status = ResponseStatus.BadRequest
                };
            }
                update.Name = entity.Name;
                update.Email = entity.Email;
                update.Password = _passwordService.Hash(entity.Password);
                update.Phone = entity.Phone;
                update.Address = entity.Address;
            if (!entity.IsActive)
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = "Account is Deactivated",
                    Status = ResponseStatus.BadRequest
                };
            }
            update.isActive = entity.IsActive;
            await _userrepo.UpdateUserAsync(update);
            return new ExecuteResult<bool>
            {
                Data = true,
                Message = "User Update Success",
                Status = ResponseStatus.Ok
            };

        }
    }
}
