namespace FaceOff
{
	public class WelcomeViewModel : BaseViewModel
	{
		#region Properties
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
		#endregion
	}
}
