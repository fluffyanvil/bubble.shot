using Microsoft.Practices.Unity;
using Prism.Mvvm;
using Prism.Unity.Windows;

namespace PhotoStorm.UniversalApp.ViewModels
{
    public class BaseViewModel : BindableBase
    {
        protected IUnityContainer Container;

        public BaseViewModel()
        {
            Container = PrismUnityApplication.Current.Container;
        }
    }
}