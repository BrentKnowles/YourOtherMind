using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Collections;
using LayoutPanels;
using CoreUtilities;
using CoreUtilities.Links;
/*This is Just an Experiment.
 * 
 * Trying to see if I can organize the notes in a better way
 * 
 * Goals
 * 1. Easy, logical maintenance
 * 2. Able to store data elsewhere
 * 3. Faster loading of data
 * 4. Logical access to data both from the LayoutPanel itself and 
 * 5. from external places requesting data (the random table system)
 */
namespace Layout
{
	public class LayoutPanel : LayoutPanelBase
	{

		#region TEMPVARIABLES

	
		HeaderBar header = null;
		#endregion

		protected LayoutInterface Notes = null;
		#region gui
		private Panel noteCanvas;
		override public Panel NoteCanvas {
			get { return noteCanvas;}
			set { noteCanvas = value;}
		}
	

		ToolStrip tabsBar = null;


		#endregion


	
	protected override void Dispose (bool disposing)
		{
			//Notes=null;
			//this will cause TestMovingNotes to fail because a panel is removed? 
			//lg.Instance.Line("LayoutPanel->Dispose", ProblemType.MESSAGE, "Called from " + new System.Diagnostics.StackFrame(1).GetMethod().Module.Name);
			if (Notes != null) {
				Notes.Dispose (); 
			}
			base.Dispose (disposing);
		

		}

		public override string Caption {
			get { return Notes.Name;}
		}
		private bool issystem = false;
		/// <summary>
		/// Gets or sets a value indicating whether this instance is system.
		/// 
		/// If true this is the SYstem layout, which has special behaviors
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is system; otherwise, <c>false</c>.
		/// </value>
		override public bool GetIsSystemLayout { 
			get { return issystem;}
			set {
				issystem = value;
			}
		}
		NoteDataXML_RichText currentTextNote=null;
		/// <summary>
		/// A reference to the active note
		/// </summary>
		/// <value>
		/// The current note.
		/// </value>
		public override NoteDataXML_RichText CurrentTextNote {
			get {
				return currentTextNote;
			}
			set {
				//currentTextNote = value;
				if (value != null)
				{
				// remove changed handler from previous textnote
				if (currentTextNote != null)
				{
						currentTextNote.GetRichTextBox().TextChanged-= HandleCurrentNoteTextChanged;
				}
					SetCurrentTextNote(value);
				// add changed handler to textnote (so the FindBar can know if a Search has been interrupted and respond properly)
				currentTextNote.GetRichTextBox().TextChanged+= HandleCurrentNoteTextChanged;

				
					FindBarStatusStrip tmpFindBar = GetFindbar();
					if (tmpFindBar != null)
						GetFindbar().SetCurrentNoteText(value.Caption);
				}
				
			}
		}

		void HandleCurrentNoteTextChanged (object sender, EventArgs e)
		{
			if (FindBar != null) {
				FindBar.UpdateSearchAfterEditingInterruption ();
			}
		}
		
		public override void SetCurrentTextNote (NoteDataXML_RichText note)
		{
			currentTextNote = note;
			if (SetParentLayoutCurrentNote != null)
			{
				SetParentLayoutCurrentNote(note);
			}
		}


		/// <summary>
		/// Builds the format toolbar.
		/// </summary>
		private void BuildFormatToolbar ()
		{
			ToolStrip formatBar = new ToolStrip();
			formatBar.Parent = this;
			formatBar.Dock = DockStyle.Top;

			ToolStripButton bold = new ToolStripButton();
			bold.Text = Loc.Instance.GetString ("BOLD");
			bold.Click+= HandleBoldClick;
			formatBar.Items.Add (bold);


			ToolStripButton strike = new ToolStripButton();
			strike.Text = Loc.Instance.GetString ("STRIKE");
			strike.Click+= HandleStrikeClick;;
			formatBar.Items.Add (strike);

			ToolStripButton zoom = new ToolStripButton();
			zoom.Text = Loc.Instance.GetString ("ZOOM");
			zoom.Click+= (object sender, EventArgs e) => DoFormatOnText(NoteDataXML_RichText.FormatText.ZOOM);
			formatBar.Items.Add (zoom);


			ToolStripButton line = new ToolStripButton();
			line.Text = Loc.Instance.GetString ("LINE");
			line.Click+= (object sender, EventArgs e) => DoFormatOnText(NoteDataXML_RichText.FormatText.LINE);
			formatBar.Items.Add (line);

			ToolStripSplitButton Bullets = new ToolStripSplitButton();
			Bullets.Text = Loc.Instance.GetString ("BULLETS");

			ToolStripButton BulletNormal = new ToolStripButton();
			BulletNormal.Click+= (object sender, EventArgs e) => DoFormatOnText(NoteDataXML_RichText.FormatText.BULLET);
			BulletNormal.Text = "bullet";

			Bullets.DropDownItems.Add (BulletNormal);


			ToolStripButton BulletNumber = new ToolStripButton();
			BulletNumber.Text = "number";
			BulletNumber.Click+= (object sender, EventArgs e) => DoFormatOnText(NoteDataXML_RichText.FormatText.BULLETNUMBER);
			Bullets.DropDownItems.Add (BulletNumber);
		//	Bullets.Click+= (object sender, EventArgs e) => DoFormatOnText(NoteDataXML_RichText.FormatText.ZOOM);
			formatBar.Items.Add (Bullets);


			ToolStripButton DefaultText = new ToolStripButton();
			DefaultText.Text = "default";
			DefaultText.Click+= (object sender, EventArgs e) => DoFormatOnText(NoteDataXML_RichText.FormatText.DEFAULTFONT);

			//	Bullets.Click+= (object sender, EventArgs e) => DoFormatOnText(NoteDataXML_RichText.FormatText.ZOOM);
			formatBar.Items.Add (DefaultText);

		}

		void HandleStrikeClick (object sender, EventArgs e)
		{
			DoFormatOnText(NoteDataXML_RichText.FormatText.STRIKETHRU);
		}

		void HandleBoldClick (object sender, EventArgs e)
		{
			DoFormatOnText(NoteDataXML_RichText.FormatText.BOLD);
		}



		public void BuildTabsBar()
		{
			tabsBar = new ToolStrip();
			tabsBar.Parent = this;
			tabsBar.Dock = DockStyle.Top;

			// RefreshTabs(); don't need to call it until after load
		}

