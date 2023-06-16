using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NanoLeafAPI.Models
{
	internal class WriteCommandExt
	{
		[JsonPropertyName("command")]
		public string Command { get; set; }
		[JsonPropertyName("animType")]
		public string AnimType { get; set; }
		[JsonPropertyName("extControlVersion")]
		public string ExtControlVersion { get; set; }
	}

	internal class WriteExtControl
	{
		[JsonPropertyName("write")]
		public WriteCommandExt Write { get; set; }

		internal WriteExtControl()
		{
			Write = new WriteCommandExt()
			{
				Command = "display",
				AnimType = "extControl",
				ExtControlVersion = "v2"
			};
		}
	}

	internal class ExtControlResponse
	{
		[JsonPropertyName("streamControlIpAddr")]
		public string StreamControlIpAddress { get; set; }
		[JsonPropertyName("streamControlPort")]
		public string StreamControlPort { get; set; }
		[JsonPropertyName("streamControlProtocol")]
		public string StreamControlProtocol { get; set; }
	}

	public class ExtControlFrame
	{
		public List<ExtControlPanel> Panels { get; set; }
	}

	public class ExtControlPanel
	{
		public ushort PanelID { get; set; }
		public byte Red { get; set; }
		public byte Green { get; set; }
		public byte Blue { get; set; }
		public ushort TransitionTime { get; set; }
	}
}
