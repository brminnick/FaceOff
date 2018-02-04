using Xamarin.UITest;

namespace FaceOff.UITests
{
    public static class AppInitializer
	{
		public static IApp StartApp(Platform platform)
		{
			if (platform == Platform.Android)
			{
				return ConfigureApp
					.Android
                    .EnableLocalScreenshots()
					.PreferIdeSettings()
					.StartApp();
			}

			return ConfigureApp
				.iOS
				.PreferIdeSettings()
				.StartApp();
		}
	}
}