		public override  void DeleteNote(NoteDataInterface NoteToDelete)
		{
			Notes.RemoveNote (NoteToDelete);
		
		}
		/// <summary>
		/// Returns the system panel for a system page.
		/// This is only relevant for the guid=system page (the main interface)
		/// and is called from the mainform
		/// </summary>
		/// <value>
		/// The get system panel.
		/// </value>
		public override NoteDataXML_SystemOnly GetSystemPanel ()
		{
			// changing things so that we CREATE a new system panel as needed.
			return Notes.GetAvailableSystemNote(this);

			/*
			foreach (NoteDataInterface note in Notes.GetNotes ()) {
				if (note.GetType() == typeof(NoteDataXML_SystemOnly))
				{
					return (Control)note.Parent;
				}
			}
			return null;*/
		}


		public void BuildLayoutToolbar ()
		{
			ToolStrip bar = new ToolStrip ();
			bar.Parent = this;
			bar.Visible = true;
			bar.Dock = DockStyle.Top;


			
			
			
			ToolStripDropDownButton AddNote = new ToolStripDropDownButton(Loc.Instance.GetString ("Add Note"));
			AddNote.DropDownOpening += HandleAddNoteDropDownOpening;

			
			
			ToolStripDropDownButton RandomTables = new ToolStripDropDownButton(Loc.Instance.GetString("Random"));
			RandomTables.ToolTipText = Loc.Instance.GetString("Generate a random result from a table. The list of tables can be changed by modifying the RandomTables table on the System layout");
			RandomTables.DropDownOpening += HandleDropDownRandomTablesOpening;


			ContextMenuStrip tabContext = new System.Windows.Forms.ContextMenuStrip();
			
			
			
			
			ToolStripButton ShowTabs = new ToolStripButton(Loc.Instance.GetString("Show Tabs?"));
			ShowTabs.CheckOnClick = true;
			ShowTabs.Checked = Notes.ShowTabs;
			ShowTabs.CheckedChanged+= HandleCheckedChanged;
			
			ToolStripButton MaximizeTabs = new ToolStripButton(Loc.Instance.GetString ("Maximize Tabs"));
			MaximizeTabs.CheckOnClick = true;
			MaximizeTabs.Checked = Notes.MaximizeTabs;
			MaximizeTabs.CheckedChanged+= HandleMaximizedTabsCheckedChanged;
			
			tabContext.Items.Add (ShowTabs);
			tabContext.Items.Add (MaximizeTabs);

			ToolStripDropDownButton tabMenu = new ToolStripDropDownButton(Loc.Instance.GetString ("Tabs"));
			tabMenu.DropDown = tabContext;


			//tabMenu.DropDownItems.Add (ShowTabs);
			//tabMenu.DropDownItems.Add (MaximizeTabs);





			//ToolStripLabel CurrentNote
			bar.Items.Add (AddNote);
			bar.Items.Add (RandomTables);
			bar.Items.Add (tabMenu);

			// tempt
//			ToolStripButton test = new ToolStripButton("test");
//			test.Click+= (object sender, EventArgs e) => {NewMessage.Show ("Records in linktable = " + GetLinkTable().GetRecords().Length);};
//			bar.Items.Add (test);



			if (this.header != null)
			this.header.SendToBack();

		}
		void HandleCheckedChanged (object sender, EventArgs e)
		{
			Notes.ShowTabs = (sender as ToolStripButton).Checked;
			RefreshTabs();
			SetSaveRequired(true);
		}
		void HandleMaximizedTabsCheckedChanged (object sender, EventArgs e)
		{
			Notes.MaximizeTabs = (sender as ToolStripButton).Checked;
			SetSaveRequired(true);
		}
		public override List<string> GetListOfStringsFromSystemTable (string tableName, int Column)
		{
			return GetListOfStringsFromSystemTable(tableName, Column, "*");
		}
		public override List<string> GetListOfStringsFromSystemTable (string tableName, int Column, string filter)
		{
			return 		GetListOfStringsFromSystemTable(tableName, Column, filter, true);
		}
		/// <summary>
		/// Gets the list of strings from system table.
		/// 
		/// This is a wrapper to other public behavior but in order to consolidate error messaging
		/// </summary>
		/// <returns>
		/// The list of strings from system table.
		/// </returns>
		/// <param name='table'>
		/// Table.
		/// </param>
		public override List<string> GetListOfStringsFromSystemTable (string tableName, int Column, string filter, bool sort)
		{
			NoteDataInterface table = FindNoteByName (tableName);
			List<string> result = new List<string>();
			if (table != null && (table is NoteDataXML_Table)) {
				if (((NoteDataXML_Table)table).Columns.Length > Column) {
					result = ((NoteDataXML_Table)table).GetValuesForColumn(Column, filter);
					if (true == sort)
					{
					result.Sort ();
					}
				}
				else
				{
					NewMessage.Show (Loc.Instance.GetStringFmt ("The table named {0} does not have at least 2 columns, as is required. If you need to reset the System Layout, do so through Options|Interface.",tableName));
				}
			} else {
				NewMessage.Show (Loc.Instance.GetStringFmt ("The table named {0} did not exist on the System Note page. If you need to reset the System Layout, do so through Options|Interface.",tableName));
			}
			return result;
		}
		/// <summary>
		/// Handles the drop down random tables opening.
		/// 
		/// Will grab a list of Pages defined on the SYSTEMPAGE for RandomTables (tablename/notename = RandomTables)
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleDropDownRandomTablesOpening (object sender, EventArgs e)
		{
			// get the sytem page (stored in layoutdetails?
			(sender as ToolStripDropDownButton).DropDown = null;
			ContextMenuStrip randoms = new System.Windows.Forms.ContextMenuStrip();
		
			// get the note with name = "RandomTables"
			//(sender as ToolStripDropDownItem).DropDownItems.Clear ();

			// get the tables from the System Note
			List<string> tablenames = LayoutDetails.Instance.TableLayout.GetListOfStringsFromSystemTable(LayoutDetails.SYSTEM_RANDOM_TABLES,1);


			//string[] tablenames = new string[2]{"charactermaker", "charactermaker|villains"};

			foreach (string table in tablenames) {
				ToolStripButton button = new ToolStripButton(table);
				button.Click+= HandleRandomTableClick;
				//(sender as ToolStripDropDownItem).DropDownItems.Add (button);
				randoms.Items.Add (button);

			}
			// load the specified page (the first part of a a|b combination, though can also be on its own

			(sender as ToolStripDropDownButton).DropDown = randoms;

			// we know this is a table. Call its random function
		}




