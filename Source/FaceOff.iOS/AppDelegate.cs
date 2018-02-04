using UIKit;
using Foundation;

using Xamarin.Forms;

using EntryCustomReturn.Forms.Plugin.iOS;

namespace FaceOff.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
		{
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
#endif
			global::Xamarin.Forms.Forms.Init();
			CustomReturnEntryRenderer.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(uiApplication, launchOptions);
		}

		#region Xamarin Test Cloud Back Door Methods

#if ENABLE_TEST_CLOUD
		[Export("getPicturePageTitle:")]
		public NSString GetPicturePageTitle(NSString noValue) =>
			new NSString(((NavigationPage)Xamarin.Forms.Application.Current.MainPage).CurrentPage.Title);

		[Export("useDefaultImageForPhoto1:")]
		public NSString UseDefaultImageForPhoto1(NSString noValue)
		{
			BackdoorHelpers.UseDefaultImageForPhoto1();
			return new NSString();
		}

		[Export("useDefaultImageForPhoto2:")]
		public NSString UseDefaultImageForPhoto2(NSString noValue)
		{
			BackdoorHelpers.UseDefaultImageForPhoto2();
			return new NSString();
		}
#endif
		#endregion
	}
}

