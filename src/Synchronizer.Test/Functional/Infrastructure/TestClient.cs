using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Synchronizer.Client;

namespace Synchronizer.Test.Functional.Infrastructure
{
    public class TestClient : IDisposable
    {
        public Sandbox Sandbox { get; }
        private readonly HttpClient _httpClient;
        private readonly Program _program;
        private readonly Task _daemon;
        private readonly CancellationTokenSource _tokenSource;

        public TestClient(string rootDirectory, HttpClient httpClient)
        {
            Sandbox = new Sandbox(rootDirectory);
            _httpClient = httpClient;
            var args = new[]{
                "-root",
                Sandbox.FullPath
            };
            _program = new Program(args);
            _tokenSource = new CancellationTokenSource();
            _daemon = Task.Run(async () => await _program.MainAsync(_tokenSource.Token));
        }

        public void Dispose()
        {
            _program.Dispose();
            _tokenSource.Cancel();
            Sandbox.Dispose();
        }
    }
}