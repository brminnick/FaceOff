using NUnit.Framework;

using Xamarin.UITest;

namespace FaceOff.UITests
{
	public class Tests : TestSetUp
	{
		public Tests(Platform platform) : base(platform)
		{
		}

		[Test]
		public void SmokeTest()
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
	}
}

