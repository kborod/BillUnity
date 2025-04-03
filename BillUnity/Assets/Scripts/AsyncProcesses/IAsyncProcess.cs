using Cysharp.Threading.Tasks;

namespace Kborod.AsyncProcesses
{
    public interface IAsyncProcess
    {
        public UniTask Run();
    }
}
