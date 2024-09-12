using Biblioteca.Domain.Enums;
using FluentValidation;
using FluentValidation.Results;

namespace Biblioteca.Domain.Entities
{
    public class Livro(string titulo, string isbn, string codigo, int idAutor) : EntityBase
    {
        public int Id { get; set; }
        public string Titulo { get; private set; } = titulo;
        public string Isbn { get; private set; } = isbn;
        public string Codigo { get; private set; } = codigo;
        public int IdAutor { get; private set; } = idAutor;
        public EStatusLivro Status { get; private set; } = EStatusLivro.Disponivel;

        public List<Genero> Generos { get; set; }
        public List<Emprestimo> Emprestimos { get; set; }
        public Autor Autor { get; set; }

        public void AdicionarGeneros(List<Genero> generos)
        {
            Generos = generos;
        }

        public void Atualizar(string titulo, string isbn, string codigo, int idAutor)
        {
            Titulo = titulo;
            Isbn = isbn;
            Codigo = codigo;
            IdAutor = idAutor;
        }

        public bool Emprestado()
        {
            return Status == EStatusLivro.Emprestado;
        }

        public void Emprestar()
        {
            Status = EStatusLivro.Emprestado;
        }

        public void Devolver()
        {
            Status = EStatusLivro.Disponivel;
        }

        public ValidationResult Validar()
        {
            return new LivroValidator().Validate(this);
        }
    }

    public class LivroValidator : AbstractValidator<Livro>
    {
        public LivroValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("O Campo Titulo é obrigatório")
                .MaximumLength(500)
                .WithMessage("O Campo Titulo deve ter no máximo 500 caracteres");

            RuleFor(x => x.Isbn)
                .NotEmpty()
                .WithMessage("O Campo Isbn é obrigatório")
                .MaximumLength(13)
                .WithMessage("O Campo Isbn deve ter no máximo 13 caracteres");

            RuleFor(x => x.Codigo)
                .NotEmpty()
                .WithMessage("O Campo Codigo é obrigatório")
                .MaximumLength(30)
                .WithMessage("O Campo Codigo deve ter no máximo 30 caracteres");

            RuleFor(x => x.IdAutor)
                .NotEmpty()
                .WithMessage("O Campo IdAutor é obrigatório");
        }
    }
}
