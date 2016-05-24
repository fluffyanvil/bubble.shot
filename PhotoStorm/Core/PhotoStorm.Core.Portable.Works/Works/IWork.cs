using PhotoStorm.Core.Portable.Adapters.Manager;
using PhotoStorm.Core.Portable.Adapters.Rules;
using PhotoStorm.Core.Portable.Works.Enums;

namespace PhotoStorm.Core.Portable.Works.Works
{
    public interface IWork
    {
        IAdapterManager AdapterManager { get; set; }
        IAdapterRule AdapterRule { get; set; }
        WorkState State { get; set; }
        void Start();
        void Stop();
    }
}