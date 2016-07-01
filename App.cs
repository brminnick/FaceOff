using System;

using Xamarin.Forms;

namespace FaceOff
{
	public class App : Application
	{
		public static bool IsBounceButtonAnimationInProgress;

		public App()
		{
			MainPage = new NavigationPage(new PicturePage())
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
	}
}

