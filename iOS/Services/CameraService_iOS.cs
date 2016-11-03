using System;

using UIKit;

namespace FaceOff.iOS
{
	public class CameraService_iOS : ICameraService
	{
		public Func<object> GetCameraOverlay()
		{
			return () =>
		  	{
				var imageView = new UIImageView(UIImage.FromBundle("face-template-overlay.png"));
				imageView.ContentMode = UIViewContentMode.ScaleAspectFit;

				var screen = UIScreen.MainScreen.Bounds;
				imageView.Frame = screen;

				return imageView;
			};
		}
	}
}
