using FluentValidation;
using QApabilities.Shared.DTOs;

namespace QApabilities.Students.API.Validators;

public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
{
    public CreateStudentDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres")
            .Matches(@"^[a-zA-ZÀ-ÿ\s]+$").WithMessage("Nome deve conter apenas letras e espaços");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Email inválido")
            .MaximumLength(100).WithMessage("Email deve ter no máximo 100 caracteres");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("CPF é obrigatório")
            .Length(11).WithMessage("CPF deve ter exatamente 11 dígitos")
            .Matches(@"^\d{11}$").WithMessage("CPF deve conter apenas números")
            .Must(BeValidCpf).WithMessage("CPF inválido");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Data de nascimento é obrigatória")
            .LessThan(DateTime.UtcNow).WithMessage("Data de nascimento não pode ser no futuro")
            .Must(BeValidAge).WithMessage("Aluno deve ter pelo menos 16 anos");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Telefone é obrigatório")
            .MaximumLength(15).WithMessage("Telefone deve ter no máximo 15 caracteres")
            .Matches(@"^[\d\s\-\(\)\+]+$").WithMessage("Telefone deve conter apenas números, espaços, hífens e parênteses");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Endereço é obrigatório")
            .MaximumLength(200).WithMessage("Endereço deve ter no máximo 200 caracteres");
    }

    private static bool BeValidCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
            return false;

        // Verificar se todos os dígitos são iguais
        if (cpf.All(c => c == cpf[0]))
            return false;

        // Validar CPF usando algoritmo oficial
        var sum = 0;
        for (int i = 0; i < 9; i++)
            sum += int.Parse(cpf[i].ToString()) * (10 - i);

        var remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;

        if (int.Parse(cpf[9].ToString()) != digit1)
            return false;

        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += int.Parse(cpf[i].ToString()) * (11 - i);

        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;

        return int.Parse(cpf[10].ToString()) == digit2;
    }

    private static bool BeValidAge(DateTime birthDate)
    {
        var minimumAge = DateTime.UtcNow.AddYears(-16);
        return birthDate <= minimumAge;
    }
} 