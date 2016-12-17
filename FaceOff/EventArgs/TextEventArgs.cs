using System;
namespace FaceOff
{
	public class TextEventArgs : EventArgs
	{
		public TextEventArgs(string text)
		{
			Text = text;
		}

		public string Text { get; }
	}
}
