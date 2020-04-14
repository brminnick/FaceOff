#if DEBUG
using FaceOff.Shared;
using Foundation;

namespace FaceOff.iOS
{
    public partial class AppDelegate
    {
        public AppDelegate() => Xamarin.Calabash.Start();

        [Export(BackdoorMethodConstants.GetPicturePageTitle + ":")]
        public NSString GetPicturePageTitle(NSString noValue)
        {
            var mainNavigationPage = (Xamarin.Forms.NavigationPage)Xamarin.Forms.Application.Current.MainPage;
            return new NSString(mainNavigationPage.CurrentPage.Title);
        }

        [Export(BackdoorMethodConstants.SubmitImageForPhoto1 + ":")]
        public async void SubmitImageForPhoto1(NSString serializedInput)
        {
            var playerEmotionModel = Deserialize<PlayerEmotionModel>(serializedInput);

            await UITestBackdoorService.SubmitImageForPhoto1(playerEmotionModel.PlayerName, playerEmotionModel.Emotion).ConfigureAwait(false);
        }

        [Export(BackdoorMethodConstants.SubmitImageForPhoto2 + ":")]
        public async void SubmitImageForPhoto2(NSString serializedInput)
        {
            var playerEmotionModel = Deserialize<PlayerEmotionModel>(serializedInput);

            await UITestBackdoorService.SubmitImageForPhoto2(playerEmotionModel.PlayerName, playerEmotionModel.Emotion).ConfigureAwait(false);
        }

        static NSString Serialize<T>(T data) => new NSString(Newtonsoft.Json.JsonConvert.SerializeObject(data));
        static T Deserialize<T>(NSString data) => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data.ToString());
    }
}
#endif
