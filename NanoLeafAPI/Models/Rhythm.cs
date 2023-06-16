using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NanoLeafAPI.Models
{
	public class Rhythm
	{
		[JsonPropertyName("rhythmConnected")]
		public bool RhythmConnected { get; set; }
		[JsonPropertyName("rhythmActive")]
		public bool? RhythmActive { get; set; }
		[JsonPropertyName("rhythmId")]
		public string RhythmId { get; set; }
		[JsonPropertyName("hardwareVersion")]
		public string HardwareVersion { get; set; }
		[JsonPropertyName("firmwareVersion")]
		public string FirmwareVersion { get; set; }
		[JsonPropertyName("auxAvailable")]
		public bool? AuxAvailable { get; set; }
		[JsonPropertyName("rhythmMode")]
		public string RhythmMode { get; set; }
		[JsonPropertyName("rhythmPos")]
		public List<Position> RhythmPos { get; set; }
	}
}
