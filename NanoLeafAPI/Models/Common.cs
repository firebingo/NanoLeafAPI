using System.Text.Json.Serialization;

namespace NanoLeafAPI.Models
{
	public class ValueInt
	{
		[JsonPropertyName("value")]
		public int Value { get; set; }
	}

	public class ValueBool
	{
		[JsonPropertyName("value")]
		public bool Value { get; set; }
	}

	public class ValueDuration
	{
		[JsonPropertyName("value")]
		public int Value { get; set; }
		[JsonPropertyName("duration")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public int? Duration { get; set; }
	}

	public class ValueMinMix
	{
		[JsonPropertyName("value")]
		public int Value { get; set; }
		[JsonPropertyName("max")]
		public int Max { get; set; }
		[JsonPropertyName("min")]
		public int Min { get; set; }
	}

	public class Position
	{
		[JsonPropertyName("x")]
		public int X { get; set; }
		[JsonPropertyName("y")]
		public int Y { get; set; }
		[JsonPropertyName("o")]
		public int O { get; set; }
	}

	internal class Select
	{
		[JsonPropertyName("select")]
		public string Value { get; set; }
	}
}
