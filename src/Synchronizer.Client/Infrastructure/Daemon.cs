using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Synchronizer.Client.Manifest;
using Synchronizer.Common.Extensions;

namespace Synchronizer.Client.Infrastructure
{
    public class Daemon : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IManifester _manifester;

        public Daemon(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _manifester = serviceProvider.Resolve<IManifester>();
        }

        public async Task StartAsync(CancellationToken ct)
        {
        }

        public void Dispose()
        {
        }
    }
}