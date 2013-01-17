using System;
using System.Windows.Forms;
using System.Drawing;

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

		// Add to this list to register more optionPanels
		protected List<iConfig> optionPanels;
		protected List<ContextMenuStrip> ContextMenus;
		protected List<NoteTextAction> NoteTextActions;
		#endregion

		#region gui
		protected MenuStrip MainMenu;
		protected StatusStrip FooterStatus; 

		Action<bool> ForceShutDownMethod;
		public static Icon MainFormIcon;

		#endregion

		void BuildFooterStatus ()
		{
			FooterStatus = new StatusStrip();
			this.Controls.Add (FooterStatus);
		}



		public virtual void BuildContextMenuStrips()
		{
			// will be overriden in mainform to create
		}
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
		public MainFormBase (string _path, Action<bool> _ForceShutDownMethod, string Storage, Icon mainFormIcon)
		{
			// stores any contextmenus we use, for access by plugin system
			ContextMenus = new List<System.Windows.Forms.ContextMenuStrip>();

			// List needs to be created here so it is available in derived MainForm AND in AddIn system
			NoteTextActions = new List<NoteTextAction> ();
			BuildListOfNoteTextAction(); 

			BuildContextMenuStrips();

			MainFormIcon = mainFormIcon;

			this.Icon = mainFormIcon;
			Path = _path;
			ForceShutDownMethod = _ForceShutDownMethod;
			//Mono.Unix.Catalog.Init ("yom", "strings");
			//Console.WriteLine(Mono.Unix.Catalog.GetString("Hello world!"));
			MainMenu = new MenuStrip();
			FindMenuItem = new ToolStripMenuItem(FindMenu);

			ToolMenuItem = new ToolStripMenuItem(ToolMenu);
			ToolMenuItem.Name="ToolsMenu";

			ContextMenuStrip AdvancedStrip = new System.Windows.Forms.ContextMenuStrip();
			AdvancedStrip.AutoSize = true;
			//AdvancedStrip.Items.Add ("test");

			ToolStripMenuItem Advanced = new ToolStripMenuItem();
			Advanced.AutoSize = true;
			Advanced.Name = "AdvancedMenu";
			Advanced.Text = Loc.Instance.GetString("Advanced");
			Advanced.DropDown = AdvancedStrip;


		
		

			ToolStripMenuItem Notes = new ToolStripMenuItem();
			Notes.Name="NotesMenu";
			Notes.Text = Loc.Instance.GetString ("Notes");




			ToolStripButton optionsButton = new ToolStripButton();
			optionsButton.Text = Loc.Instance.GetString("Options");

			optionsButton.Click+= HandleOptionsButtonClick;

			// build tools menu
			ToolMenuItem.DropDownItems.Add (optionsButton);
			ToolMenuItem.DropDownItems.Add (Advanced);


			MainMenu.Items.Add (FindMenuItem);
			MainMenu.Items.Add (Notes);
			MainMenu.Items.Add (ToolMenuItem);
			this.Controls.Add(MainMenu);
			this.FormClosing+= HandleFormClosing;


			addIns = new AddIns(System.IO.Path.Combine(Path,"plugins") , Storage);
			optionPanels = new List<iConfig>();

			// Register Option Panels
			optionPanels.Add (addIns);



			
			// STEP #3 When Option Panel closed needs to activate/deacative : return a list of Plugins that should be Activated (if not) and those that should be deactivated (if are)
			StartAndStopPlugIns();

			BuildFooterStatus();

		}
		/// <summary>
		/// Builds the addin tool strip. (Wrapper function)
		/// </summary>
		/// <param name='AddIn'>
		/// Add in.
		/// </param>
		/// <param name='but'>
		/// But.
		/// </param>
		void BuildAddinToolStrip (MefAddIns.Extensibility.mef_IBase AddIn, ToolStripItem but)
		{
			but.Name = AddIn.CalledFrom.GUID;
			but.Tag = AddIn;
			AddIn.DelegateTargetForGetAfterRespondInformation = HandleRespondInformationFromAddin;
			but.AutoSize = true;
			but.Click += HandleAddInClick;
			if (AddIn.CalledFrom.Image != null) {
				but.Image = AddIn.CalledFrom.Image;
			}

			but.ToolTipText = AddIn.CalledFrom.ToolTip;
			AddIn.Hookups.Add (but);


		}
	
		/// <summary>
		/// Starts the and stop plug ins.
		/// </summary>
		void StartAndStopPlugIns ()
		{

			if (null != addIns) {
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
							AddIn.RegisterType();

							if (AddIn.CalledFrom.IsNoteAction == true)
							{
								NoteTextActions.Add (new NoteTextAction(AddIn.ActionWithParam, AddIn.BuildFileName, AddIn.CalledFrom.MyMenuName, AddIn.CalledFrom.ToolTip));
							}
							else
							if (AddIn.CalledFrom.IsOnAMenu == true) {
				
			
			

								string myMenuName = AddIn.CalledFrom.MyMenuName;
								if (Constants.BLANK != myMenuName) {
									string parentName = AddIn.CalledFrom.ParentMenuName;
									if (Constants.BLANK != parentName) {

										if (AddIn.CalledFrom.IsOnContextStrip == true)
										{
											// search for a context strip
											ContextMenuStrip strip = ContextMenus.Find (ContextMenuStrip=>ContextMenuStrip.Name==parentName);
											if (strip != null)
											{
												ToolStripButton but = new ToolStripButton (AddIn.CalledFrom.MyMenuName);
												BuildAddinToolStrip (AddIn, but);
												strip.Items.Add (but);
											}
										}

										else // search The MainMenuInstead
										{






										// add this menu option
										ToolStripItem[] items = MainMenu.Items.Find (parentName, true);
										if (items.Length > 0) {
											if (items [0].GetType () == typeof(ToolStripMenuItem)) {

												if (((ToolStripMenuItem)items[0]).DropDown != null)
												{
												ToolStripButton but = new ToolStripButton (AddIn.CalledFrom.MyMenuName);
											    BuildAddinToolStrip (AddIn, but);



												//but.Tag = AddIn;
												//but.AutoSize = true;
												//but.Click += HandleAddInClick;
												((ToolStripMenuItem)items [0]).DropDownItems.Add (but);
												// hack to make sure menu is 'wide enough' tried associating contextmenustrip but that did not help
												((ToolStripMenuItem)items [0]).DropDownItems.Remove (((ToolStripMenuItem)items [0]).DropDownItems.Add ("removeme"));
												//((ContextMenuStrip)((ToolStripMenuItem)items[0]).DropDown).Items.Add (but);
												//		but.ToolTipText = AddIn.CalledFrom.ToolTip;
												//	AdvancedStrip.Items.Add (but);

													//((ContextMenuStrip)((ToolStripMenuItem)items[0]).Tag).Items.Add (but);
//												if (AddIn.CalledFrom.Image != null)
//												{
//													but.Image = AddIn.CalledFrom.Image;
//												}
//												AddIn.Hookups.Add (but);
												}
												else
												{
													NewMessage.Show (Loc.Instance.GetString ("A dropdown contextmenustrip needs to be defined for parent menu item"));
												}

											}
										}
										}// searching thru main mnenu
									} else {
										// create us as a parent
							
										ToolStripMenuItem parent = new ToolStripMenuItem (AddIn.CalledFrom.MyMenuName);
//										parent.Click += HandleAddInClick;
//										parent.Tag= AddIn;
//										parent.ToolTipText = AddIn.CalledFrom.ToolTip;
//										if (AddIn.CalledFrom.Image != null)
//										{
//											parent.Image = AddIn.CalledFrom.Image;
//										}
										BuildAddinToolStrip (AddIn, parent);
										MainMenu.Items.Add (parent);

										//AddIn.Hookups.Add (parent);


									}
								}
							} // OnAMenu
						} // Is allowed to be loaded (i.e., set in Options)
						// We call this method. If this is a notetype we get registered, others ignore it

					}//Not Added Yet
				}
			

				// Remove installed plugins
				if (addIns.Count > 0) {
					// We exist but we are not on the AddMe list.
					// This means we need to be removed
					for (int i = AddInsLoaded.Count-1; i >= 0; i--) {
						MefAddIns.Extensibility.mef_IBase addin = AddInsLoaded [i];
						// look at each plug and decide if it should be removed (i.e., it is no longer in the list to add
						if (myList.Contains (addin.CalledFrom.GUID) == false) {
							// we are NOT in the list but we exist in the active list
							// this means we must be destroyed!!
							NewMessage.Show ("Destroy : " + addin.CalledFrom.GUID);

							foreach (IDisposable connection in addin.Hookups) {
								connection.Dispose ();
							}
							if (addin.DeregisterType() == true)
							{
								NewMessage.Show (Loc.Instance.GetString("Because a NoteType was removed, we must shut down now, because any Layout open will not be able to be edited until this NoteType is added again."));
								Application.Exit ();
							}
						

							AddInsLoaded.Remove (addin);
						}
					}
				} // count > 0
			}
		}


		protected virtual object GetInformationForAddInBeforeRun(int getinfo)
		{
			return null;
		}
		

		protected virtual void HandleRespondInformationFromAddin (object infoFromAddIn, int typeOfInformationSentBack)
		{
		}
	

		/// <summary>
		/// Handles the add in click. (When the menu option that was added for the AddIn is clicked by usr)
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleAddInClick (object sender, EventArgs e)
		{
			// the tag contains the original addin
			if ((sender as ToolStripItem).Tag != null) {

				MefAddIns.Extensibility.mef_IBase thisAddIn = ((MefAddIns.Extensibility.mef_IBase)(sender as ToolStripItem).Tag);
				object NeededInfo = GetInformationForAddInBeforeRun(thisAddIn.TypeOfInformationNeeded);

				// if we have NO type info needed, we proceed. Otherwise, we must supply that info too
				if ( (thisAddIn.TypeOfInformationNeeded == 0) || (thisAddIn.TypeOfInformationNeeded > 0 && NeededInfo != null))
				{
				thisAddIn.SetBeforeRespondInformation(NeededInfo);
				thisAddIn.path_filelocation = path;
				thisAddIn.RespondToCallToAction();
					// this routine sends the needed info to the callback which was setup at Initialization 
					// we don't call this externally, it is called internally
					//thisAddIn.GetAfterRespondInformation();
				//object InfoFromAddIn = thisAddIn.GetAfterRespondInformation();
				//HandleRespondInformationFromAddin(InfoFromAddIn, thisAddIn.TypeOfInformationSentBack);

					if (true == thisAddIn.CalledFrom.QuickLinkShows)
					{
						if (thisAddIn.ActiveForm() == null) lg.Instance.Line ("MainFormBase->HandleAddInClick", ProblemType.WARNING, "can't, no form active!");

							else
						{
							//if (thisAddIn.QuickLinkMenuItem is ToolStripButton)
							{
								ToolStripButton link = new ToolStripButton ();
								link.Name = "quicklink";
								link.Tag =thisAddIn;
								link.Text = thisAddIn.CalledFrom.MyMenuName;
								link.BackColor = Color.Yellow;
								link.Click += HandleQuickLinkClick;
								//thisAddIn.QuickLinkMenuItem = link;
								thisAddIn.Hookups.Add (link);



							//FooterStatus.Items.Add ((ToolStripButton)thisAddIn.QuickLinkMenuItem);
								FooterStatus.Items.Add (link);
							}
						}

					}
				}
				else
				{
					lg.Instance.Line("MainFormBase->HandleAddInClick", ProblemType.WARNING, String.Format ("Addin {0} did not get called because TypeOfInformation or NeededInfo unavailable.", thisAddIn.Name));
				}


				// just a quick test to see if databse string made it in
				//NewMessage.Show (((MefAddIns.Extensibility.mef_IBase)(sender as ToolStripItem).Tag).Storage.ToString());
			}
		}

		void HandleQuickLinkClick (object sender, EventArgs e)
		{
			if ((sender as ToolStripButton).Tag is MefAddIns.Extensibility.mef_IBase) {
				(((sender as ToolStripButton).Tag as MefAddIns.Extensibility.mef_IBase).ActiveForm() as Form).BringToFront();
			}
		}

		/// <summary>
		/// Handles the options button click.
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
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
			if (CoreUtilities.FileUtils.CheckForFileError () == true) {
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
		public void Screens_AcrossTwo (bool MaxHeight)
		{
			System.Windows.Forms.Screen[] currentScreen = System.Windows.Forms.Screen.AllScreens;
			this.WindowState = FormWindowState.Normal;
			int nTotalWith = 0;
			int nSmallestHeight = 100000;
			int BiggestHeight = 0;
			foreach (Screen screen in currentScreen) {
				nTotalWith = nTotalWith + screen.WorkingArea.Width;
				if (screen.WorkingArea.Height > BiggestHeight) {
					BiggestHeight = screen.WorkingArea.Height;
				}
				if (screen.WorkingArea.Height < nSmallestHeight) {
					nSmallestHeight = screen.WorkingArea.Height;
				}
			}
			this.Top = currentScreen [0].WorkingArea.Top;
			this.Left = currentScreen[0].WorkingArea.Left;
			this.Width = nTotalWith;
			this.Height = nSmallestHeight;
			if (true == MaxHeight) {
				this.Height = BiggestHeight;
			}

			// adjust height to /smallest screen
		}
		/// <summary>
		/// Flips just across one screen
		/// </summary>
		public void Screens_JustOne()
		{
			System.Windows.Forms.Screen[] currentScreen = System.Windows.Forms.Screen.AllScreens;
			this.Top = currentScreen[0].WorkingArea.Top;
			this.Left = currentScreen[0].WorkingArea.Left;
			this.Width = currentScreen[0].WorkingArea.Width;
			this.Height = currentScreen[0].WorkingArea.Height;
			this.WindowState = FormWindowState.Maximized;
			// adjust height to /smallest screen
		}
		protected virtual void BuildListOfNoteTextAction ()
		{

		}
	}
}

