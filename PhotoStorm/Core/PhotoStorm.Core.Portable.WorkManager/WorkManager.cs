using System;
using System.Collections.Generic;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Works.Enums;
using PhotoStorm.Core.Portable.Works.Works;

namespace PhotoStorm.Core.Portable.WorkManager
{
    public class WorkManager : IWorkManager
    {
        private List<IWork> _works;

        public WorkManager()
        {
            _works = new List<IWork>();
        }

        public List<IWork> Works => _works;

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
                    _works.Remove(work);
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
                if (_works.Contains(work))
                    return;
                work.OnNewPhotosReceived += WorkOnOnNewPhotosReceived;
                _works.Add(work);
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
