using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;
using Bubbleshot.Server.Adapters.Pcl.Base;
using Bubbleshot.Server.Adapters.Pcl.Vkontakte;
using Bubbleshot.Server.Common.Pcl.Models;
using BubbleShot.UniversalApp.Models;
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
		private IStorageFolder _installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;

		public MainPageViewModel()
		{
			var adapterConfig = new VkAdapterConfig { ApiAddress = "https://api.vk.com/method/photos.search" };
			_adapter = new VkAdapter(adapterConfig);
			_adapter.NewPhotoAlertEventHandler += AdapterOnNewPhotoAlertEventHandler;
			Radius = 50000;
			Photos = new ObservableCollection<VkPhotoWithUserLink>();
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
				var imageLinks = (List<PhotoItemModel>) e.Photos;
				foreach (var imageLink in imageLinks)
				{
					await DownloadPhoto(imageLink);
				}

			}
			catch (Exception exception)
			{
				await new Windows.UI.Popups.MessageDialog(exception.Message).ShowAsync();
			}
		}

		private async Task DownloadPhoto(PhotoItemModel photoItem)
		{
			var file = await _installedLocation.CreateFileAsync(string.Format("{0}.{1}", Guid.NewGuid().ToString("N"), "jpg"), CreationCollisionOption.GenerateUniqueName);
			var downloadOperation = _backgroundDownloader.CreateDownload(new Uri(photoItem.ImageLink), file);
			await downloadOperation.StartAsync();
			var stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);


			await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				var bitmapImage = new BitmapImage();
				bitmapImage.SetSource(stream);
				Photos.Add(new VkPhotoWithUserLink {Image = bitmapImage, UserLink = photoItem.UserLink});
				
			});
			await file.DeleteAsync();
		}

		private static async Task<BitmapImage> LoadImage(IStorageFile file)
		{
			var bitmapImage = new BitmapImage();
			
			var stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);

			bitmapImage.SetSource(stream);

			return bitmapImage;

		}

		public ObservableCollection<VkPhotoWithUserLink> Photos { get; set; }

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
			_adapter?.Start(Location.Position.Latitude, Location.Position.Longitude, Radius);
			UpdateCommandsAvailability();
		}

		private void UpdateCommandsAvailability()
		{
			_startAdapterCommand.RaiseCanExecuteChanged();
			_stopAdapterCommand.RaiseCanExecuteChanged();
		}
	}
}
