using Xamarin.Forms;

namespace FaceOff
{
	public class App : Application
	{
		public static bool IsBounceButtonAnimationInProgress;

		readonly PicturePage _picturePage = new PicturePage();

		public App()
		{
			MainPage = new NavigationPage(_picturePage)
			{
				BarBackgroundColor = Color.FromHex("#1FAECE")
			};
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
#if DEBUG
		public void UseDefaultImageForPhoto1()
		{
			_picturePage.SetPhotoImage1("Happy");
		}

		public void UseDefaultImageForPhoto2()
		{
			_picturePage.SetPhotoImage2("Happy");
		}
#endif
	}
}

