using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FaceOff
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            var navigationPage = new Xamarin.Forms.NavigationPage(new WelcomePage())
            {
                BarBackgroundColor = ColorConstants.NavigationBarBackgroundColor,
                BarTextColor = ColorConstants.NaviagtionBarTextColor
            };

            navigationPage.On<iOS>().SetPrefersLargeTitles(true);

            MainPage = navigationPage;
        }

        protected override void OnStart()
        {
            base.OnStart();

            AnalyticsService.Start();
        }
    }
}

