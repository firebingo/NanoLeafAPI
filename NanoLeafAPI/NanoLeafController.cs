using NanoLeafAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace NanoLeafAPI
{
	public class NanoLeafController : IDisposable
	{
		private bool _isDisposed;
		private IPAddress _extControlIp;
		private ushort _extControlPort = 60222;
		private Socket _extControlSocket;
		private IPEndPoint _extControlEndpoint;
		private State _preExtState;
		private List<byte> _extControlStream;

		private readonly HttpClient _httpClient;
		public IPAddress IP { get; }
		public ushort Port { get; } = 16021;
		public string AuthToken { get; private set; }

		public NanoLeafController(string ip)
		{
			if (!IPAddress.TryParse(ip, out var parseIp) || parseIp == null)
				throw new ArgumentException("IP could not be parsed");

			IP = parseIp;
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri($"http://{ip}:{Port}/")
			};
		}

		public NanoLeafController(string ip, string authToken)
		{
			if (!IPAddress.TryParse(ip, out var parseIp) || parseIp == null)
				throw new ArgumentException("IP could not be parsed");

			IP = parseIp;
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri($"http://{ip}:{Port}/")
			};
			AuthToken = authToken;
		}

		public NanoLeafController(string ip, ushort port)
		{
			if (!IPAddress.TryParse(ip, out var parseIp) || parseIp == null)
				throw new ArgumentException("IP could not be parsed");

			Port = port;
			IP = parseIp;
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri($"http://{ip}:{Port}/")
			};
		}

		public NanoLeafController(string ip, ushort port, string authToken)
		{
			if (!IPAddress.TryParse(ip, out var parseIp) || parseIp == null)
				throw new ArgumentException("IP could not be parsed");

			Port = port;
			IP = parseIp;
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri($"http://{ip}:{Port}/")
			};
			AuthToken = authToken;
		}

		public async Task<string> GetNewToken()
		{
			using var newRes = await _httpClient.PostAsync("/api/v1/new", null);
			var stringRes = await newRes?.Content?.ReadAsStringAsync();
			if (!newRes.IsSuccessStatusCode || string.IsNullOrWhiteSpace(stringRes))
				throw new Exception($"API Error: Code: {newRes.StatusCode}\nResponse:\n{stringRes}");

			var tokenRes = JsonSerializer.Deserialize<NewUser>(stringRes);
			if (string.IsNullOrWhiteSpace(tokenRes?.AuthToken))
				throw new Exception($"Failed to parse response\n{stringRes}");

			AuthToken = tokenRes.AuthToken;
			return tokenRes.AuthToken;
		}

		public async Task DeleteToken()
		{
			using var deleteRes = await _httpClient.DeleteAsync($"/api/v1/{AuthToken}");
			if (!deleteRes.IsSuccessStatusCode)
				throw new Exception($"API Error: Code: {deleteRes.StatusCode}");
		}

		public async Task<ControllerInfo> GetCurrentState()
		{
			using var getRes = await _httpClient.GetAsync($"/api/v1/{AuthToken}");
			var stringRes = await getRes?.Content?.ReadAsStringAsync();
			if (!getRes.IsSuccessStatusCode || string.IsNullOrWhiteSpace(stringRes))
				throw new Exception($"API Error: Code: {getRes.StatusCode}\nResponse:\n{stringRes}");

			var infoRes = JsonSerializer.Deserialize<ControllerInfo>(stringRes);
			return infoRes ?? throw new Exception($"Failed to parse response\n{stringRes}");
		}

		public async Task SetState(SetState state)
		{
			if (state == null)
				throw new ArgumentException($"Null state");

			var stringState = JsonSerializer.Serialize(state);
			using var content = new StringContent(stringState, System.Text.Encoding.UTF8, "application/json");
			content.Headers.ContentType.CharSet = "";
			var putRes = await _httpClient.PutAsync($"/api/v1/{AuthToken}/state", content);
			var s = await putRes.Content?.ReadAsStringAsync();
			if (!putRes.IsSuccessStatusCode)
				throw new Exception($"API Error: Code: {putRes.StatusCode}");
		}

		public async Task SelectEffect(string effect)
		{
			var setValue = new Select() { Value = effect };
			var stringSelect = JsonSerializer.Serialize(setValue);
			using var content = new StringContent(stringSelect, System.Text.Encoding.UTF8, "application/json");
			var putRes = await _httpClient.PutAsync($"/api/v1/{AuthToken}/effects", content);
			if (!putRes.IsSuccessStatusCode)
				throw new Exception($"API Error: Code: {putRes.StatusCode}");
		}

		public async Task<bool> StartExternalControl()
		{
			var getState = await GetCurrentState();
			_preExtState = getState.State;

			var value = new WriteExtControl();
			var stringValue = JsonSerializer.Serialize(value);
			using var content = new StringContent(stringValue, System.Text.Encoding.UTF8, "application/json");
			var putRes = await _httpClient.PutAsync($"/api/v1/{AuthToken}/effects", content);
			var stringRes = await putRes?.Content?.ReadAsStringAsync();
			if (!putRes.IsSuccessStatusCode)
				throw new Exception($"API Error: Code: {putRes.StatusCode}\nResponse:\n{stringRes}");

			_extControlIp = IP;
			_extControlSocket?.Dispose();
			_extControlSocket = new Socket(_extControlIp.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
			_extControlEndpoint = new IPEndPoint(_extControlIp, _extControlPort);
			_extControlStream = new List<byte>();

			return true;
		}

		public async Task StopExternalControl()
		{
			var setState = new SetState()
			{
				//On = _preExtState.On,
				Brightness = new ValueDuration()
				{
					Value = _preExtState.Brightness.Value
				}
			};
			if (_preExtState.ColorMode == "hs")
			{
				setState.Hue = new ValueInt()
				{
					Value = _preExtState.Hue.Value
				};
				setState.Saturation = new ValueInt()
				{
					Value = _preExtState.Saturation.Value
				};
			}
			else if (_preExtState.ColorMode == "ct")
			{
				setState.Temperature = new ValueInt()
				{
					Value = _preExtState.Temperature.Value
				};
			}
			await SetState(setState);
			_preExtState = null;

			_extControlSocket?.Dispose();
			_extControlSocket = null;
			_extControlEndpoint = null;
		}

		public async Task SendExternalControlFrame(ExtControlFrame frame)
		{
			//Panel count
			_extControlStream.Clear();
			_extControlStream.AddRange(ShortToBytes((ushort)frame.Panels.Count));
			//Panels
			foreach (var panel in frame.Panels)
			{
				_extControlStream.AddRange(ShortToBytes(panel.PanelID));
				_extControlStream.Add(panel.Red);
				_extControlStream.Add(panel.Green);
				_extControlStream.Add(panel.Blue);
				//White, unused
				_extControlStream.Add(0);
				_extControlStream.AddRange(ShortToBytes(panel.TransitionTime));
			}

			await _extControlSocket.SendToAsync(_extControlStream.ToArray(), _extControlEndpoint);
		}

		private static byte[] ShortToBytes(ushort value)
		{
			var bytes = BitConverter.GetBytes(value);
			//Nanoleaf wants 2 byte values in big endian
			return BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed)
				return;

			if (disposing)
			{
				_extControlSocket?.Dispose();
				_httpClient?.Dispose();
			}

			_isDisposed = true;
		}

		~NanoLeafController()
		{
			Dispose(false);
		}
	}
}