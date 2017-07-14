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
        public async Task WhenHashingFile_ThenMd5IsPopulatedCorrectly()
        {
            string hash;
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(_file))
            {
                hash = md5.ComputeHash(stream).Pipe(BitConverter.ToString).Replace("-", string.Empty).ToLowerInvariant();
            }

            var metadata = await _hasher.GetHashAsync(_file);

            Assert.Equal(hash, metadata.Hash);
        }

        [Fact]
        public async Task WhenHashingFile_ThenLastModifiedIsPopulatedCorrectly()
        {
            var lastWritten = File.GetLastWriteTimeUtc(_file);

            var metadata = await _hasher.GetHashAsync(_file);

            Assert.Equal(lastWritten, metadata.LastWritten);
        }
    }
}