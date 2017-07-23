using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Synchronizer.Client.ArgsParser;
using Synchronizer.Common.Extensions;
using Synchronizer.Common.Hashing;
using Synchronizer.Common.Model;

namespace Synchronizer.Client.Manifest
{
    public class Manifester : IManifester
    {
        private readonly IArgs _args;
        private readonly IHasher _hasher;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ConcurrentDictionary<string, FileMetadata> _manifest;
        private readonly Task _watchTask;
        private readonly Task _writeTask;
        private readonly string _manifestFilePath;
        private readonly string _manifestSwapFilePath;

        public Manifester(IArgs args, IHasher hasher)
        {
            _args = args;
            _hasher = hasher;
            _cancellationTokenSource = new CancellationTokenSource();
            _manifestFilePath = Path.Combine(_args.RootDirectory, ".sync.manifest");
            _manifestSwapFilePath = Path.Combine(_args.RootDirectory, ".sync.manifest.swap");
            _manifest = new ConcurrentDictionary<string, FileMetadata>();
            if (_manifestFilePath.Pipe(File.Exists))
            {
                _manifestFilePath
                    .Pipe(File.ReadAllText)
                    .Pipe(JsonConvert.DeserializeObject<List<FileMetadata>>)
                    .ForEach(f => _manifest[f.RelativeFilePath] = f);
            }
            _watchTask = Task.Run(() => WatchAsync(_cancellationTokenSource.Token));
            _writeTask = Task.Run(() => WriteAsync(_cancellationTokenSource.Token));
        }

        private async Task WriteAsync(CancellationToken ct)
        {
            if (!File.Exists(_manifestFilePath))
            {
                if (File.Exists(_manifestSwapFilePath))
                {
                    File.Move(_manifestSwapFilePath, _manifestFilePath);
                }
                else
                {
                    using (var file = File.CreateText(_manifestFilePath))
                    {
                        await file.WriteAsync("[]");
                    }
                }
            }
            var currentFileHash = _hasher.GetFileMetadata(_manifestFilePath, _args.RootDirectory).Hash;
            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    return;
                }
                var payload = _manifest
                    .Values
                    .OrderBy(m => m.RelativeFilePath)
                    .Pipe(JsonConvert.SerializeObject);
                var currentHash = payload.Pipe(_hasher.GetHashForPayload);
                if (currentHash != currentFileHash)
                {
                    File.Copy(_manifestFilePath, _manifestSwapFilePath);
                    File.Delete(_manifestFilePath);
                    using (var file = _manifestFilePath.Pipe(File.CreateText))
                    {
                        await file.WriteAsync(payload);
                    }
                    File.Delete(_manifestSwapFilePath);
                }
            }
        }

        private async Task WatchAsync(CancellationToken ct)
        {
            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    return;
                }
                foreach (var file in Directory.GetFiles(_args.RootDirectory, "*", SearchOption.AllDirectories))
                {
                    if (file == _manifestFilePath || file == _manifestSwapFilePath)
                    {
                        continue;
                    }
                    _manifest[file] = _hasher.GetFileMetadata(file, _args.RootDirectory);
                }
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}