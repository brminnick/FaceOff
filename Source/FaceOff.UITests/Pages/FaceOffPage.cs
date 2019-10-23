using System;
using System.Linq;

using Newtonsoft.Json;

using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.iOS;
using Xamarin.UITest.Queries;

using FaceOff.Shared;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace FaceOff.UITests
{
    public class FaceOffPage : BasePage
    {
        readonly Query _emotionLabel, _photo1ActivityIndicator, _photo2ActivityIndicator,
            _photoImage1, _photoImage2, _resetButton, _scoreButton1, _scoreButton2,
            _takePhoto1Button, _takePhoto2Button, _player1NameLabel, _player2NameLabel;

        public FaceOffPage(IApp app) : base(app)
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

            _player1NameLabel = x => x.Marked(AutomationIdConstants.Player1NameLabel);
            _player2NameLabel = x => x.Marked(AutomationIdConstants.Player2NameLabel);
        }

        public string Emotion => GetEmotionUsingBackdoors();
        public string ScoreButton1Text => App.Query(_scoreButton1)?.FirstOrDefault()?.Text ?? App.Query(_scoreButton1).First().Label;
        public string ScoreButton2Text => App.Query(_scoreButton2)?.FirstOrDefault()?.Text ?? App.Query(_scoreButton2).First().Label;
        public string Player1Name => App.Query(_player1NameLabel)?.FirstOrDefault()?.Text ?? App.Query(_player1NameLabel).First().Label;
        public string Player2Name => App.Query(_player2NameLabel)?.FirstOrDefault()?.Text ?? App.Query(_player2NameLabel).First().Label;
        public bool IsScoreButton1Visible => App.Query(_scoreButton1).Any();
        public bool IsScoreButton2Visible => App.Query(_scoreButton2).Any();
        public bool IsPhotoImage1Visible => App.Query(_photoImage1).Any();
        public bool IsPhotoImage2Visible => App.Query(_photoImage2).Any();

        public void WaitForScoreButton1() => App.WaitForElement(_scoreButton1);
        public void WaitForScoreButton2() => App.WaitForElement(_scoreButton2);
        public void WaitForNoPhoto1ActivityIndicator() => App.WaitForNoElement(_photo1ActivityIndicator);
        public void WaitForNoPhoto2ActivityIndicator() => App.WaitForNoElement(_photo2ActivityIndicator);
        public void WaitForPhotoImage1() => App.WaitForElement(_photoImage1);
        public void WaitForPhotoImage2() => App.WaitForElement(_photoImage2);
        public void WaitForPicturePageToLoad() => App.WaitForElement(_takePhoto1Button);
        public void WaitForResultsPopup() => App.WaitForElement("Results");

        public bool DoesResultsPopupContainExpectedResults(EmotionType emotion, double expectedScore)
        {
            WaitForResultsPopup();
            return App.Query().Any(x => x?.Text?.Contains($"{EmotionConstants.EmotionDictionary[emotion]}: {expectedScore.ToString()}") ?? false);
        }

        public void TapResetButton()
        {
            App.Tap(_resetButton);
            App.Screenshot("Tapped Reset Button");
        }

        public void TapScoreButton1()
        {
            App.Tap(_scoreButton1);
            App.Screenshot("Tapped Score Button 1");
        }

        public void TapScoreButton2()
        {
            App.Tap(_scoreButton2);
            App.Screenshot("Tapped Score Button 2");
        }

        public void TapTakePhoto1Button()
        {
            App.Tap(_takePhoto1Button);
            App.Screenshot("Tapped Take Photo 1 Button");
        }

        public void TapTakePhoto2Button()
        {
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

        public void SubmitImageForPhoto1(EmotionType emotion)
        {
            var playerEmotionModel = new PlayerEmotionModel(Player1Name, emotion);
            var serializedInput = JsonConvert.SerializeObject(playerEmotionModel);

            switch (App)
            {
                case iOSApp iOSApp:
                    iOSApp.Invoke("submitImageForPhoto1:", serializedInput);
                    break;
                case AndroidApp androidApp:
                    androidApp.Invoke("SubmitImageForPhoto1", serializedInput);
                    break;
                default:
                    throw new NotSupportedException();
            }

            App.WaitForElement(_photoImage1);
        }

        public void SubmitImageForPhoto2(EmotionType emotion)
        {
            var playerEmotionModel = new PlayerEmotionModel(Player2Name, emotion);
            var serializedInput = JsonConvert.SerializeObject(playerEmotionModel);

            switch (App)
            {
                case iOSApp iOSApp:
                    iOSApp.Invoke("submitImageForPhoto2:", serializedInput);
                    break;
                case AndroidApp androidApp:
                    androidApp.Invoke("SubmitImageForPhoto2", serializedInput);
                    break;

                default:
                    throw new NotSupportedException();
            }

            App.WaitForElement(_photoImage2);
        }

        string GetEmotionUsingBackdoors() => App switch
        {
            iOSApp iOSApp => iOSApp.Invoke("getPicturePageTitle:", "").ToString(),
            AndroidApp androidApp => androidApp.Invoke("GetPicturePageTitle").ToString(),
            _ => throw new NotSupportedException(),
        };
    }
}

