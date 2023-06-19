using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NanoLeafAPI.Models
{
	public class PanelLayout
	{
		[JsonPropertyName("layout")]
		public Layout Layout { get; set; }
		[JsonPropertyName("globalOrientation")]
		public ValueMinMix GlobalOrientation { get; set; }
		[JsonIgnore]
		public static readonly Dictionary<PanelShape, float> SideLengths = new Dictionary<PanelShape, float>()
		{
			{ PanelShape.Triangle, 150 },
			{ PanelShape.Rhythm, 0 },
			{ PanelShape.Square, 100 },
			{ PanelShape.ControlSquareMaster, 100 },
			{ PanelShape.ControlSquarePassive, 100 },
			{ PanelShape.HexagonShape, 67 },
			{ PanelShape.TriangleShape, 134 },
			{ PanelShape.MiniTriangleShape, 67 },
			{ PanelShape.ShapeController, 0 },
			{ PanelShape.ElementsHexagons, 134 },
			{ PanelShape.ElementsHexagonsCorner, 33.5f }, //33.5 / 58?
			{ PanelShape.LinesConnector, 11 },
			{ PanelShape.LightLines, 154 },
			{ PanelShape.LightLinesSingleZone, 77 },
			{ PanelShape.ControllerCap, 11 },
			{ PanelShape.PowerConnector, 11 }
		};
	}

	public class Layout
	{
		[JsonPropertyName("numPanels")]
		public int NumPanels { get; set; }
		[JsonPropertyName("sideLength")]
		public int SideLength { get; set; }
		[JsonPropertyName("positionData")]
		public List<PanelPosition> Positions { get; set; }
	}

	public class PanelPosition : Position
	{
		[JsonPropertyName("panelId")]
		public ushort PanelID { get; set; }
		[JsonPropertyName("shapeType")]
		public PanelShape PanelShape { get; set; }
	}

	public enum PanelShape
	{
		Triangle = 0,
		Rhythm = 1,
		Square = 2,
		ControlSquareMaster = 3,
		ControlSquarePassive = 4,
		HexagonShape = 7,
		TriangleShape = 8,
		MiniTriangleShape = 9,
		ShapeController = 12,
		ElementsHexagons = 14,
		ElementsHexagonsCorner = 15,
		LinesConnector = 16,
		LightLines = 17,
		LightLinesSingleZone = 18,
		ControllerCap = 19,
		PowerConnector = 20
	}
}
