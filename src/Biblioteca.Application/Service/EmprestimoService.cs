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
        public async Task<CustomResultModel<int>> Cadastrar(CadastrarEmprestimoViewModel viewModel)
        {
            var idFuncionario = tokenService.BuscarIdFuncionario();
            var emprestimo = new Emprestimo(viewModel.IdLivro, idFuncionario, viewModel.IdUsuario, viewModel.DataInicio, viewModel.DataFim);

            var validacao = emprestimo.Validar();

            if (!validacao.IsValid)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, validacao.Errors[0].ErrorMessage));

            var livro = await livroRepository.Buscar(emprestimo.IdLivro);
            if (livro == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Livro id {emprestimo.IdLivro} não encontrado"));

            if (livro.Emprestado())
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Livro id {emprestimo.IdLivro} já se encontra emprestado"));

            if (!emprestimo.DataFimMaiorQueDataInicio())
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, "A DataFim precisa ser maior que a DataInicio"));

            var usuario = usuarioRepository.Buscar(emprestimo.IdUsuario);
            if (usuario == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Usuário id {emprestimo.IdUsuario} não encontrado"));

            var quantidadeEmprestimoAtivo = emprestimoRepository.QuantidadeEmprestimoAtivo(usuario.Id);

            if (!usuario.PossuiLimiteEmprestimo(quantidadeEmprestimoAtivo))
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Usuário id {emprestimo.IdUsuario} não pode mais pegar livros emprestados, pois já chegou no limite de {usuario.LimiteEmprestimo} livro(s)"));

            var emprestimoId = await emprestimoRepository.Cadastrar(emprestimo);
            livro.Emprestar();
            livroRepository.Atualizar(livro);

            await unitOfWork.Commit();
            return CustomResultModel<int>.Success(emprestimoId);
        }

        public async Task<CustomResultModel<int>> Devolver(DevolverEmprestimoViewModel viewModel)
        {
            var emprestimo = await emprestimoRepository.BuscarEmprestimoAtivo(viewModel.IdLivro);
            if (emprestimo == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Não há empréstimo ativo para o livro id {viewModel.IdLivro}"));

            if (emprestimo.DevolucaoExpirada())
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"A data de devolução do livro id {viewModel.IdLivro} já expirou, deveria ser devolvido dia {emprestimo.DataFim:dd/MM/yyyy}"));

            emprestimo.Devolver();
            emprestimo.Livro.Devolver();

            await unitOfWork.Commit();
            return CustomResultModel<int>.Success(emprestimo.Id);
        }

        public async Task<CustomResultModel<int>> Renovar(RenovarEmprestimoViewModel viewModel)
        {
            var emprestimo = await emprestimoRepository.BuscarEmprestimoAtivo(viewModel.IdLivro);
            if (emprestimo == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Não há empréstimo ativo para o livro id {viewModel.IdLivro}"));

            if (emprestimo.DevolucaoExpirada())
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"A data de devolução do livro id {viewModel.IdLivro} já expirou, deveria ser devolvido dia {emprestimo.DataFim:dd/MM/yyyy}"));

            if (emprestimo.JaFoiRenovado())
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"O livro id {viewModel.IdLivro} já foi renovado uma vez e não pode ser renovado no momento"));

            emprestimo.Devolver();

            var idFuncionario = tokenService.BuscarIdFuncionario();
            var dataAtual = DateTime.Now;
            var novaDataDevolucao = dataAtual.AddDays(viewModel.QuantidadeDias);

            var novoEmprestimo = new Emprestimo(emprestimo.IdLivro, idFuncionario, emprestimo.IdUsuario, dataAtual, novaDataDevolucao);
            novoEmprestimo.MarcarComoRenovacao();

            var emprestimoId = await emprestimoRepository.Cadastrar(novoEmprestimo);

            await unitOfWork.Commit();
            return CustomResultModel<int>.Success(emprestimoId);
        }

        public async Task<CustomResultModel<List<BuscarEmprestimoItemViewModel>>> Buscar(int take, int page)
        {
            var emprestimos = await emprestimoRepository.Buscar(take, page);

            var listaEmprestimo = new List<BuscarEmprestimoItemViewModel>();
            foreach (var e in emprestimos)
            {
                listaEmprestimo.Add(new BuscarEmprestimoItemViewModel { Id = e.Id, IdLivro = e.IdLivro, NomeFuncionario = e.Funcionario.Nome, NomeUsuario = e.Usuario.Nome, DataInicio = e.DataInicio.ToString("dd/MM/yyyy"), DataFim = e.DataFim.ToString("dd/MM/yyyy"), Status = e.Status.ToString(), DataDevolucao = e.DataDevolucao?.ToString("dd/MM/yyyy"), Renovado = e.Renovado });
            }

            return CustomResultModel<List<BuscarEmprestimoItemViewModel>>.Success(listaEmprestimo);
        }
    }
}
