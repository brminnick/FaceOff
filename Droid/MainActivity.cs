using Android.OS;
using Android.App;
using Android.Content.PM;

using Xamarin;

namespace FaceOff.Droid
{
	[Activity(Label = "FaceOff.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
#if DEBUG
			Insights.Initialize(InsightsConstants.InsightsDebugApiKey, this);
#else
			Insights.Initialize(InsightsConstants.InsightsReleaseApiKey, this);
#endif

			Insights.HasPendingCrashReport += (sender, isStartupCrash) =>
			{
				if (isStartupCrash)
				{
					Insights.PurgePendingCrashReports().Wait();
				}
			};

			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			LoadApplication(new App());
		}
	}
}

