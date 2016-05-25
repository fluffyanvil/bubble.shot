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

        [JsonProperty("id")]
        public Guid Id { get; }

        [JsonProperty("device")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WorkCreatorDevice WorkCreatorDevice { get; set; }

        [JsonProperty("state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WorkState State { get; set; }

        public Work(double longitude, double latitude, int radius)
        {
            _adapterManager = new AdapterManager();
            _adapterManager.OnNewPhotosReceived += AdapterManagerOnOnNewPhotosReceived;
            _adapterRule = new AdapterRule {Latitude = latitude, Longitude = longitude, Radius = radius};
            Id = System.Guid.NewGuid();
        }

        private Work()
        {
            Id = Guid.Empty;
            State = WorkState.Invalid;
        }

        public static Work EmptyWork => new Work();

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
