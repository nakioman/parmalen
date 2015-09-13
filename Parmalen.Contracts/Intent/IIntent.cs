using System.Threading.Tasks;

namespace Parmalen.Contracts.Intent
{
    public interface IIntent
    {
        Task Run(WitEntities entities);
    }
}