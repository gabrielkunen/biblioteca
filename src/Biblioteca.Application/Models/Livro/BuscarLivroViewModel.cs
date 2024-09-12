namespace Biblioteca.Application.Models.Livro
{
    public class BuscarLivroViewModel(bool sucesso, string mensagem, int id, string titulo, string isbn, string codigo, BuscarAutorLivroViewModel autor, List<string> generos) : RespostaPadraoModel(sucesso, mensagem)
    {
        public int Id { get; set; } = id;
        public string Titulo { get; set; } = titulo;
        public string Isbn { get; set; } = isbn;
        public string Codigo { get; set; } = codigo;
        public BuscarAutorLivroViewModel Autor { get; set; } = autor;
        public List<string> Generos { get; set; } = generos;
    }

    public class BuscarAutorLivroViewModel(int id, string nome, string? dataNascimento)
    {
        public int Id { get; set; } = id;
        public string Nome { get; set; } = nome;
        public string? dataNascimento { get; set; } = dataNascimento;
    }
}
