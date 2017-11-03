#if DEBUG
using System.Linq;
using Xamarin.Forms;
namespace FaceOff
{
	public static class BackdoorHelpers
	{
		public static void UseDefaultImageForPhoto1()
		{
			FaceOffPage currentPage;
			var currentNavigationPage = GetCurrentPage();

			if (currentNavigationPage is FaceOffPage)
				currentPage = currentNavigationPage as FaceOffPage;
			else
				return;

			var pictureViewModel = currentPage.BindingContext as FaceOffViewModel;

			pictureViewModel.SetPhotoImage1ToHappyForUITest("Happy");
		}

		public static void UseDefaultImageForPhoto2()
		{
			FaceOffPage currentPage;
			var currentNavigationPage = GetCurrentPage();

			if (currentNavigationPage is FaceOffPage)
				currentPage = currentNavigationPage as FaceOffPage;
			else
				return;

			var pictureViewModel = currentPage.BindingContext as FaceOffViewModel;

			pictureViewModel.SetPhotoImage2ToHappyForUITest("Happy");
		}

		static Page GetCurrentPage()
		{
			return Application.Current?.MainPage?.Navigation?.NavigationStack?.LastOrDefault();
		}
	}
}
#endif
