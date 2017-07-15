﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Synchronizer.Client.ArgsParser;
using Synchronizer.Client.Infrastructure;
using Synchronizer.Common.Extensions;

namespace Synchronizer.Client
{
    public class Program : IDisposable
    {
        private readonly IServiceProvider _iocContainer;

        public Program(string[] args) : this(args.Pipe(a => Args.For(a)))
        {
        }

        public Program(IArgs args)
        {
            _iocContainer = new IocContainer(args);
        }

        static void Main(string[] args)
        {
            using (var program = new Program(args))
            {
                program.MainAsync().GetAwaiter().GetResult();
            }
        }

        public void Dispose()
        {
        }

        public async Task MainAsync(CancellationToken ct = default(CancellationToken))
        {
            Console.WriteLine("Hello world");
        }
    }
}
