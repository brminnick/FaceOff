using NUnit.Framework;

using Xamarin.UITest;

namespace FaceOff.UITests
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	public class Tests : TestSetUp
	{
		IApp app;
		Platform platform;

		public Tests(Platform platform) : base(platform)
		{
		}

		[Test]
		public void SmokeTest()
		{
		}
	}
}

