using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using FaceOff.Shared;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using static Xamarin.Forms.Markup.GridLengths;
using static Xamarin.Forms.Markup.GridRowsColumns;

namespace FaceOff
{
    class WelcomePage : BaseContentPage<WelcomeViewModel>
    {
        const int _labelRowHeight = 20;
        const int _entryRowHeight = 45;
        const int _buttonHeight = 40;
        const int _buttonVerticalMargin = 20;

        readonly Entry _player1Entry, _player2Entry;

        public WelcomePage()
        {
            NavigationPage.SetBackButtonTitle(this, "");

            Title = "FaceOff";

            Content = new Grid
            {
                Padding = 20,

                RowDefinitions = Rows.Define(
                        (Row.Player1Label, AbsoluteGridLength(_labelRowHeight)),
                        (Row.Player1Entry, AbsoluteGridLength(_entryRowHeight)),
                        (Row.Player2Label, AbsoluteGridLength(_labelRowHeight)),
                        (Row.Player2Entry, AbsoluteGridLength(_entryRowHeight)),
                        (Row.Start, AbsoluteGridLength(_buttonHeight + _buttonVerticalMargin * 2))),

                Children =
                {
                    new DarkBlueLabel("Player 1").Row(Row.Player1Label),

                    new WelcomePageEntry(AutomationIdConstants.Player1Entry, ReturnType.Next).Assign(out _player1Entry).Row(Row.Player1Entry)
                        .Bind(Entry.TextProperty, nameof(WelcomeViewModel.Player1)),

                    new DarkBlueLabel("Player 2").Row(Row.Player2Label),

                    new WelcomePageEntry(AutomationIdConstants.Player2Entry, ReturnType.Go, new AsyncCommand(StartGame)).Assign(out _player2Entry).Row(Row.Player2Entry)
                        .Bind(Entry.TextProperty, nameof(WelcomeViewModel.Player2)),

                    new BounceButton(AutomationIdConstants.StartGameButton) { Text = "Start" }.Row(Row.Start).Margin(new Thickness(0, _buttonVerticalMargin))
                        .Invoke(startGameButton => startGameButton.Clicked += HandleStartGameButtonClicked)
                }
            };
        }

        enum Row { Player1Label, Player1Entry, Player2Label, Player2Entry, Start }

        async void HandleStartGameButtonClicked(object sender, EventArgs e) => await StartGame();

        Task StartGame()
        {
            var isPlayer1EntryTextEmpty = string.IsNullOrWhiteSpace(_player1Entry.Text);
            var isPlayer2EntryTextEmpty = string.IsNullOrWhiteSpace(_player2Entry.Text);

            if (isPlayer1EntryTextEmpty)
            {
                AnalyticsService.Track(AnalyticsConstants.StartGameButtonTapped, AnalyticsConstants.StartGameButtonTappedStatus, AnalyticsConstants.Player1NameEmpty);
                return DisplayEmptyPlayerNameAlert(1);
            }
            else if (isPlayer2EntryTextEmpty)
            {
                AnalyticsService.Track(AnalyticsConstants.StartGameButtonTapped, AnalyticsConstants.StartGameButtonTappedStatus, AnalyticsConstants.Player2NameEmpty);
                return DisplayEmptyPlayerNameAlert(2);
            }
            else
            {
                AnalyticsService.Track(AnalyticsConstants.StartGameButtonTapped, AnalyticsConstants.StartGameButtonTappedStatus, AnalyticsConstants.GameStarted);
                return MainThread.InvokeOnMainThreadAsync(() => Navigation.PushAsync(new FaceOffPage()));
            }

            Task DisplayEmptyPlayerNameAlert(int playerNumber) =>
                MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("Error", $"Player {playerNumber} Name is Blank", "OK"));
        }

        class WelcomePageEntry : Entry
        {
            public WelcomePageEntry(in string automationId, in ReturnType returnType, in ICommand? returnCommand = null)
            {
                AutomationId = automationId;

                ReturnType = returnType;
                ReturnCommand = returnCommand;

                TextColor = Color.Black;
                Margin = new Thickness(0, 0, 0, 5);
                BackgroundColor = Device.RuntimePlatform is Device.iOS ? Color.White : default;

                Placeholder = PlaceholderConstants.WelcomePagePlaceholderText;
                ClearButtonVisibility = ClearButtonVisibility.WhileEditing;
            }
        }
    }
}
