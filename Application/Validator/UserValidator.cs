using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static Application.DTOs.UserDTOs;

namespace Application.Validator
{
    public class UserValidator : AbstractValidator<AddUserDTO>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Please enter your name")
                .MaximumLength(50).MinimumLength(5);

            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email format is Invalid")
                .NotNull();

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be greater than 6 letters")
                .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain a lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain a number");
                

            RuleFor(x => x.Role)
              .NotEmpty().WithMessage("Role is required")
              .Matches("^[a-zA-Z]+$").WithMessage("Role name must be alphabetic")
              .Must(role => role == "Admin" || role == "Customer")
              .WithMessage("Role must be Admin or Customer");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required")
                .MaximumLength(10).MinimumLength(10);

            RuleFor(x => x.Address)
                .NotEmpty();

        }
    }

    public class UserUpdateValidator : AbstractValidator<UpdateUserDto>
    {
        public UserUpdateValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Please enter your name")
                .MaximumLength(50).MinimumLength(5);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be greater than 6 letters")
                .MinimumLength(6).WithMessage("Password must be greater than 6 letters")
                .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain a lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain a number");

            RuleFor(x => x.Phone)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Phone number is required")
                .Must(x => !string.IsNullOrEmpty(x))
                .WithMessage("Invalid phone number format")
                .Length(10).WithMessage("Invalid phone number");

            RuleFor(x => x.IsActive)
                .NotNull();

        }
    }

    public class LoginUserValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be greater than 6 letters")
                .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain a lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain a number");
                
        }
    }

    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Please enter your name")
               .MaximumLength(50).MinimumLength(5);

            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email format is Invalid")
                .NotNull();

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be greater than 6 letters")
                .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain a lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain a number");
                

            RuleFor(x => x.Phone)
                .MaximumLength(10).MinimumLength(10).WithMessage("Phone number must contatin 10 numbers")
                 .When(x => !string.IsNullOrEmpty(x.Phone))
                .WithMessage("Invalid phone number format");

            RuleFor(x => x.Address)
                .NotEmpty();

        }
    }

    public class UserStatusValidator : AbstractValidator<UserStatusDtos>
    {
        public UserStatusValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.isActive)
                .NotEmpty().WithMessage("Status Cannot be empty");

            RuleFor(x => x.isLocked)
                .NotEmpty().WithMessage("Status Cannot be empty");
        }
    }

    public class UserRoleValidator : AbstractValidator<UserRoleDto>
    {
        public UserRoleValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Id is Required");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role name is required")
                .Matches("^[a-zA-Z]+$").WithMessage("Role name must be alphabetic")
                .MaximumLength(20).WithMessage("Role name cannot exceed 20 characters");
        }
    }

    public class DeleteUserValidator : AbstractValidator<DeleteUserDto>
    {
        public DeleteUserValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is Required");

            RuleFor(x=>x.AdminId)
                .NotEmpty().WithMessage("AdminId is Required");

        }
    }
}