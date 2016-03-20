using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Bubbleshot.Server.Adapters.Pcl.Base;
using Bubbleshot.Server.Adapters.Pcl.Vkontakte;
using Bubbleshot.Server.Common.Pcl.Models;
using Prism.Commands;
using Prism.Windows.Mvvm;

namespace BubbleShot.UniversalApp.ViewModels
{
	public class MainPageViewModel : ViewModelBase
	{
		private readonly VkAdapter _adapter;
		private readonly BackgroundDownloader _backgroundDownloader;
		private DelegateCommand _startAdapterCommand;
		private DelegateCommand _stopAdapterCommand;
		private double _longitude;
		private double _latitude;
		private int _radius;
		private Geopoint _location;

		public MainPageViewModel()
		{
			var adapterConfig = new VkAdapterConfig { ApiAddress = "https://api.vk.com/method/photos.search" };
			_adapter = new VkAdapter(adapterConfig);
			_adapter.NewPhotoAlertEventHandler += AdapterOnNewPhotoAlertEventHandler;
			_radius = 5000;
			_longitude = 35.00511;
			_latitude = 57.876779;
			Photos = new ObservableCollection<ImageSource>();
			_backgroundDownloader = new BackgroundDownloader();
		}

		public Geopoint Location
		{
			get { return _location; }
			set
			{
				_location = value; 
				OnPropertyChanged();
			}
		}

		private async void AdapterOnNewPhotoAlertEventHandler(object sender, NewPhotoAlertEventArgs e)
		{
			try
			{
				var imageLinks = ((List<PhotoItemModel>)e.Photos).Select(p => p.ImageLink);
				foreach (var imageLink in imageLinks)
				{
					await Task.Factory.StartNew(async () =>
					{
						try
						{
							var sf = Windows.ApplicationModel.Package.Current.InstalledLocation;
							var file = await sf.CreateFileAsync(Guid.NewGuid().ToString("N"), CreationCollisionOption.GenerateUniqueName);
							var downloadOperation = _backgroundDownloader.CreateDownload(new Uri(imageLink), file);
							await downloadOperation.StartAsync();

							var result = downloadOperation.ResultFile;


							var tempImage = await LoadImage(result);

							Photos.Add(tempImage);
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

		private static async Task<BitmapImage> LoadImage(IStorageFile file)
		{
			var bitmapImage = new BitmapImage();
			var stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);

			bitmapImage.SetSource(stream);

			return bitmapImage;

		}

		public ObservableCollection<ImageSource> Photos { get; set; }

		public int Radius
		{
			get { return _radius; }
			set { _radius = value; OnPropertyChanged(); }
		}

		public double Latitude
		{
			get { return _latitude; }
			set
			{
				if (_latitude == value)
					return;
				_latitude = value;
				OnPropertyChanged();
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
				OnPropertyChanged();
			}
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

		public ICommand StartAdapterCommand => _startAdapterCommand ?? (_startAdapterCommand = new DelegateCommand(OnExecuteStartAdapter, CanExecuteStartAdapter));

		private bool CanExecuteStartAdapter()
		{
			return !_adapter.Active;
		}

		private void OnExecuteStartAdapter()
		{
			_adapter?.Start(Latitude, Longitude, Radius);
			UpdateCommandsAvailability();
		}

		private void UpdateCommandsAvailability()
		{
			_startAdapterCommand.RaiseCanExecuteChanged();
			_stopAdapterCommand.RaiseCanExecuteChanged();
		}
	}
}
