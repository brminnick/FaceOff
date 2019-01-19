#if DEBUG
using System.Linq;

using Xamarin.Forms;

namespace FaceOff
{
    public static class UITestBackdoorService
    {
        public static void UseDefaultImageForPhoto1()
        {
            if (GetCurrentPage() is FaceOffPage currentPage)
            {
                if (currentPage.BindingContext is FaceOffViewModel faceOffViewModel)
                    faceOffViewModel.SetPhotoImage1ToHappyForUITest("Happy");
            }
        }

        public static void UseDefaultImageForPhoto2()
        {
            if (GetCurrentPage() is FaceOffPage currentPage)
            {
                if (currentPage.BindingContext is FaceOffViewModel faceOffViewModel)
                    faceOffViewModel.SetPhotoImage2ToHappyForUITest("Happy");
            }
        }

        static Page GetCurrentPage() =>
            Application.Current?.MainPage?.Navigation?.ModalStack?.LastOrDefault()
            ?? Application.Current?.MainPage?.Navigation?.NavigationStack?.LastOrDefault();
    }
}
#endif
