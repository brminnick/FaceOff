using System.Net.Http;

namespace FaceOff
{
	public interface IHttpClientHelper
	{
		HttpClient Client { get; }
	}
}

