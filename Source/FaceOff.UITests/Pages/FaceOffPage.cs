using System;

using Xamarin.UITest;
using Xamarin.UITest.Queries;

using FaceOff.Shared;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace FaceOff.UITests
{
    public class FaceOffPage : BasePage
    {
        #region Constant Fields
        readonly Query _emotionLabel, _photo1ActivityIndicator, _photo2ActivityIndicator,
            _photoImage1, _photoImage2, _resetButton, _scoreButton1, _scoreButton2,
            _takePhoto1Button, _takePhoto2Button;
        #endregion

        #region Constructors
        public FaceOffPage(IApp app, Platform platform) : base(app, platform)
        {
            _emotionLabel = x => x.Marked(AutomationIdConstants.EmotionLabel);

            _photo1ActivityIndicator = x => x.Marked(AutomationIdConstants.Photo1ActivityIndicator);
            _photo2ActivityIndicator = x => x.Marked(AutomationIdConstants.Photo2ActivityIndicator);

            _photoImage1 = x => x.Marked(AutomationIdConstants.PhotoImage1);
            _photoImage2 = x => x.Marked(AutomationIdConstants.PhotoImage2);

            _resetButton = x => x.Marked(AutomationIdConstants.ResetButton);

            _scoreButton1 = x => x.Marked(AutomationIdConstants.ScoreButton1);
            _scoreButton2 = x => x.Marked(AutomationIdConstants.ScoreButton2);

            _takePhoto1Button = x => x.Marked(AutomationIdConstants.TakePhoto1Button);
            _takePhoto2Button = x => x.Marked(AutomationIdConstants.TakePhoto2Button);
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
        public void WaitForNoPhoto1ActivityIndicator() =>
            App.WaitForNoElement(_photo1ActivityIndicator);

        public void WaitForNoPhoto2ActivityIndicator() =>
            App.WaitForNoElement(_photo2ActivityIndicator);

        public void WaitForPhotoImage1() =>
            App.WaitForElement(_photoImage1);

        public void WaitForPhotoImage2() =>
            App.WaitForElement(_photoImage2);

        public void WaitForPicturePageToLoad() =>
            App.WaitForElement(_takePhoto1Button);

        public void TapResetButton()
        {
            App.ScrollDownTo(_resetButton);
            App.Tap(_resetButton);
            App.Screenshot("Tapped Reset Button");
        }

        public void TapScoreButton1()
        {
            App.ScrollDownTo(_scoreButton1);
            App.Tap(_scoreButton1);
            App.Screenshot("Tapped Score Button 1");
        }

        public void TapScoreButton2()
        {
            App.ScrollDownTo(_scoreButton2);
            App.Tap(_scoreButton2);
            App.Screenshot("Tapped Score Button 2");
        }

        public void TapTakePhoto1Button()
        {
            App.ScrollDownTo(_takePhoto1Button);
            App.Tap(_takePhoto1Button);
            App.Screenshot("Tapped Take Photo 1 Button");
        }

        public void TapTakePhoto2Button()
        {
            App.ScrollDownTo(_takePhoto2Button);
            App.Tap(_takePhoto2Button);
            App.Screenshot("Tapped Take Photo 2 Button");
        }

        public void TapOK()
        {
            App.Tap("OK");
            App.Screenshot("Tapped OK");
        }

        public void TapCancel()
        {
            App.Tap("Cancel");
            App.Screenshot("Tapped Cancel");
        }

        string GetEmotionUsingBackdoors()
        {
            if (IsiOS)
                return App.Invoke("getPicturePageTitle:", "").ToString();

            return App.Invoke("GetPicturePageTitle").ToString();
        }

        AppResult[] ScoreButton1Query()
        {
            App.WaitForElement(_scoreButton1, "Score Button 1 Did Not Appear", new TimeSpan(0, 0, 5));
            return App.Query(_scoreButton1);
        }

        AppResult[] ScoreButton2Query()
        {
            App.WaitForElement(_scoreButton2, "Score Button 2 Did Not Appear", new TimeSpan(0, 0, 5));
            return App.Query(_scoreButton2);
        }
        #endregion
    }
}

