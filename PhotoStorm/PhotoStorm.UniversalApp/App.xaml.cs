using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.ApplicationInsights;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Unity.Windows;

namespace PhotoStorm.UniversalApp
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	sealed partial class App : PrismUnityApplication
	{
        public IEventAggregator EventAggregator { get; set; }
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
		{
			WindowsAppInitializer.InitializeAsync(
				WindowsCollectors.Metadata |
				WindowsCollectors.Session);
			this.InitializeComponent();
			this.Suspending += OnSuspending;
		}

	    protected override Task OnInitializeAsync(IActivatedEventArgs args)
	    {
            EventAggregator = new EventAggregator();
            Container.RegisterInstance(NavigationService);
            Container.RegisterInstance(SessionStateService);
            Container.RegisterInstance(EventAggregator);
            return base.OnInitializeAsync(args);
	    }

	    protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
	    {
            NavigationService.Navigate("Main", null);
            Window.Current.Activate();
            return Task.FromResult<object>(null);
        }

		/// <summary>
		/// Invoked when application execution is being suspended.  Application state is saved
		/// without knowing whether the application will be terminated or resumed with the contents
		/// of memory still intact.
		/// </summary>
		/// <param name="sender">The source of the suspend request.</param>
		/// <param name="e">Details about the suspend request.</param>
		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			//TODO: Save application state and stop any background activity
			deferral.Complete();
		}
	}
}
