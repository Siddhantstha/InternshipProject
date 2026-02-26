using Application.DTOs;
using Application.Interface;
using Application.Validator;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Application.DTOs.UserDTOs;

namespace Users.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        
        private readonly IUserService _userService;
        private readonly IRegisterService _registerService;
        public UserController(IUserService service, IRegisterService registerService)
        {
            _userService = service;
            _registerService = registerService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            var result =  await _registerService.RegisterAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto entity)
        {
           var result = await _userService.LoginAsync(entity);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> View()
        {
            var view = await _userService.GetAllUserAsync();
            return Ok(view);
        }
 
        [HttpPut("{userId}")]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> Update(int userId, UpdateUserDto entity)
        {
            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(currentUser != userId.ToString())
            {
                return BadRequest("You are not allowed to update another user's profile");
            }

           var result = await _userService.UpdateUserAsync(userId ,entity);
            return Ok(result);
        }
    }
}
