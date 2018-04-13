using System;
using System.Text;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;

using Plugin.Media.Abstractions;

using Microsoft.ProjectOxford.Common;
using Microsoft.ProjectOxford.Common.Contract;

using Xamarin;
using Xamarin.Forms;

namespace FaceOff
{
    public class FaceOffViewModel : BaseViewModel
    {
        #region Constant Fields
        readonly Lazy<string[]> _emotionStringsForAlertMessageHolder = new Lazy<string[]>(() =>
            new string[] { "angry", "disrespectful", "disgusted", "scared", "happy", "blank", "sad", "surprised" });

        const string _makeAFaceAlertMessage = "take a selfie looking";
        const string _calculatingScoreMessage = "Analyzing";

        const string _playerNumberNotImplentedExceptionText = "Player Number Not Implemented";
        #endregion

        #region Fields
        ImageSource _photo1ImageSource, _photo2ImageSource;
        string _scoreButton1Text, _scoreButton2Text;
        bool _isTakeLeftPhotoButtonEnabled = true;
        bool _isTakeLeftPhotoButtonStackVisible = true;
        bool _isTakeRightPhotoButtonEnabled = true;
        bool _isTakeRightPhotoButtonStackVisible = true;
        bool _isResetButtonEnabled;
        string _pageTitle;
        bool _isCalculatingPhoto1Score, _isCalculatingPhoto2Score;
        bool _isScore1ButtonEnabled, _isScore2ButtonEnabled, _isScore1ButtonVisable, _isScore2ButtonVisable;
        bool _isPhotoImage1Enabled, _isPhotoImage2Enabled;
        string _photo1Results, _photo2Results;
        EmotionType _currentEmotionType;
        ICommand _takePhoto1ButtonPressed, _takePhoto2ButtonPressed;
        ICommand _photo1ScoreButtonPressed, _photo2ScoreButtonPressed;
        ICommand _resetButtonPressed;
        Command<EmotionPopupResponseModel> _emotionPopUpAlertResponseCommand;
        #endregion

        #region Constructors
        public FaceOffViewModel()
        {
            IsResetButtonEnabled = false;

            SetRandomEmotion();
        }
        #endregion

        #region Properties
        public Command<EmotionPopupResponseModel> EmotionPopUpAlertResponseCommand => _emotionPopUpAlertResponseCommand ??
            (_emotionPopUpAlertResponseCommand = new Command<EmotionPopupResponseModel>(async response => await ExecuteEmotionPopUpAlertResponseCommand(response).ConfigureAwait(false)));

        public ICommand TakePhoto1ButtonPressed => _takePhoto1ButtonPressed ??
            (_takePhoto1ButtonPressed = new Command(ExecuteTakePhoto1ButtonPressed));

        public ICommand TakePhoto2ButtonPressed => _takePhoto2ButtonPressed ??
            (_takePhoto2ButtonPressed = new Command(ExecuteTakePhoto2ButtonPressed));

        public ICommand ResetButtonPressed => _resetButtonPressed ??
            (_resetButtonPressed = new Command(ExecuteResetButtonPressed));

        public ICommand Photo1ScoreButtonPressed => _photo1ScoreButtonPressed ??
            (_photo1ScoreButtonPressed = new Command(ExecutePhoto1ScoreButtonPressed));

        public ICommand Photo2ScoreButtonPressed => _photo2ScoreButtonPressed ??
            (_photo2ScoreButtonPressed = new Command(ExecutePhoto2ScoreButtonPressed));

        public ImageSource Photo1ImageSource
        {
            get => _photo1ImageSource;
            set => SetProperty(ref _photo1ImageSource, value);
        }

        public ImageSource Photo2ImageSource
        {
            get => _photo2ImageSource;
            set => SetProperty(ref _photo2ImageSource, value);
        }

        public bool IsPhotoImage1Enabled
        {
            get => _isPhotoImage1Enabled;
            set => SetProperty(ref _isPhotoImage1Enabled, value);
        }

        public bool IsPhotoImage2Enabled
        {
            get => _isPhotoImage2Enabled;
            set => SetProperty(ref _isPhotoImage2Enabled, value);
        }

        public bool IsTakeLeftPhotoButtonEnabled
        {
            get => _isTakeLeftPhotoButtonEnabled;
            set => SetProperty(ref _isTakeLeftPhotoButtonEnabled, value);
        }

