using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Synchronizer.Common.Extensions;
using Synchronizer.WebApi;

namespace Synchronizer.Test.Functional.Infrastructure
{
    public class Server
    {
        private readonly Lazy<TestServer> _testServer;

        public Server()
        {
            Startup.AddConfig = (cb, env) => { };
            _testServer = new Lazy<TestServer>(() => new WebHostBuilder()
                .UseStartup<Startup>()
                .Pipe(h => new TestServer(h)));
        }

        public HttpClient Client => TestServer.CreateClient();

        public TestServer TestServer => _testServer.Value;
    }
}