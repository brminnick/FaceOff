using System;

using Xamarin.Forms;

namespace FaceOff
{
    public class BounceButton : Button
    {
        public BounceButton(string automationId)
        {
            Clicked += HandleButtonClick;
            BackgroundColor = Color.FromHex("3192B3");
            TextColor = Color.White;
            AutomationId = automationId;
        }

        async void HandleButtonClick(object sender, EventArgs e)
        {
            Unfocus();
            await this.ScaleTo(AnimationConstants.BounceButtonMaxSize, AnimationConstants.BounceButonAninmationTime);
            await this.ScaleTo(AnimationConstants.BounceButtonNormalSize, AnimationConstants.BounceButonAninmationTime);
        }
    }
}

