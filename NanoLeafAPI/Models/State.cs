using System.Text.Json.Serialization;

namespace NanoLeafAPI.Models
{
	public class State
	{
		[JsonPropertyName("on")]
		public ValueBool On { get; set; }
		[JsonPropertyName("brightness")]
		public ValueMinMix Brightness { get; set; }
		[JsonPropertyName("hue")]
		public ValueMinMix Hue { get; set; }
		[JsonPropertyName("sat")]
		public ValueMinMix Saturation { get; set; }
		[JsonPropertyName("ct")]
		public ValueMinMix Temperature { get; set; }
		[JsonPropertyName("colorMode")]
		public string ColorMode { get; set; }
	}




}
