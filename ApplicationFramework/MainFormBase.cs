using System;
using System.Windows.Forms;
using CoreUtilities;
//using Layout;
using System.Collections;
using System.Collections.Generic;

namespace appframe
{
	public class MainFormBase :  System.Windows.Forms.Form
	{
		#region variables
		private string path = "";
		protected string Path {
			get { return path;}
			set { path = value;}
		}
		private string FindMenu {
			get { return Loc.Instance.GetString ("File");}
		}
		private string ToolMenu {
			get { return Loc.Instance.GetString ("Tools");}
		}
		private ToolStripMenuItem FindMenuItem;
		private ToolStripMenuItem ToolMenuItem;
		AddIns addIns;
		List<MefAddIns.Extensibility.mef_IBase> AddInsLoaded;

		List<iConfig> optionPanels;

		#endregion

		#region gui
		protected MenuStrip MainMenu;


		Action<bool> ForceShutDownMethod;

		#endregion
		/// <summary>
		/// Initializes a new instance of the <see cref="appframe.MainFormBase"/> class.
		/// </summary>
		/// <param name='_path'>
		/// _path.
		/// </param>
		/// <param name='_ForceShutDownMethod'>
		/// _ force shut down method.
		/// </param>
		/// <param name='Storage'>
		/// Name of a database or other file source where any CORE iConfig components should store there data (currently only AddIns)
		/// </param>
		public MainFormBase (string _path, Action<bool> _ForceShutDownMethod, string Storage)
		{
			Path = _path;
			ForceShutDownMethod = _ForceShutDownMethod;
			//Mono.Unix.Catalog.Init ("yom", "strings");
			//Console.WriteLine(Mono.Unix.Catalog.GetString("Hello world!"));
			MainMenu = new MenuStrip();
			FindMenuItem = new ToolStripMenuItem(FindMenu);

			ToolMenuItem = new ToolStripMenuItem(ToolMenu);
			ToolMenuItem.Name="ToolsMenu";
			ToolStripButton optionsButton = new ToolStripButton();
			optionsButton.Text = Loc.Instance.GetString("Options");
			ToolMenuItem.DropDownItems.Add (optionsButton);
			optionsButton.Click+= HandleOptionsButtonClick;
			
			MainMenu.Items.Add (FindMenuItem);
			MainMenu.Items.Add (ToolMenuItem);
			this.Controls.Add(MainMenu);
			this.FormClosing+= HandleFormClosing;


			addIns = new AddIns(System.IO.Path.Combine(Path,"plugins") , Storage);
			optionPanels = new List<iConfig>();

			// Register Option Panels
			optionPanels.Add (addIns);



			
			// STEP #3 When Option Panel closed needs to activate/deacative : return a list of Plugins that should be Activated (if not) and those that should be deactivated (if are)
			StartAndStopPlugIns();


		}
	
		void StartAndStopPlugIns ()
		{

			List<MefAddIns.Extensibility.mef_IBase> FullListOfAddins = addIns.BuildListOfAddins ();
			List<string> myList = addIns.GetListOfInstalledPlugs ();

			// TO DO: Need to set up an IsActive system. For now assume active=true;
			foreach (MefAddIns.Extensibility.mef_IBase AddIn in FullListOfAddins) {

				MefAddIns.Extensibility.mef_IBase AddInAlreadyIn = null;
				if (AddInsLoaded != null) {
					AddInAlreadyIn = AddInsLoaded.Find (mef_IBase => mef_IBase.CalledFrom.GUID == AddIn.CalledFrom.GUID);
				} else {
					AddInsLoaded = new List<MefAddIns.Extensibility.mef_IBase> ();
				}
				if (AddInAlreadyIn == null) {



					if (myList.Contains (AddIn.CalledFrom.GUID) == true) {
						// If Plug not already Loaded then Load it
						//NewMessage.Show ("Adding " + AddIn.Name);
						AddInsLoaded.Add (AddIn);
						if (AddIn.CalledFrom.IsOnAMenu == true) {
				
			
			

							string myMenuName = AddIn.CalledFrom.MyMenuName;
							if (Constants.BLANK != myMenuName) {
								string parentName = AddIn.CalledFrom.ParentMenuName;
								if (Constants.BLANK != parentName) {
									// add this menu option
									ToolStripItem[] items = MainMenu.Items.Find (parentName, true);
									if (items.Length > 0) {
										if (items [0].GetType () == typeof(ToolStripMenuItem)) {
											ToolStripButton but = new ToolStripButton (AddIn.CalledFrom.MyMenuName);
											((ToolStripMenuItem)items [0]).DropDownItems.Add (but);
											but.Click += (object sender2, EventArgs e2) => AddIn.RespondToCallToAction ();

											AddIn.Hookups.Add (but);
										}
									}

								} else {
									// create us as a parent
							
									ToolStripMenuItem parent = new ToolStripMenuItem (AddIn.CalledFrom.MyMenuName);
									parent.Click += (object sender2, EventArgs e2) => AddIn.RespondToCallToAction ();
									MainMenu.Items.Add (parent);
									AddIn.Hookups.Add (parent);


								}
							}
						} // OnAMenu
					} // Is allowed to be loaded (i.e., set in Options)
					//TODO: Register NoteTYpes
					if (AddIn.GetType () == typeof(MefAddIns.Extensibility.mef_INotes)) {
						((MefAddIns.Extensibility.mef_INotes)AddIn).RegisterType ();
					}
					else
					{

						// Means we WERE NOT added and WE DO NOT WANT TO ADD THEM
						// Do not do anything here

					
					}
				}//Not Added Yet

			}

			// Remove installed plugins

			// We exist but we are not on the AddMe list.
			// This means we need to be removed
			for  (int i = AddInsLoaded.Count-1; i >= 0; i--)
			{
				MefAddIns.Extensibility.mef_IBase addin =  AddInsLoaded[i];
				// look at each plug and decide if it should be removed (i.e., it is no longer in the list to add
				if (myList.Contains (addin.CalledFrom.GUID) == false)
				{
					// we are NOT in the list but we exist in the active list
					// this means we must be destroyed!!
					NewMessage.Show ("Destroy : " + addin.CalledFrom.GUID);
					foreach (IDisposable connection in addin.Hookups)
					{
						connection.Dispose();
					}

					// TODO: if notetype need to degresigtser

					AddInsLoaded.Remove (addin);
				}
			}
		}

		void HandleOptionsButtonClick (object sender, EventArgs e)
		{
			OptionForm options = new OptionForm (optionPanels);
			if (options.ShowDialog () == DialogResult.OK) {
				foreach (iConfig addIn in optionPanels)
				{
					addIn.SaveRequested();
				}
				StartAndStopPlugIns();
			}





		}

		void HandleFormClosing (object sender, FormClosingEventArgs e)
		{
			if (CoreUtilities.File.CheckForFileError () == true) {
				// if this is true it means the harddrive if failing to write
				// which means we abort the app without proper shutdown
				NewMessage.Show (Loc.Instance.GetString("Your harddrive failed to write a TEST file. Because this might mean you are suffering harddrive failure we are aborting WITHOUT saving any existing files so that we do not CORRUPT them."));
				// we also set a variable to inform others of this
				//LayoutDetails.Instance.ForceShutdown = true;
				ForceShutDownMethod(true);
			}
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
		public ToolStripMenuItem GetToolMenu ()
		{
			return ToolMenuItem;
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

