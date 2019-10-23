using Android.OS;
using Android.App;
using Android.Content.PM;

using Java.Interop;

using Newtonsoft.Json;

using FaceOff.Shared;

namespace FaceOff.Droid
{
    [Activity(Label = "FaceOff.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        #region Xamarin Test Cloud Back Door Methods
#if DEBUG
        [Export(nameof(GetPicturePageTitle))]
        public string GetPicturePageTitle()
        {
            var mainNavigationPage = (Xamarin.Forms.NavigationPage)Xamarin.Forms.Application.Current.MainPage;
            return mainNavigationPage.CurrentPage.Title;
        }

        [Export(nameof(SubmitImageForPhoto1))]
        public async void SubmitImageForPhoto1(string serializedInput)
        {
            var playerEmotionModel = JsonConvert.DeserializeObject<PlayerEmotionModel>(serializedInput);

            await UITestBackdoorService.SubmitImageForPhoto1(playerEmotionModel.PlayerName, playerEmotionModel.Emotion).ConfigureAwait(false);
        }

        [Export(nameof(SubmitImageForPhoto2))]
        public async void SubmitImageForPhoto2(string serializedInput)
        {
            var playerEmotionModel = JsonConvert.DeserializeObject<PlayerEmotionModel>(serializedInput);

            await UITestBackdoorService.SubmitImageForPhoto2(playerEmotionModel.PlayerName, playerEmotionModel.Emotion).ConfigureAwait(false);
        }
#endif
        #endregion

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }
    }
}

