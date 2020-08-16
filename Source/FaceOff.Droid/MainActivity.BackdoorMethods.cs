#if DEBUG
using FaceOff.Shared;
using Java.Interop;

namespace FaceOff.Droid
{
	public partial class MainActivity
	{
        [Export(BackdoorMethodConstants.GetPicturePageTitle)]
        public string GetPicturePageTitle()
        {
            var mainNavigationPage = (Xamarin.Forms.NavigationPage)Xamarin.Forms.Application.Current.MainPage;
            return Serialize(mainNavigationPage.CurrentPage.Title);
        }

        [Export(BackdoorMethodConstants.SubmitImageForPhoto1)]
        public async void SubmitImageForPhoto1(string serializedInput)
        {
            var playerEmotionModel = Deserialize<PlayerEmotionModel>(serializedInput);

            await UITestBackdoorService.SubmitImageForPhoto1(playerEmotionModel.PlayerName, playerEmotionModel.Emotion).ConfigureAwait(false);
        }

        [Export(BackdoorMethodConstants.SubmitImageForPhoto2)]
        public async void SubmitImageForPhoto2(string serializedInput)
        {
            var playerEmotionModel = Deserialize<PlayerEmotionModel>(serializedInput);

            await UITestBackdoorService.SubmitImageForPhoto2(playerEmotionModel.PlayerName, playerEmotionModel.Emotion).ConfigureAwait(false);
        }


        static string Serialize<T>(T data) => Newtonsoft.Json.JsonConvert.SerializeObject(data);
        static T Deserialize<T>(string data) => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data.ToString());
    }
}
#endif
