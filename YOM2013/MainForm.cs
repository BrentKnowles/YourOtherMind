using System;
using System.Windows.Forms;
using CoreUtilities;
using Layout;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using AppLimit.NetSparkle;
using System.Drawing;
using System.IO;
using System.Collections;
using System.ComponentModel.Composition.Hosting;
	using System.ComponentModel.Composition;
using appframe;
using LayoutPanels;

namespace YOM2013
{
	public class MainForm : appframe.MainFormBase 
	{
		#region gui
		ToolStripMenuItem Windows;
		ContextMenuStrip TextEditContextStrip;

		#endregion
		#region Layouts

		
		//Layout.LayoutPanel CurrentLayout;
	
		NoteDataXML_SystemOnly MDIHOST=null;
		List<LayoutsInMemory> LayoutsOpen;
		
		LayoutPanel CurrentLayout {
			get { return (LayoutPanel)LayoutDetails.Instance.CurrentLayout;}
		}

		#endregion
		#region variables
		private Sparkle _sparkle; 
		private Options Settings ;
	
		#endregion
		#region delegates

		#endregion
		/// <summary>
		/// A series of true/false and similiar settings that should be double checked before deployments as some of them
		/// are for debugging
		/// 
		/// </summary>
		private void Switches()
		{

		}
	
		private void UpdateTitle(string newTitle)
		{
			this.Text = newTitle;
		}

		private void SetupMessageBox ()
		{

				NewMessage.SetupBoxFirstTime (null,MainFormIcon /*paths.DataPath + "\\Exe\\Current\\Icon1.ico"*/,
			                             ImageLayout.Stretch,
			                             Color.Green, // transprency key  
			                             new Font ("Georgia", 12), new Font ("Times", 10),
			                             Color.OrangeRed,  //button face color
			                             Color.Black, // caption color
			                             Color.Black, // message color
			                             Color.White,  // back color for form
			                             Color.OrangeRed, // back color for caption (turns it into a proper captino heading)
			                             Color.Transparent); // back color for message);
			

		}
		protected string BuildBatchFileName ()
		{
			return  System.IO.Path.Combine (System.IO.Path.GetTempPath (), Guid.NewGuid().ToString () + ".bat");
		}
		/// <summary>
		/// Builds the list of note text action.
		/// 
		/// Called from MainForm constructor.
		/// 
		/// Will hold the default batch file one AND any addin generated actions
		/// </summary>
		/// <returns>
		/// The list of note text action.
		/// </returns>
		protected override void BuildListOfNoteTextAction ()
		{

			base.BuildListOfNoteTextAction();
		

			NoteTextActions.Add (new NoteTextAction(RunAsBatchFile, BuildBatchFileName, Loc.Instance.GetString("Batch"), Loc.Instance.GetString ("Runs the text on this note as a batch file.")));


		}
		/// <summary>
		/// called from SaveTextLineToFile
		/// </summary>
		/// <param name="NoteToOpen">if present and we encounter [[title]] we replace [[title]] with NoteToOpen   ///  </param>
		/// <param name="sText"></param>
		void SaveTextLineByLine (StreamWriter writer, string[] linesOfText, string empty)
		{
			foreach (string s in linesOfText)
			{
				writer.WriteLine(s);
			}
		}

