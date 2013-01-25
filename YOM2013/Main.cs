using System;
using System.Windows.Forms;
using Layout;
using CoreUtilities;

namespace YOM2013
{
	public class MainClass
	{

		[STAThread()]
		public static void Main (string[] args)
		{
			CoreUtilities.TimerCore.TimerOn=true;
			lg.Instance.Loudness = Loud.CTRIVIAL;
			//lg.Instance.Loudness = Loud.DOFF;
			//Application.Init ();
			MainForm win = new MainForm (LayoutDetails.Instance.Path, LayoutDetails.Instance.DoForceShutDown, LayoutDetails.Instance.YOM_DATABASE,  FileUtils.GetIcon("main.ico"));
			win.Name="mainform";
			win.Show ();
			Application.Run ();
		}
	}
}

