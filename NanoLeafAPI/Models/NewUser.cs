using System.Text.Json.Serialization;

namespace NanoLeafAPI.Models
{
	internal class NewUser
	{
		[JsonPropertyName("auth_token")]
		public string AuthToken { get; set; }
	}
}
