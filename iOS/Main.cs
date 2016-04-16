using UIKit;

using Xamarin;

namespace FaceOff.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main(string[] args)
		{
#if DEBUG
			Insights.Initialize(InsightsConstants.InsightsDebugApiKey);
#else
			Xamarin.Insights.Initialize (InsightsConstants.InsightsReleaseApiKey);
#endif

			Insights.HasPendingCrashReport += (sender, isStartupCrash) =>
			{
				if (isStartupCrash)
				{
					Insights.PurgePendingCrashReports().Wait();
				}
			};

			UIApplication.Main(args, null, "AppDelegate");
		}
	}
}

