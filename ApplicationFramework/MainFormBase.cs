using System;
using System.Windows.Forms;
using CoreUtilities;

namespace appframe
{
	public class MainFormBase :  System.Windows.Forms.Form
	{
		#region gui
		protected MenuStrip MainMenu;
		#endregion
		public MainFormBase ()
		{
			//Mono.Unix.Catalog.Init ("yom", "strings");
			//Console.WriteLine(Mono.Unix.Catalog.GetString("Hello world!"));
			MainMenu = new MenuStrip();
			ToolStripMenuItem item = new ToolStripMenuItem(Loc.Instance.Cat.GetString("File"));
			
			MainMenu.Items.Add (item);
			this.Controls.Add(MainMenu);
			

		}

		/// <summary>
		/// Pushes screens across both monitors
		/// </summary>
		public void Screens_AcrossTwo()
		{
			System.Windows.Forms.Screen[] currentScreen = System.Windows.Forms.Screen.AllScreens;
			this.WindowState = FormWindowState.Normal;
			int nTotalWith = 0;
			int nSmallestHeight = 100000;
			foreach (Screen screen in currentScreen)
			{
				nTotalWith = nTotalWith + screen.WorkingArea.Width;
				if (screen.WorkingArea.Height < nSmallestHeight)
				{
					nSmallestHeight = screen.WorkingArea.Height;
				}
			}
			this.Top = currentScreen[0].WorkingArea.Top;
			this.Width = nTotalWith;
			this.Height = nSmallestHeight;
			// adjust height to /smallest screen
		}
		/// <summary>
		/// Flips just across one screen
		/// </summary>
		public void Screens_JustOne()
		{
			System.Windows.Forms.Screen[] currentScreen = System.Windows.Forms.Screen.AllScreens;
			this.Top = currentScreen[0].WorkingArea.Top;
			this.Width = currentScreen[0].WorkingArea.Width;
			this.Height = currentScreen[0].WorkingArea.Height;
			this.WindowState = FormWindowState.Maximized;
			// adjust height to /smallest screen
		}

	}
}