		/// <summary>
		/// Goes through rich edit line by line saving to a plain text file
		/// 
		/// December 2009
		///  Here we need to do a redesign.
		/// 
		/// If we encounter [[index]] on the first line we know that we have an index page. So instead we need to do the following:
		/// 
		/// Each line is either a NOTE NAME to add to the text file (which will be parsed line by line and converted to plain text)
		/// 
		/// OR
		/// 
		/// It returns a list of names that are then parsed line by line as above, in the order of the list
		///   
		/// An example index would be
		/// [[index]]
		/// _Header [[words]]
		/// [[Group,Storyboard,Chapter*,words]         !- This returns an array of note names that match the criteria (i.e., Chapter 01, Chapter 02)
		/// _Footer
		/// 
		/// 
		/// 
		/// * Note: Choose not to let the groups handl
		/// </summary>
		/// <param name="sFile"></param>
		public void SaveTextLineToFile(string[] LinesOfText,  string sFilepath)
		{
				string sWordInformation = "";
				int TotalWords = 0;
				
				
				
				
				try
				{
					StreamWriter writer = new StreamWriter(sFilepath);
					if (LinesOfText[0].ToLower() == "[[index]]")
					{
						// we are actually an index note
						// which will instead list a bunch of other pages to use
						// we now iterate through LinesOfText[1] to end and parse those instead
						for (int i = 1; i < LinesOfText.Length; i++)
						{
							string sLine = LinesOfText[i];
							bool bGetWords = false;
							ArrayList ListOfParsePages = new ArrayList();
							
						//TODO hook up to Custom Scripting Language system
							if (sLine.IndexOf("[[words]]") > -1)
							{
								
								// if we have the words keyword we know we want to display some word info at the end
								sLine = sLine.Replace("[[words]]", "").Trim();
								
								bGetWords = true;
							}
						//TODO hook up to Custom Scripting Language system
							if (sLine.IndexOf("[[Group") > -1)
							{
								//TODO Hook up again laterListOfParsePages = GetListOfPages(sLine, ref bGetWords);
								
								// we have a group
								
							}
							else
							{
								ListOfParsePages.Add(sLine);
							}
							
							// Now we go through the pages and write them into the text file
							// feb 19 2010 - added because chapter notes were not coming out in alphaetical
							ListOfParsePages.Sort();
							
							foreach (string notetoopen in ListOfParsePages)
							{
//								DrawingTest.NotePanel panel = ((mdi)_CORE_GetActiveChild()).page_Visual.GetPanelByName(notetoopen);
//								
//								
//							//TODO hook up to Custom Scripting Language system and make more efficient
//								if (panel != null)
//								{
//									RichTextBox tempBox = new RichTextBox();
//									tempBox.Rtf = panel.appearance.Text;
//								SaveTextLineByLine(writer, tempBox.Lines, notetoopen);
//									
//									if (true == bGetWords)
//									{
//										
//										int Words = RichTextBoxLinks.RichTextBoxEx.WordCount(tempBox.Text);
//										TotalWords = TotalWords + Words;
//										
//										
//										
//										
//										sWordInformation = sWordInformation + String.Format("{0}: {1}\n", notetoopen, Words.ToString());
//									}
//									
//									tempBox.Dispose();
								//}
							} //open each note list
							
							//                            panel.Dispose(); Don't think I can do this becauseit would dlette hte note, benig an ojbect
							ListOfParsePages = null;
							
						}
					}
					else
					{
						
					SaveTextLineByLine(writer, LinesOfText, "");
					}
					
					writer.Close();
					writer.Dispose();
					if (sWordInformation != "")
					{
						string sResult = Loc.Instance.GetStringFmt("Total Words:{0}\n{1}", TotalWords.ToString(), sWordInformation);
						Clipboard.SetText(sResult);
						NewMessage.Show(Loc.Instance.GetString ("Your Text Has Been Sent! Press Ctrl + V to paste word count information into current note."));
						//NewMessage.Show(sResult);
					}
				}
				catch (Exception)
				{
					NewMessage.Show(Loc.Instance.GetStringFmt("Unable to write to {0} please shut down and try again", sFilepath));
				}
				
				LinesOfText = null;

				
		}
			

		// hook
		private void RunAsBatchFile (object FileToProcess)
		{
			// save as a TMP text file


			// run as a batch file

		
			General.OpenDocument(FileToProcess.ToString (), "");
		}

