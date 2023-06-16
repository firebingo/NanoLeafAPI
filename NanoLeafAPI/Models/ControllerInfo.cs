using System.Text.Json.Serialization;

namespace NanoLeafAPI.Models
{
	public class ControllerInfo
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }
		[JsonPropertyName("serialNo")]
		public string SerialNo { get; set; }
		[JsonPropertyName("manufacturer")]
		public string Manufacturer { get; set; }
		[JsonPropertyName("firmwareVersion")]
		public string FirmwareVersion { get; set; }
		[JsonPropertyName("model")]
		public string Model { get; set; }
		[JsonPropertyName("state")]
		public State State { get; set; }
		[JsonPropertyName("effects")]
		public Effects Effects { get; set; }
		[JsonPropertyName("panelLayout")]
		public PanelLayout PanelLayout { get; set; }
		[JsonPropertyName("rhythm")]
		public Rhythm Rhythm { get; set; }
	}
}
