using Xamarin.UITest;

namespace FaceOff.UITests
{
	public abstract class BasePage
	{
		protected BasePage(IApp app, Platform platform)
		{
			App = app;
			IsAndroid = platform == Platform.Android;
			IsiOS = platform == Platform.iOS;
		}

        protected IApp App { get; }
        protected bool IsAndroid { get; }
        protected bool IsiOS { get; }
	}
}

