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
using static Application.DTOs.UserDTOs;

namespace UserProject.UnitTest
{
    public class UserTest
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IValidator<UpdateUserDto>> _updateValidatorMock;
        private readonly Mock<IValidator<LoginUserDto>> _loginValidatorMock;

        private readonly UserService _service;

        public UserTest()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _updateValidatorMock = new Mock<IValidator<UpdateUserDto>>();
            _loginValidatorMock = new Mock<IValidator<LoginUserDto>>();

            _service = new UserService(
                _userRepoMock.Object,
                _passwordServiceMock.Object,
                _tokenServiceMock.Object,
                _updateValidatorMock.Object,
                _loginValidatorMock.Object
            );
        }

        [Fact]
        public async Task GetAllUserAsync_ShouldReturnUsers()
        {
            // Arrange
            var users = new List<Domain.Entities.User>
         {
            new Domain.Entities.User
            {
            Id = 1,
            Name = "Ram",
            Email = "ram@test.com",
            Role = "Customer",
            Phone = "123",
            Address = "Kathmandu",
            isActive = true
            }
         };

            _userRepoMock.Setup(x => x.GetAllUserAsync())
                         .ReturnsAsync(users);

            // Act
            var result = await _service.GetAllUserAsync();

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(1);
            result.First().Name.Should().Be("Ram");
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var dto = new LoginUserDto
            {
                Email = "user@test.com",
                Password = "1234"
            };

            var user = new Domain.Entities.User
            {
                Id = 1,
                Email = "user@test.com",
                Password = "hashed",
                Role = "Customer",
                isActive = true
            };

            // validator success
            _loginValidatorMock
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            // repo returns user
            _userRepoMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            // password correct
            _passwordServiceMock
                .Setup(x => x.Verify(dto.Password, user.Password))
                .Returns(true);

            // token generate
            _tokenServiceMock
                .Setup(x => x.TokenGenerate(user))
                .Returns("fake_token");

            // Act
            var result = await _service.LoginAsync(dto);

            // Assert
            result.AccessToken.Should().Be("fake_token");
            result.message.Should().Be("Login Sucessfull");
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnError_WhenEmailNotFound()
        {
            var dto = new LoginUserDto
            {
                Email = "wrong@test.com",
                Password = "123"
            };

            _loginValidatorMock
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _userRepoMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync((Domain.Entities.User)null);

            var result = await _service.LoginAsync(dto);

            result.message.Should().Be("Sorry the Email is invalid or doesn't match");
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUserSuccessfully()
        {
            var dto = new UpdateUserDto
            {
                Id = 1,
                Name = "Hari",
                Email = "hari@test.com",
                Password = "123",
                Phone = "999",
                Address = "Pokhara",
                IsActive = true
            };

            var user = new Domain.Entities.User
            {
                Id = 1,
                Name = "Old",
                Email = "old@test.com",
                Password = "old",
                isActive = true
            };

            _updateValidatorMock
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _userRepoMock
                .Setup(x => x.GetUserByIdAsync(dto.Id))
                .ReturnsAsync(user);

            _passwordServiceMock
                .Setup(x => x.Hash(dto.Password))
                .Returns("hashed");

            var result = await _service.UpdateUserAsync(1, dto);

            result.Data.Should().BeTrue();
            result.Message.Should().Be("User Update Success");
        }
    }
}
