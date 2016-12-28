using System;

using Xamarin.UITest;
using Xamarin.UITest.Queries;

using FaceOff.Shared;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace FaceOff.UITests
{
	public class PicturePage : BasePage
	{
		#region Constant Fields
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
		#endregion

		#region Constructors
		public PicturePage(IApp app, Platform platform) : base(app, platform)
		{
			EmotionLabel = x => x.Marked(AutomationIdConstants.EmotionLabel);

			Photo1ActivityIndicator = x => x.Marked(AutomationIdConstants.Photo1ActivityIndicator);
			Photo2ActivityIndicator = x => x.Marked(AutomationIdConstants.Photo2ActivityIndicator);

			PhotoImage1 = x => x.Marked(AutomationIdConstants.PhotoImage1);
			PhotoImage2 = x => x.Marked(AutomationIdConstants.PhotoImage2);

			ResetButton = x => x.Marked(AutomationIdConstants.ResetButton);

			ScoreButton1 = x => x.Marked(AutomationIdConstants.ScoreButton1);
			ScoreButton2 = x => x.Marked(AutomationIdConstants.ScoreButton2);

			TakePhoto1Button = x => x.Marked(AutomationIdConstants.TakePhoto1Button);
			TakePhoto2Button = x => x.Marked(AutomationIdConstants.TakePhoto2Button);
		}
		#endregion

		#region Properties
		public string Emotion =>
			GetEmotionUsingBackdoors();

		public bool IsScoreButton1Visible =>
			ScoreButton1Query().Length > 0;

		public bool IsScoreButton2Visible =>
			ScoreButton2Query().Length > 0;
		#endregion

		#region Methods
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

		public void WaitForPhotoImage1()
		{
			app.WaitForElement(PhotoImage1);
		}

		public void WaitForPhotoImage2()
		{
			app.WaitForElement(PhotoImage2);
		}

		public void WaitForPicturePageToLoad()
		{
			app.WaitForElement(TakePhoto1Button);
		}

		public void TapOK()
		{
			app.Tap("OK");
			app.Screenshot("Tapped OK");
		}

		public void TapCancel()
		{
			app.Tap("Cancel");
			app.Screenshot("Tapped Cancel");
		}

		string GetEmotionUsingBackdoors()
		{
			if (IsiOS)
				return app.Invoke("getPicturePageTitle:", "").ToString();

			return app.Invoke("GetPicturePageTitle").ToString();
		}

		AppResult[] ScoreButton1Query()
		{
			app.WaitForElement(ScoreButton1, "Score Button 1 Did Not Appear", new TimeSpan(0, 0, 5));
			return app.Query(ScoreButton1);
		}

		AppResult[] ScoreButton2Query()
		{
			app.WaitForElement(ScoreButton2, "Score Button 2 Did Not Appear", new TimeSpan(0, 0, 5));
			return app.Query(ScoreButton2);
		}
		#endregion
	}
}

