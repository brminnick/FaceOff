using Xamarin.UITest;

namespace FaceOff.UITests
{
	public abstract class BasePage
	{
		protected BasePage(IApp app) => App = app;

		protected IApp App { get; }
	}
}