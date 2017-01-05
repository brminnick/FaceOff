using System;
using Xamarin.Forms;

namespace FaceOff
{
	public class EntryWithCustomKeyboardReturnButton : Entry
	{
		public new event EventHandler Completed;

		public static readonly BindableProperty ReturnTypeProperty =
			BindableProperty.Create<EntryWithCustomKeyboardReturnButton, ReturnType>(s => s.ReturnType, ReturnType.Done);

		public ReturnType ReturnType
		{
			get { return (ReturnType)GetValue(ReturnTypeProperty); }
			set { SetValue(ReturnTypeProperty, value); }
		}

		public void InvokeCompleted()
		{
			Completed?.Invoke(this, null);
		}
	}

	public enum ReturnType
	{
		Go,
		Next,
		Done,
		Send,
		Search
	}
}
