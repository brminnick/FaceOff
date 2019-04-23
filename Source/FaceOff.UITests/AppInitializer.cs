using System;
using Xamarin.UITest;

namespace FaceOff.UITests
{
    public static class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            switch (platform)
            {
                case Platform.Android:
                    return ConfigureApp
                        .Android
                        .PreferIdeSettings()
                        .StartApp(Xamarin.UITest.Configuration.AppDataMode.Clear);

                case Platform.iOS:
                    return ConfigureApp
                        .iOS
                        .PreferIdeSettings()
                        .StartApp(Xamarin.UITest.Configuration.AppDataMode.Clear);

                default:
                    throw new NotSupportedException();
            }
        }
    }
}

