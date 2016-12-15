using System.Linq;
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
				BarBackgroundColor = Color.FromHex("1FAECE"),
				BarTextColor = Color.White
			};

			MainPage = welcomePage;

		}
#if DEBUG
		public void UseDefaultImageForPhoto1()
		{
			PicturePage currentPage;
			var currentNavigationPage = GetCurrentPage();

			if (currentNavigationPage is PicturePage)
				currentPage = currentNavigationPage as PicturePage;
			else
				return;

			currentPage.SetPhotoImage1("Happy");
		}

		public void UseDefaultImageForPhoto2()
		{
			PicturePage currentPage;
			var currentNavigationPage = GetCurrentPage();

			if (currentNavigationPage is PicturePage)
				currentPage = currentNavigationPage as PicturePage;
			else
				return;

			currentPage.SetPhotoImage2("Happy");
		}

		Page GetCurrentPage()
		{
			return Current?.MainPage?.Navigation?.NavigationStack?.LastOrDefault();
		}
#endif
	}
}

