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
		public void TakePicture1()
		{
			//Arrange

			//Act
			PicturePage.TapTakePhoto1Button();
			PicturePage.TapOK();

			CameraPage.TapPhotoCaptureButton();
			CameraPage.TapUsePhotoButton();

			//Assert
			Assert.IsTrue(PicturePage.ScoreButton1Query().Length > 0);
		}

	}
}

