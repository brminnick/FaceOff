using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FaceOff
{
	public partial class WelcomePage : ContentPage
	{
		#region Constructors
		public WelcomePage()
		{
			InitializeComponent();
			BindingContext = new WelcomeViewModel();

			PopulateAutomationIDs();
		}
		#endregion

		#region Methods
		protected override void OnAppearing()
		{
			base.OnAppearing();

			StartGameButton.Clicked += HandleStartGameButtonClicked;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			StartGameButton.Clicked -= HandleStartGameButtonClicked;
		}

		async void HandleStartGameButtonClicked(object sender, EventArgs e)
		{
			var isPlayer1EntryTextEmpty = string.IsNullOrWhiteSpace(Player1Entry.Text);
			var isPlayer2EntryTextEmpty = string.IsNullOrWhiteSpace(Player2Entry.Text);

			if (isPlayer1EntryTextEmpty)
				DisplayEmptyPlayerNameAlert(1);
			else if (isPlayer2EntryTextEmpty)
				DisplayEmptyPlayerNameAlert(2);
			else
				await Navigation.PushAsync(new PicturePage(Player1Entry.Text, Player2Entry.Text));
		}

		void DisplayEmptyPlayerNameAlert(int playerNumber)
		{
			DisplayAlert("Error", $"Player {playerNumber} Name is Blank", "OK");
		}

		void PopulateAutomationIDs()
		{
			Player1Entry.AutomationId = AutomationIdConstants.Player1Entry;
			Player2Entry.AutomationId = AutomationIdConstants.Player2Entry;
			StartGameButton.AutomationId = AutomationIdConstants.StartGameButton;
		}
		#endregion
	}
}
