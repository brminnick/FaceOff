using Xamarin.UITest;

namespace FaceOff.UITests
{
	public class BasePage
	{
		protected readonly IApp app;
		protected readonly bool IsAndroid;
		protected readonly bool IsiOS;

		protected BasePage(IApp app, Platform platform)
		{
			this.app = app;

			IsAndroid = platform == Platform.Android;
			IsiOS = platform == Platform.iOS;
		}
	}
}

