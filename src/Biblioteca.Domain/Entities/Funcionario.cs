﻿using Biblioteca.Domain.Enums;
using FluentValidation;
using FluentValidation.Results;

namespace Biblioteca.Domain.Entities
{
    public class Funcionario(string nome, string senha, string email, DateTime dataNascimento, ETipoFuncionario tipo) : EntityBase
    {
        public int Id { get; set; }
        public string Nome { get; private set; } = nome;
        public string Senha { get; private set; } = senha;
        public string Email { get; private set; } = email;
        public DateTime DataNascimento { get; private set; } = dataNascimento;
        public ETipoFuncionario Tipo { get; private set; } = tipo;
        public List<Emprestimo> Emprestimos { get; set; }

        public ValidationResult Validar()
        {
            return new FuncionarioValidator().Validate(this);
        }
    }

    public class FuncionarioValidator : AbstractValidator<Funcionario>
    {
        public FuncionarioValidator()
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

            RuleFor(x => x.Senha)
                .NotEmpty()
                .WithMessage("O Campo Senha é obrigatório")
                .MaximumLength(500)
                .WithMessage("O Campo Senha deve ter no máximo 500 caracteres");

            RuleFor(x => x.DataNascimento)
                .NotEmpty()
                .WithMessage("O Campo DataNascimento é obrigatório");

            RuleFor(x => x.Tipo)
                .NotNull()
                .WithMessage("O Campo Tipo é obrigatório");
        }
    }
}