		/// <summary>
		/// breaking down the functionality of a search
		/// Because I needt oa ccess table notes
		/// </summary>
		/// <param name="searchterm"></param>
		/// <returns></returns>
//		private string DoInteriorSearch(string searchterm, Appearance[] objects, string path, string ExtraFilesFolder)
//		{
//			
//			foreach (Appearance appearance in objects)
//			{
//				
//				
//				
//				if (appearance != null && appearance.Caption != null && appearance.Caption != "" && appearance.Caption.ToLower() == searchterm)
//				{
//
//
//				
//
//					return appearance.RandomCharacter(this);
//				}
//				
//				
//				/// This did nothing because the NotePanel does not actually exist (October 2012)
//				if (appearance!= null && appearance.ShapeType == Appearance.shapetype.Panel)
//				{
//					
//					// - get filename to load
//					// NOPE: Do not have access to this   appearance.virtualDesktop.GetFileNameForNotePanel();
//					
//					string filename = NotePanelPanel.GetBaseFileName(path, ExtraFilesFolder, appearance);
//					// NewMessage.Show(filename);
//					//filename appears correct
//					
//					
//					// This is ICKY. BUt I have to actually create a virtual desktop and try to search
//					
//					
//					
//					Appearance[] items = VirtualDesktop.LoadScrapbookJustFiles(filename);
//					if (items != null)
//					{
//						string result =  DoInteriorSearch(searchterm, items, path, ExtraFilesFolder);
//						if (result != null && result != "")
//						{
//							return result;
//						}
//					}
//					
//					// now load manually.
//				}
//			}
//			return "";
//		}
		/// <summary>
		/// Gets the random table details. (When button clicked on Layoutbar)
		/// </summary>
		/// <returns>
		/// The random table details.
		/// </returns>
		string GetRandomTableDetails(string identifier)
		{
			try
			{
				string[] pars = identifier.Split(new char[1] {'|'});
				
				string LayoutName = pars[0];
				string tableString = Constants.BLANK;
				if (pars.Length > 1)
				{
					tableString = pars[1];
				}
				
			
				if (MasterOfLayouts.ExistsByName(LayoutName) == true)
				{
					string LayoutGUID = MasterOfLayouts.GetGuidFromName(LayoutName);
					//LayoutDatabase randompageLayout = new LayoutDatabase(LayoutGUID);
					LayoutPanel temporaryLayoutPanel = new LayoutPanel("", false);
					temporaryLayoutPanel.LoadLayout(LayoutGUID, false, TextEditContextStrip);
					//randompageLayout.LoadFrom(temporaryLayoutPanel);

					// now search for the note (the table)
				if (Constants.BLANK == tableString)
				{
					// no table was defined so we look for a table named start
					tableString = "start";
				}
					
				tableString = tableString.ToLower();
					NoteDataInterface table = temporaryLayoutPanel.FindNoteByName(tableString);
				

					if (table != null && table is NoteDataXML_Table)
					{
						return 	((NoteDataXML_Table)table).GetRandomTableResults ();
					}
					else
					{
						NewMessage.Show (Loc.Instance.GetStringFmt("This was not a valid table. Is there a table named {0} on the layout {1}?", tableString,LayoutName));
					}
					temporaryLayoutPanel.Dispose ();
				}
				else
				{
					NewMessage.Show (Loc.Instance.GetStringFmt("The layout named {0} does not exist", LayoutName));
				}
			
	
			}
			catch (Exception ex)
			{
				NewMessage.Show(ex.ToString());
			}
			return Constants.BLANK;
		}

		/// <summary>
		/// Handles the random table click.
		/// // This is the actual loading of the table, pasting the result into the Current Text Box
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleRandomTableClick (object sender, EventArgs e)
		{

			// get the table name
			string value = ((ToolStripItem)sender).Text;

	
			
			string table = "";
			// override table name with current text on note 
			if (CurrentTextNote != null) {
				if (CurrentTextNote.SelectedText != "") {
					table = CurrentTextNote.SelectedText.ToLower ().Trim ();
				}
				
				if (table != "") {
					value = value + "|" + table;
				}
				
				string sResult = GetRandomTableDetails (value);
				
				
				// Sep 2012
				// can define things like 
				// if there's the word PROMPT in the text then we prompt
				// case sensitive
				if (value.IndexOf ("PROMPT") > -1) {
					NewMessage.Show (sResult);
				} else {
					// we want text to appear beside selection and not replace it.
					CurrentTextNote.SelectionStart = CurrentTextNote.SelectionStart + table.Length;
					CurrentTextNote.SelectionLength = 0;
					CurrentTextNote.SelectedText = String.Format ("{0}{1}", Environment.NewLine, sResult);
				}
			} else {
				lg.Instance.Line("LayoutPanel->HandleRandomTableClick", ProblemType.MESSAGE, String.Format ("LayoutPanel {0} has no CurrentTextNote assigned", this.GUID));
				NewMessage.Show (Loc.Instance.GetString ("You must select a note first"));
			}
		}

		/*public LayoutPanel(string parentGUID) : this (parentGUID, false)
		{


		}
*/
		/// <summary>
		/// Initializes a new instance of the <see cref="Layout.LayoutPanel"/> class.
		/// 
		/// 
		/// </summary>
		/// <param name='GUID'>
		/// Unique identifier of PARENT. The GUID needs to be blank unless this is a CHILD note of another Layout.
		/// </param>
		/// <param name='IsSystem'>
		/// if true (default is false) means this ist he special one of a kind system layout
		/// </param>
		public LayoutPanel (string parentGUID, bool IsSystem)
		{


		
			ParentGUID = parentGUID;
			GetIsSystemLayout = IsSystem;


			//Type[] typeList = LayoutDetails.Instance.TypeList;


			LayoutDetails.Instance.AddToList (typeof(NoteDataXML_Panel), new NoteDataXML_Panel ().RegisterType ());

			/*Type[] newTypeList = new Type[typeList.Length + 1];

			for (int i = 0; i < typeList.Length; i++)
			{
				newTypeList[i] = typeList[i];
			}

			newTypeList[newTypeList.Length-1] = typeof(NoteDataXML_Panel);
			LayoutDetails.Instance.TypeList = newTypeList;
*/

			//ToolStripContainer toolstrips = new ToolStripContainer();

			//toolstrips.Dock = DockStyle.Top;
		//	this.Controls.Add (toolstrips);

			//this.BackColor = Color.Pink;
			if (!GetIsChild && !GetIsSystemLayout)
				BuildFormatToolbar ();
			if (!GetIsSystemLayout) BuildTabsBar ();






			NoteCanvas = new Panel();
			NoteCanvas.Dock = DockStyle.Fill;
			NoteCanvas.Parent = this;
			NoteCanvas.BringToFront();
			this.AutoScroll = true;
			//this.Enter += HandleGotFocus;
			//this.MouseEnter+=HandleGotFocus;



		}

//		public override void SystemNoteHasClosedDown (bool closed)
//		{
//			lg.Instance.Line("SystemNoteHasClosedDown", ProblemType.MESSAGE, "GUID OF THIS LAYOUT IS " + this.GUID, Loud.CTRIVIAL);
//			if (GetSaveRequired == true) {
//				NewMessage.Show ("You need a save when a system layout panel was closed");
//				SetSaveRequired(false);
//			}
//		}

