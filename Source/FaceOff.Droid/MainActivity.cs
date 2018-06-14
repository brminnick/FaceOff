using Android.OS;
using Android.App;
using Android.Content.PM;

using Java.Interop;

using Xamarin.Forms;

using Plugin.Permissions;
using Plugin.CurrentActivity;

namespace FaceOff.Droid
{
    [Activity(Label = "FaceOff.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }

        #region Xamarin Test Cloud Back Door Methods
#if DEBUG
		[Export(nameof(GetPicturePageTitle))]
        public string GetPicturePageTitle() =>
            ((NavigationPage)Xamarin.Forms.Application.Current.MainPage).CurrentPage.Title;

		[Export(nameof(UseDefaultImageForPhoto1))]
        public void UseDefaultImageForPhoto1() =>
            BackdoorHelpers.UseDefaultImageForPhoto1();
        
		[Export(nameof(UseDefaultImageForPhoto2))]
        public void UseDefaultImageForPhoto2() =>
            BackdoorHelpers.UseDefaultImageForPhoto2();
#endif
        #endregion
    }
}

