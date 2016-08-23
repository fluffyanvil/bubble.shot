using System.Collections.ObjectModel;
using System.Windows.Input;
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
	    private ICommand _showDetails;
	    private DelegateCommand<object> _selectItemCommand;

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

		public bool IsShowLink => true;

		public ICommand CloseDetails => _cLoseDetails ?? (_cLoseDetails = new DelegateCommand(OnExecuteCloseDetails));

		private void OnExecuteCloseDetails()
		{
			DetailsIsVisible = false;
		}

		public ICommand ShowDetails => _showDetails ?? (_showDetails = new DelegateCommand(OnExecuteShowDetails));

		private void OnExecuteShowDetails()
		{
			if (!DetailsIsVisible)
				DetailsIsVisible = true;
		}

	    public ICommand SelectItemCommand => _selectItemCommand ?? (_selectItemCommand = new DelegateCommand<object>(OnExecuteSelectitemCommand));

	    private void OnExecuteSelectitemCommand(object o)
	    {
            SelectedItem = (PhotoWithUserLink)o;
        }

	    public PhotoWithUserLink SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				_selectedItem = value;
				OnPropertyChanged();
			    DetailsIsVisible = _selectedItem != null;
			}
		}

		public ObservableCollection<PhotoWithUserLink> Photos { get; set; }
	}
}
