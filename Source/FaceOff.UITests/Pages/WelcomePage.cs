using System.Linq;

using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;

using FaceOff.Shared;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;
using Xamarin.UITest.iOS;
using System;

namespace FaceOff.UITests
{
    public class WelcomePage : BasePage
    {
        readonly Query _player1Entry, _player2Entry, _startGameButton;

        public WelcomePage(IApp app) : base(app)
        {
            _player1Entry = x => x.Marked(AutomationIdConstants.Player1Entry);
            _player2Entry = x => x.Marked(AutomationIdConstants.Player2Entry);
            _startGameButton = x => x.Marked(AutomationIdConstants.StartGameButton);
        }

        public string Player1EntryPlaceholderText => GetPlaceholderText(AutomationIdConstants.Player1Entry);
        public string Player2EntryPlaceholderText => GetPlaceholderText(AutomationIdConstants.Player2Entry);
        public bool IsErrorMessageDisplayed => GetErrorMessageQuery().Any();

        public void WaitForPageToLoad() => App.WaitForElement("FaceOff");
        public void ClearPlayer1EntryText() => App.ClearText(_player1Entry);
        public void ClearPlayer2EntryText() => App.ClearText(_player2Entry);

        public void EnterPlayer1Name(string name)
        {
            App.ClearText(_player1Entry);
            App.EnterText(_player1Entry, name);
            App.DismissKeyboard();
            App.Screenshot("Entered Player 1 Name");
        }

        public void EnterPlayer2Name(string name)
        {
            App.ClearText(_player2Entry);
            App.EnterText(_player2Entry, name);
            App.DismissKeyboard();
            App.Screenshot("Entered Player 1 Name");
        }

        public void TapStartGameButton()
        {
            App.Tap(_startGameButton);
            App.Screenshot("Start Game Button Tapped");
        }

        AppResult[] GetErrorMessageQuery()
        {
            App.WaitForElement("Error");
            return App.Query("Error");
        }

        string GetPlaceholderText(string entryAutomationId) => App switch
        {
            AndroidApp androidApp => androidApp.Query(x => x.Marked(entryAutomationId).Invoke("getHint")).First().ToString(),
            iOSApp iosApp => iosApp.Query(x => x.Marked(entryAutomationId).Invoke("placeholder")).First().ToString(),
            _ => throw new NotSupportedException(),
        };
    }
}

