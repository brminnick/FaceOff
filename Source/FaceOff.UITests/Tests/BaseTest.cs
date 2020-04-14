using System;
using NUnit.Framework;

using Xamarin.UITest;

namespace FaceOff.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public abstract class BaseTest
    {
        readonly Platform _platform;

        IApp? _app;
        FaceOffPage? _faceOffPage;
        CameraPage? _cameraPage;
        WelcomePage? _welcomePage;

        protected BaseTest(Platform platform) => _platform = platform;

        protected IApp App => _app ?? throw new NullReferenceException();
        protected FaceOffPage FaceOffPage => _faceOffPage ?? throw new NullReferenceException();
        protected CameraPage CameraPage => _cameraPage ?? throw new NullReferenceException();
        protected WelcomePage WelcomePage => _welcomePage ?? throw new NullReferenceException();

        [SetUp]
        public virtual void TestSetup()
        {
            _app = AppInitializer.StartApp(_platform);

            _faceOffPage = new FaceOffPage(App);
            _cameraPage = new CameraPage(App);
            _welcomePage = new WelcomePage(App);

            WelcomePage.WaitForPageToLoad();
            WelcomePage.ClearPlayer1EntryText();
            WelcomePage.ClearPlayer2EntryText();

            App.Screenshot("App Launched");
        }
    }
}

