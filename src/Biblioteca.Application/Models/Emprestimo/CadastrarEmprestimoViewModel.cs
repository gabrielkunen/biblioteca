namespace Biblioteca.Application.Models.Emprestimo
{
    public class CadastrarEmprestimoViewModel
    {
        public int IdLivro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
