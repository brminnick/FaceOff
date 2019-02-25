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

            FaceOffPage.WaitForScoreButton1();

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

            FaceOffPage.WaitForScoreButton2();

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
            Assert.IsFalse(FaceOffPage.IsScoreButton1Visible);
            Assert.IsFalse(FaceOffPage.IsScoreButton2Visible);
            Assert.IsFalse(FaceOffPage.IsPhotoImage1Visible);
            Assert.IsFalse(FaceOffPage.IsPhotoImage2Visible);
        }

        [TestCase(EmotionType.Anger, 100)]
        [TestCase(EmotionType.Contempt, 52.7)]
        [TestCase(EmotionType.Disgust, 98.8)]
        [TestCase(EmotionType.Fear, 99.6)]
        [TestCase(EmotionType.Happiness, 100)]
        [TestCase(EmotionType.Neutral, 99.9)]
        [TestCase(EmotionType.Sadness, 100)]
        [TestCase(EmotionType.Surprise, 81.2)]
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
            Assert.IsTrue(FaceOffPage.DoesResultsPopupContainExpectedResults(emotion, expectedScore));
        }

        [TestCase(EmotionType.Anger, 100)]
        [TestCase(EmotionType.Contempt, 52.7)]
        [TestCase(EmotionType.Disgust, 98.8)]
        [TestCase(EmotionType.Fear, 99.6)]
        [TestCase(EmotionType.Happiness, 100)]
        [TestCase(EmotionType.Neutral, 99.9)]
        [TestCase(EmotionType.Sadness, 100)]
        [TestCase(EmotionType.Surprise, 81.2)]
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
            Assert.IsTrue(FaceOffPage.DoesResultsPopupContainExpectedResults(emotion, expectedScore));
        }
    }
}

