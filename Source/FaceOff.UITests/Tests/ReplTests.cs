using System;

using NUnit.Framework;

using Xamarin.UITest;

namespace FaceOff.UITests
{
    public class ReplTests : BaseTest
    {
        public ReplTests(Platform platform) : base(platform)
        {
        }

        [Test, Ignore]
        public void Repl()
        {
            App.Repl();
        }
    }
}

