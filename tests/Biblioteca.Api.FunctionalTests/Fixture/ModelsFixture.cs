using Biblioteca.Application.Models.Autor;

namespace Biblioteca.Api.FunctionalTests.Fixture
{
    public class ModelsFixture
    {
        public CadastrarAutorViewModel CadastrarAutorVmValido;
        public AtualizarAutorViewModel AtualizarAutorVmValido;

        public ModelsFixture()
        {
            CadastrarAutorVmValido = new CadastrarAutorViewModel 
            { 
                Nome = "Gabriel", 
                DataNascimento = new DateTime(1995, 10, 12)
            };

            AtualizarAutorVmValido = new AtualizarAutorViewModel
            {
                Nome = "Gabriel",
                DataNascimento = new DateTime(1995, 10, 12)
            };
        }
    }
}
