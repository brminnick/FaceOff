using Android.OS;
using Android.App;
using Android.Content.PM;

using Java.Interop;

using Xamarin.Forms;

namespace FaceOff.Droid
{
	[Activity(Label = "FaceOff.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

			LoadApplication(new App());
		}

		#region Xamarin Test Cloud Back Door Methods
#if DEBUG
		[Export("GetPicturePageTitle")]
		public string GetPicturePageTitle()
		{
			return ((NavigationPage)Xamarin.Forms.Application.Current.MainPage).CurrentPage.Title;
		}
#endif
		#endregion
	}
}