        public bool IsTakeLeftPhotoButtonStackVisible
        {
            get => _isTakeLeftPhotoButtonStackVisible;
            set => SetProperty(ref _isTakeLeftPhotoButtonStackVisible, value);
        }

        public bool IsTakeRightPhotoButtonEnabled
        {
            get => _isTakeRightPhotoButtonEnabled;
            set => SetProperty(ref _isTakeRightPhotoButtonEnabled, value);
        }

        public bool IsTakeRightPhotoButtonStackVisible
        {
            get => _isTakeRightPhotoButtonStackVisible;
            set => SetProperty(ref _isTakeRightPhotoButtonStackVisible, value);
        }

        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }

        public string ScoreButton1Text
        {
            get => _scoreButton1Text;
            set => SetProperty(ref _scoreButton1Text, value);
        }

        public string ScoreButton2Text
        {
            get => _scoreButton2Text;
            set => SetProperty(ref _scoreButton2Text, value);
        }

        public bool IsCalculatingPhoto1Score
        {
            get => _isCalculatingPhoto1Score;
            set => SetProperty(ref _isCalculatingPhoto1Score, value);
        }

        public bool IsCalculatingPhoto2Score
        {
            get => _isCalculatingPhoto2Score;
            set => SetProperty(ref _isCalculatingPhoto2Score, value);
        }

        public bool IsResetButtonEnabled
        {
            get => _isResetButtonEnabled;
            set => SetProperty(ref _isResetButtonEnabled, value);
        }

        public bool IsScore1ButtonEnabled
        {
            get => _isScore1ButtonEnabled;
            set => SetProperty(ref _isScore1ButtonEnabled, value);
        }

        public bool IsScore2ButtonEnabled
        {
            get => _isScore2ButtonEnabled;
            set => SetProperty(ref _isScore2ButtonEnabled, value);
        }

        public bool IsScore1ButtonVisable
        {
            get => _isScore1ButtonVisable;
            set => SetProperty(ref _isScore1ButtonVisable, value);
        }

        public bool IsScore2ButtonVisable
        {
            get => _isScore2ButtonVisable;
            set => SetProperty(ref _isScore2ButtonVisable, value);
        }

        string[] EmotionStringsForAlertMessage => _emotionStringsForAlertMessageHolder.Value;
        #endregion

        #region Events
        public event EventHandler<AlertMessageEventArgs> PopUpAlertAboutEmotionTriggered;
        public event EventHandler<string> AllEmotionResultsAlertTriggered;
        public event EventHandler ScoreButton1RevealTriggered;
        public event EventHandler ScoreButton2RevealTriggered;
        public event EventHandler PhotoImage1RevealTriggered;
        public event EventHandler PhotoImage2RevealTriggered;
        #endregion

        #region Methods
        #region UITest Backdoor Methods
#if DEBUG
        public void SetPhotoImage1ToHappyForUITest(string photo1ImageString)
        {
            Photo1ImageSource = photo1ImageString;

            var allEmotionsStringBuilder = new StringBuilder();
            allEmotionsStringBuilder.AppendLine($"Anger: 0%");
            allEmotionsStringBuilder.AppendLine($"Contempt: 0%");
            allEmotionsStringBuilder.AppendLine($"Disgust: 0%");
            allEmotionsStringBuilder.AppendLine($"Fear: 0%");
            allEmotionsStringBuilder.AppendLine($"Happiness: 100%");
            allEmotionsStringBuilder.AppendLine($"Neutral: 0%");
            allEmotionsStringBuilder.AppendLine($"Sadness: 0%");
            allEmotionsStringBuilder.AppendLine($"Surprise: 0%");

            _photo1Results = allEmotionsStringBuilder.ToString();
            ScoreButton1Text = "Score: 100%";

            SetEmotion(EmotionType.Happiness);

            IsTakeLeftPhotoButtonEnabled = false;
            IsTakeLeftPhotoButtonStackVisible = false;

            OnPhotoImage1RevealTriggered();
            OnScoreButton1RevealTriggered();
        }

