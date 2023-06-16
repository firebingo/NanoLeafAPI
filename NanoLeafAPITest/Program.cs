using NanoLeafAPI;
using NanoLeafAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Program
{
	static bool _run = true;
	static PanelLayout _layout;
	static NanoLeafController _ctl;

	public static async Task Main()
	{
		var ip = "";
		var token = "";

		if (string.IsNullOrWhiteSpace(token))
		{
			Console.WriteLine("Hold power button on controller until lights start blinking, then hit enter");
			Console.ReadLine();
			_ctl = new NanoLeafController(ip);
			token = await _ctl.GetNewToken();
			Console.WriteLine(token);
		}
		else
			_ctl = new NanoLeafController(ip, token);

		var state = await _ctl.GetCurrentState();
		_layout = state.PanelLayout;
		var startCtl = await _ctl.StartExternalControl();

		_ = RunThread();
		Console.ReadLine();
		_run = false;
		await Task.Delay(100);
		await _ctl.StopExternalControl();
	}

	public static async Task RunThread()
	{
		ushort delta = 16;
		var buffer = new byte[1];
		do
		{
			await Task.Delay(delta);
			if (!_run)
				break;

			var frame = new ExtControlFrame()
			{
				Panels = new List<ExtControlPanel>()
			};
			foreach (var panel in _layout.Layout.Positions)
			{
				Random.Shared.NextBytes(buffer);
				var r = buffer[0];
				Random.Shared.NextBytes(buffer);
				var g = buffer[0];
				Random.Shared.NextBytes(buffer);
				var b = buffer[0];
				frame.Panels.Add(new ExtControlPanel()
				{
					PanelID = panel.PanelID,
					Red = r,
					Blue = g,
					Green = b,
					TransitionTime = 0
				});
			}
			if (!_run)
				break;
			await _ctl.SendExternalControlFrame(frame);
		} while (_run);
	}
}