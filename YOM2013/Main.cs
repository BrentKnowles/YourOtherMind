using System;
using System.Windows.Forms;

namespace YOM2013
{
	public class MainClass
	{
		public static void Main (string[] args)
		{
			//Application.Init ();
			MainForm win = new MainForm ();
			win.Show ();
			Application.Run ();
		}
	}
}

