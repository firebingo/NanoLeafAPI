using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NanoLeafAPI.Models
{
	public class PanelLayout
	{
		[JsonPropertyName("layout")]
		public Layout Layout { get; set; }
	}

	public class Layout
	{
		[JsonPropertyName("numPanels")]
		public int NumPanels { get; set; }
		[JsonPropertyName("sideLength")]
		public int SideLength { get; set; }
		[JsonPropertyName("positionData")]
		public List<PanelPosition> Positions { get; set; }
		[JsonPropertyName("globalOrientation")]
		public ValueMinMix GlobalOrientation { get; set; }
	}

	public class PanelPosition : Position
	{
		[JsonPropertyName("panelId")]
		public ushort PanelID { get; set; }
	}
}
