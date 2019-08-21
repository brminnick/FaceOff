namespace FaceOff
{
	public class WelcomeViewModel : BaseViewModel
	{
		public string Player1
		{
			get => PreferencesService.Player1Name;
			set => PreferencesService.Player1Name = value;
		}

		public string Player2
		{
			get => PreferencesService.Player2Name;
			set => PreferencesService.Player2Name = value;
		}
	}
}
