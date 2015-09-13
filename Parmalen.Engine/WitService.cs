using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Autofac.Features.Metadata;
using log4net;
using Newtonsoft.Json;
using Parmalen.Contracts;
using Parmalen.Contracts.Intent;
using Parmalen.Contracts.Record;
using Parmalen.Engine.Configuration;

namespace Parmalen.Engine
{
    public class WitService
    {
        private readonly IStreamRecord _streamRecord;
        private readonly ILog _log;
        private readonly ParmalenConfigurationSection _configuration;

        private const string WitApiSpeechUrl = "https://api.wit.ai/speech";
        private const string WitApiTextUrl = "https://api.wit.ai/message";
        private const string WitApiVersion = "20150901";

        public WitService(IEnumerable<Meta<IStreamRecord>> streamRecords, ILog log)
        {
            _log = log;
            _configuration = ParmalenConfigurationSection.GetSection();
            _streamRecord = streamRecords.First(x => x.Metadata["Name"].Equals(_configuration.StreamRecordType)).Value;
        }

        public async Task<WitResponse> CaptureSpeechIntentAsync()
        {
            _log.Info("Start Method: CaptureSpeechIntentAsync");
            var streamInfo = await _streamRecord.RecordAsync().WithTimeout(TimeSpan.FromSeconds(_configuration.MaxRecordTime));
            if (streamInfo != null)
            {
                try
                {
                    var endian = streamInfo.LittleEndian ? "little" : "big";
                    var uri = $"{WitApiSpeechUrl}?version={WitApiVersion}";
                    var webRequest = WebRequest.Create(uri);
                    webRequest.Method = "POST";
                    webRequest.Headers["Authorization"] = $"Bearer {_configuration.WitAccessToken}";
                    webRequest.ContentLength = streamInfo.Bytes.Length;
                    webRequest.ContentType =
                        $"{streamInfo.AudioType};encoding={streamInfo.Encoding};bits={streamInfo.Bits};rate={streamInfo.Rate};endian={endian}";
                    using (var writeStream = await webRequest.GetRequestStreamAsync())
                    {
                        await writeStream.WriteAsync(streamInfo.Bytes, 0, streamInfo.Bytes.Length);
                        var response = await webRequest.GetResponseAsync();
                        using (var stream = response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                var result = reader.ReadToEnd();
                                _log.DebugFormat("Results: {0} ", result);
                                _log.Info("End Method: CaptureSpeechIntentAsync with results");
                                return JsonConvert.DeserializeObject<WitResponse>(result);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.FatalFormat("Error in Method: CaptureSpeechIntentAsync, exception: {0}", e);
                    throw new ApplicationException("Can't get intent", e);
                }
            }
            _log.Info("End Method: CaptureSpeechIntentAsync without results");
            return null;
        }

        public async Task<WitResponse> CaptureTextIntentAsync(string value)
        {
            _log.InfoFormat("Start Method: CaptureTextIntentAsync with value: {0}", value);
            try
            {
                var uri = $"{WitApiTextUrl}?version={WitApiVersion}&q={Uri.EscapeDataString(value)}";
                var webRequest = WebRequest.Create(uri);
                webRequest.Method = "GET";
                webRequest.Headers["Authorization"] = $"Bearer {_configuration.WitAccessToken}";

                var response = await webRequest.GetResponseAsync();
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var result = reader.ReadToEnd();
                        _log.DebugFormat("Results: {0}", result);
                        _log.Info("End Method: CaptureTextIntentAsync");
                        return JsonConvert.DeserializeObject<WitResponse>(result);
                    }
                }
            }
            catch (Exception e)
            {
                _log.FatalFormat("Error in Method: CaptureTextIntentAsync, exception: {0}", e);
                throw new ApplicationException("Can't get intent", e);
            }
        }
    }
}