		/// <summary>
		/// Builds the menu for note text actions.
		/// </summary>
		/// <param name='noteTextActions'>
		/// Note text actions.
		/// </param>
		void BuildMenuForNoteTextActions (List<NoteTextAction> noteTextActions)
		{   



			ToolStripItem[] foldersFound = TextEditContextStrip.Items.Find ("actionfolder", true);
			ToolStripMenuItem folder = null;
			if (foldersFound != null && foldersFound.Length > 0) {
				folder = (ToolStripMenuItem)foldersFound [0];
				// remove previous contextmenu strip
				folder.DropDown = null;
			
			}
			if (folder == null) {
				folder = new ToolStripMenuItem ();
				folder.Text = Loc.Instance.GetString ("Actions");
				folder.Name = "actionfolder";
				TextEditContextStrip.Items.Add (folder);
			}
			ContextMenuStrip actionStrip = new ContextMenuStrip ();

			foreach (NoteTextAction action in noteTextActions) {
				ToolStripButton button = new ToolStripButton ();
				button.Text = action.GetMenuTitle ();
				button.ToolTipText = action.GetMenuTooltip ();
				button.Tag = action;
				button.Click += HandleNoteTextActionClick;
				actionStrip.Items.Add (button);
			}
			// allow full width for last item dynamically added
			actionStrip.Items.Remove (actionStrip.Items.Add ("hack"));
			folder.DropDown = actionStrip;

		}
		/// <summary>
		/// Handles the note text action click. (i.e., run as batch file)
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleNoteTextActionClick (object sender, EventArgs e)
		{
			if ((sender as ToolStripButton).Tag != null && ((sender as ToolStripButton).Tag is NoteTextAction)) {
				if (LayoutDetails.Instance.CurrentLayout.CurrentTextNote != null) {
					string[] lines = LayoutDetails.Instance.CurrentLayout.CurrentTextNote.Lines ();
					if (lines.Length > 0) {
						string FileName = ((NoteTextAction)(sender as ToolStripButton).Tag).BuildTheFileName();
						SaveTextLineToFile (lines, FileName);

						((NoteTextAction)(sender as ToolStripButton).Tag).RunAction (FileName);
					} 
				}
				else {
					NewMessage.Show (Loc.Instance.GetString ("Please select a text note."));
				}
			}
		}

/// <summary>
/// Initializes a new instance of the <see cref="YOM2013.MainForm"/> class.
/// </summary>
/// <param name='_path'>
/// _path.
		/// This is set in Main.cs (either debug directory or directory where install sets)
/// </param>
/// <param name='ForceShutDownMethod'>
/// Force shut down method.
/// </param>
/// <param name='storage'>
/// Storage.
/// </param>
		public MainForm (string _path, Action<bool>ForceShutDownMethod, string storage, Icon mainIcon) : base (_path,ForceShutDownMethod,storage, mainIcon)
		{



			

			this.Load += HandleFormLoad;

		

			SetupMessageBox ();
			LayoutsOpen = new List<LayoutsInMemory> ();

			Switches ();

			lg.Instance.Loudness = Loud.CTRIVIAL;
			//lg.Instance.Loudness = Loud.DOFF;
			LayoutDetails.Instance.LoadLayoutRef = LoadLayout;


			try {
				Loc.Instance.ChangeLocale ("en-US");
				lg.Instance.Line ("MainForm", ProblemType.MESSAGE, String.Format ("Current culture {0}", System.Threading.Thread.CurrentThread.CurrentUICulture));
				lg.Instance.Line ("MainForm", ProblemType.MESSAGE,Loc.Instance.Cat.GetString ("Loading"));
			
			} catch (Exception ex) {
				Console.WriteLine (ex.ToString ());
			}

			Console.WriteLine (LayoutDetails.Instance.Path);



			this.FormClosed += HandleFormClosed;
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

			//GetSystemPanel ();

			if (MasterOfLayouts.ExistsByGUID ("example") == false) {
				DefaultLayouts.CreateExampleLayout (this,TextEditContextStrip);
			}

			//SystemLayout.Visible = false;
			Layout.LayoutPanel SystemLayout;
			if (MasterOfLayouts.ExistsByGUID ("system") == false || MasterOfLayouts.ExistsByGUID("tables") == false) {
				NewMessage.Show ("recreating system note");
				DefaultLayouts.CreateASystemLayout (TextEditContextStrip);//did not work.SaveLayout();

//				LayoutDetails.Instance.SystemLayout = SystemLayout;
//				SystemLayout.SaveLayout();
//				SystemLayout.Dock = System.Windows.Forms.DockStyle.Fill;
//				SystemLayout.BringToFront ();
			} else {

				// we always do a load

			}

			// Can't do this because the notes are all part of an interior Layout
//			SplitContainer container = new SplitContainer();
//			this.Controls.Add (container);
//			container.Dock = DockStyle.Fill;

			SystemLayout = new Layout.LayoutPanel (CoreUtilities.Constants.BLANK, true);
			//container.Panel1.Controls.Add (SystemLayout);
			SystemLayout.Parent = this;
			SystemLayout.Visible = true;
			SystemLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			SystemLayout.BringToFront ();
			SystemLayout.LoadLayout ("system", false, TextEditContextStrip);
			LayoutDetails.Instance.SystemLayout = SystemLayout;


			// now load the table layout which is a subnote of System (For FASTER lookups)
			// we REBUILD this if necessary, above
			LayoutDetails.Instance.TableLayout=new Layout.LayoutPanel(Constants.BLANK, false);
			LayoutDetails.Instance.TableLayout.LoadLayout ("tables", false, TextEditContextStrip);

			//LoadLayout ( "");




			ToolStripMenuItem file = this.GetFileMenu ();
			if (file == null) {
				throw new Exception ("File menu was not defined in base class");
			} else {


				ToolStripMenuItem New = new ToolStripMenuItem (Loc.Instance.Cat.GetString ("New Layout"));
				file.DropDownItems.Add (New);
				//	((ToolStripMenuItem)MainMenu.Items [0]).DropDownItems.Add (Save);
				New.Click += HandleNewClick;

				ToolStripMenuItem Test = new ToolStripMenuItem (Loc.Instance.Cat.GetString ("TEST"));
				file.DropDownItems.Add (Test);
				//	((ToolStripMenuItem)MainMenu.Items [0]).DropDownItems.Add (Save);
				Test.Click += HandleTestClick;

				ToolStripMenuItem Save = new ToolStripMenuItem (Loc.Instance.Cat.GetString ("Save"));
				file.DropDownItems.Add (Save);
				//	((ToolStripMenuItem)MainMenu.Items [0]).DropDownItems.Add (Save);
				Save.Click += HandleSaveClick;

				ToolStripMenuItem Backup = new ToolStripMenuItem (Loc.Instance.Cat.GetString ("Backup"));
				Backup.Click += HandleBackupClick;
				file.DropDownItems.Add (Backup);
			}

			ToolStripSeparator sep = new ToolStripSeparator ();
			GetToolMenu ().DropDownItems.Add (sep);

			ToolStripMenuItem Monitor_AllScreens = new ToolStripMenuItem (Loc.Instance.GetString ("Extend View"));
			GetToolMenu ().DropDownItems.Add (Monitor_AllScreens);
			//	((ToolStripMenuItem)MainMenu.Items [0]).DropDownItems.Add (Save);
			Monitor_AllScreens.Click += HandleMonitorAllScreensClick;

			ToolStripMenuItem Monitor_OneScreen = new ToolStripMenuItem(Loc.Instance.GetString ("Single Screen"));
			GetToolMenu().DropDownItems.Add (Monitor_OneScreen);
			Monitor_OneScreen.Click += HandleMonitorOneScreenClick;



			Windows = new ToolStripMenuItem (Loc.Instance.GetString ("Windows"));
			Windows.DropDownOpening += HandleWindowsMenuDropDownOpening;





			// Adding in right order to Main Menu

			MainMenu.Items.Add (Windows);


			ToolStripButton import = new ToolStripButton ("Import");
			import.Click += HandleImportOldFilesClick;
			MainMenu.Items.Add (import);
			// DELEGATES
			LayoutDetails.Instance.UpdateTitle = UpdateTitle;


			//KEY HANDLERS

			this.KeyPreview = true;
			this.KeyDown += HandleMainFormKeyDown;

			// Add option panels to options menu
			Settings = new Options(LayoutDetails.Instance.YOM_DATABASE);
			Options_InterfaceElements SettingsInterfaceOptions = new Options_InterfaceElements(LayoutDetails.Instance.YOM_DATABASE);
	
		    optionPanels.Add(Settings);
			optionPanels.Add (SettingsInterfaceOptions);



		


			//Check for updates
			string sparkupdatefile = "http://www.yourothermind.com/yomcast.xml";//update.applimit.com/netsparkle/versioninfo.xml
			try {
				_sparkle = new Sparkle (sparkupdatefile); 
				//_sparkle.ShowDiagnosticWindow = true;
				_sparkle.StartLoop (true); 


				lg.Instance.Line("MainForm.MainForm", ProblemType.MESSAGE, "Loading appcast: " + sparkupdatefile);
			} catch (Exception ex) {
				lg.Instance.Line("MainForm.MainForm", ProblemType.ERROR, "Sparkle was unable to find the update file " + ex.ToString());
			}



		}

