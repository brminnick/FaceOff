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

		protected TestSetUp(Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
		public virtual void TestSetup()
		{
			app = AppInitializer.StartApp(platform);

			PicturePage = new PicturePage(app, platform);
			PicturePage.WaitForPicturePageToLoad();

			app.Screenshot("App Launched");
		}
	}
}

