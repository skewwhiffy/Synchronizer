using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Synchronizer.Common.Model;
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
            _sandbox.CreateRandomFolders();
            _sandbox.CreateRandomFiles(3);
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

            infiniteLoopGuard = 5;
            var files = Directory
                .GetFiles(_sandbox.DirectoryName, "*", SearchOption.AllDirectories)
                .Where(f => !f.Contains("manifest"))
                .ToList();
            List<FileMetadata> manifestDeserialized;
            while (true)
            {
                string manifestRaw = null;
                try
                {
                    manifestRaw = File.ReadAllText(expectedManifestFile);
                    manifestDeserialized = JsonConvert.DeserializeObject<List<FileMetadata>>(manifestRaw);
                    if (files.Count == manifestDeserialized.Count)
                    {
                        break;
                    }
                }
                catch
                {
                }
                await Task.Delay(100);
                infiniteLoopGuard--;
                infiniteLoopGuard.Should().BeGreaterOrEqualTo(0);
            }

            foreach (var file in files)
            {

            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}