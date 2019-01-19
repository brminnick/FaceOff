using UIKit;
using Foundation;

using Xamarin.Forms;

namespace FaceOff.iOS
{
    [Register(nameof(AppDelegate))]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();
            Microsoft.AppCenter.Distribute.Distribute.DontCheckForUpdatesInDebug();

#if DEBUG
            Xamarin.Calabash.Start();
#endif

            LoadApplication(new App());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        #region Xamarin Test Cloud Back Door Methods

#if DEBUG
        [Export("getPicturePageTitle:")]
        public NSString GetPicturePageTitle(NSString noValue)
        {
            var mainNavigationPage = Xamarin.Forms.Application.Current.MainPage as Xamarin.Forms.NavigationPage;
            return new NSString(mainNavigationPage.CurrentPage.Title);
        }

        [Export("useDefaultImageForPhoto1:")]
        public NSString UseDefaultImageForPhoto1(NSString noValue)
        {
            UITestBackdoorService.UseDefaultImageForPhoto1();
            return new NSString();
        }

        [Export("useDefaultImageForPhoto2:")]
        public NSString UseDefaultImageForPhoto2(NSString noValue)
        {
            UITestBackdoorService.UseDefaultImageForPhoto2();
            return new NSString();
        }
#endif
        #endregion
    }
}

