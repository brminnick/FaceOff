using System;

using Xamarin.Forms;

namespace FaceOff
{
	public class BounceButton : Button
	{
		public BounceButton() : base()
		{
			Clicked += HandleButtonClick;
			Style = StylesConstants.ButtonStyle;
		}

		public BounceButton(string automationId) : base()
		{
			Clicked += HandleButtonClick;
			Style = StylesConstants.ButtonStyle;
			AutomationId = automationId;
		}

		void HandleButtonClick(object sender, EventArgs e)
		{
			if (App.IsBounceButtonAnimationInProgress)
				return;

			var bounceButton = (BounceButton)sender;
			App.IsBounceButtonAnimationInProgress = true;

			Device.BeginInvokeOnMainThread(async () =>
			{
				Unfocus();
				await bounceButton?.ScaleTo(AnimationConstants.BounceButtonMaxSize, AnimationConstants.BounceButonAninmationTime);
				await bounceButton?.ScaleTo(AnimationConstants.BounceButtonNormalSize, AnimationConstants.BounceButonAninmationTime);
				App.IsBounceButtonAnimationInProgress = false;
			});
		}
	}
}

