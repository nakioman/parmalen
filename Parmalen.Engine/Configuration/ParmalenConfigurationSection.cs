using System.Configuration;

namespace Parmalen.Engine.Configuration
{
    public class ParmalenConfigurationSection : ConfigurationSection
    {
        public static ParmalenConfigurationSection GetSection()
        {
            return (ParmalenConfigurationSection) ConfigurationManager.GetSection("parmalen");
        }

        [ConfigurationProperty("maxRecordTime", DefaultValue = 0, IsRequired = true)]
        public int MaxRecordTime
        {
            get { return (int) this["maxRecordTime"]; }
            set { this["maxRecordTime"] = value; }
        }

        [ConfigurationProperty("witAccessToken", IsRequired = true)]
        public string WitAccessToken
        {
            get { return (string) this["witAccessToken"]; }
            set { this["witAccessToken"] = value; }
        }

        [ConfigurationProperty("streamRecordType", DefaultValue = "test", IsRequired = true)]
        public string StreamRecordType
        {
            get { return (string) this["streamRecordType"]; }
            set { this["streamRecordType"] = value; }
        }
    }
}