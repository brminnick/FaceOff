using NUnit.Framework;

using Xamarin.UITest;

namespace FaceOff.UITests
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	public abstract class TestSetUp
	{
		protected IApp app;
		protected Platform platform;

		protected PicturePage PicturePage;
		protected CameraPage CameraPage;
		protected WelcomePage WelcomePage;

		protected TestSetUp(Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
		public virtual void TestSetup()
		{
			app = AppInitializer.StartApp(platform);

			PicturePage = new PicturePage(app, platform);
			CameraPage = new CameraPage(app, platform);
			WelcomePage = new WelcomePage(app, platform);

			WelcomePage.WaitForPageToLoad();

			app.Screenshot("App Launched");
		}
	}
}

