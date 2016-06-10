using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;
using PhotoStorm.UniversalApp.Models;
using Prism.Commands;
using Prism.Windows.Mvvm;

namespace PhotoStorm.UniversalApp.ViewModels
{
	public class GalleryPivotItemViewModel : ViewModelBase
	{
		private bool _detailsIsVisible;

		private PhotoWithUserLink _selectedItem;
		private DelegateCommand _cLoseDetails;

		private double _availableModalSize;
		private double _dynamicPhotoSize;
		private int _maximumColumns;
		private DelegateCommand<object> _removeItemCommand;
		private DelegateCommand _removeAllItemsCommand;
		private ICommand _adaptWindowSizeCommand;
		private ICommand _adaptWrapGridSizeCommand;
		private ICommand _showDetails;

		public GalleryPivotItemViewModel()
		{
			Photos = new ObservableCollection<PhotoWithUserLink>();
		}

		public bool DetailsIsVisible
		{
			get { return _detailsIsVisible; }
			set
			{
				_detailsIsVisible = value;
				OnPropertyChanged();
			}
		}

		public ICommand RemoveItemCommand => _removeItemCommand ??
											 (_removeItemCommand = new DelegateCommand<object>(OnExecuteRemoveItemCommand));

		private void OnExecuteRemoveItemCommand(object parameter)
		{
			var photo = parameter as PhotoWithUserLink;
			if (SelectedItem != photo)
			{
				Photos.Remove(photo);
			}
		}

		public ICommand RemoveAllItemsCommand => _removeAllItemsCommand ?? (_removeAllItemsCommand = new DelegateCommand(OnExecuteRemoveAllItemsCommand));

		private void OnExecuteRemoveAllItemsCommand()
		{
			SelectedItem = null;
			Photos.Clear();
		}

		public ICommand AdaptWindowSizeCommand => _adaptWindowSizeCommand ?? (_adaptWindowSizeCommand = new DelegateCommand<SizeChangedEventArgs>(OnExecuteAdaptWindowSizeCommand));

		public ICommand AdaptWrapGridSizeCommand => _adaptWrapGridSizeCommand ?? (_adaptWrapGridSizeCommand = new DelegateCommand<SizeChangedEventArgs>(OnExecuteAdaptWrapGridSizeCommand));

		private void OnExecuteAdaptWrapGridSizeCommand(SizeChangedEventArgs e)
		{
			var isLandscape = e.NewSize.Width > e.NewSize.Height;
			DynamicPhotoSize = isLandscape ? e.NewSize.Width / 10 : e.NewSize.Width / 5;
			MaximumColumns = (int)(e.NewSize.Width / DynamicPhotoSize);
		}

		private void OnExecuteAdaptWindowSizeCommand(SizeChangedEventArgs e)
		{
			var borderWidth = 30;
			var isLandscape = e.NewSize.Width > e.NewSize.Height;
			AvailableModalSize = isLandscape ? e.NewSize.Height - 3 * borderWidth : e.NewSize.Width - borderWidth;

		}

		public bool IsShowLink => true;

		public ICommand CloseDetails => _cLoseDetails ?? (_cLoseDetails = new DelegateCommand(OnExecuteCloseDetails));

		private void OnExecuteCloseDetails()
		{
			//SelectedItem = null;
			DetailsIsVisible = false;
		}

		public ICommand ShowDetails => _showDetails ?? (_showDetails = new DelegateCommand(OnExecuteShowDetails));

		private void OnExecuteShowDetails()
		{
			if (!DetailsIsVisible)
				DetailsIsVisible = true;
		}

		public PhotoWithUserLink SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				_selectedItem = value;
				OnPropertyChanged();
			}
		}

		public double AvailableModalSize
		{
			get { return _availableModalSize; }
			set
			{
				_availableModalSize = value;
				OnPropertyChanged();
			}
		}

		public double DynamicPhotoSize
		{
			get { return _dynamicPhotoSize; }
			set
			{
				_dynamicPhotoSize = value;
				OnPropertyChanged();
			}
		}

		public int MaximumColumns
		{
			get { return _maximumColumns; }
			set
			{
				_maximumColumns = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<PhotoWithUserLink> Photos { get; set; }
	}
}
