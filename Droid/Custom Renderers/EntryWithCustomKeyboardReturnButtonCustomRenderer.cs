using Android.Widget;
using Android.Views.InputMethods;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using FaceOff;
using FaceOff.Droid;

[assembly: ExportRenderer(typeof(EntryWithCustomKeyboardReturnButton), typeof(EntryWithCustomKeyboardReturnButtonCustomRenderer))]
namespace FaceOff.Droid
{
	public class EntryWithCustomKeyboardReturnButtonCustomRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			var customEntry = Element as EntryWithCustomKeyboardReturnButton;

			if (Control != null && customEntry != null)
			{
				SetKeyboardButtonType(customEntry.ReturnType);

				Control.EditorAction += (object sender, TextView.EditorActionEventArgs args) =>
				{
					if (customEntry?.ReturnType != ReturnType.Next)
						customEntry?.Unfocus();

					customEntry?.InvokeCompleted();
				};
			}
		}

		void SetKeyboardButtonType(ReturnType returnType)
		{
			switch (returnType)
			{
				case ReturnType.Go:
					Control.ImeOptions = ImeAction.Go;
					Control.SetImeActionLabel("Go", ImeAction.Go);
					break;
				case ReturnType.Next:
					Control.ImeOptions = ImeAction.Next;
					Control.SetImeActionLabel("Next", ImeAction.Next);
					break;
				case ReturnType.Send:
					Control.ImeOptions = ImeAction.Send;
					Control.SetImeActionLabel("Send", ImeAction.Send);
					break;
				case ReturnType.Search:
					Control.ImeOptions = ImeAction.Search;
					Control.SetImeActionLabel("Search", ImeAction.Search);
					break;
				default:
					Control.ImeOptions = ImeAction.Done;
					Control.SetImeActionLabel("Done", ImeAction.Done);
					break;
			}
		}
	}
}

