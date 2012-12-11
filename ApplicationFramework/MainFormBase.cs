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

	}
}

