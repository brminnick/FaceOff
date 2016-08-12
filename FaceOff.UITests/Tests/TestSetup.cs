using NUnit.Framework;

using Xamarin.UITest;

namespace FaceOff.UITests
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	public abstract class TestSetUp
	{
		protected IApp App;
		protected Platform Platform;

		protected PicturePage PicturePage;

		protected TestSetUp(Platform platform)
		{
			Platform = platform;
		}

		[SetUp]
		public virtual void TestSetup()
		{
			App = AppInitializer.StartApp(Platform);

			PicturePage = new PicturePage(App, Platform);

			App.Screenshot("App Launched");
		}
	}
}

