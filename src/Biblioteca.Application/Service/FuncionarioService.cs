using Biblioteca.Application.Interface;
using Biblioteca.Application.Models.Result;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Enums;
using Biblioteca.Application.Models.Funcionario;
using Biblioteca.Domain.Interfaces.Repository;
using Biblioteca.Domain.Interfaces;

namespace Biblioteca.Application.Service
{
    public class FuncionarioService(IFuncionarioRepository funcionarioRepository, IUnitOfWork unitOfWork, ISenhaService senhaService, ITokenService tokenService) : IFuncionarioService
    {
        public async Task<CustomResultModel<int>> Cadastrar(string pepper, CadastrarFuncionarioViewModel viewModel)
        {
            var senhaValida = senhaService.SenhaValida(viewModel.Senha);

            if (senhaValida.IsFailure)
                return CustomResultModel<int>.Failure(senhaValida.Error);

            var salt = senhaService.GenerateSalt();
            var hash = senhaService.ComputeHash(viewModel.Senha, salt, pepper, 11);

            var funcionario = new Funcionario(viewModel.Nome, salt, hash, viewModel.Email, viewModel.DataNascimento, viewModel.Tipo);

            var funcionarioExistente = funcionarioRepository.JaCadastrado(funcionario.Email);
            if (funcionarioExistente)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Funcionário com email {funcionario.Email} já cadastrado."));

            var validacao = funcionario.Validar();
            if (!validacao.IsValid)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, validacao.Errors[0].ErrorMessage));

            var funcionarioId = await funcionarioRepository.Cadastrar(funcionario);
            await unitOfWork.Commit();

            return CustomResultModel<int>.Success(funcionarioId);
        }

        public CustomResultModel<string> Logar(string pepper, string authToken, LogarFuncionarioViewModel viewModel)
        {
            var funcionario = funcionarioRepository.Buscar(viewModel.Email);

            if (funcionario == null)
                return CustomResultModel<string>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Email ou senha inválidos"));

            var hash = senhaService.ComputeHash(viewModel.Senha, funcionario.Salt, pepper, 11);
            if (funcionario.Hash != hash)
                return CustomResultModel<string>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Email ou senha inválidos"));

            var bearerToken = tokenService.Gerar(authToken, funcionario);

            return CustomResultModel<string>.Success(bearerToken);
        }
    }
}
