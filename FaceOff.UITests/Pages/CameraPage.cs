 using Xamarin.UITest;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace FaceOff.UITests
{
	public class CameraPage : BasePage
	{
		readonly Query PhotoCaptureButton;
		readonly Query CancelPhotoButton;

		readonly Query RetakePhotoButton;
		readonly Query UsePhotoButton;

		public CameraPage(IApp app, Platform platform) : base(app, platform)
		{
			if (IsiOS)
			{
				PhotoCaptureButton = x => x.Marked("PhotoCapture");
				CancelPhotoButton = x => x.Marked("Cancel");
				RetakePhotoButton = x => x.Marked("Retake");
				UsePhotoButton = x => x.Marked("Use Photo");
			}
			else 
			{
				PhotoCaptureButton = x => x.Marked("");
				CancelPhotoButton = x => x.Marked("");
				RetakePhotoButton = x => x.Marked("");
				UsePhotoButton = x => x.Marked("");
			}
		}

		public void TapPhotoCaptureButton()
		{
			app.Tap(PhotoCaptureButton);
			app.Screenshot("Tapped Photo Capture Button");
		}

		public void TapCancelPhotoButton()
		{
			app.Tap(CancelPhotoButton);
			app.Screenshot("Tapped Cancel Photo Button");
		}

		public void TapRetakePhotoButton()
		{
			app.Tap(RetakePhotoButton);
			app.Screenshot("Tapped Retake Photo Button");
		}

		public void TapUsePhotoButton()
		{
			app.Tap(UsePhotoButton);
			app.Screenshot("Tapped Use Photo Button");
		}
	}
}

