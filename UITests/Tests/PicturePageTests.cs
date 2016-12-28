using NUnit.Framework;

using Xamarin.UITest;
using Xamarin.UITest.iOS;

namespace FaceOff.UITests
{
	public class PicturePageTests : TestSetUp
	{
		public PicturePageTests(Platform platform) : base(platform)
		{
		}

		public override void TestSetup()
		{
			base.TestSetup();

			WelcomePage.EnterPlayer1Name("First Player");
			WelcomePage.EnterPlayer2Name("Second Player");
			WelcomePage.TapStartGameButton();
			
			PicturePage.WaitForPicturePageToLoad();
		}

		[Test]
		public void TakePictureOne()
		{
			//Arrange

			//Act
			PicturePage.TapTakePhoto1Button();
			PicturePage.TapOK();

			if (platform == Platform.Android)
				return;

			CameraPage.TapPhotoCaptureButton();
			CameraPage.TapUsePhotoButton();

			//Assert
			Assert.IsTrue(PicturePage.IsScoreButton1Visible);
		}

		[Test]
		public void TakePictureTwo()
		{
			//Arrange

			//Act
			PicturePage.TapTakePhoto2Button();
			PicturePage.TapOK();

			if (platform == Platform.Android)
				return;

			CameraPage.TapPhotoCaptureButton();
			CameraPage.TapUsePhotoButton();

			//Assert
			Assert.IsTrue(PicturePage.IsScoreButton2Visible);
		}

		[Test]
		public void VerifyResetButton()
		{
			//Arrange
			string firstEmotion = PicturePage.Emotion;
			string secondEmotion;

			//Act
			if (platform == Platform.Android)
				return;

			PicturePage.TapTakePhoto1Button();
			PicturePage.TapOK();

			CameraPage.TapPhotoCaptureButton();
			CameraPage.TapUsePhotoButton();

			PicturePage.WaitForPhotoImage1();
			PicturePage.TapResetButton();

			//Assert
			secondEmotion = PicturePage.Emotion;
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
			PicturePage.TapScoreButton1();

			//Assert
			Assert.IsTrue(app.Query("Results").Length > 0);
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
			PicturePage.TapScoreButton2();

			//Assert
			Assert.IsTrue(app.Query("Results").Length > 0);
		}
	}
}

