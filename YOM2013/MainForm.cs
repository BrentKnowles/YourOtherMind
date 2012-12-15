using System;
using System.Windows.Forms;
using CoreUtilities;
using Layout;

namespace YOM2013
{
	public class MainForm : appframe.MainFormBase 
	{
		#region gui
		Layout.LayoutPanel CurrentLayout;
		#endregion
		public MainForm ()
		{





			try {
				Loc.Instance.ChangeLocale ("en-US");
				Console.WriteLine ("Current culture {0}", System.Threading.Thread.CurrentThread.CurrentUICulture);
				Console.WriteLine (Loc.Instance.Cat.GetString ("Hello World3!"));
				Console.WriteLine (Loc.Instance.Cat.GetStringFmt ("This program is running proccess {0}", "Just kidding"));
			} catch (Exception ex) {
				Console.WriteLine (ex.ToString ());
			}

			Console.WriteLine(LayoutDetails.Instance.Path);



			this.FormClosed += HandleFormClosed;
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			CurrentLayout = new Layout.LayoutPanel (CoreUtilities.Constants.BLANK);
			CurrentLayout.Parent = this;
			CurrentLayout.Visible = true;
			CurrentLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			CurrentLayout.BringToFront ();
			Console.WriteLine (Loc.Instance.Cat.GetString ("A new test!"));

			ToolStripMenuItem file = this.GetFileMenu ();
			if (file == null) {
				throw new Exception ("File menu was not defined in base class");
			} else {


					ToolStripMenuItem Save = new ToolStripMenuItem (Loc.Instance.Cat.GetString ("Save"));
				file.DropDownItems.Add(Save);
			//	((ToolStripMenuItem)MainMenu.Items [0]).DropDownItems.Add (Save);
				Save.Click += HandleSaveClick;

				ToolStripMenuItem Backup = new ToolStripMenuItem(Loc.Instance.Cat.GetString("Backup"));
				Backup.Click += HandleBackupClick;
				file.DropDownItems.Add (Backup);
			}


			//Screens_AcrossTwo();

		}

		void HandleBackupClick (object sender, EventArgs e)
		{
			Console.WriteLine (CurrentLayout.Backup ());
		}

		void HandleSaveClick (object sender, EventArgs e)
		{
			CurrentLayout.SaveLayout ();
		}

		void HandleFormClosed (object sender, FormClosedEventArgs e)
		{
			if (false == LayoutDetails.Instance.ForceShutdown) {
				if (true == CurrentLayout.GetSaveRequired) {
					NewMessage.Show ("shoulda saved");
				}
			} else {
				// we DO NOT allow subforms to save in the situation where there might be corruption
				NewMessage.Show ("Shutting down without saving due to file corruption");
			}
			Application.Exit ();
		}
	}
}



