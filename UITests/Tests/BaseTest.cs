using NUnit.Framework;

using Xamarin.UITest;

namespace FaceOff.UITests
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	public abstract class BaseTest
	{
		protected IApp app;
		protected Platform platform;

		protected FaceOffPage FaceOffPage;
		protected CameraPage CameraPage;
		protected WelcomePage WelcomePage;

		protected BaseTest(Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
		public virtual void TestSetup()
		{
			app = AppInitializer.StartApp(platform);

			FaceOffPage = new FaceOffPage(app, platform);
			CameraPage = new CameraPage(app, platform);
			WelcomePage = new WelcomePage(app, platform);

			WelcomePage.WaitForPageToLoad();

			app.Screenshot("App Launched");
		}
	}
}

