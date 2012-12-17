namespace MefAddIns
{
	using MefAddIns.Extensibility;
	using System.ComponentModel.Composition;
	using System;
	using System.Windows.Forms;
	using System.Diagnostics;

	/// <summary>
	/// Provides an implementation of a supported language by implementing ISupportedLanguage. 
	/// Moreover it uses Export attribute to make it available thru MEF framework.
	/// </summary>
	[Export(typeof(mef_IShowFormBasic))]
	public class Addin_MemoryTrack : mef_IShowFormBasic
	{
		public string Author
		{
			get { return @"Brent Knowles"; }
		}
		public string Version
		{
			get { return @"1.0.0.0"; }
		}
		public string Description
		{
			get { return "Displays current memory usage."; }
		}
		public string Name
		{
			get { return @"Memory"; }
		}

		public void Boom()
		{
			Console.WriteLine("BRAINS!!");
		}
		public string Tester(string s)
		{ 

			Console.WriteLine (s);
			return "Archie!";
		}
		public void ShowWindow()
		{
			Process proc = Process.GetCurrentProcess();
			long size = proc.PrivateMemorySize64;
			MessageBox.Show (size.ToString());
		}

	}
}
