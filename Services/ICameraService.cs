using System;
namespace FaceOff
{
	public interface ICameraService
	{
		Func<object> GetCameraOverlay();
	}
}
