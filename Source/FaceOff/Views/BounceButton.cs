using System;

using Xamarin.Forms;

namespace FaceOff
{
    public class BounceButton : Button
    {
        #region Constructors
        public BounceButton()
        {
            Clicked += HandleButtonClick;
            Style = StylesConstants.ButtonStyle;
        }

        public BounceButton(string automationId) : this() => AutomationId = automationId;
        #endregion

        #region Finalizers
        ~BounceButton() => Clicked -= HandleButtonClick;
        #endregion

        #region Methods
        void HandleButtonClick(object sender, EventArgs e)
        {
            var bounceButton = (BounceButton)sender;

            Device.BeginInvokeOnMainThread(async () =>
            {
                Unfocus();
                await bounceButton?.ScaleTo(AnimationConstants.BounceButtonMaxSize, AnimationConstants.BounceButonAninmationTime);
                await bounceButton?.ScaleTo(AnimationConstants.BounceButtonNormalSize, AnimationConstants.BounceButonAninmationTime);
            });
        }
        #endregion
    }
}

