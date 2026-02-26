using Application.DTOs;
using Application.Interface;
using Domain.Entities;
using Domain.Interface;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Service
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IValidator<RegisterUserDto> _validator;
        public RegisterService(IUserRepository userRepository , IPasswordService passwordService , IValidator<RegisterUserDto> validator)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _validator = validator;
        }
        public async Task<ExecuteResult<bool>> RegisterAsync(RegisterUserDto dto)
        {
            var result = await _validator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = result.Errors.First().ToString(),
                    Status = ResponseStatus.BadRequest
                };
            }
            
            if (await _userRepository.EmailExistsAsync(dto.Email))
            {
                return new ExecuteResult<bool>
                {
                    Data = false,
                    Message = "Email Already Exists",
                    Status = ResponseStatus.BadRequest
                };
            }
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = _passwordService.Hash(dto.Password),
                Phone = dto.Phone,
                Address = dto.Address,
                Role = Roles.Customer,
                isActive = true,
                isLocked = false
            };
            await _userRepository.AddUserAsync(user);
            return new ExecuteResult<bool>
            {
                Data = true,
                Message = "User Register Success",
                Status = ResponseStatus.Ok
            };
        }
    }
}
