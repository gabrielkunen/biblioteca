using Biblioteca.Domain.Enums;
using FluentValidation;
using FluentValidation.Results;

namespace Biblioteca.Domain.Entities
{
    public class Emprestimo(int idLivro, int idFuncionario, int idUsuario, DateTime dataInicio, DateTime dataFim) : EntityBase
    {
        public int Id { get; set; }
        public int IdLivro { get; private set; } = idLivro;
        public int IdFuncionario { get; private set; } = idFuncionario;
        public int IdUsuario { get; private set; } = idUsuario;
        public DateTime DataInicio { get; private set; } = dataInicio;
        public DateTime DataFim { get; private set; } = dataFim;
        public EStatusEmprestimo Status { get; private set; } = EStatusEmprestimo.Aberto;
        public DateTime? DataDevolucao { get; private set; }
        public bool Renovado { get; private set; } = false;

        public Livro Livro { get; set; }
        public Funcionario Funcionario { get; set; }
        public Usuario Usuario { get; set; }

        public bool DataFimMaiorQueDataInicio()
        {
            return DataFim > DataInicio;
        }

        public bool DevolucaoExpirada()
        {
            return DateTime.Now > DataFim;
        }

        public void Devolver()
        {
            Status = EStatusEmprestimo.Encerrado;
            DataDevolucao = DateTime.Now;
        }

        public void MarcarComoRenovacao()
        {
            Renovado = true;
        }

        public bool JaFoiRenovado()
        {
            return Renovado;
        }

        public ValidationResult Validar()
        {
            return new EmprestimoValidator().Validate(this);
        }
    }

    public class EmprestimoValidator : AbstractValidator<Emprestimo>
    {
        public EmprestimoValidator()
        {
            RuleFor(x => x.IdLivro)
                .NotEmpty()
                .WithMessage("O Campo IdLivro é obrigatório");

            RuleFor(x => x.IdFuncionario)
                .NotEmpty()
                .WithMessage("O Campo IdFuncionario é obrigatório");

            RuleFor(x => x.IdUsuario)
                .NotEmpty()
                .WithMessage("O Campo IdUsuario é obrigatório");

            RuleFor(x => x.DataInicio)
                .NotEmpty()
                .WithMessage("O Campo DataInicio é obrigatório");

            RuleFor(x => x.DataFim)
                .NotEmpty()
                .WithMessage("O Campo DataFim é obrigatório");
        }
    }
}