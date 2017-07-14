using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Synchronizer.Common.Extensions;
using Synchronizer.Common.Model;

namespace Synchronizer.Common.Hashing
{
    public class Hasher : IHasher
    {
        public Task<FileMetadata> GetHashAsync(string path)
        {
            var hash = GetHash(path);
            var lastWritten = GetLastWritten(path);
            return new FileMetadata
            {
                Hash = hash,
                LastWritten = lastWritten
            }.Pipe(Task.FromResult);
        }

        private DateTime GetLastWritten(string path)
        {
            return path.Pipe(File.GetLastWriteTimeUtc);
        }

        private string GetHash(string path)
        {
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(path))
            {
                return md5
                    .ComputeHash(stream)
                    .Pipe(BitConverter.ToString)
                    .Replace("-", string.Empty)
                    .ToLowerInvariant();
            }
        }
    }
}