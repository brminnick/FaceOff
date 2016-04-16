using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Plugin.Media;
using Plugin.Media.Abstractions;

using Newtonsoft.Json;

using Xamarin.Forms;

using FaceOff.Model;
using System.Net;
using Microsoft.ProjectOxford.Emotion;

namespace FaceOff
{
	public class PictureViewModel : BaseViewModel
	{
		#region Constant Fields
		readonly string _cognitiveServicesEmotionApiUrl = DependencyService.Get<EmotionApiUriHelper>().EmotionApiUri;
		#endregion

		#region Fields
		ImageSource _photo1ImageSource, _photo2ImageSource;
		bool _isTakePhoto1ButtonEnabled = true;
		bool _isTakePhoto2ButtonEnabled = true;
		#endregion

		#region Constructors
		public PictureViewModel()
		{
			TakePhoto1ButtonPressed = new Command(async () =>
			{
				var imageMediaFile = await GetMediaFileFromCamera("FaceOff", "PhotoImage1");

				var emotionArray = await GetEmotionResults(imageMediaFile);

				Photo1ImageSource = ImageSource.FromStream(() =>
				{
					return GetPhotoStream(imageMediaFile, true);
				});
				IsTakePhoto1ButtonEnabled = false;
			});

			TakePhoto2ButtonPressed = new Command(async () =>
			{
				var imageMediaFile = await GetMediaFileFromCamera("FaceOff", "PhotoImage2");

				var emotionArray = await GetEmotionResults(imageMediaFile);

				Photo2ImageSource = ImageSource.FromStream(() =>
				{
					return GetPhotoStream(imageMediaFile, true);
				});
				IsTakePhoto2ButtonEnabled = false;
			});

			ResetButtonPressed = new Command(() =>
			{
				Photo1ImageSource = null;
				Photo2ImageSource = null;
				IsTakePhoto1ButtonEnabled = true;
				IsTakePhoto2ButtonEnabled = true;
			});
		}
		#endregion

		#region Properties
		public Command TakePhoto1ButtonPressed { get; protected set; }
		public Command TakePhoto2ButtonPressed { get; protected set; }
		public Command ResetButtonPressed { get; protected set; }
		public Command SubmitButtonPressed { get; protected set; }

		public ImageSource Photo1ImageSource
		{
			get
			{
				return _photo1ImageSource;
			}
			set
			{
				OnPropertyChanged("Photo1ImageSource");
				_photo1ImageSource = value;
			}
		}

		public ImageSource Photo2ImageSource
		{
			get
			{
				return _photo2ImageSource;
			}
			set
			{
				OnPropertyChanged("Photo2ImageSource");
				_photo2ImageSource = value;
			}
		}

		public bool IsPhotoImage1Enabled
		{
			get { return !IsTakePhoto1ButtonEnabled; }
		}
		public bool IsPhotoImage2Enabled
		{
			get { return !IsTakePhoto2ButtonEnabled; }
		}

		public bool IsTakePhoto1ButtonEnabled
		{
			get
			{
				return _isTakePhoto1ButtonEnabled;
			}
			set
			{
				OnPropertyChanged("IsTakePhoto1ButtonEnabled");
				OnPropertyChanged("IsPhotoImage1Enabled");
				_isTakePhoto1ButtonEnabled = value;
			}
		}

		public bool IsTakePhoto2ButtonEnabled
		{
			get
			{
				return _isTakePhoto2ButtonEnabled;
			}
			set
			{
				OnPropertyChanged("IsTakePhoto2ButtonEnabled");
				OnPropertyChanged("IsPhotoImage2Enabled");
				_isTakePhoto2ButtonEnabled = value;
			}
		}
		#endregion

		#region Methods
		Stream GetPhotoStream(MediaFile mediaFile, bool disposeMediaFile)
		{
			var stream = mediaFile.GetStream();

			if (disposeMediaFile)
				mediaFile.Dispose();

			return stream;
		}

		async Task<MediaFile> GetMediaFileFromCamera(string directory, string filename)
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				//Todo Handle case when no camera is available
				return null;
			}

			var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
			{
				Directory = directory,
				Name = filename
			});

			return file;
		}

		async Task<Microsoft.ProjectOxford.Emotion.Contract.Emotion[]> GetEmotionResults(MediaFile mediaFile)
		{
			if (mediaFile == null)
				return null;

			var emotionClient = new EmotionServiceClient(CognitiveServicesConstants.EmotionApiKey);


			return await emotionClient.RecognizeAsync(GetPhotoStream(mediaFile, false));
		}

		#endregion
	}
}

