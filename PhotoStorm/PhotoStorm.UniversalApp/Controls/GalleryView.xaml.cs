using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace PhotoStorm.UniversalApp.Controls
{
    public sealed partial class GalleryView : UserControl
    {
        private int _x1;
        private int _x2;

        public GalleryView()
        {
            this.InitializeComponent();
            FlipView.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            FlipView.ManipulationStarted += (s, e) => _x1 = (int)e.Position.X;
            FlipView.ManipulationCompleted += (s, e) => {
                _x2 = (int)e.Position.X;
                if (_x1 > _x2)
                {
                    if (FlipView.Items != null && FlipView.SelectedIndex != FlipView.Items.Count - 1)
                    {
                        FlipView.SelectedIndex++;
                    }
                }
                else
                {
                    if (FlipView.Items != null && FlipView.SelectedIndex != 0)
                    {
                        FlipView.SelectedIndex--;
                    }
                }
            };
        }
    }
}
