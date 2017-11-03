using System;
using System.IO;
using System.Threading.Tasks;

using Plugin.Media;
using Plugin.Media.Abstractions;

using Xamarin.Forms;

namespace FaceOff
{
    public static class MediaService
    {
        #region Events 
        public static event EventHandler DisplayNoCameraAvailableAlert;
        #endregion

        #region Methods
        public static Stream GetPhotoStream(MediaFile mediaFile, bool disposeMediaFile)
        {
            var stream = mediaFile.GetStream();

            if (disposeMediaFile)
                mediaFile.Dispose();

            return stream;
        }

        public static async Task<MediaFile> GetMediaFileFromCamera(string directory, PlayerNumberType playerNumber)
        {
            await CrossMedia.Current.Initialize().ConfigureAwait(false);

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                OnDisplayNoCameraAvailableAlert();
                return null;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Small,
                Directory = directory,
                Name = playerNumber.ToString(),
                DefaultCamera = CameraDevice.Front,
                OverlayViewProvider = DependencyService.Get<ICameraService>()?.GetCameraOverlay()
            }).ConfigureAwait(false);

            return file;
        }

        static void OnDisplayNoCameraAvailableAlert() =>
            DisplayNoCameraAvailableAlert?.Invoke(null, EventArgs.Empty);
        #endregion
    }
}