		/// <summary>
		/// Handles the got focus. This is where we set the CurrentLayout
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleGotFocus (object sender, EventArgs e)
		{
			//LayoutDetails.Instance.CurrentLayout = this;
		}

		/// <summary>
		/// Create buttons with the list of types we can make
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleAddNoteDropDownOpening (object sender, EventArgs e)
		{
			(sender as ToolStripDropDownButton).DropDownItems.Clear ();

			foreach (Type t in LayoutDetails.Instance.ListOfTypesToStoreInXML()) {
				ToolStripButton AddNote = new ToolStripButton(String.Format ("{0}",LayoutDetails.Instance.GetNameFromType(t)));

				AddNote.Tag = t;

				AddNote.Click += HandleAddNoteClick;
				(sender as ToolStripDropDownButton).DropDownItems.Add (AddNote);

			}





		}

		/// <summary>
		/// Count the number of notes on the main screen (not inside of other panels).
		/// This is a faster lookup than CountNotes() which  l;ooks through childrens
		/// </summary>
		public int Count()
		{
			return Notes.CountNotes();
		}
		/// <summary>
		/// Counts the notes. Added for testing. This COUNTS all the notes on the page by going through the children
		/// So it can be slow
		/// </summary>
		/// <returns>
		/// The notes.
		/// </returns>
		public override int CountNotes()
		{

			int count = 0;
			System.Collections.ArrayList list = GetAllNotes();
			foreach (Layout.NoteDataInterface note in list) {
				count++;
				//Console.WriteLine(String.Format ("**{0}** GUID = {1} Name={2}", count, note.GuidForNote, note.Caption));
			}
			return count;

			//return Notes.GetAllNotes().Count;
		}

		void AddNoteFromMenu (Type TypeTest)
		{
			if (null != TypeTest) {
				NoteDataInterface note = null;
				try {
					note = (NoteDataInterface)Activator.CreateInstance (TypeTest, -1, -1);
					Notes.Add (note);
					note.CreateParent (this);
					note.BringToFrontAndShow ();
					// march 2013, added this for operations that are not guaranteed to work during loading and must be delayed. of corse
					// they must happen immediately after a create
					note.UpdateAfterLoad();
					UpdateListOfNotes ();
				}
				catch (Exception ex) {
					NewMessage.Show (ex.ToString ());
				}
				SetSaveRequired (true);
			}
			else {
				lg.Instance.Line ("LayoutPanel.HandleAddNoteClick", ProblemType.ERROR, String.Format ("{0} Type not found", TypeTest.ToString ()));
			}
		}

		/// <summary>
		/// Generic Handle for adding notes by type
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleAddNoteClick (object sender, EventArgs e)
		{

			if (Notes == null) {
				NewMessage.Show (Loc.Instance.Cat.GetString ("Load or create a note first"));
				return;
			}

			if ((sender as ToolStripButton).Tag == null) {
				lg.Instance.Line ("LayoutPanel.HandleAddNoteClick", ProblemType.WARNING, "Unable to Add a Note of this Type Because Tag was Null");
			} else {
				//Type t = typeof(NoteDataXML);
				//Console.WriteLine (t.Assembly.FullName.ToString());
				//Console.WriteLine (t.AssemblyQualifiedName.ToString());
				//string TypeToTest =  (sender as ToolStripButton).Tag.ToString ();
				//NoteDataInterface note = (NoteDataInterface)Activator.CreateInstance ("LayoutPanels", TypeToTest);
				Type TypeTest = Type.GetType (((Type)(sender as ToolStripButton).Tag).AssemblyQualifiedName);

				if (null == TypeTest)
				{
					// usually happens when a plugin is dynamically added
					// This tries to load the Type differently
					TypeTest=(Type)(sender as ToolStripButton).Tag;
				}
				//Type TypeTest = Type.GetType (t.AssemblyQualifiedName.ToString());

				AddNoteFromMenu (TypeTest);
			}
		}

	
	









		public override void SaveLayout ()
		{
			if (null != Notes) {
				lg.Instance.Line("LayoutPanel.SaveLayout", ProblemType.MESSAGE, "Saved");

				// March 4 2013
				if (GetIsChild == true && Notes.ParentGuid == Constants.BLANK)
				{
					Notes.ParentGuid =  GetAbsoluteParent().GUID;// GUID;// ParentGUID;
				}
				if (Notes.SaveTo() == true)
				{
				SetSaveRequired (false);
				}
				else
				{
					NewMessage.Show (Loc.Instance.Cat.GetString("This note is already being saved. Try again."));
				}
				if (GUID == SYSTEM_LAYOUT)
				{
					 
					LayoutDetails.Instance.TableLayout=new Layout.LayoutPanel(Constants.BLANK, false);
					// Jan 14 2013 - changing this to a subnote.
					LayoutDetails.Instance.TableLayout.LoadLayout ("tables", true, TextEditContextStrip);




				}
			}
			else
			{
				lg.Instance.Line("LayoutPanel.SaveNoteClick", ProblemType.MESSAGE,"No notes loaded");
			}
		}


		public override void UpdateListOfNotes ()
		{

			RefreshTabs();
			Notes.UpdateListOfNotes();
		
		}


