using System;
using System.Collections.Generic;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Works.Enums;
using PhotoStorm.Core.Portable.Works.Works;

namespace PhotoStorm.Core.Portable.WorkManager
{
    public class WorkManager : IWorkManager
    {
        public WorkManager()
        {
        }

        public HashSet<IWork> Works => WorkManagerStaticFields.Works;

        public void StartWork(IWork work)
        {
            try
            {
                if (work.State == WorkState.New || work.State == WorkState.Stopped)
                    work.Start();
            }
            catch (Exception)
            {
                
            }
        }

        public void StopWork(IWork work)
        {
            try
            {
                if (work.State == WorkState.InProgress)
                {
                    work.Stop();
                    WorkManagerStaticFields.Works.Remove(work);
                }
            }
            catch (Exception)
            {
                
            }
        }

        public void AddWork(IWork work)
        {
            try
            {
                if (WorkManagerStaticFields.Works.Contains(work))
                    return;
                work.OnNewPhotosReceived += WorkOnOnNewPhotosReceived;
                WorkManagerStaticFields.Works.Add(work);
                StartWork(work);
            }
            catch (Exception)
            {
                
            }
        }

        private void WorkOnOnNewPhotosReceived(object sender, NewPhotoAlertEventArgs newPhotoAlertEventArgs)
        {
            try
            {
                OnNewPhotosReceived?.Invoke(sender, newPhotoAlertEventArgs);
            }
            catch (Exception ex)
            {
                
            }
        }

        public event EventHandler<NewPhotoAlertEventArgs> OnNewPhotosReceived;
    }
}
