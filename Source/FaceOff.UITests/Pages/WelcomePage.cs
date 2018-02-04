using System.Linq;

using Xamarin.UITest;
using Xamarin.UITest.Queries;

using FaceOff.Shared;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace FaceOff.UITests
{
    public class WelcomePage : BasePage
    {
        #region Constant Fields
        readonly Query _player1Entry, _player2Entry, _startGameButton;
		#endregion

		#region Constructors
		public WelcomePage(IApp app, Platform platform) : base(app, platform)
        {
            _player1Entry = x => x.Marked(AutomationIdConstants.Player1Entry);
            _player2Entry = x => x.Marked(AutomationIdConstants.Player2Entry);
            _startGameButton = x => x.Marked(AutomationIdConstants.StartGameButton);
        }
        #endregion

        #region Properties
        public string Player1EntryPlaceholderText =>
            GetPlaceholderText(AutomationIdConstants.Player1Entry);

        public string Player2EntryPlaceholderText =>
            GetPlaceholderText(AutomationIdConstants.Player2Entry);

        public bool IsErrorMessageDisplayed =>
            GetErrorMessageQuery().Any();
        #endregion

        #region Methods
        public void WaitForPageToLoad() =>
            App.WaitForElement("Face Off");

        public void EnterPlayer1Name(string name)
        {
            App.Tap(_player1Entry);
            App.EnterText(name);
            App.DismissKeyboard();
            App.Screenshot("Entered Player 1 Name");
        }

        public void EnterPlayer2Name(string name)
        {
            App.Tap(_player2Entry);
            App.EnterText(name);
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

        string GetPlaceholderText(string entryAutomationId)
        {
            if (IsAndroid)
                return App.Query(x => x.Marked(entryAutomationId)?.Invoke("getHint"))?.FirstOrDefault()?.ToString();

            return App.Query(x => x.Marked(entryAutomationId)?.Invoke("placeholder"))?.FirstOrDefault()?.ToString();
        }
        #endregion
    }
}

