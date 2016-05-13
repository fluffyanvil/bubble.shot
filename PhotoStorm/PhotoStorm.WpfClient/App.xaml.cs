using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BubbleShot.WpfClient.ViewModels;
using BubbleShot.WpfClient.Views;

namespace BubbleShot.WpfClient
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
