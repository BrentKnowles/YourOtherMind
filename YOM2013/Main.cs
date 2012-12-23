using System;
using System.Windows.Forms;
using Layout;

namespace YOM2013
{
	public class MainClass
	{

		[STAThread()]
		public static void Main (string[] args)
		{
			//Application.Init ();
			MainForm win = new MainForm (LayoutDetails.Instance.Path, LayoutDetails.Instance.DoForceShutDown, LayoutDetails.Instance.YOM_DATABASE);
			win.Show ();
			Application.Run ();
		}
	}
}

