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
        #region Constant Fields
        static readonly TypeInfo _applicationTypeInfo = Application.Current.GetType().GetTypeInfo();
        #endregion

        #region Fields
        static FaceOffViewModel _faceOffViewModel;
        #endregion

        #region Properties
        static FaceOffViewModel FaceOffViewModel
        {
            get
            {
                if (_faceOffViewModel is null && GetCurrentPage().BindingContext is FaceOffViewModel faceOffViewModel)
                    _faceOffViewModel = faceOffViewModel;

                return _faceOffViewModel;
            }
        }
        #endregion

        #region Methods
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
            return Application.Current?.MainPage?.Navigation?.ModalStack?.LastOrDefault()
                 ?? Application.Current?.MainPage?.Navigation?.NavigationStack?.LastOrDefault();
        }
        #endregion
    }
}
#endif
