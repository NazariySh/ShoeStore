using FluentValidation;
using ShoeStore.Application.DTOs.Auth;
using ShoeStore.Domain.Repositories;

namespace ShoeStore.Application.Validators.Auth.Register;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public const int MaxFirstNameLength = 50;
    public const int MaxLastNameLength = 50;
    public const int MaxEmailLength = 50;
    public const int MaxPhoneNumberLength = 25;

    private readonly IUnitOfWork _unitOfWork;

    public RegisterDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(MaxFirstNameLength).WithMessage($"First name must not exceed {MaxFirstNameLength} characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(MaxLastNameLength).WithMessage($"Last name must not exceed {MaxLastNameLength} characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(MaxEmailLength).WithMessage($"Email must not exceed {MaxEmailLength} characters.")
            .MustAsync(BeUniqueEmail).WithMessage("Email already taken.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(MaxPhoneNumberLength).WithMessage($"Phone number must not exceed {MaxPhoneNumberLength} characters.")
            .Matches(@"^\+?\d{7,25}$").WithMessage("Invalid phone number format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W]").WithMessage("Password must contain at least one special character.");

        _unitOfWork = unitOfWork;
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Users.IsEmailUniqueAsync(
            email,
            cancellationToken);
    }
}