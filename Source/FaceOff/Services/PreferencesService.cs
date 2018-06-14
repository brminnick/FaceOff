using Xamarin.Essentials;

namespace FaceOff
{
    public static class PreferencesService
    {
        #region Constant Fields
        const string _playerNameDefault = "";
        #endregion

        #region Properties
        public static string Player1Name
        {
            get => Preferences.Get(nameof(Player1Name), _playerNameDefault);
            set => Preferences.Set(nameof(Player1Name), value);
        }

        public static string Player2Name
        {
            get => Preferences.Get(nameof(Player2Name), _playerNameDefault);
            set => Preferences.Set(nameof(Player2Name), value);
        }
        #endregion
    }
}