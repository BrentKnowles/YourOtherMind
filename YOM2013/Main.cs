// Main.cs
//
// Copyright (c) 2013 Brent Knowles (http://www.brentknowles.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Review documentation at http://www.yourothermind.com for updated implementation notes, license updates
// or other general information/
// 
// Author information available at http://www.brentknowles.com or http://www.amazon.com/Brent-Knowles/e/B0035WW7OW
// Full source code: https://github.com/BrentKnowles/YourOtherMind
//###
using System;
using System.Windows.Forms;
using Layout;
using CoreUtilities;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace YOM2013
{
	public class MainClass


	{

		//
		// For a single instance
		//
		//
		enum ShowWindowConstants {SW_HIDE = 0, SW_SHOWNORMAL = 1,  SW_NORMAL = 1,  SW_SHOWMINIMIZED = 2
			, SW_SHOWMAXIMIZED = 3,  SW_MAXIMIZE = 3,  SW_SHOWNOACTIVATE = 4,  SW_SHOW = 5,  SW_MINIMIZE = 6
			,  SW_SHOWMINNOACTIVE = 7,  SW_SHOWNA = 8,  SW_RESTORE = 9,  SW_SHOWDEFAULT = 10,  SW_FORCEMINIMIZE = 11
			,  SW_MAX = 11}

		[DllImport("User32.dll")]
		public static extern int ShowWindowAsync(IntPtr hWnd, int swCommand);



		[STAThread()]
		public static void Main (string[] args)
		{
			CoreUtilities.TimerCore.TimerOn = true;
			lg.Instance.Loudness = Loud.CTRIVIAL;
			//lg.Instance.Loudness = Loud.DOFF;
			//Application.Init ();
			string icontouse = "yom2.ico";

#if(DEBUG)
			icontouse = "main.ico";
#endif


			Process me = Process.GetCurrentProcess ();
			
			Process[] RunningProcesses = Process.GetProcessesByName (me.ProcessName.ToString ()); 
			
			//Change the name above to suit your application
			// Only works if not running in the debugger
			if (RunningProcesses.Length == 1 || Debugger.IsAttached) {

				MainForm win = new MainForm (LayoutDetails.Instance.Path, LayoutDetails.Instance.DoForceShutDown, LayoutDetails.Instance.YOM_DATABASE, FileUtils.GetIcon (icontouse), MasterOfLayouts.GetDatabaseType);
				win.Name = "mainform";
				//	win.Show ();
				Application.Run (win);
			} else {
				ShowWindowAsync(RunningProcesses[0].MainWindowHandle, (int)ShowWindowConstants.SW_SHOWMINIMIZED);
				ShowWindowAsync(RunningProcesses[0].MainWindowHandle, (int)ShowWindowConstants.SW_RESTORE);
			}
		}
	}
}

