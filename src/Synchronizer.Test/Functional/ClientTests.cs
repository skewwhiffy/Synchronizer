using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Synchronizer.Test.Functional.Infrastructure;
using Xunit;

namespace Synchronizer.Test.Functional
{
    public class ClientTests : IDisposable
    {
        private readonly TestClient _client;
        private readonly Sandbox _sandbox;

        public ClientTests()
        {
            var httpClient = new HttpClient();
            _client = new TestClient(Directory.GetCurrentDirectory(), httpClient);
            _sandbox = _client.Sandbox;
        }

        [Fact]
        public async Task WhenClientStarts_ThenClientProducesManifest()
        {
            var expectedManifestFile = Path.Combine(_sandbox.FullPath, ".sync.manifest");
            var infiniteLoopGuard = 5;
            while (true)
            {
                if (File.Exists(expectedManifestFile))
                {
                    break;
                }
                await Task.Delay(100);
                infiniteLoopGuard--;
                infiniteLoopGuard.Should().BeGreaterOrEqualTo(0);
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}