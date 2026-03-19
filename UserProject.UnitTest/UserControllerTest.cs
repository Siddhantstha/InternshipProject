using Application.DTOs;
using Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Users.Api.Controllers;
using Xunit;
using static Application.DTOs.UserDTOs;

namespace UserProject.UnitTest
{
    public class UserControllerTest
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IRegisterService> _registerServiceMock;

        private readonly UserController _controller;

        public UserControllerTest()
        {
            _userServiceMock = new Mock<IUserService>();
            _registerServiceMock = new Mock<IRegisterService>();

            _controller = new UserController(
                _userServiceMock.Object,
                _registerServiceMock.Object
            );
        }
        [Fact]
        public async Task Register_ReturnsOk()
        {
            var dto = new RegisterUserDto();

            _registerServiceMock
            .Setup(x => x.RegisterAsync(dto))
            .ReturnsAsync(new ExecuteResult<bool>
             {
                 Data = true,
                 Message = "Success"
             });
            var result = await _controller.Register(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsOk()
        {
            var dto = new LoginUserDto();

            _userServiceMock
                .Setup(x => x.LoginAsync(dto))
                .ReturnsAsync(new LoginResponseDto());

            var result = await _controller.Login(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task View_ReturnsOk()
        {
            _userServiceMock
                .Setup(x => x.GetAllUserAsync())
                .ReturnsAsync(new List<ViewUserDto>());

            var result = await _controller.View();

            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        private void SetUser(string userId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId)
            }));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenUserMatches()
        {
            SetUser("1");

            var dto = new UpdateUserDto { Id = 1 };

            _userServiceMock
                .Setup(x => x.UpdateUserAsync(1, dto))
                .ReturnsAsync(new ExecuteResult<bool> { Data = true });

            var result = await _controller.Update(1, dto);

            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenDifferentUser()
        {
            SetUser("2");

            var dto = new UpdateUserDto { Id = 1 };

            var result = await _controller.Update(1, dto);

            var badResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(400, badResult.StatusCode);
        }
    }
}
