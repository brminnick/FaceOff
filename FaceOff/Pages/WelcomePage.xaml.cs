using System;

using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FaceOff.Shared;

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
			PopulatePlaceholderText();
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
			{
				Insights.Track(InsightsConstants.StartGameButtonTapped, InsightsConstants.StartGameButtonTappedStatus, InsightsConstants.Player1NameEmpty);
				DisplayEmptyPlayerNameAlert(1);
			}
			else if (isPlayer2EntryTextEmpty)
			{
				Insights.Track(InsightsConstants.StartGameButtonTapped, InsightsConstants.StartGameButtonTappedStatus, InsightsConstants.Player2NameEmpty);
				DisplayEmptyPlayerNameAlert(2);
			}
			else
			{
				Insights.Track(InsightsConstants.StartGameButtonTapped, InsightsConstants.StartGameButtonTappedStatus, InsightsConstants.GameStarted);
				await Navigation.PushAsync(new PicturePage(Player1Entry.Text, Player2Entry.Text));
			}
		}

		void DisplayEmptyPlayerNameAlert(int playerNumber)
		{
			Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Error", $"Player {playerNumber} Name is Blank", "OK"));
		}

		void PopulateAutomationIDs()
		{
			Player1Entry.AutomationId = AutomationIdConstants.Player1Entry;
			Player2Entry.AutomationId = AutomationIdConstants.Player2Entry;
			StartGameButton.AutomationId = AutomationIdConstants.StartGameButton;
		}

		void PopulatePlaceholderText()
		{
			Player1Entry.Placeholder = PlaceholderConstants.WelcomePagePlaceholderText;
			Player2Entry.Placeholder = PlaceholderConstants.WelcomePagePlaceholderText;
		}
		#endregion
	}
}
