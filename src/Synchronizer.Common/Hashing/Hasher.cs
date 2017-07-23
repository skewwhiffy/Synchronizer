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
        public FileMetadata GetFileMetadata(string path, string root)
        {
            var hash = GetHashForFile(path);
            var lastWritten = GetLastWritten(path);
            return new FileMetadata
            {
                Hash = hash,
                LastWritten = lastWritten
            };
        }

        public string GetHashForPayload(string payload)
        {
            using (var md5 = MD5.Create())
            {
                return payload
                    .Pipe(System.Text.Encoding.ASCII.GetBytes)
                    .Pipe(md5.ComputeHash)
                    .Pipe(FormatHash);
            }
        }

        private DateTime GetLastWritten(string path)
        {
            return path.Pipe(File.GetLastWriteTimeUtc);
        }

        private string GetHashForFile(string path)
        {
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(path))
            {
                return md5
                    .ComputeHash(stream)
                    .Pipe(FormatHash);
            }
        }

        private string FormatHash(byte[] value) => value
            .Pipe(BitConverter.ToString)
            .Replace("-", string.Empty)
            .ToLowerInvariant();
    }
}