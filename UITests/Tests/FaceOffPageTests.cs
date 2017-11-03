using System.Linq;

using NUnit.Framework;

using Xamarin.UITest;
using Xamarin.UITest.iOS;

namespace FaceOff.UITests
{
	public class FaceOffPageTests : BaseTest
	{
		public FaceOffPageTests(Platform platform) : base(platform)
		{
		}

		public override void TestSetup()
		{
			base.TestSetup();

			WelcomePage.EnterPlayer1Name("First Player");
			WelcomePage.EnterPlayer2Name("Second Player");
			WelcomePage.TapStartGameButton();
			
			FaceOffPage.WaitForPicturePageToLoad();
		}


		[Test]
		public void TakePictureOne()
		{
            //Arrange

			//Act
			FaceOffPage.TapTakePhoto1Button();
			FaceOffPage.TapOK();

			if (platform == Platform.Android)
				return;

			CameraPage.TapPhotoCaptureButton();
			CameraPage.TapUsePhotoButton();

			//Assert
            Assert.IsTrue(FaceOffPage.IsScoreButton1Visible);
		}

		[Test]
		public void TakePictureTwo()
		{
			//Arrange

			//Act
			FaceOffPage.TapTakePhoto2Button();
			FaceOffPage.TapOK();

			if (platform == Platform.Android)
				return;

			CameraPage.TapPhotoCaptureButton();
			CameraPage.TapUsePhotoButton();

			//Assert
            Assert.IsTrue(FaceOffPage.IsScoreButton2Visible);
		}

		[Test]
		public void VerifyResetButton()
		{
			//Arrange
            string firstEmotion = FaceOffPage.Emotion;
			string secondEmotion;

			//Act
			if (platform == Platform.Android)
				return;

            FaceOffPage.TapTakePhoto1Button();
            FaceOffPage.TapOK();

			CameraPage.TapPhotoCaptureButton();
			CameraPage.TapUsePhotoButton();

            FaceOffPage.WaitForPhotoImage1();
            FaceOffPage.TapResetButton();

			//Assert
            secondEmotion = FaceOffPage.Emotion;
			Assert.AreNotEqual(firstEmotion, secondEmotion);
		}

		[Test]
		public void VerifyPhoto1Results()
		{
			//Arrange
			if(app is iOSApp)
				app.Invoke("useDefaultImageForPhoto1:", "");
			else
				app.Invoke("UseDefaultImageForPhoto1");

			//Act
			app.Screenshot("Test Image Loaded");
            FaceOffPage.TapScoreButton1();

			//Assert
			Assert.IsTrue(app.Query("Results").Any());
		}

		[Test]
		public void VerifyPhoto2Results()
		{
			//Arrange
			if (app is iOSApp)
				app.Invoke("useDefaultImageForPhoto2:", "");
			else
				app.Invoke("UseDefaultImageForPhoto2");

			//Act
			app.Screenshot("Test Image Loaded");
            FaceOffPage.TapScoreButton2();

			//Assert
			Assert.IsTrue(app.Query("Results").Any());
		}
	}
}