		/// <summary>
		/// Builds the text editor context menu strip.
		/// 
		/// This will set up the context menu strip that the text boxes will later acquire
		/// </summary>
		public override void BuildContextMenuStrips ()
		{
			TextEditContextStrip = new System.Windows.Forms.ContextMenuStrip();
			TextEditContextStrip.Name = "TextEditContextStrip";

			ContextMenus.Add (TextEditContextStrip);


			TextEditContextStrip.Opening+= HandleTextEditOpening;
		}

		void HandleTextEditOpening (object sender, System.ComponentModel.CancelEventArgs e)
		{

			BuildMenuForNoteTextActions(NoteTextActions);
		}

		/// <summary>
		/// Handles the form load.
		/// Does a save when it is first loaded, in case we REBUILT data (fresh install)
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleFormLoad (object sender, EventArgs e)
		{
			this.Save ();
		}

		void HandleMonitorOneScreenClick (object sender, EventArgs e)
		{
			Screens_JustOne();
		}

		void HandleImportOldFilesClick (object sender, EventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog ();
			if (open.ShowDialog () == DialogResult.OK) {
				TestAndSaveIfNecessary ();
				string guid = System.Guid.NewGuid().ToString();
				//CurrentLayout = new LayoutPanel(Constants.BLANK);
				
				LayoutPanel newLayout = CreateLayoutContainer(guid);
				newLayout.NewLayoutFromOldFile (guid, open.FileName);
				LayoutDetails.Instance.CurrentLayout = newLayout;
			}
		}

