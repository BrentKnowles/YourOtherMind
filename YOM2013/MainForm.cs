using System;
using System.Windows.Forms;
using CoreUtilities;

namespace YOM2013
{
	public class MainForm : appframe.MainFormBase 
	{
		#region gui
		Layout.LayoutPanel MainLayout;
		#endregion
		public MainForm ()
		{





			try {
				Loc.Instance.ChangeLocale("en-US");
				Console.WriteLine ("Current culture {0}", System.Threading.Thread.CurrentThread.CurrentUICulture);
				Console.WriteLine(Loc.Instance.Cat.GetString ("Hello World3!"));
				Console.WriteLine(Loc.Instance.Cat.GetStringFmt("This program is running proccess {0}", "Just kidding"));
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}

			ToolStripMenuItem Save = new ToolStripMenuItem(Loc.Instance.Cat.GetString("Save"));
			((ToolStripMenuItem)MainMenu.Items[0]).DropDownItems.Add (Save);
			Save.Click += HandleSaveClick;


			this.FormClosed += HandleFormClosed;
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			MainLayout = new Layout.LayoutPanel();
			MainLayout.Parent = this;
			MainLayout.Visible = true;
			MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			MainLayout.BringToFront();
			Console.WriteLine(Loc.Instance.Cat.GetString ("A new test!"));

			//Screens_AcrossTwo();

		}

		void HandleSaveClick (object sender, EventArgs e)
		{
			MainLayout.SaveLayout ();
		}

		void HandleFormClosed (object sender, FormClosedEventArgs e)
		{
			Application.Exit ();
		}
	}
}



