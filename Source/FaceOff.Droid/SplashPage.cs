using System.Threading.Tasks;

using Android.OS;
using Android.App;
using Android.Util;
using Android.Content;
using Android.Support.V7.App;

namespace FaceOff.Droid
{
	[Activity(Theme = "@style/Splash", MainLauncher = true, NoHistory = true)]
	public class SplashActivity : AppCompatActivity
	{
		static readonly string Tag = "X:" + typeof(SplashActivity).Name;

		public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
		{
			base.OnCreate(savedInstanceState, persistentState);
			Log.Debug(Tag, "SplashActivity.OnCreate");
		}

		protected override void OnResume()
		{
			base.OnResume();

			var startupWork = new Task(() =>
			{
				Log.Debug(Tag, "Created Splash Screen Task.");
			});

			startupWork.ContinueWith(t =>
			{
				Log.Debug(Tag, "Work is finished - start MainActivity.");
				StartActivity(new Intent(Application.Context, typeof(MainActivity)));
			}, TaskScheduler.FromCurrentSynchronizationContext());

			startupWork.Start();
		}
	}
}

