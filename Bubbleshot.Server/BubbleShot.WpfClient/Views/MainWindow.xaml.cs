using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using BubbleShot.WpfClient.ViewModels;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Design;
using Prism.Regions;

namespace BubbleShot.WpfClient.Views
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
			var newClientView = new ClientView();
			var newClientViewModel = new ClientViewModel(_taskbarIcon);
			newClientView.DataContext = newClientViewModel;
			ViewModel.AddNewClientCommand?.Execute(newClientView);
		}
	}
}
