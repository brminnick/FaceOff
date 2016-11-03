using System;
namespace FaceOff
{
	public class AlertMessageEventArgs : EventArgs
	{
		public AlertMessageEventArgs(AlertMessageModel alertMessage)
		{
			Message = alertMessage;
		}

		public AlertMessageModel Message { get; }
	}
}
