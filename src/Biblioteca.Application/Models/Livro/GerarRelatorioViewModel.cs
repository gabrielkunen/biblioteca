using Biblioteca.Domain.Enums;
using FluentValidation;
using FluentValidation.Results;

namespace Biblioteca.Application.Models.Livro;

public class GerarRelatorioLivroViewModel
{
    public ETipoConteudo? TipoConteudo { get; set; }
    
    public ValidationResult Validar()
    {
        return new GerarRelatorioLivroViewModelValidator().Validate(this);
    }
}

public class GerarRelatorioLivroViewModelValidator : AbstractValidator<GerarRelatorioLivroViewModel>
{
    public GerarRelatorioLivroViewModelValidator()
    {
        RuleFor(x => x.TipoConteudo)
            .NotEmpty()
            .WithMessage("O Campo TipoConteudo é obrigatório");
    }
}