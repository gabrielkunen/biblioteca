namespace Biblioteca.Application.Models.Livro
{
    public class AtualizarLivroViewModel
    {
        public string Titulo { get; set; }
        public string Isbn { get; set; }
        public string Codigo { get; set; }
        public int IdAutor { get; set; }
        public List<int> IdsGeneros { get; set; } = [];
    }
}
