using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using FaceOff;
using FaceOff.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(EntryCustomRenderer))]
namespace FaceOff.iOS
{
	public class EntryCustomRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
				return;

			Control.ClearButtonMode = UITextFieldViewMode.WhileEditing;

		}
	}
}