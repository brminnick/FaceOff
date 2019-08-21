using System;

using Xamarin.Forms;

using FaceOff.Shared;

namespace FaceOff
{
    public class WelcomePage : BaseContentPage<WelcomeViewModel>
    {
        readonly Entry _player1Entry, _player2Entry;

        public WelcomePage()
        {
            var player1Label = new DarkBlueLabel { Text = "Player 1" };
            var player2Label = new DarkBlueLabel { Text = "Player 2" };

            _player1Entry = new Entry
            {
                AutomationId = AutomationIdConstants.Player1Entry,
                Placeholder = PlaceholderConstants.WelcomePagePlaceholderText,
                ReturnType = ReturnType.Next,
                ReturnCommand = new Command(() => _player2Entry.Focus())
            };
            _player1Entry.SetBinding(Entry.TextProperty, nameof(ViewModel.Player1));

            _player2Entry = new Entry
            {
                AutomationId = AutomationIdConstants.Player2Entry,
                Placeholder = PlaceholderConstants.WelcomePagePlaceholderText,
                ReturnType = ReturnType.Go,
                ReturnCommand = new Command(() => StartGame())
            };
            _player2Entry.SetBinding(Entry.TextProperty, nameof(ViewModel.Player2));
            
            var startGameButton = new BounceButton(AutomationIdConstants.StartGameButton)
            {
                Margin = new Thickness(0, 20, 0, 0),
                Text = "Start"
            };
            startGameButton.Clicked += HandleStartGameButtonClicked;

            NavigationPage.SetBackButtonTitle(this, "");

            Title = "FaceOff";
            BackgroundColor = Color.FromHex("#91E2F4");

            Content = new StackLayout
            {
                Padding = 20,
                Children = {
                    player1Label,
                    _player1Entry,
                    player2Label,
                    _player2Entry,
                    startGameButton
                }
            };
        }

        void DisplayEmptyPlayerNameAlert(int playerNumber) =>
            Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Error", $"Player {playerNumber} Name is Blank", "OK"));

        void HandleStartGameButtonClicked(object sender, EventArgs e) => StartGame();

        void StartGame()
        {
            var isPlayer1EntryTextEmpty = string.IsNullOrWhiteSpace(_player1Entry.Text);
            var isPlayer2EntryTextEmpty = string.IsNullOrWhiteSpace(_player2Entry.Text);

            if (isPlayer1EntryTextEmpty)
            {
                AnalyticsService.Track(AnalyticsConstants.StartGameButtonTapped, AnalyticsConstants.StartGameButtonTappedStatus, AnalyticsConstants.Player1NameEmpty);
                DisplayEmptyPlayerNameAlert(1);
            }
            else if (isPlayer2EntryTextEmpty)
            {
                AnalyticsService.Track(AnalyticsConstants.StartGameButtonTapped, AnalyticsConstants.StartGameButtonTappedStatus, AnalyticsConstants.Player2NameEmpty);
                DisplayEmptyPlayerNameAlert(2);
            }
            else
            {
                AnalyticsService.Track(AnalyticsConstants.StartGameButtonTapped, AnalyticsConstants.StartGameButtonTappedStatus, AnalyticsConstants.GameStarted);
                Device.BeginInvokeOnMainThread(async () => await Navigation.PushAsync(new FaceOffPage()));
            }
        }
    }
}
