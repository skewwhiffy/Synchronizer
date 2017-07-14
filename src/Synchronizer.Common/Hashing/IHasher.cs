using System.Threading.Tasks;
using Synchronizer.Common.Model;

namespace Synchronizer.Common.Hashing
{
    public interface IHasher
    {
        Task<FileMetadata> GetHashAsync(string path);
    }
}