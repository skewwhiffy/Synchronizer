using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Synchronizer.Common.Extensions;
using Synchronizer.WebApi;

namespace Synchronizer.Test.Functional.Infrastructure
{
    public class Server : IDisposable
    {
        private readonly Lazy<TestServer> _testServer;
        private readonly Sandbox _sandbox;

        public Server(string rootDirectory)
        {
            _sandbox = new Sandbox(rootDirectory);
            Startup.AddConfig = (cb, env) => { };
            _testServer = new Lazy<TestServer>(() => new WebHostBuilder()
                .UseStartup<Startup>()
                .Pipe(h => new TestServer(h)));
        }

        public HttpClient Client => TestServer.CreateClient();

        public TestServer TestServer => _testServer.Value;

        public void Dispose()
        {
            _sandbox.Dispose();
        }
    }
}