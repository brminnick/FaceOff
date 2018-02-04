using System.Threading.Tasks;

using Plugin.Connectivity;

namespace FaceOff
{
    public static class ConnectionService
    {
        public static async Task<bool> IsInternetConnectionAvailable() =>
            CrossConnectivity.Current.IsConnected &&
            await CrossConnectivity.Current.IsRemoteReachable("google.com").ConfigureAwait(false);
    }
}
