using Xamarin.Forms;

namespace FaceOff
{
	public class FaceOffButton : Button
	{
		public FaceOffButton() : base()
		{
			const int _animationTime = 100;
			Clicked += async (sender, e) =>
			{
				var btn = (FaceOffButton)sender;
				await btn.ScaleTo(1.2, _animationTime);
				await btn.ScaleTo(1, _animationTime);
			};
		}
	}
}

