using System;
using System.Net.Http;

using ModernHttpClient;

using FaceOff;
using FaceOff.iOS;

using Xamarin;
using Xamarin.Forms;

[assembly: Dependency(typeof(HttpClientHelperiOS))]
namespace FaceOff.iOS
{
	public class HttpClientHelperiOS : IHttpClientHelper
	{
		HttpClient _client;
		public HttpClient Client
		{
			get
			{
				try
				{
					return _client = new HttpClient(new NativeMessageHandler());
				}
				catch (Exception e)
				{
					Insights.Report(e);
					return null;
				}
			}
		}
	}
}

