using System.IO;
using System.Threading.Tasks;

namespace Parmalen.Contracts
{
    public interface IStreamRecord
    {
        Task<StreamInfo> RecordAsync();
    }
}
