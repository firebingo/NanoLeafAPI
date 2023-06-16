using System.Text.Json.Serialization;

namespace NanoLeafAPI.Models
{
	public class SetState
	{
		[JsonPropertyName("on")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public ValueBool On { get; set; }
		[JsonPropertyName("brightness")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public ValueDuration Brightness { get; set; }
		[JsonPropertyName("hue")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public ValueInt Hue { get; set; }
		[JsonPropertyName("sat")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public ValueInt Saturation { get; set; }
		[JsonPropertyName("ct")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public ValueInt Temperature { get; set; }
	}
}
