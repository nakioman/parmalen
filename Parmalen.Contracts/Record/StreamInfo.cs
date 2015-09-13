namespace Parmalen.Contracts.Record
{
    public class StreamInfo
    {
        public StreamInfo()
        {
            LittleEndian = true;
        }

        public byte[] Bytes { get; set; }
        public string AudioType { get; set; }
        public string Encoding { get; set; }
        public uint Bits { get; set; }
        public string Rate { get; set; }
        public bool LittleEndian { get; set; } 
    }
}