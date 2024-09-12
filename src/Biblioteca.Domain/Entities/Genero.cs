using FluentValidation;
using FluentValidation.Results;

namespace Biblioteca.Domain.Entities
{
    public class Genero(string nome) : EntityBase
    {
        public int Id { get; private set; }
        public string Nome { get; private set; } = nome;

        public List<Livro> Livros { get; set; }

        public void Atualizar(string nome)
        {
            Nome = nome;
        }
        public ValidationResult Validar()
        {
            return new GeneroValidator().Validate(this);
        }
    }

    public class GeneroValidator : AbstractValidator<Genero>
    {
        public GeneroValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("O Campo Nome é obrigatório")
                .MaximumLength(250)
                .WithMessage("O Campo Nome deve ter no máximo 250 caracteres");
        }
    }
}
