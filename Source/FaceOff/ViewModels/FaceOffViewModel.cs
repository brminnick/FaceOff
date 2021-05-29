using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using FaceOff.Shared;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Plugin.Media.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FaceOff
{
    public class FaceOffViewModel : BaseViewModel
    {
        readonly IReadOnlyList<string> _emotionStringsForAlertMessage =
            new string[] { "angry", "disrespectful", "disgusted", "scared", "happy", "blank", "sad", "surprised" };

        const string _makeAFaceAlertMessage = "take a selfie looking";
        const string _calculatingScoreMessage = "Analyzing";

        const string _playerNumberNotImplentedExceptionText = "Player Number Not Implemented";

        readonly WeakEventManager<string> _emotionResultsGatheredEventManager = new();
        readonly WeakEventManager<GameInitializedEventArgs> _gameInitializedEventManager = new();
        readonly WeakEventManager<PlayerNumberType> _generateEmotionResultsStartedEventManager = new();
        readonly WeakEventManager<PlayerNumberType> _generateEmotionResultsCompletedEventManager = new();

        ImageSource? _photo1ImageSource, _photo2ImageSource;

        bool _isTakePhotoButton1Enabled = true;
        bool _isTakePhotoButton2Enabled = true;
        bool _isCalculatingPhoto1Score, _isCalculatingPhoto2Score;
        bool _isScore1ButtonEnabled, _isScoreButton2Enabled;
        EmotionType _currentEmotionType;

        string _photo1Results = string.Empty,
            _photo2Results = string.Empty,
            _pageTitle = string.Empty,
            _scoreButton1Text = string.Empty,
            _scoreButton2Text = string.Empty;

        public FaceOffViewModel()
        {
            SetRandomEmotion();

            TakePhotoButton1Tapped = new Command(ExecuteTakePhoto1ButtonTapped, () => IsTakePhotoButton1Enabled);
            TakePhotoButton2Tapped = new Command(ExecuteTakePhoto2ButtonTapped, () => IsTakePhotoButton2Enabled);
            ScoreButton1Command = new Command(ExecutePhoto1ScoreButtonTapped, () => IsScoreButton1Enabled);
            ScoreButton2Command = new Command(ExecutePhoto2ScoreButtonTapped, () => IsScoreButton2Enabled);
            ResetButtonCommand = new Command(ExecuteResetButtonTapped, () => IsResetButtonEnabled);
            EmotionPopUpAlertResponseCommand = new AsyncCommand<EmotionPopupResponseModel>(response => response switch
            {
                null => Task.CompletedTask,
                _ => ExecuteEmotionPopUpAlertResponseCommand(response)
            });
        }

        public event EventHandler<GameInitializedEventArgs> GameInitialized
        {
            add => _gameInitializedEventManager.AddEventHandler(value);
            remove => _gameInitializedEventManager.RemoveEventHandler(value);
        }

        public event EventHandler<string> EmotionResultsGathered
        {
            add => _emotionResultsGatheredEventManager.AddEventHandler(value);
            remove => _emotionResultsGatheredEventManager.RemoveEventHandler(value);
        }

        public event EventHandler<PlayerNumberType> GenerateEmotionsResultsCompleted
        {
            add => _generateEmotionResultsCompletedEventManager.AddEventHandler(value);
            remove => _generateEmotionResultsCompletedEventManager.RemoveEventHandler(value);
        }

        public event EventHandler<PlayerNumberType> GenerateEmotionResultsStarted
        {
            add => _generateEmotionResultsStartedEventManager.AddEventHandler(value);
            remove => _generateEmotionResultsStartedEventManager.RemoveEventHandler(value);
        }

        public Command ResetButtonCommand { get; }
        public Command TakePhotoButton1Tapped { get; }
        public Command TakePhotoButton2Tapped { get; }
        public Command ScoreButton1Command { get; }
        public Command ScoreButton2Command { get; }
        public ICommand EmotionPopUpAlertResponseCommand { get; }

        public ImageSource? FrameImageSource1
        {
            get => _photo1ImageSource;
            set => SetProperty(ref _photo1ImageSource, value);
        }

        public ImageSource? FrameImageSource2
        {
            get => _photo2ImageSource;
            set => SetProperty(ref _photo2ImageSource, value);
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

        public bool IsCalculatingPlayer1Score
        {
            get => _isCalculatingPhoto1Score;
            set => SetProperty(ref _isCalculatingPhoto1Score, value, () => MainThread.BeginInvokeOnMainThread(ResetButtonCommand.ChangeCanExecute));
        }

        public bool IsCalculatingPhoto2Score
        {
            get => _isCalculatingPhoto2Score;
            set => SetProperty(ref _isCalculatingPhoto2Score, value, () => MainThread.BeginInvokeOnMainThread(ResetButtonCommand.ChangeCanExecute));
        }

        bool IsResetButtonEnabled => !(IsCalculatingPlayer1Score || IsCalculatingPhoto2Score);

        bool IsScoreButton1Enabled
        {
            get => _isScore1ButtonEnabled;
            set => SetProperty(ref _isScore1ButtonEnabled, value, () => MainThread.BeginInvokeOnMainThread(ScoreButton1Command.ChangeCanExecute));
        }

        bool IsScoreButton2Enabled
        {
            get => _isScoreButton2Enabled;
            set => SetProperty(ref _isScoreButton2Enabled, value, () => MainThread.BeginInvokeOnMainThread(ScoreButton2Command.ChangeCanExecute));
        }

        bool IsTakePhotoButton1Enabled
        {
            get => _isTakePhotoButton1Enabled;
            set => SetProperty(ref _isTakePhotoButton1Enabled, value, () => MainThread.BeginInvokeOnMainThread(TakePhotoButton1Tapped.ChangeCanExecute));
        }

        bool IsTakePhotoButton2Enabled
        {
            get => _isTakePhotoButton2Enabled;
            set => SetProperty(ref _isTakePhotoButton2Enabled, value, () => MainThread.BeginInvokeOnMainThread(TakePhotoButton2Tapped.ChangeCanExecute));
        }

        #region UITest Backdoor Methods
#if DEBUG
        public Task SubmitPhoto(EmotionType emotion, PlayerModel player)
        {
            _currentEmotionType = emotion;
            SetPageTitle(_currentEmotionType);

            return ExecuteGetPhotoResultsWorkflow(player);
        }
#endif
        #endregion
        void ExecuteTakePhoto1ButtonTapped() =>
            ExecutePopUpAlert(new PlayerModel(PlayerNumberType.Player1, PreferencesService.Player1Name));

        void ExecuteTakePhoto2ButtonTapped() =>
            ExecutePopUpAlert(new PlayerModel(PlayerNumberType.Player2, PreferencesService.Player2Name));

        void ExecutePopUpAlert(PlayerModel playerModel)
        {
            logPhotoButtonTapped(playerModel.PlayerNumber);
            DisableButtons(playerModel.PlayerNumber);

            var title = EmotionConstants.EmotionDictionary[_currentEmotionType];
            var message = $"{playerModel.PlayerName}, {_makeAFaceAlertMessage} {_emotionStringsForAlertMessage[(int)_currentEmotionType]}";
            OnGameInitialized(title, message, playerModel);

            static void logPhotoButtonTapped(PlayerNumberType playerNumber)
            {
                switch (playerNumber)
                {
                    case PlayerNumberType.Player1:
                        AnalyticsService.Track(AnalyticsConstants.PhotoButton1Tapped);
                        break;
                    case PlayerNumberType.Player2:
                        AnalyticsService.Track(AnalyticsConstants.PhotoButton2Tapped);
                        break;
                    default:
                        throw new NotSupportedException(_playerNumberNotImplentedExceptionText);
                }
            }
        }

        Task ExecuteEmotionPopUpAlertResponseCommand(EmotionPopupResponseModel response)
        {
            var player = response.Player;

            if (response.IsPopUpConfirmed)
                return ExecuteTakePhotoWorkflow(player);

            EnableButtons(player.PlayerNumber);

            return Task.CompletedTask;
        }

        async Task ExecuteTakePhotoWorkflow(PlayerModel player)
        {
            player.ImageMediaFile = await MediaService.GetMediaFileFromCamera("FaceOff", player.PlayerNumber).ConfigureAwait(false);

            if (player.ImageMediaFile is null)
                EnableButtons(player.PlayerNumber);
            else
                await ExecuteGetPhotoResultsWorkflow(player).ConfigureAwait(false);
        }

        async Task ExecuteGetPhotoResultsWorkflow(PlayerModel player)
        {
            AnalyticsService.Track(AnalyticsConstants.PhotoTaken);

            ConfigureUIForPendingEmotionResults(player);

            var results = await GenerateEmotionResults(player).ConfigureAwait(false);

            ConfigureUIForFinalizedEmotionResults(player, results);
        }

        void ConfigureUIForPendingEmotionResults(PlayerModel player)
        {
            OnGenerateEmotionResultsStarted(player.PlayerNumber);

            SetIsEnabledForOppositePhotoButton(true, player.PlayerNumber);

            SetIsEnabledForCurrentPhotoButton(false, player.PlayerNumber);

            SetScoreButtonText(_calculatingScoreMessage, player.PlayerNumber);

            SetPhotoImageSource(player.ImageMediaFile, player.PlayerNumber);

            SetIsCalculatingPhotoScore(true, player.PlayerNumber);
        }

        void ConfigureUIForFinalizedEmotionResults(PlayerModel player, string results)
        {
            OnGenerateEmotionsResultsCompleted(player.PlayerNumber);

            SetPhotoResultsText(results, player.PlayerNumber);

            SetIsCalculatingPhotoScore(false, player.PlayerNumber);

            SetIsEnabledForCurrentPlayerScoreButton(true, player.PlayerNumber);
        }

        async Task<string> GenerateEmotionResults(PlayerModel player)
        {
            List<Emotion> emotionArray = Enumerable.Empty<Emotion>().ToList();
            string emotionScore;

            try
            {
                emotionArray = await EmotionService.GetEmotionResultsFromMediaFile(player.ImageMediaFile).ConfigureAwait(false);
                emotionScore = EmotionService.GetPhotoEmotionScore(emotionArray, 0, _currentEmotionType);
            }
            catch (APIErrorException e) when (e.Response.StatusCode is System.Net.HttpStatusCode.Unauthorized)
            {
                AnalyticsService.Report(e);

                emotionScore = EmotionService.ErrorMessageDictionary[ErrorMessageType.InvalidAPIKey];
            }
            catch (HttpRequestException e) when (e.Message.Contains("offline"))
            {
                AnalyticsService.Report(e);

                emotionScore = EmotionService.ErrorMessageDictionary[ErrorMessageType.DeviceOffline];
            }
            catch (Exception e)
            {
                AnalyticsService.Report(e);

                emotionScore = EmotionService.ErrorMessageDictionary[ErrorMessageType.ConnectionToCognitiveServicesFailed];
            }

            var doesEmotionScoreContainErrorMessage = EmotionService.DoesStringContainErrorMessage(emotionScore);

            if (doesEmotionScoreContainErrorMessage)
            {
                var errorMessageKey = EmotionService.ErrorMessageDictionary.First(x => x.Value.Contains(emotionScore)).Key;

                switch (errorMessageKey)
                {
                    case ErrorMessageType.NoFaceDetected:
                        AnalyticsService.Track(EmotionService.ErrorMessageDictionary[ErrorMessageType.NoFaceDetected]);
                        break;
                    case ErrorMessageType.MultipleFacesDetected:
                        AnalyticsService.Track(EmotionService.ErrorMessageDictionary[ErrorMessageType.MultipleFacesDetected]);
                        break;
                    case ErrorMessageType.GenericError:
                        AnalyticsService.Track(EmotionService.ErrorMessageDictionary[ErrorMessageType.MultipleFacesDetected]);
                        break;
                }

                SetScoreButtonText(emotionScore, player.PlayerNumber);
            }
            else
                SetScoreButtonText($"Score: {emotionScore}", player.PlayerNumber);

            return EmotionService.GetStringOfAllPhotoEmotionScores(emotionArray, 0);
        }

        void ExecuteResetButtonTapped()
        {
            AnalyticsService.Track(AnalyticsConstants.ResetButtonTapped);

            SetRandomEmotion();

            FrameImageSource1 = null;
            FrameImageSource2 = null;

            IsTakePhotoButton1Enabled = true;

            IsTakePhotoButton2Enabled = true;

            ScoreButton1Text = string.Empty;
            ScoreButton2Text = string.Empty;

            IsScoreButton1Enabled = false;
            IsScoreButton2Enabled = false;

            _photo1Results = string.Empty;
            _photo2Results = string.Empty;
        }

        void ExecutePhoto1ScoreButtonTapped()
        {
            AnalyticsService.Track(AnalyticsConstants.ResultsButton1Tapped);
            OnEmotionResultsGathered(_photo1Results);
        }

        void ExecutePhoto2ScoreButtonTapped()
        {
            AnalyticsService.Track(AnalyticsConstants.ResultsButton2Tapped);
            OnEmotionResultsGathered(_photo2Results);
        }

        void SetRandomEmotion() => SetEmotion(EmotionService.GetRandomEmotionType(_currentEmotionType));

        void SetEmotion(EmotionType emotionType)
        {
            _currentEmotionType = emotionType;

            SetPageTitle(_currentEmotionType);
        }

        void SetIsEnabledForOppositePhotoButton(bool isEnabled, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    IsTakePhotoButton2Enabled = isEnabled;
                    break;
                case PlayerNumberType.Player2:
                    IsTakePhotoButton1Enabled = isEnabled;
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
                    IsScoreButton1Enabled = isEnabled;
                    break;
                case PlayerNumberType.Player2:
                    IsScoreButton2Enabled = isEnabled;
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
                    IsTakePhotoButton1Enabled = isEnabled;
                    break;
                case PlayerNumberType.Player2:
                    IsTakePhotoButton2Enabled = isEnabled;
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

        void SetPhotoImageSource(MediaFile? imageMediaFile, PlayerNumberType playerNumber)
        {
            switch (playerNumber)
            {
                case PlayerNumberType.Player1:
                    FrameImageSource1 = ImageSource.FromStream(() => imageMediaFile?.GetStream());
                    break;
                case PlayerNumberType.Player2:
                    FrameImageSource2 = ImageSource.FromStream(() => imageMediaFile?.GetStream());
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
                    IsCalculatingPlayer1Score = isCalculatingScore;
                    break;
                case PlayerNumberType.Player2:
                    IsCalculatingPhoto2Score = isCalculatingScore;
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

        void SetPageTitle(EmotionType emotionType) =>
            PageTitle = EmotionConstants.EmotionDictionary[emotionType];

        void EnableButtons(PlayerNumberType playerNumber) =>
            SetIsEnabledForButtons(true, playerNumber);

        void DisableButtons(PlayerNumberType playerNumber) =>
            SetIsEnabledForButtons(false, playerNumber);

        void OnEmotionResultsGathered(string emotionResults) =>
            _emotionResultsGatheredEventManager.RaiseEvent(this, emotionResults, nameof(EmotionResultsGathered));

        void OnGenerateEmotionResultsStarted(PlayerNumberType playerNumber) =>
            _generateEmotionResultsStartedEventManager.RaiseEvent(this, playerNumber, nameof(GenerateEmotionResultsStarted));

        void OnGenerateEmotionsResultsCompleted(PlayerNumberType playerNumber) =>
            _generateEmotionResultsCompletedEventManager.RaiseEvent(this, playerNumber, nameof(GenerateEmotionsResultsCompleted));

        void OnGameInitialized(string title, string message, PlayerModel player) =>
            _gameInitializedEventManager.RaiseEvent(this, new GameInitializedEventArgs(title, message, player), nameof(GameInitialized));
    }
}

