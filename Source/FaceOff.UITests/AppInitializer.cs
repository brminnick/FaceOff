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
                        .StartApp();

                case Platform.iOS:
                    return ConfigureApp
                        .iOS
                        .PreferIdeSettings()
                        .StartApp();

                default:
                    throw new NotSupportedException();
            }
        }
    }
}

