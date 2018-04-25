using Newtonsoft.Json;

namespace FaceOff
{
	public class FaceApiModel
	{
		[JsonProperty("faceId")]
		public string FaceId { get; set; }

		[JsonProperty("faceAttributes")]
		public FaceAttributes FaceAttributes { get; set; }
	}
}