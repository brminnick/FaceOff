using System.Web;

using Xamarin.Forms;

using FaceOff.iOS;

[assembly: Dependency(typeof(EmotionApiUriHelperiOS))]
namespace FaceOff.iOS
{
	public class EmotionApiUriHelperiOS : EmotionApiUriHelper
	{
		public string EmotionApiUri
		{
			get{
				return "https://api.projectoxford.ai/emotion/v1.0/recognize?" + HttpUtility.ParseQueryString(string.Empty);
			}
		}
	}
}

