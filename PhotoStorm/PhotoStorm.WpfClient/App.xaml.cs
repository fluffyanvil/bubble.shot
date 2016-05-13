using System;
using System.Windows;
using PhotoStorm.WpfApplication.ViewModels;
using PhotoStorm.WpfApplication.Views;

namespace PhotoStorm.WpfApplication
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			try
			{
				var vm = new MainViewModel();
				var mw = new MainWindow();
				mw.DataContext = vm;
				mw.Show();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Error");
			}
		}
	}
}
