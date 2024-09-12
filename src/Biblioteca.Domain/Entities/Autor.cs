using FluentValidation;
using FluentValidation.Results;

namespace Biblioteca.Domain.Entities
{
    public class Autor : EntityBase
    {
        public int Id { get; set; }
        public string Nome { get; private set; }
        public DateTime? DataNascimento { get; private set; }

        public List<Livro> Livros { get; set; }

        public Autor(string nome, DateTime? dataNascimento)
        { 
            Nome = nome;
            DataNascimento = dataNascimento;
        }

        public void Atualizar(string nome, DateTime? dataNascimento)
        {
            Nome = nome;
            DataNascimento = dataNascimento;
        }

        public ValidationResult Validar()
        {
            return new AutorValidator().Validate(this);
        }
    }

    public class AutorValidator : AbstractValidator<Autor>
    {
        public AutorValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("O Campo Nome é obrigatório")
                .MaximumLength(500)
                .WithMessage("O Campo Nome deve ter no máximo 500 caracteres");
        }
    }
}
