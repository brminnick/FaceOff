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
			PicturePage.TapTakePhoto1Button();
		}

	}
}

