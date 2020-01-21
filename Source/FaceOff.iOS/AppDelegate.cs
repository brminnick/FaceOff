using UIKit;
using Foundation;

using Newtonsoft.Json;

using FaceOff.Shared;

namespace FaceOff.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Xamarin.Forms.Forms.Init();
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
            var mainNavigationPage = (Xamarin.Forms.NavigationPage)Xamarin.Forms.Application.Current.MainPage;
            return new NSString(mainNavigationPage.CurrentPage.Title);
        }

        [Export("submitImageForPhoto1:")]
        public async void SubmitImageForPhoto1(NSString serializedInput)
        {
            var playerEmotionModel = JsonConvert.DeserializeObject<PlayerEmotionModel>(serializedInput.ToString());

            await UITestBackdoorService.SubmitImageForPhoto1(playerEmotionModel.PlayerName, playerEmotionModel.Emotion).ConfigureAwait(false);
        }

        [Export("submitImageForPhoto2:")]
        public async void SubmitImageForPhoto2(NSString serializedInput)
        {
            var playerEmotionModel = JsonConvert.DeserializeObject<PlayerEmotionModel>(serializedInput.ToString());

            await UITestBackdoorService.SubmitImageForPhoto2(playerEmotionModel.PlayerName, playerEmotionModel.Emotion).ConfigureAwait(false);
        }
#endif
        #endregion
    }
}

