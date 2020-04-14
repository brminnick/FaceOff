using System;
using System.Threading.Tasks;
using AsyncAwaitBestPractices.MVVM;
using FaceOff.Shared;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FaceOff
{
    class WelcomePage : BaseContentPage<WelcomeViewModel>
    {
        readonly Entry _player1Entry, _player2Entry;

        public WelcomePage()
        {
            var player1Label = new DarkBlueLabel("Player 1");
            var player2Label = new DarkBlueLabel("Player 2");

            _player1Entry = new WelcomePageEntry(AutomationIdConstants.Player1Entry)
            {
                ReturnType = ReturnType.Next
            };
            _player1Entry.SetBinding(Entry.TextProperty, nameof(WelcomeViewModel.Player1));

            _player2Entry = new WelcomePageEntry(AutomationIdConstants.Player2Entry)
            {
                ReturnType = ReturnType.Go,
                ReturnCommand = new AsyncCommand(StartGame)
            };
            _player2Entry.SetBinding(Entry.TextProperty, nameof(WelcomeViewModel.Player2));

            var startGameButton = new BounceButton(AutomationIdConstants.StartGameButton)
            {
                Text = "Start",
                Margin = new Thickness(0, 20, 0, 0),
            };
            startGameButton.Clicked += HandleStartGameButtonClicked;

            NavigationPage.SetBackButtonTitle(this, "");

            Title = "FaceOff";

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

        async void HandleStartGameButtonClicked(object sender, EventArgs e) => await StartGame();

        async Task StartGame()
        {
            var isPlayer1EntryTextEmpty = string.IsNullOrWhiteSpace(_player1Entry.Text);
            var isPlayer2EntryTextEmpty = string.IsNullOrWhiteSpace(_player2Entry.Text);

            if (isPlayer1EntryTextEmpty)
            {
                AnalyticsService.Track(AnalyticsConstants.StartGameButtonTapped, AnalyticsConstants.StartGameButtonTappedStatus, AnalyticsConstants.Player1NameEmpty);
                await DisplayEmptyPlayerNameAlert(1);
            }
            else if (isPlayer2EntryTextEmpty)
            {
                AnalyticsService.Track(AnalyticsConstants.StartGameButtonTapped, AnalyticsConstants.StartGameButtonTappedStatus, AnalyticsConstants.Player2NameEmpty);
                await DisplayEmptyPlayerNameAlert(2);
            }
            else
            {
                AnalyticsService.Track(AnalyticsConstants.StartGameButtonTapped, AnalyticsConstants.StartGameButtonTappedStatus, AnalyticsConstants.GameStarted);
                await MainThread.InvokeOnMainThreadAsync(() => Navigation.PushAsync(new FaceOffPage()));
            }

            Task DisplayEmptyPlayerNameAlert(int playerNumber) =>
                MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("Error", $"Player {playerNumber} Name is Blank", "OK"));
        }

        class WelcomePageEntry : Entry
        {
            public WelcomePageEntry(in string automationId)
            {
                ClearButtonVisibility = ClearButtonVisibility.WhileEditing;
                BackgroundColor = Device.RuntimePlatform is Device.iOS ? Color.White : default;
                AutomationId = automationId;
                Placeholder = PlaceholderConstants.WelcomePagePlaceholderText;
                TextColor = Color.Black;
            }
        }
    }
}
