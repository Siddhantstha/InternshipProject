using Application.DTOs;
using Application.Interface;
using Application.Service;
using Domain.Entities;
using Domain.Interface;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UserProject.UnitTest
{
    public class AdminTest
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;

        private readonly Mock<IValidator<AddUserDTO>> _addValidatorMock;
        private readonly Mock<IValidator<UserStatusDtos>> _statusValidatorMock;
        private readonly Mock<IValidator<UserRoleDto>> _roleValidatorMock;
        private readonly Mock<IValidator<DeleteUserDto>> _deleteValidatorMock;

        private readonly AdminService _service;

        public AdminTest()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _passwordServiceMock = new Mock<IPasswordService>();

            _addValidatorMock = new Mock<IValidator<AddUserDTO>>();
            _statusValidatorMock = new Mock<IValidator<UserStatusDtos>>();
            _roleValidatorMock = new Mock<IValidator<UserRoleDto>>();
            _deleteValidatorMock = new Mock<IValidator<DeleteUserDto>>();

            _service = new AdminService(
                _userRepoMock.Object,
                _passwordServiceMock.Object,
                _addValidatorMock.Object,
                _statusValidatorMock.Object,
                _roleValidatorMock.Object,
                _deleteValidatorMock.Object
            );
        }

        [Fact]
        public async Task AddUserAsync_WhenValidationFails_ReturnsBadRequest()
        {
            //Arrange
            var dto = new AddUserDTO();

            var failures = new[]
            {
             new ValidationFailure("Email","Invalid Email")
             };

            _addValidatorMock
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult(failures));
            //Act
            var result = await _service.AddUserAsync(dto);

            //Assert
            result.Data.Should().BeFalse();
            result.Status.Should().Be(ResponseStatus.BadRequest);

            _userRepoMock.Verify(
                x => x.AddUserAsync(It.IsAny<Domain.Entities.User>()),
                Times.Never
            );
        }

        [Fact]
        public async Task AddUserAsync_WhenValid_AddsUser()
        {
            var dto = new AddUserDTO
            {
                Name = "John",
                Email = "john@test.com",
                Password = "123",
                Role = Roles.Customer
            };

            _addValidatorMock
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _passwordServiceMock
                .Setup(x => x.Hash(dto.Password))
                .Returns("hashed");

            var result = await _service.AddUserAsync(dto);

            result.Data.Should().BeTrue();
            result.Status.Should().Be(ResponseStatus.Ok);

            _userRepoMock.Verify(
                x => x.AddUserAsync(It.IsAny<Domain.Entities.User>()),
                Times.Once
            );
        }

        [Fact]
        public async Task ChangeUserRoleAsync_WhenUserNotFound_ReturnsBadRequest()
        {
            var dto = new UserRoleDto
            {
                UserId = 1,
                Role = Roles.Admin
            };

            _roleValidatorMock
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _userRepoMock
                .Setup(x => x.GetUserByIdAsync(dto.UserId))
                .ReturnsAsync((Domain.Entities.User)null);

            var result = await _service.ChangeUserRoleAsync(dto);

            result.Data.Should().BeFalse();
            result.Message.Should().Be("User not Found");
        }

        [Fact]
        public async Task ChangeUserStatusAsync_WhenUserIsNotAdmin_ReturnsBadRequest()
        {
            var dto = new UserStatusDtos
            {
                UserId = 2,
                isActive = true,
                isLocked = false
            };

            var normalUser = new Domain.Entities.User
            {
                Id = 1,
                Role = "Customer"
            };

            _statusValidatorMock
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _userRepoMock
                .Setup(x => x.GetUserByIdAsync(1))
                .ReturnsAsync(normalUser);

            var result = await _service.ChangeUserStatusAsync(1, dto);

            result.Data.Should().BeFalse();
            result.Message.Should().Be("Only Admin is Authorized");
        }

        [Fact]
        public async Task DeleteUserAsync_WhenAdminDeletesSelf_ReturnsBadRequest()
        {
            var dto = new DeleteUserDto
            {
                AdminId = 1,
                UserId = 1
            };

            _deleteValidatorMock
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            var result = await _service.DeleteUserAsync(dto);

            result.Data.Should().BeFalse();
            result.Message.Should().Be("Admin Cannot delete themself");
        }
    }
}
