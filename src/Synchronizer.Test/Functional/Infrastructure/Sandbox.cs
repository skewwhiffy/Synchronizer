using System;
using System.IO;

namespace Synchronizer.Test.Functional.Infrastructure
{
    public class Sandbox : IDisposable
    {
        private string _rootDirectory;

        public Sandbox(string rootDirectory)
        {
            _rootDirectory = rootDirectory;
            DirectoryName = Guid.NewGuid().ToString();
            Directory.CreateDirectory(FullPath);
        }

        public string DirectoryName { get; }

        public string FullPath => Path.Combine(_rootDirectory, DirectoryName);

        public void Dispose()
        {
            var infiniteLoopGuard = 5;
            while (true)
            {
                try
                {
                    Directory.Delete(FullPath, true);
                    break;
                }
                catch
                {
                    infiniteLoopGuard--;
                    if (infiniteLoopGuard <= 0)
                    {
                        throw;
                    }
                }
            }
        }
    }
}