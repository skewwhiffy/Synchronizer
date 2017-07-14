using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Synchronizer.Common.Extensions;
using Synchronizer.Test.Functional.Infrastructure;
using Xunit;

namespace Synchronizer.Test.Functional
{
    public class ServerTests
    {
        private Server _server;

        public ServerTests()
        {
            _server = new Server();
        }

        [Fact]
        public async Task WhenIPing_ThenIGetAResponse()
        {
            var client = _server.Client;

            var response = await client.GetAsync("ping");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}