using Xamarin.UITest;

using FaceOff.Shared;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;
using System.Linq;
using Xamarin.UITest.Queries;

namespace FaceOff.UITests
{
	public class WelcomePage : BasePage
	{
		#region Constant Fields
		readonly Query Player1Entry;
		readonly Query Player2Entry;
		readonly Query StartGameButton;
		#endregion

		#region Constructors
		public WelcomePage(IApp app, Platform platform) : base(app, platform)
		{
			Player1Entry = x => x.Marked(AutomationIdConstants.Player1Entry);
			Player2Entry = x => x.Marked(AutomationIdConstants.Player2Entry);
			StartGameButton = x => x.Marked(AutomationIdConstants.StartGameButton);
		}
		#endregion

		#region Properties
		public string Player1EntryPlaceholderText =>
			GetPlaceholderText(AutomationIdConstants.Player1Entry);

		public string Player2EntryPlaceholderText =>
			GetPlaceholderText(AutomationIdConstants.Player2Entry);

		public bool IsErrorMessageDisplayed =>
			GetErrorMessageQuery().Length > 0;
		#endregion

		#region Methods
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

		public void WaitForPageToLoad()
		{
			app.WaitForElement("Face Off");
		}

		AppResult[] GetErrorMessageQuery()
		{
			app.WaitForElement("Error");
			return app.Query("Error");
		}

		string GetPlaceholderText(string entryAutomationId)
		{
			if (IsAndroid)
			{
				return app.Query(x => x.Marked(entryAutomationId).Invoke("getHint")).FirstOrDefault().ToString();
			}

			return app.Query(x => x.Marked(entryAutomationId).Invoke("placeholder")).FirstOrDefault().ToString();
		}
		#endregion
	}
}

