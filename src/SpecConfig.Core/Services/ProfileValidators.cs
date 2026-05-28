using FluentValidation;
using SpecConfig.Core.Models;

namespace SpecConfig.Core.Services
{
    public class ExportProfileValidator : AbstractValidator<ExportProfile>
    {
        public ExportProfileValidator()
        {
            RuleFor(x => x.Tables).NotNull().WithMessage("Таблицы не могут быть null");
        }
    }

    public class SpecifierProfileValidator : AbstractValidator<SpecifierProfile>
    {
        public SpecifierProfileValidator()
        {
            RuleFor(x => x.ProfileName).NotEmpty().WithMessage("Имя профиля обязательно");
        }
    }
}
