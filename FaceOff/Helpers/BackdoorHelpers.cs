#if DEBUG
using System.Linq;
using Xamarin.Forms;
namespace FaceOff
{
	public static class BackdoorHelpers
	{
		public static void UseDefaultImageForPhoto1()
		{
			PicturePage currentPage;
			var currentNavigationPage = GetCurrentPage();

			if (currentNavigationPage is PicturePage)
				currentPage = currentNavigationPage as PicturePage;
			else
				return;

			var pictureViewModel = currentPage.BindingContext as PictureViewModel;

			pictureViewModel.SetPhotoImage1ToHappyForUITest("Happy");
		}

		public static void UseDefaultImageForPhoto2()
		{
			PicturePage currentPage;
			var currentNavigationPage = GetCurrentPage();

			if (currentNavigationPage is PicturePage)
				currentPage = currentNavigationPage as PicturePage;
			else
				return;

			var pictureViewModel = currentPage.BindingContext as PictureViewModel;

			pictureViewModel.SetPhotoImage2ToHappyForUITest("Happy");
		}

		static Page GetCurrentPage()
		{
			return Application.Current?.MainPage?.Navigation?.NavigationStack?.LastOrDefault();
		}
	}
}
#endif
