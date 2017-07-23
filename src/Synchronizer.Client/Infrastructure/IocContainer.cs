using System;
using Microsoft.Extensions.DependencyInjection;
using Synchronizer.Client.ArgsParser;
using Synchronizer.Client.Manifest;
using Synchronizer.Common.Hashing;

namespace Synchronizer.Client.Infrastructure
{
    public class IocContainer : IServiceProvider
    {
        private readonly IServiceProvider _provider;

        public IocContainer(IArgs args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IArgs>(args);
            serviceCollection.AddTransient<IManifester, Manifester>();
            serviceCollection.AddTransient<IHasher, Hasher>();
            _provider = serviceCollection.BuildServiceProvider();
        }

        public object GetService(Type serviceType) => _provider.GetService(serviceType);
    }
}