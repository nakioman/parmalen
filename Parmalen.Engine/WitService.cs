using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.Metadata;
using log4net;
using Newtonsoft.Json;
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

        public WitResponse CaptureSpeechIntent()
        {
            _log.Info("Start Method: CaptureSpeechIntent");
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_configuration.MaxRecordTime));
            var task = _streamRecord.RecordAsync();
            try
            {
                task.Wait(cts.Token);
            }
            catch (OperationCanceledException)
            {
                _log.Info("End Method: CaptureSpeechIntent without results");
                return null;
            }
            if (task.IsCompleted)
            {
                try
                {
                    var streamInfo = task.Result;
                    var endian = streamInfo.LittleEndian ? "little" : "big";
                    var uri = $"{WitApiSpeechUrl}?version={WitApiVersion}";
                    var webRequest = WebRequest.Create(uri);
                    webRequest.Method = "POST";
                    webRequest.Headers["Authorization"] = $"Bearer {_configuration.WitAccessToken}";
                    webRequest.ContentLength = streamInfo.Bytes.Length;
                    webRequest.ContentType =
                        $"{streamInfo.AudioType};encoding={streamInfo.Encoding};bits={streamInfo.Bits};rate={streamInfo.Rate};endian={endian}";
                    using (var writeStream = webRequest.GetRequestStream())
                    {
                        writeStream.Write(streamInfo.Bytes, 0, streamInfo.Bytes.Length);
                        var response = webRequest.GetResponse();
                        using (var stream = response.GetResponseStream())
                        {
                            if (stream != null)
                            {
                                using (var reader = new StreamReader(stream))
                                {
                                    var result = reader.ReadToEnd();
                                    _log.DebugFormat("Results: {0} ", result);
                                    _log.Info("End Method: CaptureSpeechIntent with results");
                                    return JsonConvert.DeserializeObject<WitResponse>(result);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.FatalFormat("Error in Method: CaptureSpeechIntent, exception: {0}", e);
                    throw new ApplicationException("Can't get intent", e);
                }
            }
            _log.Info("End Method: CaptureSpeechIntent without results");
            return null;
        }

        public WitResponse CaptureTextIntent(string value)
        {
            _log.InfoFormat("Start Method: CaptureTextIntentAsync with value: {0}", value);
            try
            {
                var uri = $"{WitApiTextUrl}?version={WitApiVersion}&q={Uri.EscapeDataString(value)}";
                var webRequest = WebRequest.Create(uri);
                webRequest.Method = "GET";
                webRequest.Headers["Authorization"] = $"Bearer {_configuration.WitAccessToken}";

                var response = webRequest.GetResponse();
                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
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
                return null;
            }
            catch (Exception e)
            {
                _log.FatalFormat("Error in Method: CaptureTextIntentAsync, exception: {0}", e);
                throw new ApplicationException("Can't get intent", e);
            }
        }
    }
}