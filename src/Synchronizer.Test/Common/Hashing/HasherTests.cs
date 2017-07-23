using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Synchronizer.Common.Extensions;
using Synchronizer.Common.Hashing;
using Synchronizer.Test.Helpers;
using Xunit;

namespace Synchronizer.Test.Common.Hashing
{
    public class HasherTests
    {
        private readonly IHasher _hasher;
        private readonly string _file;

        public HasherTests()
        {

            _hasher = new Hasher();
            var pwd = Directory.GetCurrentDirectory();
            _file = Directory.GetFiles(pwd).TakeRandom();
        }

        [Fact]
        public void WhenHashingFile_ThenMd5IsPopulatedCorrectly()
        {
            string hash;
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(_file))
            {
                hash = md5.ComputeHash(stream).Pipe(BitConverter.ToString).Replace("-", string.Empty).ToLowerInvariant();
            }

            var metadata = _hasher.GetFileMetadata(_file, Path.GetDirectoryName(_file));

            Assert.Equal(hash, metadata.Hash);
        }

        [Fact]
        public void WhenHashingFile_ThenLastModifiedIsPopulatedCorrectly()
        {
            var lastWritten = File.GetLastWriteTimeUtc(_file);

            var metadata = _hasher.GetFileMetadata(_file, Path.GetDirectoryName(_file));

            Assert.Equal(lastWritten, metadata.LastWritten);
        }
    }
}