        public void SetPhotoImage2ToHappyForUITest(string photo2ImageString)
        {
            Photo2ImageSource = photo2ImageString;

            var allEmotionsStringBuilder = new StringBuilder();
            allEmotionsStringBuilder.AppendLine($"Anger: 0%");
            allEmotionsStringBuilder.AppendLine($"Contempt: 0%");
            allEmotionsStringBuilder.AppendLine($"Disgust: 0%");
            allEmotionsStringBuilder.AppendLine($"Fear: 0%");
            allEmotionsStringBuilder.AppendLine($"Happiness: 100%");
            allEmotionsStringBuilder.AppendLine($"Neutral: 0%");
            allEmotionsStringBuilder.AppendLine($"Sadness: 0%");
            allEmotionsStringBuilder.AppendLine($"Surprise: 0%");

            _photo2Results = allEmotionsStringBuilder.ToString();
            ScoreButton2Text = "Score: 100%";

            SetEmotion(EmotionType.Happiness);

            IsTakeRightPhotoButtonEnabled = false;
            IsTakeRightPhotoButtonStackVisible = false;

            OnPhotoImage2RevealTriggered();
            OnScoreButton2RevealTriggered();
        }
#endif
        #endregion
        void ExecuteTakePhoto1ButtonPressed() =>
            ExecutePopUpAlert(new PlayerModel(PlayerNumberType.Player1, Settings.Player1Name));

        void ExecuteTakePhoto2ButtonPressed() =>
            ExecutePopUpAlert(new PlayerModel(PlayerNumberType.Player2, Settings.Player2Name));

        void ExecutePopUpAlert(PlayerModel playerModel)
        {
            LogPhotoButtonTapped(playerModel.PlayerNumber);
            DisableButtons(playerModel.PlayerNumber);

            var title = EmotionService.EmotionDictionary[_currentEmotionType];
            var message = $"{playerModel.PlayerName}, {_makeAFaceAlertMessage} {EmotionStringsForAlertMessage[(int)_currentEmotionType]}";
            OnPopUpAlertAboutEmotionTriggered(title, message, playerModel);
        }

        async Task ExecuteEmotionPopUpAlertResponseCommand(EmotionPopupResponseModel response)
        {
            var player = response.Player;

            if (response.IsPopUpConfirmed)
                await ExecuteTakePhotoWorkflow(player).ConfigureAwait(false);
            else
                EnableButtons(player.PlayerNumber);
        }

        async Task ExecuteTakePhotoWorkflow(PlayerModel player)
        {
            player.ImageMediaFile = await MediaService.GetMediaFileFromCamera("FaceOff", player.PlayerNumber).ConfigureAwait(false);

            if (player.ImageMediaFile == null)
                EnableButtons(player.PlayerNumber);
            else
                await ExecuteGetPhotoResultsWorkflow(player).ConfigureAwait(false);
        }

        async Task ExecuteGetPhotoResultsWorkflow(PlayerModel player)
        {
            AnalyticsHelpers.Track(AnalyticsConstants.PhotoTaken);

            await ConfigureUIForPendingEmotionResults(player).ConfigureAwait(false);

            var results = await GenerateEmotionResults(player).ConfigureAwait(false);

            await ConfigureUIForFinalizedEmotionResults(player, results).ConfigureAwait(false);
        }

        async Task ConfigureUIForPendingEmotionResults(PlayerModel player)
        {
            RevealPhoto(player.PlayerNumber);

            SetIsEnabledForCurrentPhotoButton(false, player.PlayerNumber);
            SetIsVisibleForCurrentPhotoStack(false, player.PlayerNumber);

            SetScoreButtonText(_calculatingScoreMessage, player.PlayerNumber);

            SetPhotoImageSource(player.ImageMediaFile, player.PlayerNumber);

            SetIsCalculatingPhotoScore(true, player.PlayerNumber);

            SetResetButtonIsEnabled();

            await WaitForAnimationsToFinish((int)Math.Ceiling(AnimationConstants.PhotoImageAninmationTime * 2.5)).ConfigureAwait(false);

            RevealPhotoButton(player.PlayerNumber);
        }

        Task ConfigureUIForFinalizedEmotionResults(PlayerModel player, string results)
        {
            SetPhotoResultsText(results, player.PlayerNumber);

            SetIsCalculatingPhotoScore(false, player.PlayerNumber);

            SetResetButtonIsEnabled();

            player.ImageMediaFile.Dispose();

            return WaitForAnimationsToFinish((int)Math.Ceiling(AnimationConstants.ScoreButonAninmationTime * 2.5));
        }

