using System;
using System.IO;
using System.Threading.Tasks;

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

        public void CreateRandomFiles(int fileCountPerDirectory)
        {
            var directories = Directory.GetDirectories(DirectoryName, "*", SearchOption.AllDirectories);
            foreach (var directory in directories)
            {
                for (var i = 0; i < fileCountPerDirectory; i++)
                {
                    var newFile = Path.Combine(directory, $"{Guid.NewGuid()}.txt");
                    using (var file = File.CreateText(newFile))
                    {
                        file.WriteLine(Guid.NewGuid().ToString());
                    }
                }
            }
        }

        public void CreateRandomFolders(string start = null, int iterations = 0)
        {
            if (iterations < 0)
            {
                return;
            }
            start = start ?? DirectoryName;
            for (var i = 0; i < 3; i++)
            {
                var newDirectory = Path.Combine(start, Guid.NewGuid().ToString());
                Directory.CreateDirectory(newDirectory);
                CreateRandomFolders(newDirectory, iterations - 1);
            }
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