using NUnit.Framework;

using Xamarin.UITest;
using Xamarin.UITest.iOS;

namespace FaceOff.UITests
{
	public class Tests : TestSetUp
	{
		public Tests(Platform platform) : base(platform)
		{
		}

		[Test]
		public void LaunchApp()
		{
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
			Assert.IsTrue(PicturePage.ScoreButton1Query().Length > 0);
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
			Assert.IsTrue(PicturePage.ScoreButton2Query().Length > 0);
		}

		[Test]
		public void VerifyResetButton()
		{
			//Arrange
			string firstEmotion = PicturePage.GetEmotion();
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
			secondEmotion = PicturePage.GetEmotion();
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

