using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using Hardcodet.Wpf.TaskbarNotification;
using PhotoStorm.WpfApplication.Properties;
using Prism.Commands;
using ClientView = PhotoStorm.WpfApplication.Views.ClientView;

namespace PhotoStorm.WpfApplication.ViewModels
{
	public class MainViewModel : INotifyPropertyChanged
	{
		public ObservableCollection<TabItem> TabItems { get; set; } 
		public ICommand RemoveClientCommand => _removeClientCommand ?? (_removeClientCommand = new DelegateCommand<ClientView>(OnExecuteRemoveClientCommand, CanExecuteRemoveClientCommand));
		public TaskbarIcon TaskbarIcon { get; set; }
		private bool CanExecuteRemoveClientCommand(ClientView vm)
		{
			return true;
		}

		private void OnExecuteRemoveClientCommand(ClientView vm)
		{
			try
			{
				var targetTabItem = TabItems.FirstOrDefault(t => Equals(t.Content, vm));
				TabItems.Remove(targetTabItem);
				_removeClientCommand.RaiseCanExecuteChanged();
			}
			catch (Exception)
			{
				
			}
		}

		private DelegateCommand _addNewClientConmmand;
		private DelegateCommand<ClientView> _removeClientCommand;

		public ICommand AddNewClientCommand => _addNewClientConmmand ?? (_addNewClientConmmand = new DelegateCommand(OnExecuteAddNewClientCommand, CanExecuteAddNewClientCommand));

		private void OnExecuteAddNewClientCommand()
		{
			var newClient = new ClientViewModel(TaskbarIcon) {Name = Guid.NewGuid().ToString()};
			var newClientView = new ClientView
			{
				DataContext = newClient,
				RemoveClientCommand = RemoveClientCommand
			};
			var newTabItem = new TabItem {Content = newClientView};
			newTabItem.Header = newClient.Name;
			TabItems.Add(newTabItem);
		}

		private bool CanExecuteAddNewClientCommand()
		{
			return TabItems != null;
		}

		public ObservableCollection<ClientViewModel> NestedClientViewModels { get; set; }
		public MainViewModel()
		{
			TabItems = new ObservableCollection<TabItem>();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
