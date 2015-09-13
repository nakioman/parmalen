using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using log4net;
using Parmalen.Contracts;
using Parmalen.Contracts.Record;

namespace Parmalen.SoxStream
{
    [Export(typeof(IStreamRecord))]
    [Name("soxStreamRecord")]
    public class SoxStreamRecord : IStreamRecord
    {
        public ILog Log { get; set; }
        private readonly SoxStreamConfiguration _config;

        public SoxStreamRecord()
        {
            _config = SoxStreamConfiguration.GetConfig();
        }

        public async Task<StreamInfo> RecordAsync()
        {
            Log.Info("Start Method: RecordAsync (SoxStreamRecord)");
            var arguments = $"-q -r {_config.Rate} -c 1 -e {_config.Encoding} -b {_config.Bits} -t {_config.Device} -t wav - {_config.Effect}";
            using (var process = new Process())
            {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = _config.Path;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                var baseStream = process.StandardOutput.BaseStream as FileStream;
                byte[] bytes;

                using (var ms = new MemoryStream())
                {
                    var buffer = new byte[4096];
                    var lastRead = 0;
                    do
                    {
                        if (baseStream != null) lastRead = await baseStream.ReadAsync(buffer, 0, buffer.Length);
                        await ms.WriteAsync(buffer, 0, lastRead);
                    } while (lastRead > 0);

                    bytes = ms.ToArray();
                }
                var streamInfo = new StreamInfo
                {
                    AudioType = "audio/raw",
                    LittleEndian = true,
                    Bits = _config.Bits,
                    Encoding = _config.Encoding,
                    Rate = _config.Rate,
                    Bytes = bytes
                };
                Log.Info("End Method: RecordAsync (SoxStreamRecord)");
                return streamInfo;
            }
        }
    }
}
