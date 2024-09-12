using Biblioteca.Application.Interface;
using Biblioteca.Application.Models.Emprestimo;
using Biblioteca.Application.Models.Result;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Enums;
using Biblioteca.Domain.Interfaces;
using Biblioteca.Domain.Interfaces.Repository;

namespace Biblioteca.Application.Service
{
    public class EmprestimoService(IEmprestimoRepository emprestimoRepository, ITokenService tokenService, IUsuarioRepository usuarioRepository, ILivroRepository livroRepository, IUnitOfWork unitOfWork) : IEmprestimoService
    {
        private readonly IEmprestimoRepository _emprestimoRepository = emprestimoRepository;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
        private readonly ILivroRepository _livroRepository = livroRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CustomResultModel<int>> Cadastrar(CadastrarEmprestimoViewModel viewModel)
        {
            var idFuncionario = _tokenService.BuscarIdFuncionario();
            var emprestimo = new Emprestimo(viewModel.IdLivro, idFuncionario, viewModel.IdUsuario, viewModel.DataInicio, viewModel.DataFim);

            var validacao = emprestimo.Validar();

            if (!validacao.IsValid)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, validacao.Errors[0].ErrorMessage));

            var livro = await _livroRepository.Buscar(emprestimo.IdLivro);
            if (livro == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Livro id {emprestimo.IdLivro} não encontrado"));

            if (livro.Emprestado())
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Livro id {emprestimo.IdLivro} já se encontra emprestado"));

            if (!emprestimo.DataFimMaiorQueDataInicio())
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"A DataFim precisa ser maior que a DataInicio"));

            var usuario = _usuarioRepository.Buscar(emprestimo.IdUsuario);
            if (usuario == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Usuário id {emprestimo.IdUsuario} não encontrado"));

            var quantidadeEmprestimoAtivo = _emprestimoRepository.QuantidadeEmprestimoAtivo(usuario.Id);

            if (!usuario.PossuiLimiteEmprestimo(quantidadeEmprestimoAtivo))
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Usuário id {emprestimo.IdUsuario} não pode mais pegar livros emprestados, pois já chegou no limite de {usuario.LimiteEmprestimo} livro(s)"));

            var emprestimoId = await _emprestimoRepository.Cadastrar(emprestimo);
            livro.Emprestar();
            _livroRepository.Atualizar(livro);

            await _unitOfWork.Commit();
            return CustomResultModel<int>.Success(emprestimoId);
        }

        public async Task<CustomResultModel<int>> Devolver(DevolverEmprestimoViewModel viewModel)
        {
            var emprestimo = await _emprestimoRepository.BuscarEmprestimoAtivo(viewModel.IdLivro);
            if (emprestimo == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Não há empréstimo ativo para o livro id {viewModel.IdLivro}"));

            if (emprestimo.DevolucaoExpirada())
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"A data de devolução do livro id {viewModel.IdLivro} já expirou, deveria ser devolvido dia {emprestimo.DataFim:dd/MM/yyyy}"));

            emprestimo.Devolver();
            emprestimo.Livro.Devolver();

            await _unitOfWork.Commit();
            return CustomResultModel<int>.Success(emprestimo.Id);
        }

        public async Task<CustomResultModel<int>> Renovar(RenovarEmprestimoViewModel viewModel)
        {
            var emprestimo = await _emprestimoRepository.BuscarEmprestimoAtivo(viewModel.IdLivro);
            if (emprestimo == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Não há empréstimo ativo para o livro id {viewModel.IdLivro}"));

            if (emprestimo.DevolucaoExpirada())
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"A data de devolução do livro id {viewModel.IdLivro} já expirou, deveria ser devolvido dia {emprestimo.DataFim:dd/MM/yyyy}"));

            if (emprestimo.JaFoiRenovado())
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"O livro id {viewModel.IdLivro} já foi renovado uma vez e não pode ser renovado no momento"));

            emprestimo.Devolver();

            var idFuncionario = _tokenService.BuscarIdFuncionario();
            var dataAtual = DateTime.Now;
            var novaDataDevolucao = dataAtual.AddDays(viewModel.QuantidadeDias);

            var novoEmprestimo = new Emprestimo(emprestimo.IdLivro, idFuncionario, emprestimo.IdUsuario, dataAtual, novaDataDevolucao);
            novoEmprestimo.MarcarComoRenovacao();

            var emprestimoId = await _emprestimoRepository.Cadastrar(novoEmprestimo);

            await _unitOfWork.Commit();
            return CustomResultModel<int>.Success(emprestimoId);
        }
    }
}