		void HandleWindowsMenuDropDownOpening (object sender, EventArgs e)
		{
			RefreshWindowsMenu();
		}

		void HandleMonitorAllScreensClick (object sender, EventArgs e)
		{

			Screens_AcrossTwo(Settings.MultipleScreenHigh);
		}

		void HandleTestClick (object sender, EventArgs e)
		{
		
		}
		/// <summary>
		/// returns a valid record if ourGUID is found to be in memory
		/// </summary>
		/// <param name='ourGuid'>
		/// Our GUID.
		/// </param>
		LayoutsInMemory LayoutPresent (string ourGuid)
		{
			LayoutsInMemory existing = LayoutsOpen.Find (LayoutsInMemory => LayoutsInMemory.GUID == ourGuid);
			return existing;
		}

		/// <summary>
		/// Toggles the current note maximized. (responds to the F6 key (default) raised in main form
		/// </summary>
		void ToggleCurrentNoteMaximized ()
		{
			if (CurrentLayout != null) {
				string ourGUID = CurrentLayout.GUID;

				// find the Layout with this GUID, so we can reference the maximized state
				lg.Instance.Line("TogguleCurrentNoteMaximized", ProblemType.TEMPORARY, String.Format ("Searching for GUID:<{0} >", ourGUID));
				LayoutsInMemory existing = LayoutPresent(ourGUID);

				if (null != existing) {
					Console.WriteLine ("ourGUID = {0} // FoundGUID = {1}", ourGUID, existing.GUID);
					// We assume we are operating only on the CURRENT Layout
					existing.Maximized = !existing.Maximized;
					existing.Container.Maximize(existing.Maximized);
					//Maximize (existing.Maximized);
				} else {
					lg.Instance.Line ("MainForm.ToggleCurrentNoteMaximized", ProblemType.WARNING, " never found the note which is odd " + ourGUID);
				}
			}
			/*
			if (CurrentLayout.Parent.Dock != DockStyle.Fill) {
				lastDockStyle = CurrentLayout.Parent.Dock;
				CurrentLayout.Parent.Dock = DockStyle.Fill;
				CurrentLayout.Parent.BringToFront ();
			}
			else {
				CurrentLayout.Parent.Dock = lastDockStyle;
			}*/
		}
		void HandleMainFormKeyDown (object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F6) {

				ToggleCurrentNoteMaximized ();
				//wont operate on LAYOUT FROMn ListOfOpenLayouts
				//CurrentLayout.ToggleCurrentNoteMaximized();


			}
		}


