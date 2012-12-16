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
		Layout.LayoutPanel SystemLayout;
		#endregion
		public MainForm ()
		{

			lg.Instance.Loudness = Loud.CTRIVIAL;



			try {
				Loc.Instance.ChangeLocale ("en-US");
				Console.WriteLine ("Current culture {0}", System.Threading.Thread.CurrentThread.CurrentUICulture);
				Console.WriteLine (Loc.Instance.Cat.GetString ("Hello World3!"));
				Console.WriteLine (Loc.Instance.Cat.GetStringFmt ("This program is running proccess {0}", "Just kidding"));
			} catch (Exception ex) {
				Console.WriteLine (ex.ToString ());
			}

			Console.WriteLine (LayoutDetails.Instance.Path);



			this.FormClosed += HandleFormClosed;
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

			//GetSystemPanel ();
			SystemLayout = new Layout.LayoutPanel (CoreUtilities.Constants.BLANK, true);
			SystemLayout.Parent = this;
			SystemLayout.Visible = true;
			SystemLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			SystemLayout.BringToFront ();
			//SystemLayout.Visible = false;
			SystemLayout.LoadLayout ("system");

			Control MDIHOST = SystemLayout.GetSystemPanel ();
			if (MDIHOST == null) {
				NewMessage.Show ("no system panel was found on system layout");
				Application.Exit ();
			}


			CurrentLayout = new Layout.LayoutPanel (CoreUtilities.Constants.BLANK);
			CurrentLayout.BorderStyle = BorderStyle.Fixed3D;
			CurrentLayout.Parent = MDIHOST;
			CurrentLayout.Visible = true;
			CurrentLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			CurrentLayout.BringToFront ();


			ToolStripMenuItem file = this.GetFileMenu ();
			if (file == null) {
				throw new Exception ("File menu was not defined in base class");
			} else {


				ToolStripMenuItem New = new ToolStripMenuItem (Loc.Instance.Cat.GetString ("New Layout"));
				file.DropDownItems.Add(New);
				//	((ToolStripMenuItem)MainMenu.Items [0]).DropDownItems.Add (Save);
				New.Click += HandleNewClick;

					ToolStripMenuItem Save = new ToolStripMenuItem (Loc.Instance.Cat.GetString ("Save"));
				file.DropDownItems.Add(Save);
			//	((ToolStripMenuItem)MainMenu.Items [0]).DropDownItems.Add (Save);
				Save.Click += HandleSaveClick;

				ToolStripMenuItem Backup = new ToolStripMenuItem(Loc.Instance.Cat.GetString("Backup"));
				Backup.Click += HandleBackupClick;
				file.DropDownItems.Add (Backup);
			}


			//Screens_AcrossTwo();
			this.KeyPreview = true;
			this.KeyDown += HandleMainFormKeyDown;
		}

		DockStyle lastDockStyle= DockStyle.None;

		void HandleMainFormKeyDown (object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F6) {
				if (CurrentLayout.Parent.Dock != DockStyle.Fill)
				{
					lastDockStyle = CurrentLayout.Parent.Dock;
				CurrentLayout.Parent.Dock = DockStyle.Fill;
				CurrentLayout.Parent.BringToFront();
				}
				else
				{
					CurrentLayout.Parent.Dock = lastDockStyle;
				}
			}
		}

		void HandleNewClick (object sender, EventArgs e)
		{
			if (true == CurrentLayout.GetSaveRequired) {
				NewMessage.Show ("shoulda saved");
			}
			string guid = System.Guid.NewGuid().ToString();
			//CurrentLayout = new LayoutPanel(Constants.BLANK);
			CurrentLayout.NewLayout (guid);
		}

		void HandleBackupClick (object sender, EventArgs e)
		{
			Console.WriteLine (CurrentLayout.Backup ());
		}

		void Save()
		{

			SystemLayout.SaveLayout();
			CurrentLayout.SaveLayout ();

		}

		void HandleSaveClick (object sender, EventArgs e)
		{
			Save ();
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



