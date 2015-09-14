using System.Configuration;
using System.Reflection;

namespace Parmalen.Weather
{
    public class WeatherIntentConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("eSpeakPath", IsRequired = true)]
        public string ESpeakPath
        {
            get { return (string)this["eSpeakPath"]; }
            set { this["eSpeakPath"] = value; }
        }

        [ConfigurationProperty("voiceName", IsRequired = true)]
        public string VoiceName
        {
            get { return (string)this["voiceName"]; }
            set { this["voiceName"] = value; }
        }

        [ConfigurationProperty("weatherUnits", IsRequired = true)]
        public string WeatherUnits {
            get { return (string)this["weatherUnits"]; }
            set { this["weatherUnits"] = value; }
        }

        [ConfigurationProperty("weatherLanguage", IsRequired = true)]
        public string WeatherLanguage {
            get { return (string)this["weatherLanguage"]; }
            set { this["weatherLanguage"] = value; }
        }

        [ConfigurationProperty("weatherApiKey", IsRequired = true)]
        public string WeatherApiKey {
            get { return (string)this["weatherApiKey"]; }
            set { this["weatherApiKey"] = value; }
        }

        public static WeatherIntentConfiguration GetSection()
        {
            var assembly = Assembly.GetAssembly(typeof(WeatherIntentConfiguration));
            return (WeatherIntentConfiguration)ConfigurationManager.OpenExeConfiguration(assembly.Location).GetSection("weatherIntent");
        }
    }
}