	/// <summary>
	/// Loads the layout.
	/// </summary>
	/// <param name='_GUID'>
	/// _ GUI.
	/// </param>
	/// <param name='IsSubPanel'>
	/// If set to <c>true</c> is sub panel.
	/// </param>
		public override void LoadLayout (string _GUID, bool IsSubPanel, ContextMenuStrip textEditorContextStrip)
		{
			TextEditContextStrip = textEditorContextStrip;
			//NewMessage.Show (_GUID);
			// super important to track parent child relationships
			GUID = _GUID;
			// ToDO: Check for save first!

			NoteCanvas.Controls.Clear ();
			// disable autoscroll because if an object is loaded off-screen before others it changes the centering of every object
			NoteCanvas.AutoScroll = false;


			if (Constants.BLANK == GUID) {
				NewMessage.Show ("You must specify a layout to load");
				return;
			}


			Notes = new LayoutDatabase (GUID);
			Notes.IsSubPanel = IsSubPanel;
			if (Notes.LoadFrom (this) == false) {
				lg.Instance.Line ("LayoutPanel.LoadLayout", ProblemType.MESSAGE, "This note is blank still.");
				//Notes = null;
				//NewMessage.Show ("That note does not exist");
			} else {
				if (this.header != null)
					header.UpdateHeader ();
				UpdateListOfNotes ();
				//	NewMessage.Show (String.Format ("Name={0}, Status={1}", Notes.Name, Notes.Status));
			}
		
			NoteCanvas.AutoScroll = true;
			RefreshTabs ();
			if (!GetIsChild && !GetIsSystemLayout) {
				if (header != null)
					header.Dispose ();
				header = new HeaderBar (this, this.Notes);
				
			}
			SetSaveRequired (false);


			UpdateLayoutToolbar ();
			this.BackColor = Notes.BackgroundColor;
			IsLoaded = true;

			// certain notes need to call an update routine AFTER all the other
			// notes on the page have been updated
			foreach (NoteDataInterface note in LayoutDetails.Instance.UpdateAfterLoadList) {

				note.UpdateAfterLoad();
			//TODO: do speed testing of this aftewreards
			}

			LayoutDetails.Instance.UpdateAfterLoadList = new List<NoteDataInterface>();

		}

		private void UpdateLayoutToolbar()
		{
			if (!GetIsSystemLayout) BuildLayoutToolbar ();

		}

		override public bool ShowTabs 
		{ get { return Notes.ShowTabs; } set {
				Notes.ShowTabs = value; 
			} }
		
		/// <summary>
		/// Refreshs the tabs.
		/// </summary>
		public override void RefreshTabs ()
		{
			//return; //disabling this made NO difference on a 6 second load card


			if (Notes.ShowTabs == true && tabsBar != null) {
				lg.Instance.Line("LayoutPanel->RefreshTables", ProblemType.MESSAGE,String.Format (">>> refresh tabs for: {0}!<<<", this.Name), Loud.CTRIVIAL);
				tabsBar.Visible = true;
				tabsBar.Items.Clear ();

				NoteDataInterface[] notes_array = Notes.GetNotesSorted();
				// redraw the list of tabs
				foreach (NoteDataInterface note in notes_array) {
					if (tabsBar.Items.Count > 0) {
						// insert sep
						ToolStripSeparator sep = new ToolStripSeparator ();
						tabsBar.Items.Add (sep);
					}
					ToolStripButton but = new ToolStripButton ();
					but.Text = note.Caption;
					but.Tag = note.GuidForNote;
					but.Click+= HandleTabButtonClick;
					tabsBar.Items.Add (but);

				}
			} else {
				if (tabsBar != null) tabsBar.Visible = false;
			}
		}

		void HandleTabButtonClick (object sender, EventArgs e)
		{
			if (sender is ToolStripButton && (sender as ToolStripButton).Tag != null) {
				string guid = (sender as ToolStripButton).Tag.ToString ();
				NoteDataInterface note = FindNoteByGuid(guid);
				if (note != null)
				{
					GoToNote(note);
					if (Notes.MaximizeTabs)
					{
						note.Dock = DockStyle.Fill;
						note.UpdateLocation();
					}
					else
					{
						note.Dock = DockStyle.None;
						note.UpdateLocation();
					}

				}
				else
				{
					lg.Instance.Line ("LayoutPanel->HandleTabButtonClick", ProblemType.WARNING, String.Format ("Note with guid = {0} not found",guid));
				}

			}
		}


		/// <summary>
		/// Only for converting ol files
		/// </summary>
		/// <param name='_GUID'>
		/// _ GUI.
		/// </param>
		public  void NewLayoutFromOldFile (string _GUID, string File, bool BULKIMPORT)
		{
			// NOTE: We will override this GUID in the LoadFromOld routine
			GUID= _GUID;
			// check to see if exists already
			if (Notes != null && Notes.IsLayoutExists (GUID)) {
				NewMessage.Show("that layout exists already");
			} else {
				// if somehow we haven't not supplied a GUID then provide one now
				if (Constants.BLANK == GUID) GUID = System.Guid.NewGuid ().ToString ();
				
				string bulk = Constants.BLANK;
				if (true == BULKIMPORT)
				{
					bulk = System.Guid.NewGuid().ToString();
				}
				noteCanvas.Controls.Clear ();
				//
				Notes = new LayoutDatabase (GUID);
				string newGUID = ((LayoutDatabase)Notes).LoadFromOld (File, this, bulk);
				LayoutDetails.Instance.TransactionsList.AddEvent(new Transactions.TransactionImportLayout(DateTime.Now, newGUID,Notes.Name));

				System.Collections.ObjectModel.ReadOnlyCollection<NoteDataInterface> thenotes =  Notes.GetNotes();

				foreach (NoteDataInterface note in thenotes)
				{
					if (note is NoteDataXML_Timeline)
					{
						// we do not create the timeline table because they *should* be comign with
						((NoteDataXML_Timeline)note).TableCreated = true;
					}
					note.CreateParent(this);
				}
				if (!GetIsChild) {
					if (header != null) header.Dispose();
					header = new HeaderBar(this, this.Notes);
					
					
				}
			}
			NoteCanvas.AutoScroll = true;
		}

	
		/// <summary>
		/// Must be called when creating a new layout
		/// </summary>
		/// <param name='GUID'>
		/// GUI.
		/// </param>
		public override void NewLayout (string _GUID, bool AddDefaultNote, ContextMenuStrip textEditorContextStrip)
		{
			TextEditContextStrip = textEditorContextStrip;
			GUID= _GUID;
			// check to see if exists already
			if (Notes != null && Notes.IsLayoutExists (GUID)) {
				NewMessage.Show("that layout exists already");
			} else {
				// if somehow we haven't not supplied a GUID then provide one now
				if (Constants.BLANK == GUID) GUID = System.Guid.NewGuid ().ToString ();


				noteCanvas.Controls.Clear ();
					//
				Notes = new LayoutDatabase (GUID);


				// Not sure a linktable was added to a child note!? Adding both tests (which is bad) TODO there should only be one test
				if (GetIsChild == false && Notes.IsSubPanel == false)
				{
					NoteDataXML_Table temp = new NoteDataXML_Table();
					// we set this to NOT STICKY_TABLE so that it knows to build a link table.
					temp.GuidForNote="nonsense";
					Notes.CreateLinkTableIfNecessary(temp, this);
				}
			
				if (true == AddDefaultNote)
				{
				NoteDataXML_RichText newNote = new NoteDataXML_RichText();
				AddNote (newNote);
				}
				if (!GetIsChild) {
					if (header != null) header.Dispose();
					header = new HeaderBar(this, this.Notes);
				

				}
				UpdateLayoutToolbar();
				this.BackColor = Notes.BackgroundColor;
			}
			NoteCanvas.AutoScroll = true;
			IsLoaded = true;
		}
		public void SetName(string newName)
		{
			Notes.Name = newName;
		}

