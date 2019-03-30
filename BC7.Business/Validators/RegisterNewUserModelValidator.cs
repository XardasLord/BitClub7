using BC7.Business.Models;
using FluentValidation;

namespace BC7.Business.Validators
{
    public class RegisterNewUserModelValidator : AbstractValidator<RegisterNewUserModel>
    {
        public RegisterNewUserModelValidator()
        {
            RuleFor(x => x.Login)
                .NotNull()
                .MinimumLength(8);

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotNull()
                .MinimumLength(8);

            RuleFor(x => x.FirstName)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.LastName)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Street)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.City)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.ZipCode)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Country)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.BtcWalletAddress)
                .NotEmpty()
                .NotNull();
        }
    }
}
