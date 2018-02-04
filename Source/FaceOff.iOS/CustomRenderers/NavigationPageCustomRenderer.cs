using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using FaceOff.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageCustomRenderer))]
namespace FaceOff.iOS
{
    public class NavigationPageCustomRenderer : NavigationRenderer
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                NavigationBar.PrefersLargeTitles = true;

                NavigationBar.LargeTitleTextAttributes = new UIStringAttributes
                {
                    ForegroundColor = UIColor.White
                };
            }
        }
    }
}
