namespace Biblioteca.Application.Models.Emprestimo
{
    public class BuscarEmprestimoViewModel(bool sucesso, string mensagem, List<BuscarEmprestimoItemViewModel> emprestimos) : RespostaPadraoModel(sucesso, mensagem)
    {
        public List<BuscarEmprestimoItemViewModel> Emprestimos { get; set; } = emprestimos;
    }

    public class BuscarEmprestimoItemViewModel
    {
        public int Id { get; set; }
        public int IdLivro { get; set; }
        public string NomeFuncionario { get; set; }
        public string NomeUsuario { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }
        public string Status { get; set; }
        public string? DataDevolucao { get; set; }
        public bool Renovado { get; set; }
    }
}
