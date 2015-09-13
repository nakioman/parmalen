using System.Threading.Tasks;

namespace Parmalen.Contracts.Record
{
    public interface IStreamRecord
    {
        Task<StreamInfo> RecordAsync();
    }
}
