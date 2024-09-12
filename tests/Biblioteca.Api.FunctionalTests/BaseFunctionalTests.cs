using Xunit;

namespace Biblioteca.Api.FunctionalTests
{
    public class BaseFunctionalTests : IClassFixture<FunctionalTestWebApplicationFactory>
    {
        public BaseFunctionalTests(FunctionalTestWebApplicationFactory factory)
        {
            HttpClient = factory.CreateClient();
        }

        protected HttpClient HttpClient { get; init; }
    }
}
