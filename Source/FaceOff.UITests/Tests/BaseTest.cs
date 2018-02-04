using NUnit.Framework;

using Xamarin.UITest;

namespace FaceOff.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public abstract class BaseTest
    {
        protected BaseTest(Platform platform) => Platform = platform;

        protected IApp App { get; private set; }
        protected Platform Platform { get; private set; }

        protected FaceOffPage FaceOffPage { get; private set; }
        protected CameraPage CameraPage { get; private set; }
        protected WelcomePage WelcomePage { get; private set; }

        [SetUp]
        public virtual void TestSetup()
        {
            App = AppInitializer.StartApp(Platform);

            FaceOffPage = new FaceOffPage(App, Platform);
            CameraPage = new CameraPage(App, Platform);
            WelcomePage = new WelcomePage(App, Platform);

            WelcomePage.WaitForPageToLoad();

            App.Screenshot("App Launched");
        }
    }
}

