using BC7.Business.Models;
using FluentValidation;

namespace BC7.Business.Validators
{
    public class CreateMultiAccountModelValidator : AbstractValidator<CreateMultiAccountModel>
    {
        public CreateMultiAccountModelValidator()
        {
            RuleFor(x => x.Reflink)
                .NotNull()
                .NotEmpty();
        }
    }
}
