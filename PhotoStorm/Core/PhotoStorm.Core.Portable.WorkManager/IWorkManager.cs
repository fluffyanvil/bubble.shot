using System;
using System.Collections.Generic;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Works.Works;

namespace PhotoStorm.Core.Portable.WorkManager
{
    public interface IWorkManager
    {
        List<IWork> Works { get; }
        void StartWork(IWork work);
        void StopWork(IWork work);
        void DeleteWork(IWork work);
        void AddWork(IWork work);
        event EventHandler<NewPhotoAlertEventArgs> OnNewPhotosReceived;
    }
}