using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Parmalen.Contracts;
using Parmalen.Contracts.Intent;

namespace Parmalen.Weather
{
    [Export(typeof(IIntent))]
    [Name("weather")]
    public class WeatherIntent : IIntent
    {
        private readonly WeatherIntentConfiguration _config;
        private const string GeoIpUri = "http://www.telize.com/geoip";
        private const string WeatherUri = "http://api.openweathermap.org/data/2.5/forecast/daily";

        public ILog Log { get; set; }

        public WeatherIntent()
        {
            _config = WeatherIntentConfiguration.GetSection();
        }
        public async Task Run(WitEntities entities)
        {
            string city = entities.Locations?.First().Value ?? await GetCurrentCity();
            var dateTime = entities.DateTimes?.First().Value ?? DateTimeOffset.Now;
            var weather = await GetWeatherForecast(city, dateTime);

            using (var process = new Process())
            {
                var conditions = new StringBuilder();
                foreach (var weatherType in weather.Weather)
                {
                    conditions.Append($"{weatherType.Description}, ");
                }
                var arguments = $"-v {_config.VoiceName} \"La temperatura es de {Math.Round(weather.Temperature.Day)} grados, humedad {weather.Humidity} por ciento, {conditions}\"";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = _config.ESpeakPath;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();
            }
        }

        private async Task<string> GetCurrentCity()
        {
            Log.Info("Start Method: GetCurrentCity");
            var webRequest = WebRequest.Create(GeoIpUri);
            webRequest.Method = "GET";

            var response = await webRequest.GetResponseAsync();
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    var result = reader.ReadToEnd();
                    Log.DebugFormat("Results: {0}", result);
                    var geoIp = JsonConvert.DeserializeObject<GeoIp>(result);
                    Log.Info("End Method: GetCurrentCity");
                    return geoIp.City;
                }
            }
        }

        private async Task<Forecast> GetWeatherForecast(string city, DateTimeOffset dateTime)
        {
            Log.Info("Start Method: GetWeatherForecast");
            var uri = $"{WeatherUri}?q={city}&units={_config.WeatherUnits}&type=accurate&lang={_config.WeatherLanguage}&APPID={_config.WeatherApiKey}";
            var webRequest = WebRequest.Create(uri);
            webRequest.Method = "GET";

            try
            {
                var response = await webRequest.GetResponseAsync();
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var result = reader.ReadToEnd();
                        Log.DebugFormat("Results: {0}", result);
                        var weather = JsonConvert.DeserializeObject<WeatherResponse>(result);
                        Log.Info("End Method: GetWeatherForecast");
                        return weather.Forecasts.Single(x => x.DateTime.Year == dateTime.Year && x.DateTime.DayOfYear == dateTime.DayOfYear);
                    }
                }
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Error in method GetWeatherForecast, error: {0}", e);
            }
            return null;
        }
    }
}
