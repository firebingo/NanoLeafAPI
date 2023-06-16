using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NanoLeafAPI.Models
{
	public class Effects
	{
		[JsonPropertyName("select")]
		public string Select { get; set; }
		[JsonPropertyName("effectsList")]
		public List<string> EffectsList { get; set; }
	}
}
