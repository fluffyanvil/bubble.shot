using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using BubbleShot.UniversalApp.Views;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using Prism.Windows.AppModel;
using Prism.Windows.Navigation;

namespace BubbleShot.UniversalApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App
    {
		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			
			InitializeComponent();
			
		}

	    protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
	    {
			NavigationService.Navigate("Main", Container);
			return Task.FromResult<object>(null);
		}

	    protected override void OnLaunched(LaunchActivatedEventArgs args)
	    {
			
			if (IsSuspending)
			    SessionStateService.RestoreSessionStateAsync();
		    base.OnLaunched(args);
	    }

		protected override Task OnSuspendingApplicationAsync()
		{
			SessionStateService.SaveAsync();
			return base.OnSuspendingApplicationAsync();
		}
	}
}
