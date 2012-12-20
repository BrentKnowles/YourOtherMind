namespace MefAddIns
{
	using MefAddIns.Extensibility;
	using System.ComponentModel.Composition;
	using System;
	using System.Windows.Forms;
	using System.Diagnostics;
	using System.Collections.Generic;
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

		long min = long.MaxValue;
		long max = 0;
		Addin_Test2.memoryForm form;

		public void RespondToCallToAction ()
		{
			ShowWindow ();
		}

		public void ShowWindow()
		{
			Timer timer = new Timer();
			timer.Tick +=new EventHandler(timer_Tick);
			timer.Interval = 2000;
			timer.Start();
			form = new Addin_Test2.memoryForm();

			form.Show();
		}
		void timer_Tick(object sender, EventArgs e)
		{
			Process proc = Process.GetCurrentProcess();
			long size = proc.PrivateMemorySize64;
			if (size < min)
			{
				min = size;
				form.labelMin.Text = String.Format ("Min: {0}",size.ToString());
			}
			if (size > max)
			{
				max = size;
				form.labelMax.Text = String.Format ("Max: {0}",size.ToString());
			}
			form.labelMemoryUse.Text = String.Format ("Current: {0}",size.ToString());
		}
		bool iscopy = false;

		public bool IsCopy {
			get{ return iscopy;}
			set{  iscopy = value;}
		}

		string GUID = "memorytrack";
		public PlugInAction CalledFrom { 
			get
			{
				PlugInAction action = new PlugInAction();
				action.HotkeyNumber = -1;
				action.MyMenuName = "Memory Usage";
				action.ParentMenuName = "ToolsMenu";
				//action.ParentMenuName = "";
				action.IsOnAMenu = true;
				action.IsOnAToolbar = false;
				action.IsANote = false;
				action.GUID = GUID;

				return action;
			} 
		}
		public void SetGuid(string s)
		{
			GUID = s;
		}
		List<IDisposable> hookups = null;
		public List<IDisposable>  Hookups {
			get {
				if (null == hookups)
					hookups = new List<IDisposable> ();
				return hookups;
			}
			set { hookups = value;}

		}


	}
}
