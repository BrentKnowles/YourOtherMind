using System;
using System.Windows.Forms;
using CoreUtilities;
using Layout;
using System.Collections.ObjectModel;




using System.ComponentModel.Composition.Hosting;
	using System.ComponentModel.Composition;


namespace YOM2013
{
	public class MainForm : appframe.MainFormBase 
	{
		#region gui
		Layout.LayoutPanel CurrentLayout;
		Layout.LayoutPanel SystemLayout;
		Control MDIHOST=null;
		#endregion
		/// <summary>
		/// A series of true/false and similiar settings that should be double checked before deployments as some of them
		/// are for debugging
		/// 
		/// </summary>
		private void Switches()
		{

		}
	

		public MainForm (string _path, Action<bool>ForceShutDownMethod, string storage) : base (_path,ForceShutDownMethod,storage)
		{

			//Path = LayoutDetails.Instance.Path;

			Switches();

			lg.Instance.Loudness = Loud.CTRIVIAL;
			LayoutDetails.Instance.LoadLayoutRef = LoadLayout;


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

			MDIHOST = SystemLayout.GetSystemPanel ();
			if (MDIHOST == null) {
				NewMessage.Show ("no system panel was found on system layout");
				Application.Exit ();
			}



			//LoadLayout ( "");


			ToolStripMenuItem file = this.GetFileMenu ();
			if (file == null) {
				throw new Exception ("File menu was not defined in base class");
			} else {


				ToolStripMenuItem New = new ToolStripMenuItem (Loc.Instance.Cat.GetString ("New Layout"));
				file.DropDownItems.Add(New);
				//	((ToolStripMenuItem)MainMenu.Items [0]).DropDownItems.Add (Save);
				New.Click += HandleNewClick;

				ToolStripMenuItem Test = new ToolStripMenuItem (Loc.Instance.Cat.GetString ("TEST"));
				file.DropDownItems.Add(Test);
				//	((ToolStripMenuItem)MainMenu.Items [0]).DropDownItems.Add (Save);
				Test.Click += HandleTestClick;

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

		void HandleTestClick (object sender, EventArgs e)
		{
		
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
		/// <summary>
		/// Loads the layout.
		/// </summary>
		/// <param name='MDIHOST'>
		/// MDIHOS.
		/// </param>
		/// <param name='guidtoload'>
		/// Guidtoload.
		/// </param>
		void LoadLayout (string guidtoload)
		{
			if (MDIHOST == null) {
				throw new Exception("Major problem. No MDIHOST set.");
			}
			if (CurrentLayout != null) {
				TestAndSaveIfNecessary ();
				CurrentLayout.Dispose();
			}
			CurrentLayout = new Layout.LayoutPanel (CoreUtilities.Constants.BLANK);
			CurrentLayout.BorderStyle = BorderStyle.Fixed3D;
			CurrentLayout.Parent = MDIHOST;
			CurrentLayout.Visible = true;
			CurrentLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			CurrentLayout.BringToFront ();
			CurrentLayout.LoadLayout(guidtoload);
			LayoutDetails.Instance.CurrentLayout = CurrentLayout;
		}

		void HandleNewClick (object sender, EventArgs e)
		{

			TestAndSaveIfNecessary ();
			string guid = System.Guid.NewGuid().ToString();
			//CurrentLayout = new LayoutPanel(Constants.BLANK);
			CurrentLayout.NewLayout (guid);
		}

		void HandleBackupClick (object sender, EventArgs e)
		{
			if (CurrentLayout == null) {
				NewMessage.Show ("Please load a layout first");
				// TODO: Backup should be an operation at the MasterOfLayoutsLevel
			}
			else
			Console.WriteLine (CurrentLayout.Backup ());
		}

		void Save ()
		{

			SystemLayout.SaveLayout ();
			if (CurrentLayout != null) {
				CurrentLayout.SaveLayout ();
			}

		}

		void HandleSaveClick (object sender, EventArgs e)
		{
			Save ();
		}
		/// <summary>
		/// TODO: Finish properly
		/// 
		/// - test if Autosave is on and does a save (if needed)
		/// </summary>
		/// <returns>
		/// <c>true</c>, if and save if necessary was tested, <c>false</c> otherwise.
		/// </returns>
		void TestAndSaveIfNecessary ()
		{
			if (CurrentLayout != null) {
				if (true == CurrentLayout.GetSaveRequired) {
					NewMessage.Show ("shoulda saved");
				}
			}
		}

		void HandleFormClosed (object sender, FormClosedEventArgs e)
		{
			if (false == LayoutDetails.Instance.ForceShutdown) {
				TestAndSaveIfNecessary ();
			} else {
				// we DO NOT allow subforms to save in the situation where there might be corruption
				NewMessage.Show ("Shutting down without saving due to file corruption");
			}
			Application.Exit ();
		}
	}
}



