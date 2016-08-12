using UIKit;
using Foundation;

using Xamarin.Forms;

namespace FaceOff.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
#endif

			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}

		#region Xamarin Test Cloud Back Door Methods

#if ENABLE_TEST_CLOUD
		[Export("getPicturePageTitle:")]
		public NSString GetPicturePageTitle(NSString noValue)
		{
			return new NSString(((NavigationPage)Xamarin.Forms.Application.Current.MainPage).CurrentPage.Title);
		}
#endif
		#endregion
	}
}