        async Task<string> GenerateEmotionResults(PlayerModel player)
        {
            Emotion[] emotionArray;
            string emotionScore;

            try
            {
                emotionArray = await EmotionService.GetEmotionResultsFromMediaFile(player.ImageMediaFile, false).ConfigureAwait(false);
                emotionScore = await EmotionService.GetPhotoEmotionScore(emotionArray, 0, _currentEmotionType).ConfigureAwait(false);
            }
            catch (ClientException clientException) when (clientException.HttpStatus.Equals(System.Net.HttpStatusCode.Unauthorized))
            {
                AnalyticsHelpers.Report(clientException);

                emotionArray = null;
                emotionScore = EmotionService.ErrorMessageDictionary[ErrorMessageType.InvalidAPIKey];
            }
            catch (Exception e) when (e.Message.Contains("offline"))
            {
                AnalyticsHelpers.Report(e);

                emotionArray = null;
                emotionScore = EmotionService.ErrorMessageDictionary[ErrorMessageType.DeviceOffline];
            }
            catch (Exception e)
            {
                AnalyticsHelpers.Report(e);

                emotionArray = null;
                emotionScore = EmotionService.ErrorMessageDictionary[ErrorMessageType.ConnectionToCognitiveServicesFailed];
            }

            var doesEmotionScoreContainErrorMessage = EmotionService.DoesStringContainErrorMessage(emotionScore);

            if (doesEmotionScoreContainErrorMessage)
            {
                var errorMessageKey = EmotionService.ErrorMessageDictionary.FirstOrDefault(x => x.Value.Contains(emotionScore)).Key;

                switch (errorMessageKey)
                {
                    case ErrorMessageType.NoFaceDetected:
                        AnalyticsHelpers.Track(EmotionService.ErrorMessageDictionary[ErrorMessageType.NoFaceDetected]);
                        break;
                    case ErrorMessageType.MultipleFacesDetected:
                        AnalyticsHelpers.Track(EmotionService.ErrorMessageDictionary[ErrorMessageType.MultipleFacesDetected]);
                        break;
                    case ErrorMessageType.GenericError:
                        AnalyticsHelpers.Track(EmotionService.ErrorMessageDictionary[ErrorMessageType.MultipleFacesDetected]);
                        break;
                }

                SetScoreButtonText(emotionScore, player.PlayerNumber);
            }
            else
                SetScoreButtonText($"Score: {emotionScore}", player.PlayerNumber);

            return EmotionService.GetStringOfAllPhotoEmotionScores(emotionArray, 0);
        }

        void ExecuteResetButtonPressed()
        {
            AnalyticsHelpers.Track(AnalyticsConstants.ResetButtonTapped);

            SetRandomEmotion();

            Photo1ImageSource = null;
            Photo2ImageSource = null;

            IsTakeLeftPhotoButtonEnabled = true;
            IsTakeLeftPhotoButtonStackVisible = true;

            IsTakeRightPhotoButtonEnabled = true;
            IsTakeRightPhotoButtonStackVisible = true;

            ScoreButton1Text = null;
            ScoreButton2Text = null;

            IsScore1ButtonEnabled = false;
            IsScore2ButtonEnabled = false;

            IsScore1ButtonVisable = false;
            IsScore2ButtonVisable = false;

            _photo1Results = null;
            _photo2Results = null;

            IsPhotoImage1Enabled = false;
            IsPhotoImage2Enabled = false;
        }

        void ExecutePhoto1ScoreButtonPressed()
        {
            AnalyticsHelpers.Track(AnalyticsConstants.ResultsButton1Tapped);
            OnAllEmotionResultsAlertTriggered(_photo1Results);
        }

        void ExecutePhoto2ScoreButtonPressed()
        {
            AnalyticsHelpers.Track(AnalyticsConstants.ResultsButton2Tapped);
            OnAllEmotionResultsAlertTriggered(_photo2Results);
        }

        void SetRandomEmotion()
        {
            _currentEmotionType = EmotionService.GetRandomEmotionType(_currentEmotionType);

            SetPageTitle(_currentEmotionType);
        }

        void SetEmotion(EmotionType emotionType)
        {
            _currentEmotionType = emotionType;

            SetPageTitle(_currentEmotionType);
        }

