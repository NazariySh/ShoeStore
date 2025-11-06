using FluentValidation;
using ShoeStore.Application.DTOs.Accounts;

namespace ShoeStore.Application.Validators.Accounts;

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public const int MaxLineLength = 100;
    public const int MaxCityLength = 50;
    public const int MaxStateLength = 50;
    public const int MaxCountryLength = 50;
    public const int MaxPostalCodeLength = 10;

    public AddressDtoValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(MaxLineLength).WithMessage($"Street must not exceed {MaxLineLength} characters.");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State is required.")
            .MaximumLength(MaxStateLength).WithMessage($"State must not exceed {MaxStateLength} characters.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(MaxCityLength).WithMessage($"City must not exceed {MaxCityLength} characters.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(MaxCountryLength).WithMessage($"Country must not exceed {MaxCountryLength} characters.");

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("Postal code is required.")
            .MaximumLength(MaxPostalCodeLength).WithMessage($"Postal code must not exceed {MaxPostalCodeLength} characters.");
    }
}