		public override List<NoteDataInterface> GetAvailableFolders ()
		{
			return Notes.GetAvailableFolders();

		}
		public override void AddNote(NoteDataInterface note)
		{

			Notes.Add (note);
			// added to simplilfy things but need to test
			note.CreateParent(this);
			// march 2013, added this for operations that are not guaranteed to work during loading and must be delayed. of corse
			// they must happen immediately after a create
			// ** I realizes this is needed in ADDNOTEFROMMENU and note here. So commented out, in case it breaks things [didn't notice problems with it but if its inot needed, its not needed]
		//	note.UpdateAfterLoad();
			RefreshTabs ();
		}
		public override void MoveNote (string GUIDOfNoteToMove, string GUIDOfLayoutToMoveItTo)
		{
			lg.Instance.Line("LayoutPanel->MoveNote", ProblemType.MESSAGE, String.Format ("moving note {0} to panel {1}", 
			                                                                              GUIDOfNoteToMove, GUIDOfLayoutToMoveItTo), Loud.ACRITICAL);
			// Take 1 was using LayoutDatabase

			// Take 2 - just manipulating the note arrays here and then saving?

			//  a. find note we want to move
		//	NoteDataXML_Panel newPanel = null;
			NoteDataInterface movingNote = null;

			System.Collections.ObjectModel.ReadOnlyCollection<NoteDataInterface> notes = Notes.GetNotes ();
			int PositionOfNewPanel=-1;
			// try to resolve a 'missing parent' noteproblem with TestMovingNotes unit test
			for (int i = 0; i < notes.Count; i++) {

				if (notes[i].GuidForNote == GUIDOfLayoutToMoveItTo && notes[i].IsPanel == true) {
					PositionOfNewPanel=i;
				}
				if (notes[i].GuidForNote == GUIDOfNoteToMove) {
					movingNote = notes[i];
					
				}
			}


//			foreach (NoteDataInterface note in notes) {
//				if (note.GuidForNote == GUIDOfLayoutToMoveItTo && note.IsPanel == true) {
//					newPanel = (NoteDataXML_Panel)note;
//				}
//				if (note.GuidForNote == GUIDOfNoteToMove) {
//					movingNote = note;
//
//				}
//			}
			if (movingNote != null) {
				movingNote.Save ();
			}

			if ("up" == GUIDOfLayoutToMoveItTo) {
				// we want to move OUT of this folder
				if (Constants.BLANK == this.ParentGUID) {
					// I do not have a parent which means UP maes no sense
					NewMessage.Show (Loc.Instance.Cat.GetString ("This note does not have a parent. Cannot be moved."));
					return;
				} else {
					if (movingNote != null) {
						Notes.RemoveNote (movingNote);

						bool done = false;
						LayoutPanel upstreamLayout = null;
						Control source = this;
						while (!done) {
							// search for appropriate parent? Is this a HACK
							Control control = source.Parent;
							if (control is LayoutPanel) {
								if ((control as LayoutPanel).GUID == this.ParentGUID) {
									// we are the layoutpanel we need to be on
									done = true;
									upstreamLayout = (LayoutPanel)control;
								}
							}
							if (false == done) {
								if (source.Parent == null) {
									done = true;
								} else
									source = source.Parent;
							}
						}


						upstreamLayout.AddNote (movingNote);
						upstreamLayout.SaveLayout ();
					
					}
				}
			} else {

				if (-1 == PositionOfNewPanel)
				{
					throw new Exception("PositionOfNewPanel is still -1. We did not find the panel with guid = " + GUIDOfLayoutToMoveItTo);
				}


				if (/*newPanel == null*/ null==notes[PositionOfNewPanel] || null==movingNote) {
					NewMessage.Show (Loc.Instance.Cat.GetString ("Destination or note was null. No move"));
					return;
				}




				// add note
				//newPanel.AddNote (movingNote);


				// remove note

				// THe Add has to HAPPEN first because the Index gets Changed (and hence would be messed up)
				// but still runing into issues
				NoteDataXML_Panel NewPanel =(NoteDataXML_Panel)notes[PositionOfNewPanel];

				Notes.RemoveNote (movingNote);
				NewPanel.AddNote(movingNote);

			}
			/*
			try {
				movingNote.Location = new Point (0, 0);
			} catch (Exception) {
				lg.Instance.Line("LayoutPanel.MoveNote", ProblemType.WARNING, "problem setting location of note. Ok if this is unit testing");
			}
			*/
			SaveLayout ();

			// must save before calling this
			//if (newPanel != null) {

//			// both 'paths' come through here, the UP path, the PositionOfNewPanel will not have been set
//			if (-1 != PositionOfNewPanel && notes[PositionOfNewPanel] !=null){
//
//				//newPanel.Update (this);
//
//				//error happens because of this line! Destroying the old note to build the new?
//				// WAS: notes[PositionOfNewPanel].Update(this);
//
//				// can't work
//				//movingNote.CreateParent(notes[PositionOfNewPanel]);
//
//				// so we tru
//				//((NoteDataXML_Panel)notes[PositionOfNewPanel]).
//
//				// all this removed in favor of adding a CreateParent to the NoteDataXML_Panel.AddNote
//
//			}

			this.RefreshTabs();
			//  b. 


			/*Take 1
			Notes.MoveNote (GUIDOfNoteToMove, GUIDOfLayoutToMoveItTo);
			foreach (NoteDataInterface note in Notes.GetNotes ()) {
				if (note.GuidForNote == GUIDOfLayoutToMoveItTo)
				{
					note.CreateParent(this);
				}
			}
			SaveLayout();
			*/
			// need to reload the Layout We Added To First before saving this layotu
			//SaveTo();
		}

