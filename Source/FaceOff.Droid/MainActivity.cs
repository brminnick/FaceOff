using Android.OS;
using Android.App;
using Android.Content.PM;

using Java.Interop;

using Xamarin.Forms;

using Plugin.Permissions;

using EntryCustomReturn.Forms.Plugin.Android;

namespace FaceOff.Droid
{
    [Activity(Label = "FaceOff.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CustomReturnEntryRenderer.Init();

            LoadApplication(new App());
        }

        #region Xamarin Test Cloud Back Door Methods
#if DEBUG
        [Export("GetPicturePageTitle")]
        public string GetPicturePageTitle() =>
            ((NavigationPage)Xamarin.Forms.Application.Current.MainPage).CurrentPage.Title;

        [Export("UseDefaultImageForPhoto1")]
        public void UseDefaultImageForPhoto1() =>
            BackdoorHelpers.UseDefaultImageForPhoto1();

        [Export("UseDefaultImageForPhoto2")]
        public void UseDefaultImageForPhoto2() =>
            BackdoorHelpers.UseDefaultImageForPhoto2();
#endif
        #endregion
    }
}

