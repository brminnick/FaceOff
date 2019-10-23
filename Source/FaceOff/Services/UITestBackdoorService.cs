#if DEBUG
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Plugin.Media.Abstractions;

using Xamarin.Forms;

using FaceOff.Shared;

namespace FaceOff
{
    public static class UITestBackdoorService
    {
        static readonly TypeInfo _applicationTypeInfo = Application.Current.GetType().GetTypeInfo();

        static FaceOffViewModel? _faceOffViewModel;
        static FaceOffViewModel FaceOffViewModel => _faceOffViewModel ??= (FaceOffViewModel)GetCurrentPage().BindingContext;

        public static Task SubmitImageForPhoto1(string playerName, EmotionType emotion)
        {
            var player1 = new PlayerModel(PlayerNumberType.Player1, playerName)
            {
                ImageMediaFile = new MediaFile($"{Xamarin.Essentials.FileSystem.AppDataDirectory}/player1photo", () => _applicationTypeInfo.Assembly.GetManifestResourceStream($"{_applicationTypeInfo.Namespace}.Images.{emotion.ToString()}.png"))
            };

            return FaceOffViewModel.SubmitPhoto(emotion, player1);
        }

        public static Task SubmitImageForPhoto2(string playerName, EmotionType emotion)
        {
            var player2 = new PlayerModel(PlayerNumberType.Player2, playerName)
            {
                ImageMediaFile = new MediaFile($"{Xamarin.Essentials.FileSystem.AppDataDirectory}/player2photo", () => _applicationTypeInfo.GetTypeInfo().Assembly.GetManifestResourceStream($"{_applicationTypeInfo.Namespace}.Images.{emotion.ToString()}.png"))
            };

            return FaceOffViewModel.SubmitPhoto(emotion, player2);
        }

        static Page GetCurrentPage()
        {
            return Application.Current.MainPage.Navigation.ModalStack.LastOrDefault()
                 ?? Application.Current.MainPage.Navigation.NavigationStack.Last();
        }
    }
}
#endif
