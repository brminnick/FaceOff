using System.Threading.Tasks;

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
				BarBackgroundColor = Color.FromHex("#1FAECE")
			};

			MainPage = welcomePage;

		}
#if DEBUG
		public void UseDefaultImageForPhoto1()
		{
			PicturePage currentPage;
			var currentNavigationPage = Current.MainPage as NavigationPage;

			if (currentNavigationPage.CurrentPage is PicturePage)
				currentPage = Current.MainPage as PicturePage;
			else
				return;

			currentPage.SetPhotoImage1("Happy");
		}

		public void UseDefaultImageForPhoto2()
		{
			PicturePage currentPage;
			var currentNavigationPage = Current.MainPage as NavigationPage;

			if (currentNavigationPage.CurrentPage is PicturePage)
				currentPage = Current.MainPage as PicturePage;
			else
				return;

			currentPage.SetPhotoImage2("Happy");
		}
#endif
	}
}

