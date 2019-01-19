using System.Linq;

using NUnit.Framework;

using Xamarin.UITest;
using Xamarin.UITest.Android;

using FaceOff.Shared;

namespace FaceOff.UITests
{
    public class FaceOffPageTests : BaseTest
    {
        public FaceOffPageTests(Platform platform) : base(platform)
        {
        }

        public override void TestSetup()
        {
            base.TestSetup();

            WelcomePage.EnterPlayer1Name("First Player");
            WelcomePage.EnterPlayer2Name("Second Player");
            WelcomePage.TapStartGameButton();

            FaceOffPage.WaitForPicturePageToLoad();
        }


        [Test]
        public void TakePictureOne()
        {
            //Arrange

            //Act
            FaceOffPage.TapTakePhoto1Button();
            FaceOffPage.TapOK();

            if (App is AndroidApp)
                return;

            CameraPage.TapPhotoCaptureButton();
            CameraPage.TapUsePhotoButton();

            //Assert
            Assert.IsTrue(FaceOffPage.IsScoreButton1Visible);
        }

        [Test]
        public void TakePictureTwo()
        {
            //Arrange

            //Act
            FaceOffPage.TapTakePhoto2Button();
            FaceOffPage.TapOK();

            if (App is AndroidApp)
                return;

            CameraPage.TapPhotoCaptureButton();
            CameraPage.TapUsePhotoButton();

            //Assert
            Assert.IsTrue(FaceOffPage.IsScoreButton2Visible);
        }

        [Test]
        public void VerifyResetButton()
        {
            //Arrange
            string firstEmotion = FaceOffPage.Emotion;
            string secondEmotion;

            //Act
            if (App is AndroidApp)
                return;

            FaceOffPage.TapTakePhoto1Button();
            FaceOffPage.TapOK();

            CameraPage.TapPhotoCaptureButton();
            CameraPage.TapUsePhotoButton();

            FaceOffPage.WaitForPhotoImage1();
            FaceOffPage.TapResetButton();

            //Assert
            secondEmotion = FaceOffPage.Emotion;
            Assert.AreNotEqual(firstEmotion, secondEmotion);
        }

        [TestCase(EmotionType.Anger, 100)]
        [TestCase(EmotionType.Contempt, 83.5)]
        [TestCase(EmotionType.Happiness, 100)]
        [TestCase(EmotionType.Sadness, 100)]
        public void VerifyPhoto1Results(EmotionType emotion, double expectedScore)
        {
            //Arrange 

            //Act
            FaceOffPage.SubmitImageForPhoto1(emotion);
            App.Screenshot($"Player 1 Image Submitted: {emotion.ToString()}");

            //Assert
            Assert.AreEqual(EmotionConstants.EmotionDictionary[emotion], FaceOffPage.Emotion);
            Assert.IsTrue(FaceOffPage.ScoreButton1Text.Contains(expectedScore.ToString()));

            //Act
            FaceOffPage.TapScoreButton1();
            FaceOffPage.WaitForResultsPopup();

            //Assert
            var doesPopupContainCorrectResults = App.Query().Any(x => x?.Text?.Contains($"{EmotionConstants.EmotionDictionary[emotion]}: {expectedScore.ToString()}") ?? false);
            Assert.IsTrue(doesPopupContainCorrectResults);
        }

        [TestCase(EmotionType.Anger, 100)]
        [TestCase(EmotionType.Contempt, 83.5)]
        [TestCase(EmotionType.Happiness, 100)]
        [TestCase(EmotionType.Sadness, 100)]
        public void VerifyPhoto2Results(EmotionType emotion, double expectedScore)
        {
            //Arrange

            //Act
            FaceOffPage.SubmitImageForPhoto2(emotion);
            App.Screenshot($"Player 2 Image Submitted: {emotion.ToString()}");

            //Assert
            Assert.AreEqual(EmotionConstants.EmotionDictionary[emotion], FaceOffPage.Emotion);
            Assert.IsTrue(FaceOffPage.ScoreButton2Text.Contains(expectedScore.ToString()));

            //Act
            FaceOffPage.TapScoreButton2();
            FaceOffPage.WaitForResultsPopup();

            //Assert
            var doesPopupContainCorrectResults = App.Query().Any(x => x?.Text?.Contains($"{EmotionConstants.EmotionDictionary[emotion]}: {expectedScore.ToString()}") ?? false);
            Assert.IsTrue(doesPopupContainCorrectResults);
        }
    }
}

