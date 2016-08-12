using Xamarin.UITest;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;
namespace FaceOff.UITests
{
	public class PicturePage : BasePage
	{
		readonly Query EmotionLabel;

		readonly Query Photo1ActivityIndicator;
		readonly Query Photo2ActivityIndicator;
		 
		readonly Query PhotoImage1;
		readonly Query PhotoImage2;
		 
		readonly Query ResetButton;
		 
		readonly Query ScoreButton1;
		readonly Query ScoreButton2;
		 
		readonly Query TakePhoto1Button;
		readonly Query TakePhoto2Button;

		public PicturePage(IApp app, Platform platform) : base(app, platform)
		{
			EmotionLabel = x => x.Marked(AutomationIdConstants.EmotionLabelAutomationId);

			Photo1ActivityIndicator = x => x.Marked(AutomationIdConstants.Photo1ActivityIndicatorAutomationId);
			Photo2ActivityIndicator = x => x.Marked(AutomationIdConstants.Photo2ActivityIndicatorAutomationId);

			PhotoImage1 = x => x.Marked(AutomationIdConstants.PhotoImage1AutomationId);
			PhotoImage2 = x => x.Marked(AutomationIdConstants.PhotoImage2AutomationId);

			ResetButton = x => x.Marked(AutomationIdConstants.ResetButtonAutomationId);

			ScoreButton1 = x => x.Marked(AutomationIdConstants.ScoreButton1AutomationId);
			ScoreButton2 = x => x.Marked(AutomationIdConstants.ScoreButton2AutomationId);

			TakePhoto1Button = x => x.Marked(AutomationIdConstants.TakePhoto1ButtonAutomationId);
			TakePhoto2Button = x => x.Marked(AutomationIdConstants.TakePhoto2ButtonAutomationId);
		}

		public void TapResetButton()
		{
			app.ScrollDownTo(ResetButton);
			app.Tap(ResetButton);
			app.Screenshot("Tapped Reset Button");
		}

		public void TapScoreButton1()
		{
			app.ScrollDownTo(ScoreButton1);
			app.Tap(ScoreButton1);
			app.Screenshot("Tapped Score Button 1");
		}

		public void TapScoreButton2()
		{
			app.ScrollDownTo(ScoreButton2);
			app.Tap(ScoreButton2);
			app.Screenshot("Tapped Score Button 2");
		}

		public void TapTakePhoto1Button()
		{
			app.ScrollDownTo(TakePhoto1Button);
			app.Tap(TakePhoto1Button);
			app.Screenshot("Tapped Take Photo 1 Button");
		}

		public void TapTakePhoto2Button()
		{
			app.ScrollDownTo(TakePhoto2Button);
			app.Tap(TakePhoto2Button);
			app.Screenshot("Tapped Take Photo 2 Button");
		}

		public void WaitForNoPhoto1ActivityIndicator()
		{
			app.WaitForNoElement(Photo1ActivityIndicator);
		}

		public void WaitForNoPhoto2ActivityIndicator()
		{
			app.WaitForNoElement(Photo2ActivityIndicator);
		}

		public string GetEmotion()
		{
			return app.Query(EmotionLabel)[0]?.Text;
		}
	}
}

