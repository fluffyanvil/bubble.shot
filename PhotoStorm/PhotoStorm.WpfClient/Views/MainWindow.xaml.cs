using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using PhotoStorm.WpfApplication.ViewModels;

namespace PhotoStorm.WpfApplication.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		MainViewModel ViewModel => (MainViewModel) DataContext;
		TaskbarIcon _taskbarIcon;
		public MainWindow()
		{
			InitializeComponent();
			_taskbarIcon = (TaskbarIcon)FindResource("TaskbarIcon");
		}

		public override void OnApplyTemplate()
		{
			ViewModel.TaskbarIcon = _taskbarIcon;
			var newClientView = new PhotoStorm.WpfApplication.Views.ClientView();
			var newClientViewModel = new ClientViewModel(_taskbarIcon);
			newClientView.DataContext = newClientViewModel;
			ViewModel.AddNewClientCommand?.Execute(newClientView);
		}
	}
}
