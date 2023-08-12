#if DEBUG
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FaceOff.Shared;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace FaceOff
{
	public static class UITestBackdoorService
	{
		static readonly TypeInfo _applicationTypeInfo = Application.Current.GetType().GetTypeInfo();

		public static Task SubmitImageForPhoto1(string playerName, EmotionType emotion)
		{
			var player1 = new PlayerModel(PlayerNumberType.Player1, playerName)
			{
				ImageMediaFile = new MediaFile($"{Xamarin.Essentials.FileSystem.AppDataDirectory}/player1photo", () => _applicationTypeInfo.Assembly.GetManifestResourceStream($"{_applicationTypeInfo.Namespace}.Images.{emotion}.png"))
			};

			return GetFaceOffViewModel().SubmitPhoto(emotion, player1);
		}

		public static Task SubmitImageForPhoto2(string playerName, EmotionType emotion)
		{
			var player2 = new PlayerModel(PlayerNumberType.Player2, playerName)
			{
				ImageMediaFile = new MediaFile($"{Xamarin.Essentials.FileSystem.AppDataDirectory}/player2photo", () => _applicationTypeInfo.GetTypeInfo().Assembly.GetManifestResourceStream($"{_applicationTypeInfo.Namespace}.Images.{emotion}.png"))
			};

			return GetFaceOffViewModel().SubmitPhoto(emotion, player2);
		}

		static FaceOffViewModel GetFaceOffViewModel() => (FaceOffViewModel)GetCurrentPage().BindingContext;

		static Page GetCurrentPage()
		{
			return Application.Current.MainPage.Navigation.ModalStack.LastOrDefault()
				 ?? Application.Current.MainPage.Navigation.NavigationStack.Last();
		}
	}
}
#endif