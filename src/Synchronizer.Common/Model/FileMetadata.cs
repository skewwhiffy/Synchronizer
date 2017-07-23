using System;

namespace Synchronizer.Common.Model
{
    public class FileMetadata
    {
        public string RelativeFilePath { get; set; }
        public string Hash { get; set; }
        public DateTime LastWritten { get; set; }
    }
}