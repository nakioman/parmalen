using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using NAudio.Wave;
using Parmalen.Contracts;
using Parmalen.Contracts.Record;

namespace Parmalen.StreamTests
{
    [Export(typeof(IStreamRecord))]
    [Name("testStreamRecord")]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class TestStreamRecord : IStreamRecord
    {
        public ILog Log { get; set; }
        private bool _alreadyTested;

        public TestStreamRecord()
        {
            _alreadyTested = false;
        }

        public async Task<StreamInfo> RecordAsync()
        {
            Log.Info("Start Method: RecordAsync");
            if (_alreadyTested)
            {
                Log.Debug("Audio file already tested, skipping");
                Log.Info("End Method: RecordAsync");
                return null;
            }

            Log.Debug("Using test wav file");
            using (var reader = new MediaFoundationReader(Path.Combine(GetAssemblyPath(), @"test.wav")))
            {
                var bytes = new byte[reader.Length];
                await reader.ReadAsync(bytes, 0, (int)reader.Length);
                _alreadyTested = true;
                Log.Info("End Method: RecordAsync");
                return new StreamInfo
                {
                    AudioType = "audio/raw",
                    LittleEndian = true,
                    Encoding = "signed-integer",
                    Bits = 16,
                    Rate = "16000",
                    Bytes = bytes
                };
            }
        }

        private string GetAssemblyPath()
        {
            var asm = Assembly.GetAssembly(GetType());
            var fileInfo = new FileInfo(asm.Location);
            return fileInfo.DirectoryName;
        }

    }
}
