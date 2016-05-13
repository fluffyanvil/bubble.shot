using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Hardcodet.Wpf.TaskbarNotification;
using PhotoStorm.Core.Portable.Adapters.EventArgs;
using PhotoStorm.Core.Portable.Adapters.Vkontakte;
using PhotoStorm.WpfApplication.Properties;
using Prism.Commands;
using Prism.Events;

namespace PhotoStorm.WpfApplication.ViewModels
{
    public class ClientViewModel : INotifyPropertyChanged
	{
		private readonly VkAdapter _adapter;
		private DelegateCommand _startAdapterCommand;
		private double _latitude;
		private double _longitude;
		private int _radius;
		private DelegateCommand _stopAdapterCommand;
		private readonly Dispatcher _dispatcher;
		private string _name;
		private int _index;
		IEventAggregator _eventAggregator;

		public int Index
		{
			get { return _index; }
			set { _index = value; OnPropertyChanged(nameof(Index));}
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; OnPropertyChanged(nameof(Name));}
		}

		public bool FromView { get; set; }

		public delegate void LocationChangedHandler();

		public event LocationChangedHandler LocationChangedEvent;

		protected virtual void OnLocationChanged()
		{
			if (FromView)
				return;
			LocationChangedEvent?.Invoke();
		}

		public double Latitude
		{
			get { return _latitude; }
			set
			{
				if (_latitude == value)
					return;
				_latitude = value;
				OnPropertyChanged(nameof(Latitude));
				OnLocationChanged();
			}
		}

		public double Longitude
		{
			get { return _longitude; }
			set
			{
				if (_longitude == value)
					return;
				_longitude = value;
				OnPropertyChanged(nameof(Longitude));
				OnLocationChanged();
			}
		}

		public int Radius
		{
			get { return _radius; }
			set { _radius = value; OnPropertyChanged(nameof(Radius));}
		}

		public ICommand StopAdapterCommand => _stopAdapterCommand ?? (_stopAdapterCommand = new DelegateCommand(OnExecuteStopAdapterCommand, CanExecuteStopAdapterCommand));

		private bool CanExecuteStopAdapterCommand()
		{
			return _adapter.Active;
		}

		private void OnExecuteStopAdapterCommand()
		{
			_adapter.Stop();
			UpdateCommandsAvailability();
		}

		public ICommand StartAdapterCommand => _startAdapterCommand ??
		                                       (_startAdapterCommand = new DelegateCommand(OnExecuteStartAdapterCommand, CanExecuteStartAdapterCommand));

		private bool CanExecuteStartAdapterCommand()
		{
			return !_adapter.Active;
		}

		private void OnExecuteStartAdapterCommand()
		{
			//_adapter?.Start(Latitude, Longitude, Radius);
			UpdateCommandsAvailability();
		}

		private void UpdateCommandsAvailability()
		{
			_startAdapterCommand.RaiseCanExecuteChanged();
			_stopAdapterCommand.RaiseCanExecuteChanged();
		}

		public ObservableCollection<ImageSource> Photos { get; set; }

		public ClientViewModel(TaskbarIcon taskbarIcon)
		{
			var adapterConfig = new VkAdapterConfig { ApiAddress = "https://api.vk.com/method/photos.search" };
			_adapter = new VkAdapter(adapterConfig);
			_adapter.OnNewPhotosReceived += AdapterOnNewPhotoAlertEventHandler;
			_radius = 5000;
			_longitude = 35.00511;
			_latitude = 57.876779;
			_dispatcher = Dispatcher.CurrentDispatcher;
			Photos = new ObservableCollection<ImageSource>();
			_eventAggregator = new EventAggregator();
			
		}

		private void AdapterOnNewPhotoAlertEventHandler(object sender, NewPhotoAlertEventArgs newPhotoAlertEventArgs)
		{
			try
			{
				var imageLinks = newPhotoAlertEventArgs.Photos.Select(p => p.ImageLink);
				foreach (var imageLink in imageLinks)
				{
					Task.Factory.StartNew(() =>
					{
						try
						{
							var bytes = new WebClient().DownloadData(imageLink);
							var tempImage = new BitmapImage();
							tempImage.BeginInit();
							tempImage.CacheOption = BitmapCacheOption.OnLoad;
							tempImage.StreamSource = new MemoryStream(bytes);
							tempImage.EndInit();
							tempImage.Freeze();
							_dispatcher.BeginInvoke(new Action(() =>
							{
								Photos.Add(tempImage);
								
							}));
						}
						catch (Exception)
						{
							
						}
					});
				}
			}
			catch (Exception exception)
			{
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
