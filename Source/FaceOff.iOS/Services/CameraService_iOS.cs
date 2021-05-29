using System;
using FaceOff.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(CameraService_iOS))]
namespace FaceOff.iOS
{
    public class CameraService_iOS : ICameraService
    {
        static readonly Func<object> _cameraOverlay = () =>
        {
            var imageView = new UIImageView(UIImage.FromBundle("Camera-Face-Overlay.png"))
            {
                ContentMode = UIViewContentMode.ScaleAspectFit
            };

            var screen = UIScreen.MainScreen.Bounds;
            imageView.Frame = screen;

            return imageView;
        };

        public Func<object> GetCameraOverlay() => _cameraOverlay;
    }
}
