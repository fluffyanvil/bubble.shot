using System.Threading.Tasks;
using Prism.Unity.Windows;

namespace BubbleShot.UniversalApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : PrismUnityApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
			this.InitializeComponent();
        }

	    protected override Task OnLaunchApplicationAsync(Windows.ApplicationModel.Activation.LaunchActivatedEventArgs args)
	    {
			NavigationService.Navigate("Main", Container);
			return Task.FromResult<object>(null);
		}
    }
}
