using System;
using System.Net.Http;
using ModernHttpClient;

using Xamarin.Forms;

using FaceOff;
using FaceOff.Droid;

[assembly: Dependency(typeof(HttpClientHelperAndroid))]
namespace FaceOff.Droid
{
	public class HttpClientHelperAndroid : IHttpClientHelper
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
					return null;
				}
			}
		}
	}
}

