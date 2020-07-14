using System;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FaceOff
{
    public static class MediaService
    {
        readonly static WeakEventManager _noCameraDetectedEventManager = new WeakEventManager();
        readonly static WeakEventManager _permissionsDeniedEventManager = new WeakEventManager();

        public static event EventHandler NoCameraDetected
        {
            add => _noCameraDetectedEventManager.AddEventHandler(value);
            remove => _noCameraDetectedEventManager.RemoveEventHandler(value);
        }

        public static event EventHandler PermissionsDenied
        {
            add => _permissionsDeniedEventManager.AddEventHandler(value);
            remove => _permissionsDeniedEventManager.RemoveEventHandler(value);
        }

        public static async Task<MediaFile?> GetMediaFileFromCamera(string directory, PlayerNumberType playerNumber)
        {
            await CrossMedia.Current.Initialize().ConfigureAwait(false);

            var arePermissionsGranted = await ArePermissionsGranted().ConfigureAwait(false);
            if (!arePermissionsGranted)
            {
                OnPermissionsDenied();
                return null;
            }

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                OnNoCameraDetected();
                return null;
            }

            return await MainThread.InvokeOnMainThreadAsync(() =>
            {
                return CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    ModalPresentationStyle = MediaPickerModalPresentationStyle.OverFullScreen,
                    PhotoSize = PhotoSize.Small,
                    Directory = directory,
                    Name = playerNumber.ToString(),
                    DefaultCamera = CameraDevice.Front,
                    OverlayViewProvider = DependencyService.Get<ICameraService>()?.GetCameraOverlay()
                });
            }).ConfigureAwait(false);
        }

        static Task<bool> ArePermissionsGranted() => MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var cameraStatusRequestTask = Permissions.RequestAsync<Permissions.Camera>();
            var storageWriteStatusRequestTask = Permissions.RequestAsync<Permissions.StorageWrite>();
            var storageReadStatusRequestTask = Permissions.RequestAsync<Permissions.StorageRead>();
            var photosPermissionRequestTask = Permissions.RequestAsync<Permissions.StorageRead>();

            await Task.WhenAll(cameraStatusRequestTask, storageWriteStatusRequestTask, storageReadStatusRequestTask, photosPermissionRequestTask).ConfigureAwait(false);

            var cameraStatus = await cameraStatusRequestTask.ConfigureAwait(false);
            var storageWriteStatus = await storageWriteStatusRequestTask.ConfigureAwait(false);
            var storageReadStatus = await storageReadStatusRequestTask.ConfigureAwait(false);
            var photosPermission = await photosPermissionRequestTask.ConfigureAwait(false);

            return cameraStatus is PermissionStatus.Granted
                    && storageWriteStatus is PermissionStatus.Granted
                    && storageReadStatus is PermissionStatus.Granted
                    && photosPermission is PermissionStatus.Granted;
        });

        static void OnNoCameraDetected() => _noCameraDetectedEventManager.RaiseEvent(null, EventArgs.Empty, nameof(NoCameraDetected));
        static void OnPermissionsDenied() => _permissionsDeniedEventManager.RaiseEvent(null, EventArgs.Empty, nameof(PermissionsDenied));
    }
}
