using Xamarin.Forms;

namespace FaceOff
{
	public class FrameImage : Frame
	{
		Image _contentImage;

		public FrameImage()
		{
			HasShadow = false;
			ContentImage = new Image();
			Content = ContentImage;
		}
		public Image ContentImage
		{
			get
			{
				return _contentImage;
			}
			set
			{
				_contentImage = value;
			}
		}
	}
}