		public override System.Collections.ArrayList GetAllNotes()
		{
			return this.Notes.GetAllNotes();
		}

		public override void SetSaveRequired (bool NeedSave)
		{
			lg.Instance.Line("SetSaveRequired", ProblemType.MESSAGE, this.GUID+"Value of _savequired BEFORE SET is " + _saverequired, Loud.CTRIVIAL);
			_saverequired = NeedSave;
			lg.Instance.Line("SetSaveRequired", ProblemType.MESSAGE, this.GUID+"set to " + NeedSave.ToString(), Loud.CTRIVIAL);
			lg.Instance.Line("SetSaveRequired", ProblemType.MESSAGE, this.GUID+"Value of _savequired is " + _saverequired, Loud.CTRIVIAL);
			lg.Instance.Line("SetSaveRequired", ProblemType.MESSAGE, this.GUID+"Value of GetSaveRequired is " + GetSaveRequired, Loud.CTRIVIAL);
			if (SetSubNoteSaveRequired != null) {

				SetSubNoteSaveRequired(NeedSave);
			}
		}

		/// <summary>
		/// Gos to note.
		// goes to a note on this page
		// ASSUMES: Actually open in the interface
		// NOTE: Get Notes will NOT GET INTERFACE elements for notes found in subpanels (only the note) because Layout is set to null when we search for them.
		//       We have to do adanced detective work after getting the note
		/// </summary>
		/// <param name='guid'>
		/// GUID.
		/// </param>
		public override NoteDataInterface FindNoteByGuid (string guid)
		{
			// go through list of notes until we find a match
			foreach (NoteDataInterface note in Notes.GetAllNotes()) {
				if (note.GuidForNote == guid)
				{
					return note;
				}
			
			}
			return null;
		}
		/// <summary>
		/// Finds the name of the note by.
		/// Because names are not unique, will return FIRST name match
		/// </summary>
		/// <returns>
		/// The note by name.
		/// </returns>
		/// <param name='name'>
		/// Name.
		/// </param>
		public override NoteDataInterface FindNoteByName(string name)
		{
			// go through list of notes until we find a match
			foreach (NoteDataInterface note in Notes.GetAllNotes()) {
				if (note.Caption == name)
				{
					return note;
				}
				
			}
			return null;
		}

		/// <summary>
		/// Finds the subpanel note.
		/// This is called after a FindByGuid or FindByName when we want the actual
		/// note with its interface (which those do not return).
		/// 
		/// NOTE (Feb 2013) - This will be called by a couple different routes. A combination of FindNoteBy and the more explicit GoToNoteOnSameLayout
		/// 
		/// This will call down the panel chain, looking for the note
		/// </summary>
		/// <param name='note'>
		/// Note.
		/// </param>
		public override NoteDataInterface FindSubpanelNote (NoteDataInterface note)
		{
			// dig deeper BUT ONLY if we are loaded
			// now sure how to know this (TO DO)
			foreach (Control control in this.noteCanvas.Controls) {
				if (control is NotePanel) {

					NoteDataInterface foundNote = ((control as NotePanel).GetChild () as NoteDataInterface);

					if (foundNote.GuidForNote == note.GuidForNote) {
						note = foundNote;
						return note;
					}
					else
					if (foundNote is NoteDataXML_Panel) {
						// do this search inside this
						note = (foundNote as NoteDataXML_Panel).FindSubpanelNote (note);
						// february 2013 - error is that we are returning early, and not 'finding' a note
						// the entire point of this to return a Note with a GUI (for in-layout operations)
						// so we test to make sure we actually have found something valid before returning!
						// i.e., going down the 'wrong panel' early, aborts the actual search prematurely
						if (note.ParentNotePanel != null)
						{
						return note;
						}

					}
				}
			}

			return note;
		}
		private void ShowAndFlash(NoteDataInterface note)
		{
			if (note.ParentNotePanel != null) {
				// if we ever go to a note we make it visisble
				note.Visible = true;
				note.BringToFrontAndShow ();
				note.Flash ();
			} else {
				lg.Instance.Line("LayoutPanel->GoToNote", ProblemType.MESSAGE, "Even with advanced search we did not find note");
			}
		}

		/// <summary>
		/// Will BRINGTOFRONT the indicated note and make it flash.
		/// 
		/// We return the note mostly for testing purposes
		/// </summary>
		/// <param name='note'>
		/// Note.
		/// </param>
		public override NoteDataInterface GoToNote (NoteDataInterface note)
		{
			note = GetNote (note);
			ShowAndFlash(note);
			return note;
//			// at this point if the note does not have a parent (because it is a subnote and this is not instantiated when 
//			// searching
//			// then we need to search for it, to find it
//			if (note.ParentNotePanel == null && this.Controls != null && this.Controls.Count > 0) {
//				note = FindSubpanelNote(note);
//
//			
//			}
//				
//
//			if (note.ParentNotePanel != null) {
//				// if we ever go to a note we make it visisble
//				note.Visible = true;
//				note.BringToFrontAndShow ();
//				note.Flash ();
//			} else {
//				lg.Instance.Line("LayoutPanel->GoToNote", ProblemType.MESSAGE, "Even with advanced search we did not find note");
//			}
//			return note;
		}

		// the fast version, looking 
		public NoteDataInterface GetNote (NoteDataInterface note)
		{
			// at this point if the note does not have a parent (because it is a subnote and this is not instantiated when 
			// searching
			// then we need to search for it, to find it
			if (note.ParentNotePanel == null && this.Controls != null && this.Controls.Count > 0) {
				note = FindSubpanelNote(note);
				
				
			}
			
			

			return note;
		}
		public override NoteDataInterface GetNoteOnSameLayout (string GUID, bool GoTo)
		{
			return GetNoteOnSameLayout(GUID, GoTo, Constants.BLANK);
		}


