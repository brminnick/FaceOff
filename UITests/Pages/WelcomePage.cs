using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace FaceOff.UITests
{
	public class WelcomePage :BasePage
	{
		readonly Query Player1Entry;
		readonly Query Player2Entry;
		readonly Query StartGameButton;

		public WelcomePage(IApp app, Platform platform) : base(app, platform)
		{
			Player1Entry = x => x.Marked(AutomationIdConstants.Player1Entry);
			Player2Entry = x => x.Marked(AutomationIdConstants.Player2Entry);
			StartGameButton = x => x.Marked(AutomationIdConstants.StartGameButton);
		}

		public void EnterPlayer1Name(string name)
		{
			app.Tap(Player1Entry);
			app.EnterText(name);
			app.DismissKeyboard();
			app.Screenshot("Entered Player 1 Name");
		}

		public void EnterPlayer2Name(string name)
		{
			app.Tap(Player2Entry);
			app.EnterText(name);
			app.DismissKeyboard();
			app.Screenshot("Entered Player 1 Name");
		}

		public void TapStartGameButton()
		{
			app.Tap(StartGameButton);
			app.Screenshot("Start Game Button Tapped");
		}

		public bool IsErrorMessageDisplayed()
		{
			var errorMessageQuery = app.Query("Error");
			return errorMessageQuery.Length > 0;
		}
	}
}

