using System.Linq;

using Xamarin.Forms;

namespace FaceOff
{
	public class App : Application
	{
		public static bool IsBounceButtonAnimationInProgress;

		public App()
		{
			var welcomePage = new NavigationPage(new WelcomePage())
			{
				BarBackgroundColor = Color.FromHex("1FAECE"),
				BarTextColor = Color.White
			};

			MainPage = welcomePage;

		}
	}
}

