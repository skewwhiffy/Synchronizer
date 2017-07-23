using System.Threading.Tasks;
using Synchronizer.Common.Model;

namespace Synchronizer.Common.Hashing
{
    public interface IHasher
    {
        FileMetadata GetFileMetadata(string path, string root);
        string GetHashForPayload(string payload);
    }
}