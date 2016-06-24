using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace TestingControllersSample.Tests.IntegrationTests
{
    public class HomeControllerTests : IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient _client;

        public HomeControllerTests(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task ReturnsInitialListOfBrainstormSessions()
        {
            var response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var testSession = Startup.GetTestSession();
            Assert.True(responseString.Contains(testSession.Name));
        }

        [Fact]
        public async Task PostAddsNewBrainstormSession()
        {
            var message = new HttpRequestMessage(HttpMethod.Post, "/");
            var data = new Dictionary<string, string>();
            string testSessionName = Guid.NewGuid().ToString();
            data.Add("SessionName", testSessionName);

            message.Content = new FormUrlEncodedContent(data);

            var response = await _client.SendAsync(message);

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/", response.Headers.Location.ToString());
        }
    }
}