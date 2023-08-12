using Xamarin.Essentials;

namespace FaceOff
{
	public static class PreferencesService
	{
		public static string Player1Name
		{
			get => Preferences.Get(nameof(Player1Name), string.Empty);
			set => Preferences.Set(nameof(Player1Name), value);
		}

		public static string Player2Name
		{
			get => Preferences.Get(nameof(Player2Name), string.Empty);
			set => Preferences.Set(nameof(Player2Name), value);
		}
	}
}