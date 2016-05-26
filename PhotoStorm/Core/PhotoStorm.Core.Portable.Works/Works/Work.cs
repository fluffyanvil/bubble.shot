using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Adapters.Manager;
using PhotoStorm.Core.Portable.Adapters.Rules;
using PhotoStorm.Core.Portable.Works.Enums;

namespace PhotoStorm.Core.Portable.Works.Works
{
    public class Work : IWork
    {
        private readonly IAdapterManager _adapterManager;
        private readonly IAdapterRule _adapterRule;

        [JsonProperty("workId")]
        public Guid Id { get; set; }

        [JsonProperty("ownerId")]
        public Guid OwnerId { get; }

        [JsonProperty("state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WorkState State { get; set; }

        public Work(Guid ownerId, double longitude, double latitude, int radius)
        {
            _adapterManager = new AdapterManager();
            _adapterManager.OnNewPhotosReceived += AdapterManagerOnOnNewPhotosReceived;
            _adapterRule = new AdapterRule {Latitude = latitude, Longitude = longitude, Radius = radius};
            Id = Guid.NewGuid();
            OwnerId = ownerId;
        }

        private Work()
        {
            
        }

        public static Work EmptyWork => new Work
        {
            Id = Guid.Empty,
            State = WorkState.Invalid
        };

        private void AdapterManagerOnOnNewPhotosReceived(object sender, NewPhotoAlertEventArgs newPhotoAlertEventArgs)
        {
            try
            {
                OnNewPhotosReceived?.Invoke(this, newPhotoAlertEventArgs);
            }
            catch (Exception)
            {
                
            }
        }

        public Guid Start()
        {
            try
            {
                if (!this._adapterManager.CanStart) return Guid.Empty;
                _adapterManager.Start(_adapterRule);
                State = WorkState.InProgress;
                return Id;
            }
            catch (Exception)
            {
                
            }
            return Guid.Empty;
        }

        public Guid Stop()
        {
            try
            {
                if (!_adapterManager.CanStop) return Guid.Empty;
                _adapterManager.Stop();
                State = WorkState.Stopped;
                return Id;
            }
            catch (Exception)
            {
                
            }
            return Guid.Empty;
        }

        public event EventHandler<NewPhotoAlertEventArgs> OnNewPhotosReceived;
    }
}
