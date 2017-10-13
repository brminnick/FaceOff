using System;

using Xamarin.Forms;

namespace FaceOff
{
    public class BounceButton : Button
    {
        #region Fields
        bool _isBounceButtonAnimationInProgress = false;
        #endregion

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
            if (_isBounceButtonAnimationInProgress)
                return;

            var bounceButton = (BounceButton)sender;
            _isBounceButtonAnimationInProgress = true;

            Device.BeginInvokeOnMainThread(async () =>
            {
                Unfocus();
                await bounceButton?.ScaleTo(AnimationConstants.BounceButtonMaxSize, AnimationConstants.BounceButonAninmationTime);
                await bounceButton?.ScaleTo(AnimationConstants.BounceButtonNormalSize, AnimationConstants.BounceButonAninmationTime);
                _isBounceButtonAnimationInProgress = false;
            });
        }
        #endregion
    }
}

