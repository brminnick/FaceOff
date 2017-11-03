using Xamarin.UITest;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace FaceOff.UITests
{
    public class CameraPage : BasePage
    {
        readonly Query _photoCaptureButton, _cancelPhotoButton, _retakePhotoButton, _usePhotoButton;

        public CameraPage(IApp app, Platform platform) : base(app, platform)
        {
            if (IsiOS)
            {
                _photoCaptureButton = x => x.Marked("PhotoCapture");
                _cancelPhotoButton = x => x.Marked("Cancel");
                _retakePhotoButton = x => x.Marked("Retake");
                _usePhotoButton = x => x.Marked("Use Photo");
            }
            else
            {
                _photoCaptureButton = x => x.Marked("");
                _cancelPhotoButton = x => x.Marked("");
                _retakePhotoButton = x => x.Marked("");
                _usePhotoButton = x => x.Marked("");
            }
        }

        public void TapPhotoCaptureButton()
        {
            App.Tap(_photoCaptureButton);
            App.Screenshot("Tapped Photo Capture Button");
        }

        public void TapCancelPhotoButton()
        {
            App.Tap(_cancelPhotoButton);
            App.Screenshot("Tapped Cancel Photo Button");
        }

        public void TapRetakePhotoButton()
        {
            App.Tap(_retakePhotoButton);
            App.Screenshot("Tapped Retake Photo Button");
        }

        public void TapUsePhotoButton()
        {
            App.Tap(_usePhotoButton);
            App.Screenshot("Tapped Use Photo Button");
        }
    }
}

