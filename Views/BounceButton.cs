using Xamarin.Forms;

namespace FaceOff
{
	public class BounceButton : Button
	{
		public BounceButton() : base()
		{
			const int _animationTime = 100;
			Clicked +=  (sender, e) =>
			{
				var btn = (BounceButton)sender;

				Device.BeginInvokeOnMainThread(async () =>
				{
					await btn.ScaleTo(1.2, _animationTime);
					await btn.ScaleTo(1, _animationTime);
				});
			};
		}
	}
}

