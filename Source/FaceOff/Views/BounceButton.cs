using System;
using Xamarin.Forms;

namespace FaceOff
{
    class BounceButton : Button
    {
        public BounceButton(in string automationId)
        {
            AutomationId = automationId;
            TextColor = ColorConstants.ButtonTextColor;
            BackgroundColor = ColorConstants.ButtonBackgroundColor;

            Clicked += HandleButtonClick;
        }

        async void HandleButtonClick(object sender, EventArgs e)
        {
            Unfocus();
            await this.ScaleTo(AnimationConstants.BounceButtonMaxSize, AnimationConstants.BounceButonAninmationTime);
            await this.ScaleTo(AnimationConstants.BounceButtonNormalSize, AnimationConstants.BounceButonAninmationTime);
        }
    }
}

