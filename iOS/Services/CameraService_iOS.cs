using System;

using UIKit;

using Xamarin.Forms;

using FaceOff.iOS;

[assembly: Dependency(typeof(CameraService_iOS))]
namespace FaceOff.iOS
{
	public class CameraService_iOS : ICameraService
	{
		public Func<object> GetCameraOverlay()
		{
			return () =>
		  	{
				var imageView = new UIImageView(UIImage.FromBundle("Camera-Face-Overlay.png"));
				imageView.ContentMode = UIViewContentMode.ScaleAspectFit;

				var screen = UIScreen.MainScreen.Bounds;
				imageView.Frame = screen;

				return imageView;
			};
		}
	}
}
