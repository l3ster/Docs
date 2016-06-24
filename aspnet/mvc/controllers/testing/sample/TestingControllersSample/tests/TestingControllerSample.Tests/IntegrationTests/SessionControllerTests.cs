using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace TestingControllersSample.Tests.IntegrationTests
{
    public class SessionControllerTests : IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient _client;

        public SessionControllerTests(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task IndexReturnsCorrectSessionPage()
        {
            var response = await _client.GetAsync("/Session/Index/1");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var testSession = Startup.GetTestSession();
            Assert.True(responseString.Contains(testSession.Name));

            // ideas are loaded client-side
        }
    }
}