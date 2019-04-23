using NUnit.Framework;

using Xamarin.UITest;

namespace FaceOff.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public abstract class BaseTest
    {
        readonly Platform _platform;

        protected BaseTest(Platform platform) => _platform = platform;

        protected IApp App { get; private set; }
        protected FaceOffPage FaceOffPage { get; private set; }
        protected CameraPage CameraPage { get; private set; }
        protected WelcomePage WelcomePage { get; private set; }

        [SetUp]
        public virtual void TestSetup()
        {
            App = AppInitializer.StartApp(_platform);

            FaceOffPage = new FaceOffPage(App);
            CameraPage = new CameraPage(App);
            WelcomePage = new WelcomePage(App);

            WelcomePage.WaitForPageToLoad();
            WelcomePage.ClearPlayer1EntryText();
            WelcomePage.ClearPlayer2EntryText();

            App.Screenshot("App Launched");
        }
    }
}

