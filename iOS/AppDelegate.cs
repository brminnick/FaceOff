using UIKit;
using Foundation;

using Xamarin.Forms;

namespace FaceOff.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		App _app;

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
#endif

			global::Xamarin.Forms.Forms.Init();

			LoadApplication(_app = new App());

			return base.FinishedLaunching(app, options);
		}

		#region Xamarin Test Cloud Back Door Methods

#if ENABLE_TEST_CLOUD
		[Export("getPicturePageTitle:")]
		public NSString GetPicturePageTitle(NSString noValue)
		{
			return new NSString(((NavigationPage)Xamarin.Forms.Application.Current.MainPage).CurrentPage.Title);
		}

		[Export("useDefaultImageForPhoto1:")]
		public NSString UseDefaultImageForPhoto1(NSString noValue)
		{
			_app.UseDefaultImageForPhoto1();
			return new NSString();
		}

		[Export("useDefaultImageForPhoto2:")]
		public NSString UseDefaultImageForPhoto2(NSString noValue)
		{
			_app.UseDefaultImageForPhoto2();
			return new NSString();
		}
#endif
		#endregion
	}
}