		/// <summary>
		/// Similiar to GoToNote but faster, skipping the FindNoteByGUID, and doing it as part of this search
		/// </summary>
		/// <param name='GUID'>
		/// GUI.
		/// </param>
		public override NoteDataInterface GetNoteOnSameLayout (string GUID, bool GoTo, string TextToFindInRichEdit)
		{
			// we make a fake note, knowing we'll find the real deal
			NoteDataInterface fakeNote = new NoteDataXML ();
			fakeNote.GuidForNote = GUID;

			//	fakeNote = FindNoteByGuid
			fakeNote.Caption="findme";
			fakeNote = GetNote (fakeNote);
			LayoutPanel myParent = null;

			if (null == fakeNote || "findme" == fakeNote.Caption) {
				// try to look at parent
				myParent = (LayoutPanel)this.GetAbsoluteParent();
				if (null != myParent)
				{
					fakeNote = myParent.GetNote(fakeNote);
				}
			}


			if (null != fakeNote) {
				if (GoTo) {
					ShowAndFlash (fakeNote);

				
					if (Constants.BLANK != TextToFindInRichEdit) {

						if (fakeNote is NoteDataXML_RichText) {

							CurrentTextNote = (NoteDataXML_RichText)fakeNote;

							// if we needed to find a parent then the findbar is probably null
							if (null == FindBar && null != myParent)
							{
								myParent.FindBar.DoFind (TextToFindInRichEdit, false, CurrentTextNote.GetRichTextBox (),0);
							}
							else
							{
							FindBar.DoFind (TextToFindInRichEdit, false, CurrentTextNote.GetRichTextBox (),0);
							}
						}
					}

				}
			} else {
				throw new Exception(GUID + " was an invalid note in GetNoteOnSameLayout");
			}
			return fakeNote;
		}

		/// <summary>
		/// Gets the link table.
		/// 
		/// Will build it, if necessary, at this point (the point when something needs to access it)
		/// </summary>
		/// <returns>
		/// The link table.
		/// </returns>
		public override CoreUtilities.Links.LinkTable GetLinkTable ()
		{



			if (GetIsChild == false) {
				return Notes.GetLinkTable ();

	
			}


			LayoutPanel absoluteParent = (LayoutPanel)GetAbsoluteParent ();
			if (absoluteParent != null) {
				return absoluteParent.GetLinkTable ();
			}

//			// if I am a child, get my parents LinkTable, please
//			if (this.Parent != null) {
//			//	NewMessage.Show (this.Parent.Name);
//				LayoutPanel layoutParent = null;
//				Control looker = this.Parent;
//				while (layoutParent == null)
//				{
//					if (looker is LayoutPanel)
//					{
//						layoutParent= (looker as LayoutPanel);
//					}
//					else
//					{
//						looker = looker.Parent;
//					}
//				}
//				return layoutParent.GetLinkTable ();
//			} else {
//				NewMessage.Show (Loc.Instance.GetStringFmt("No parent for this layout {0} with ParentGUID = {1}", this.Notes.Name, this.ParentGUID));
//			}




			return null;
		}

		// For testing
		public override string Section {
			get { return Notes.Section;}
		}
		public override string Subtype {
			get { return Notes.Subtype;}

		}
		public override string Notebook {
			get { return Notes.Notebook;}
		}
		public override string Keywords {
			get { return Notes.Keywords;}
		}
		public override string ToString ()
		{
			return string.Format ("[LayoutPanel: NoteCanvas={0}, Caption={1}, GetIsSystemLayout={2}, CurrentTextNote={3}, ShowTabs={4}, Section={5}, Subtype={6}, Notebook={7}, Keywords={8}]", NoteCanvas, Caption, GetIsSystemLayout, CurrentTextNote, ShowTabs, Section, Subtype, Notebook, Keywords);
		}
		/// <summary>
		/// Sets the parent fields. Calledf rom NoteDataXML_Panel
		/// and is used to make the children have the same fields
		/// as the parents to make GetRandomNote work
		/// </summary>
		/// <param name='section'>
		/// Section.
		/// </param>
		public override void SetParentFields (string section, string keywords, string subtype, string notebook)
		{
			if (GetIsChild == false) {
				throw new Exception("this should only be called on CHILD PANELS");
			} else {
				Notes.Section = section;
				Notes.Keywords = keywords;
				Notes.Subtype = subtype;
				Notes.Notebook = notebook;
			}
		}
		public override void ClearDrag ()
		{


			// not turn off drag on every note

			foreach (Control control in noteCanvas.Controls) {
				if (control is NotePanel)
				{
					((NotePanel)control).GetChild().EndDrag();
					if (((NotePanel)control).GetChild() is NoteDataXML_Panel)
					{
						((NoteDataXML_Panel)((NotePanel)control).GetChild()).ClearDrag();
					}
				}
			}

			foreach (NoteDataInterface note in Notes.GetAllNotes()) {
				note.EndDrag();
			}
			base.ClearDrag ();
		}


		/// <summary>
		///  // [[Group,Storyboard,Chapter*,words]]
		// Look for storyboard nameed 'Storyboard'
		// Go through the groups with names matching Chapter*
		// Get list of page names from within those groups
		//   Excluding those that start with -- 
		// October 2012 - if you pass ALLONSTICKIT into then it gets all of them in the entire notebook
		/// </summary>
		/// <returns>an array of matching names for this storyboard panel</returns>
		public override string[] GetListOfGroupEmNameMatching (string sStoryboardName, string sGroupMatch)
		{
			// 1. Get the storyboard


			NoteDataInterface note = FindNoteByName (sStoryboardName);
			if (note == null) {
				NewMessage.Show (Loc.Instance.GetStringFmt("The storyboard {0} was not found", sStoryboardName));
				return null;
			}


			NoteDataXML_GroupEm StoryBoardNote = (NoteDataXML_GroupEm) GoToNote(note);
			ArrayList items = new ArrayList();
			
			if (StoryBoardNote != null)
			{
				
				
				
				{
					
					sGroupMatch = sGroupMatch.Replace("*", "").Trim();
					// 2. Have that storyboard return the list of names
					//  NewMessage.Show(panel.groupEms1.Groups[0].ToString());
					foreach (ListViewGroup group in StoryBoardNote.GetGroups())
					{
						if (group.Name.IndexOf(sGroupMatch) > -1)
						{
							// we have a name match
							// grab all qualifying pages
							foreach (ListViewItem item in group.Items)
							{
								// only add us if we are not excluded
								if (item.Text.IndexOf("--") == -1)
								{
									items.Add(item.Text);
								}
							}
						}
					}
				}
			}
			else
			{
				
				if ("ALLONSTICKIT" == sStoryboardName)
				{
					ArrayList arrayObjects = Notes.GetAllNotes();
					
					//bool AllNull = true;
					foreach (NoteDataInterface app in arrayObjects)
					{
						if (null != app && null != app.Caption && Constants.BLANK != app.Caption)
						{
							items.Add(app.Caption);
						}
					}
					arrayObjects = null;
				}
			}
			
			
			
			return (string[])items.ToArray(typeof(string));
		}
		public override void BringNoteToFront(string guid)
		{
			GetNoteOnSameLayout(guid, true);
		}
	}
}

