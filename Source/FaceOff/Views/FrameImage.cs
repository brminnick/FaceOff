using Xamarin.Forms;

namespace FaceOff
{
	public class FrameImage : Frame
	{
		Image _contentImage;

		public FrameImage(string automationId)
		{
			HasShadow = false;
			ContentImage = new Image();
			Content = ContentImage;
			AutomationId = automationId;
		}
		public Image ContentImage
		{
			get { return _contentImage; }
			set { _contentImage = value; }
		}
	}
}

