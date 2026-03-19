using Application.DTOs;
using Application.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using User.api.Controllers;
using Xunit;

namespace UserProject.UnitTest
{
    public class AdminControllerTests
    {
        private readonly Mock<IAdminService> _adminServiceMock;
        private readonly AdminController _controller;

        public AdminControllerTests()
        {
            _adminServiceMock = new Mock<IAdminService>();

            _controller = new AdminController(
                _adminServiceMock.Object
            );
        }

        [Fact]
        public async Task Create_ReturnsSuccess()
        {
            var dto = new AddUserDTO();

            _adminServiceMock
                .Setup(x => x.AddUserAsync(dto))
                .ReturnsAsync(new ExecuteResult<bool>
                {
                    Data = true,
                    Message = "Success"
                });

            var result = await _controller.Create(dto);

            Assert.True(result.Data);
        }

        [Fact]
        public async Task ChangeRole_ReturnsSuccess()
        {
            var dto = new UserRoleDto();

            _adminServiceMock
                .Setup(x => x.ChangeUserRoleAsync(dto))
                .ReturnsAsync(new ExecuteResult<bool>
                {
                    Data = true
                });

            var result = await _controller.ChangeRole(dto);

            Assert.True(result.Data);
        }

        [Fact]
        public async Task ChangeStatus_ReturnsSuccess()
        {
            var dto = new UserStatusDtos();

            _adminServiceMock
                .Setup(x => x.ChangeUserStatusAsync(1, dto))
                .ReturnsAsync(new ExecuteResult<bool>
                {
                    Data = true
                });

            var result = await _controller.ChangeStatus(1, dto);

            Assert.True(result.Data);
        }

        [Fact]
        public async Task DeleteUser_ReturnsSuccess()
        {
            var dto = new DeleteUserDto();

            _adminServiceMock
                .Setup(x => x.DeleteUserAsync(dto))
                .ReturnsAsync(new ExecuteResult<bool>
                {
                    Data = true,
                    Message = "Delete Success"
                });

            var result = await _controller.DeleteUser(dto);

            Assert.True(result.Data);
        }
    }
}
