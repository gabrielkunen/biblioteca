namespace Biblioteca.Application.Models.Genero;

public class BuscarGeneroViewModel(bool sucesso, string mensagem, int id, string nome) : RespostaPadraoModel(sucesso, mensagem)
{
    public int Id { get; set; } = id;
    public string Nome { get; set; } = nome;
}