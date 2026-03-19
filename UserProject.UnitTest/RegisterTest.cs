using Application.DTOs;
using Application.Interface;
using Application.Service;
using Application.Validator;
using Domain.Entities;
using Domain.Interface;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace UserProject.UnitTest
{
    public class RegisterTest
    {
        private readonly Mock<IValidator<RegisterUserDto>> _validatorMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;

        private readonly RegisterService _service;

        public RegisterTest()
        {
            _validatorMock = new Mock<IValidator<RegisterUserDto>>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordServiceMock = new Mock<IPasswordService>();

            _service = new RegisterService(
                _userRepositoryMock.Object,
                _passwordServiceMock.Object,
                _validatorMock.Object
            );
        }
        [Fact]
        public async Task testregister_whenvalidationfails_ReturnBadRequest()
        {
            // Arrange (prepare fake behavior)

            var dto = new RegisterUserDto();

            var failures = new[]
            {
                new ValidationFailure("Email", "Invalid Email")
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult(failures));

            // Act (call method)

            var result = await _service.RegisterAsync(dto);

            // Assert (check result)

            result.Data.Should().BeFalse();
            result.Status.Should().Be(ResponseStatus.BadRequest);
        }

        [Fact]
        public async Task registerEmail_WhenEmailExists_ReturnBadRequest()
        {
            //Arrang
            var email = new RegisterUserDto
            {
                Email = "test123@gmail.com"
            };
            _validatorMock
                .Setup(x => x.ValidateAsync(email, default))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock
                .Setup(x => x.EmailExistsAsync(email.Email))
                .ReturnsAsync(true);

            //Act
            var result = await _service.RegisterAsync(email);

            //Assert
            result.Data.Should().BeFalse();
            result.Message.Should().Be("Email Already Exists");
        }

        [Fact]
        public async Task RegisterTest_WhenEverythingIsValid_ReturnsSuccess()
        {
            var dto = new RegisterUserDto
            {
                Name = "John",
                Email = "john@test.com",
                Password = "123",
                Phone = "111111",
                Address = "Nepal"
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock
                .Setup(x => x.EmailExistsAsync(dto.Email))
                .ReturnsAsync(false);

            _passwordServiceMock
                .Setup(x => x.Hash(dto.Password))
                .Returns("hashed");

            var result = await _service.RegisterAsync(dto);

            result.Data.Should().BeTrue();
            result.Status.Should().Be(ResponseStatus.Ok);

            _userRepositoryMock.Verify(
                x => x.AddUserAsync(It.IsAny<Domain.Entities.User>()),
                Times.Once
            );
        }
    }
}
