// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace FaceOff
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		#region Constant Fields
		const string _player1NameKey = "Player1NameKey";
		const string _player2NameKey = "Player2NameKey";
		static readonly string _playerNameDefault = string.Empty;
		#endregion

		#region Properties
		static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		public static string Player1Name
		{
			get
			{
				return AppSettings.GetValueOrDefault(_player1NameKey, _playerNameDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(_player1NameKey, value);
			}
		}

		public static string Player2Name
		{
			get
			{
				return AppSettings.GetValueOrDefault(_player2NameKey, _playerNameDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(_player2NameKey, value);
			}
		}
		#endregion

	}
}