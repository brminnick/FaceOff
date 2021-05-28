using System;
using Newtonsoft.Json;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.iOS;

namespace FaceOff.UITests
{
    static class BackdoorMethodService
    {
        public static object InvokeBackdoorMethod(this IApp app, string backdoorMethodName, string parameter = "") => (app, parameter) switch
        {
            (iOSApp iosApp, _) => iosApp.Invoke(backdoorMethodName + ":", parameter),
            (AndroidApp androidApp, "") => androidApp.Invoke(backdoorMethodName),
            (AndroidApp androidApp, _) => androidApp.Invoke(backdoorMethodName, parameter),
            _ => throw new NotSupportedException("Platform Not Supported"),
        };

        public static T InvokeBackdoorMethod<T>(this IApp app, string backdoorMethodName, string parameter = "")
        {
            var result = app.InvokeBackdoorMethod(backdoorMethodName, parameter).ToString();
            return JsonConvert.DeserializeObject<T>(result) ?? throw new JsonException();
        }
    }
}
