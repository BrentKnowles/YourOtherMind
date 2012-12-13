using System;
using System.Windows.Forms;
using CoreUtilities;

namespace appframe
{
	public class MainFormBase :  System.Windows.Forms.Form
	{
		#region variables

		private string FindMenu {
			get { return Loc.Instance.Cat.GetString ("File");}
		}
		private ToolStripMenuItem FindMenuItem;

		#endregion

		#region gui
		protected MenuStrip MainMenu;



		#endregion
		public MainFormBase ()
		{
			//Mono.Unix.Catalog.Init ("yom", "strings");
			//Console.WriteLine(Mono.Unix.Catalog.GetString("Hello world!"));
			MainMenu = new MenuStrip();
			FindMenuItem = new ToolStripMenuItem(FindMenu);
			
			MainMenu.Items.Add (FindMenuItem);
			this.Controls.Add(MainMenu);
			

		}
		/// <summary>
		/// Gets the file menu.
		/// </summary>
		/// <returns>
		/// The file menu.
		/// </returns>
		public ToolStripMenuItem GetFileMenu ()
		{
			return FindMenuItem;
			//return (ToolStripMenuItem)MainMenu.Items.Find (FindMenu, true)[0];
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

