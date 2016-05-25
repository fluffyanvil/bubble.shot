using System;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Adapters.Manager;
using PhotoStorm.Core.Portable.Adapters.Rules;
using PhotoStorm.Core.Portable.Works.Enums;

namespace PhotoStorm.Core.Portable.Works.Works
{
    public interface IWork
    {
        Guid Id { get; }
        WorkCreatorDevice WorkCreatorDevice { get; set; }
        WorkState State { get; set; }
        Guid Start();
        Guid Stop();
        event EventHandler<NewPhotoAlertEventArgs> OnNewPhotosReceived;
    }
}