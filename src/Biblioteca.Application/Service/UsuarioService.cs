using Biblioteca.Application.Interface;
using Biblioteca.Application.Models.Result;
using Biblioteca.Application.Models.Usuario;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Enums;
using Biblioteca.Domain.Interfaces;
using Biblioteca.Domain.Interfaces.Repository;

namespace Biblioteca.Application.Service
{
    public class UsuarioService(IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork) : IUsuarioService
    {
        public async Task<CustomResultModel<int>> Cadastrar(CadastrarUsuarioViewModel viewModel)
        {
            var usuario = new Usuario(viewModel.Nome, viewModel.Email, viewModel.LimiteEmprestimo, viewModel.DataNascimento, viewModel.Tipo);

            var usuarioExistente = usuarioRepository.JaCadastrado(usuario.Email);
            if (usuarioExistente)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, $"Usuário com email {usuario.Email} já cadastrado."));

            var validacao = usuario.Validar();
            if (!validacao.IsValid)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, validacao.Errors[0].ErrorMessage));

            var usuarioId = await usuarioRepository.Cadastrar(usuario);

            return CustomResultModel<int>.Success(usuarioId);
        }

        public CustomResultModel<BuscarUsuarioViewModel> BuscarPorId(int id)
        {
            var usuario = usuarioRepository.Buscar(id);
            if (usuario == null)
                return CustomResultModel<BuscarUsuarioViewModel>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Usuário id {id} não encontrado"));

            return CustomResultModel<BuscarUsuarioViewModel>.Success(new BuscarUsuarioViewModel(true, "Usuário buscado com sucesso", usuario.Id, usuario.Nome, usuario.Email, usuario.DataNascimento?.ToString("yyyy-MM-dd"), usuario.Tipo));
        }

        public async Task<CustomResultModel<int>> Atualizar(int id, AtualizarUsuarioViewModel viewModel)
        {
            var usuario = usuarioRepository.Buscar(id);

            if (usuario == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Usuário id {id} não encontrado"));

            usuario.Atualizar(viewModel.Nome, viewModel.Email, viewModel.LimiteEmprestimo, viewModel.DataNascimento, viewModel.Tipo);

            var validacao = usuario.Validar();
            if (!validacao.IsValid)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.BadRequest, validacao.Errors[0].ErrorMessage));

            usuarioRepository.Atualizar(usuario);
            await unitOfWork.Commit();

            return CustomResultModel<int>.Success(usuario.Id);
        }

        public async Task<CustomResultModel<int>> Deletar(int id)
        {
            var usuario = usuarioRepository.Buscar(id);

            if (usuario == null)
                return CustomResultModel<int>.Failure(new CustomErrorModel(ECodigoErro.NotFound, $"Usuário id {id} não encontrado"));

            usuarioRepository.Deletar(usuario);
            await unitOfWork.Commit();
            
            return CustomResultModel<int>.Success(usuario.Id);
        }
    }
}