        void RevealPhoto(PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    OnPhotoImage1RevealTriggered();
                    break;
                case PlayerNumberType.Player2:
                    OnPhotoImage2RevealTriggered();
                    break;
                default:
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetIsEnabledForOppositePhotoButton(bool isEnabled, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    IsTakeRightPhotoButtonEnabled = isEnabled;
                    break;
                case PlayerNumberType.Player2:
                    IsTakeLeftPhotoButtonEnabled = isEnabled;
                    break;
                default:
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetIsEnabledForCurrentPlayerScoreButton(bool isEnabled, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    IsScore1ButtonEnabled = isEnabled;
                    break;
                case PlayerNumberType.Player2:
                    IsScore2ButtonEnabled = isEnabled;
                    break;
                default:
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetIsEnabledForButtons(bool isEnabled, PlayerNumberType playerNumber)
        {
            SetIsEnabledForOppositePhotoButton(isEnabled, playerNumber);
            SetIsEnabledForCurrentPlayerScoreButton(isEnabled, playerNumber);
        }

        void SetIsEnabledForCurrentPhotoButton(bool isEnabled, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    IsTakeLeftPhotoButtonEnabled = isEnabled;
                    break;
                case PlayerNumberType.Player2:
                    IsTakeRightPhotoButtonEnabled = isEnabled;
                    break;
                default:
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetScoreButtonText(string scoreButtonText, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    ScoreButton1Text = scoreButtonText;
                    break;
                case PlayerNumberType.Player2:
                    ScoreButton2Text = scoreButtonText;
                    break;
                default:
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetIsVisibleForCurrentPhotoStack(bool isVisible, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    IsTakeLeftPhotoButtonStackVisible = isVisible;
                    break;
                case PlayerNumberType.Player2:
                    IsTakeRightPhotoButtonStackVisible = isVisible;
                    break;
                default:
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetPhotoImageSource(MediaFile imageMediaFile, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    Photo1ImageSource = ImageSource.FromStream(() =>
                    {
                        return MediaService.GetPhotoStream(imageMediaFile, false);
                    });
                    break;
                case PlayerNumberType.Player2:
                    Photo2ImageSource = ImageSource.FromStream(() =>
                    {
                        return MediaService.GetPhotoStream(imageMediaFile, false);
                    });
                    break;
                default:
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetIsCalculatingPhotoScore(bool isCalculatingScore, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    IsCalculatingPhoto1Score = isCalculatingScore;
                    break;
                case PlayerNumberType.Player2:
                    IsCalculatingPhoto2Score = isCalculatingScore;
                    break;
                default:
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
            }
        }

        void RevealPhotoButton(PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    OnScoreButton1RevealTriggered();
                    break;
                case PlayerNumberType.Player2:
                    OnScoreButton2RevealTriggered();
                    break;
                default:
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetPhotoResultsText(string results, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    _photo1Results = results;
                    break;
                case PlayerNumberType.Player2:
                    _photo2Results = results;
                    break;
                default:
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
            }
        }

        void LogPhotoButtonTapped(PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    AnalyticsHelpers.Track(AnalyticsConstants.PhotoButton1Tapped);
                    break;
                case PlayerNumberType.Player2:
                    AnalyticsHelpers.Track(AnalyticsConstants.PhotoButton2Tapped);
                    break;
                default:
                    throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
            }
        }

        void SetPageTitle(EmotionType emotionType) =>
            PageTitle = EmotionService.EmotionDictionary[emotionType];

        Task WaitForAnimationsToFinish(int waitTimeInSeconds) => Task.Delay(waitTimeInSeconds);

        void EnableButtons(PlayerNumberType playerNumber) =>
            SetIsEnabledForButtons(true, playerNumber);

        void DisableButtons(PlayerNumberType playerNumber) =>
            SetIsEnabledForButtons(false, playerNumber);

        void SetResetButtonIsEnabled() =>
            IsResetButtonEnabled = !(IsCalculatingPhoto1Score || IsCalculatingPhoto2Score);

        void OnAllEmotionResultsAlertTriggered(string emotionResults) =>
            AllEmotionResultsAlertTriggered?.Invoke(this, emotionResults);

        void OnPhotoImage1RevealTriggered() =>
            PhotoImage1RevealTriggered?.Invoke(this, EventArgs.Empty);

        void OnScoreButton1RevealTriggered() =>
            ScoreButton1RevealTriggered?.Invoke(this, EventArgs.Empty);

        void OnPhotoImage2RevealTriggered() =>
            PhotoImage2RevealTriggered?.Invoke(this, EventArgs.Empty);

        void OnScoreButton2RevealTriggered() =>
            ScoreButton2RevealTriggered?.Invoke(this, EventArgs.Empty);

        void OnPopUpAlertAboutEmotionTriggered(string title, string message, PlayerModel player) =>
            PopUpAlertAboutEmotionTriggered?.Invoke(this, new AlertMessageEventArgs(title, message, player));
        #endregion
    }
}

