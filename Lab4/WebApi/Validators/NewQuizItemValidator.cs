using ApplicationCore.Models.QuizAggregate;
using FluentValidation;
using WebApi.Dto;

namespace WebApi.Validators;

public class NewQuizItemValidator : AbstractValidator<NewQuizItemDto>
{
    public NewQuizItemValidator()
    {
        RuleFor(q => q.Question)
                .NotEmpty().WithMessage("Pytanie nie może być puste.")
                .MaximumLength(200).WithMessage("Pytanie nie może być dłuższe niż 200 znaków.")
                .MinimumLength(3).WithMessage("Pytanie nie może być krótsze od 3 znaków!");

        RuleFor(q => q.Options)
            .NotEmpty().WithMessage("Opcje odpowiedzi nie mogą być puste.")
            .Must(options => options != null && options.Any()).WithMessage("Opcje odpowiedzi nie mogą być puste.")
            .When(q => q.Options != null);

        RuleFor(q => q.CorrectOptionIndex)
            .Must((dto, correctIndex) => dto.Options != null && dto.Options.Count() > correctIndex)
            .WithMessage("Indeks poprawnej odpowiedzi wykracza poza zakres dostępnych opcji.");

        RuleFor(q => q.Options)
            .Must((dto, options) => dto.CorrectOptionIndex >= 0 && dto.CorrectOptionIndex < options.Count())
            .WithMessage("Indeks poprawnej odpowiedzi wykracza poza zakres dostępnych opcji.");
    }
}
