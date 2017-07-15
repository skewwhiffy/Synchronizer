using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Synchronizer.Common.Extensions;
using Synchronizer.Test.Functional.Infrastructure;
using Xunit;

namespace Synchronizer.Test.Functional
{
    public class ServerTests : IDisposable
    {
        private Server _server;

        public ServerTests()
        {
            _server = new Server(Directory.GetCurrentDirectory()); ;
        }

        public void Dispose()
        {
            _server.Dispose();
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