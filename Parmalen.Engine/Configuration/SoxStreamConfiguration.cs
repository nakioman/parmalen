using System.Configuration;

namespace Parmalen.Engine.Configuration
{
    public class SoxStreamConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }

        [ConfigurationProperty("rate", IsRequired = true)]
        public string Rate
        {
            get { return (string)this["rate"]; }
            set { this["rate"] = value; }
        }

        [ConfigurationProperty("encoding", IsRequired = true)]
        public string Encoding
        {
            get { return (string)this["encoding"]; }
            set { this["encoding"] = value; }
        }

        [ConfigurationProperty("bits", IsRequired = true)]
        public uint Bits
        {
            get { return (uint)this["bits"]; }
            set { this["bits"] = value; }
        }

        [ConfigurationProperty("device", IsRequired = true)]
        public string Device
        {
            get { return (string)this["device"]; }
            set { this["device"] = value; }
        }

        [ConfigurationProperty("effect")]
        public string Effect
        {
            get { return (string)this["effect"]; }
            set { this["effect"] = value; }
        }

        public static SoxStreamConfiguration GetConfig()
        {
            return (SoxStreamConfiguration)ConfigurationManager.GetSection("sox");
        }
    }
}