using System.Threading.Tasks;

namespace Parmalen.Contracts
{
    public interface IIntent
    {
        Task Run();
    }
}