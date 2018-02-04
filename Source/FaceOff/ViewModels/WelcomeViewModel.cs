namespace FaceOff
{
	public class WelcomeViewModel : BaseViewModel
	{
		#region Properties
		public string Player1
		{
			get => Settings.Player1Name;
			set => Settings.Player1Name = value;
		}

		public string Player2
		{
			get => Settings.Player2Name;
			set => Settings.Player2Name = value;
		}
		#endregion
	}
}
