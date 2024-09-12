using Biblioteca.Domain.Enums;
using FluentValidation;
using FluentValidation.Results;

namespace Biblioteca.Domain.Entities
{
    public class Usuario(string nome, string email, int limiteEmprestimo, DateTime? dataNascimento, ETipoUsuario tipo) : EntityBase
    {
        public int Id { get; set; }
        public string Nome { get; private set; } = nome;
        public string Email { get; private set; } = email;
        public int LimiteEmprestimo { get; private set; } = limiteEmprestimo;
        public DateTime? DataNascimento { get; private set; } = dataNascimento;
        public ETipoUsuario Tipo { get; private set; } = tipo;

        public List<Emprestimo> Emprestimos { get; set; }

        public void Atualizar(string nome, string email, int limiteEmprestimo, DateTime? dataNascimento, ETipoUsuario tipo)
        {
            Nome = nome;
            Email = email;
            LimiteEmprestimo = limiteEmprestimo;
            DataNascimento = dataNascimento;
            Tipo = tipo;
        }

        public bool PossuiLimiteEmprestimo(int quantidadeEmprestimosAtivos)
        {
            return quantidadeEmprestimosAtivos < LimiteEmprestimo;
        }

        public ValidationResult Validar()
        {
            return new UsuarioValidator().Validate(this);
        }
    }

    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("O Campo Nome é obrigatório")
                .MaximumLength(500)
                .WithMessage("O Campo Nome deve ter no máximo 500 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O Campo Email é obrigatório")
                .MaximumLength(200)
                .WithMessage("O Campo Email deve ter no máximo 200 caracteres");

            RuleFor(x => x.Tipo)
                .NotNull()
                .WithMessage("O Campo Tipo é obrigatório");

            RuleFor(x => x.LimiteEmprestimo)
                .NotEmpty()
                .WithMessage("O Campo LimiteEmprestimo é obrigatório");
        }
    }
}
