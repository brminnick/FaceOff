using System;

using NUnit.Framework;

using Xamarin.UITest;

namespace FaceOff.UITests
{
	public class ReplTests : TestSetUp
	{
		public ReplTests(Platform platform):base(platform)
		{
		}

		[Test]
		public void Repl()
		{
			app.Repl();
		}
	}
}