		/// <summary>
		/// Creates the layout container.
		/// The GUID is required to associate the recordkeeping with the actual objects
		/// Called via Load and New
		/// </summary>
		LayoutPanel CreateLayoutContainer(string guid)
		{
			MDIHOST = LayoutDetails.Instance.SystemLayout.GetSystemPanel ();
			if (MDIHOST == null) {
				NewMessage.Show ("no system panel was found on system layout");
				Application.Exit ();
			}
			
			
			if (MDIHOST == null) {
				throw new Exception("Major problem. No MDIHOST set.");
			}
			if (CurrentLayout != null) {
				TestAndSaveIfNecessary ();
				//	CurrentLayout.Dispose();
			}
			
			//Maximize = MDIHOST.Maximize;
			
			LayoutPanel newLayout =  new Layout.LayoutPanel (CoreUtilities.Constants.BLANK, false);
			newLayout.BorderStyle = BorderStyle.Fixed3D;
			newLayout.Parent = MDIHOST.ParentNotePanel;
			newLayout.Visible = true;
			newLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			// this is necessary else we lose one of the toolbars
			newLayout.BringToFront ();
			
			
			LayoutsInMemory newLayoutStruct = new LayoutsInMemory();
			newLayoutStruct.Maximized = false;
			newLayoutStruct.LayoutPanel = newLayout;
			newLayoutStruct.Container = MDIHOST;
			newLayoutStruct.GUID = guid;


			MDIHOST.AlertWhenClosed =AlertWhenClosed;

			LayoutsOpen.Add (newLayoutStruct);
			
			LayoutDetails.Instance.SystemLayout.SetSaveRequired(false);
			return newLayout;
		}
		/// <summary>
		/// Alerts the when closed.
		/// 
		/// Called from NoteDataXML_System, when closed by the user.
		/// 
		/// Intended to remove them from the list of open windows.
		/// </summary>
		/// <param name='panel'>
		/// Panel.
		/// </param>
		void AlertWhenClosed (LayoutPanelBase panel)
		{
			LayoutsInMemory existing = LayoutPresent (panel.GUID);

			if (null != existing) {
				LayoutsOpen.Remove (existing);
				// Dec 29 2012 - adding this 
				// Pick one of the other layouts to become current
				if (LayoutsOpen.Count>0)
				{
//					LayoutDetails.Instance.CurrentLayout=LayoutsOpen[0].LayoutPanel;
					GotoExistingLayout( LayoutsOpen[0].Container,LayoutsOpen[0].LayoutPanel);

				}
				else
				LayoutDetails.Instance.CurrentLayout = null;
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

			// if Layout NOT open already THEN open it
			LayoutsInMemory existing = LayoutPresent  (guidtoload);
			if (existing == null) {
				LayoutPanel newLayout = CreateLayoutContainer (guidtoload);
				newLayout.LoadLayout (guidtoload, false, TextEditContextStrip);
				LayoutDetails.Instance.CurrentLayout = newLayout;
			} else {

				GotoExistingLayout(existing.Container, existing.LayoutPanel);
			}



			// otherwise just 'go to it'
			//

			//CurrentLayout = new Layout.LayoutPanel (CoreUtilities.Constants.BLANK);
			//CurrentLayout.Name;
			/*CurrentLayout.BorderStyle = BorderStyle.Fixed3D;
			CurrentLayout.Parent = MDIHOST.Parent;
			CurrentLayout.Visible = true;
			CurrentLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			CurrentLayout.BringToFront ();
			CurrentLayout.LoadLayout(guidtoload);
			*/
			//RebuildWindowMenu(); NOPE: Call this when the window is opened

		}

		void RefreshWindowsMenu ()
		{
			Windows.DropDownItems.Clear ();
			foreach (LayoutsInMemory layoutstruct in LayoutsOpen) {
				ToolStripButton button = new ToolStripButton(layoutstruct.LayoutPanel.Caption);

				button.DisplayStyle = ToolStripItemDisplayStyle.Text;
				button.Tag = layoutstruct;
				button.Click+= HandleRefreshWindowsMenuClick;
				Windows.DropDownItems.Add(button);
			}
			/*MainMenu.MaximumSize = new System.Drawing.Size(0,0);
			MainMenu.Width = 200;
			MainMenu.AutoSize = true;
			Windows.Width = 200;
			Windows.AutoSize = true;
*/
			//HACK: If you do not do this then text is truncated on the buttons! http://stackoverflow.com/questions/1550077/toolstripbutton-text-gets-cut-off-in-contextmenustrip
			Windows.DropDownItems.Remove (Windows.DropDownItems.Add ("empty"));

		}

		/// <summary>
		/// Handles the refresh windows menu click.
		/// Opens the specified window
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleRefreshWindowsMenuClick (object sender, EventArgs e)
		{
			GotoExistingLayout( ((LayoutsInMemory)(sender as ToolStripButton).Tag).Container,((LayoutsInMemory)(sender as ToolStripButton).Tag).LayoutPanel);
		}

		void GotoExistingLayout (NoteDataXML_SystemOnly panel, LayoutPanel layoutPanel)
		{

			panel.ParentNotePanel.BringToFront();
			panel.ParentNotePanel.Focus();
			LayoutDetails.Instance.CurrentLayout = layoutPanel;
			panel.Flash();
		}


		void HandleNewClick (object sender, EventArgs e)
		{

			TestAndSaveIfNecessary ();
			string guid = System.Guid.NewGuid().ToString();
			//CurrentLayout = new LayoutPanel(Constants.BLANK);

			LayoutPanel newLayout = CreateLayoutContainer(guid);
			newLayout.NewLayout (guid, true, TextEditContextStrip);
			LayoutDetails.Instance.CurrentLayout = newLayout;
		}

		void HandleBackupClick (object sender, EventArgs e)
		{
			MasterOfLayouts master = new MasterOfLayouts();
			Console.WriteLine (master.Backup());
			master.Dispose();
		}

		void Save ()
		{

			LayoutDetails.Instance.SystemLayout.SaveLayout ();
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
					NewMessage.Show ("Should have saved says Layout=" + CurrentLayout.Caption);
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
		protected override object GetInformationForAddInBeforeRun (int getinfo)
		{
			string returnvalue = Constants.BLANK;
			switch (getinfo) {
			case (int)GetInformationADDINS.GET_SELECTED_TEXT:
				if (LayoutDetails.Instance.CurrentLayout.CurrentTextNote != null && LayoutDetails.Instance.CurrentLayout.CurrentTextNote.SelectedText != Constants.BLANK)
				{
					returnvalue = LayoutDetails.Instance.CurrentLayout.CurrentTextNote.SelectedText;
				}
				else
				{
					NewMessage.Show (Loc.Instance.GetString("Please select text inside of a note before activating this feature."));
					returnvalue = null;
				}
				break;
			}
			return returnvalue;
		}
		protected override void HandleRespondInformationFromAddin (object infoFromAddIn, int typeOfInformationSentBack)
		{
			switch (typeOfInformationSentBack) {
			case (int)SetInformationADDINS.REPLACE_SELECTED_TEXT:
				if (LayoutDetails.Instance.CurrentLayout.CurrentTextNote.SelectionLength > 0)
				{
					bool hasspace=false;
					string newword =  infoFromAddIn.ToString ();
					if (LayoutDetails.Instance.CurrentLayout.CurrentTextNote.SelectedText[LayoutDetails.Instance.CurrentLayout.CurrentTextNote.SelectedText.Length-1] == ' ')
					{
						hasspace = true;
						newword = newword + ' ';

					}
					LayoutDetails.Instance.CurrentLayout.CurrentTextNote.SelectedText = newword;

				}
				break;
			}
		}
	}
}



