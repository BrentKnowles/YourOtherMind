using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using LayoutPanels;
using CoreUtilities;
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
	public class LayoutPanel : LayoutPanelBase, LayoutPanelInterface
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
		/// <summary>
		/// A reference to the active note
		/// </summary>
		/// <value>
		/// The current note.
		/// </value>
		public override NotePanelInterface CurrentNote {
			get {
				throw new System.NotImplementedException ();
			}
			set {
				throw new System.NotImplementedException ();
			}
		}

		ToolStrip tabsBar = null;
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

		#endregion


		public override string Caption {
			get { return Notes.Name;}
		}

		// set in NoteDataXML_Panel so that a child Layout will tell a higher level to save, if needed
		public Action<bool> SetSubNoteSaveRequired = null;
		/// <summary>
		/// Builds the format toolbar.
		/// </summary>
		private void BuildFormatToolbar ()
		{
			ToolStrip formatBar = new ToolStrip();
			formatBar.Parent = this;
			formatBar.Dock = DockStyle.Top;

			ToolStripButton bold = new ToolStripButton();
			bold.Text = "BOLD";
			formatBar.Items.Add (bold);
		}



		public void TabsBar()
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
		public NoteDataXML_SystemOnly GetSystemPanel ()
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

		public void LayoutToolbar ()
		{
			ToolStrip bar = new ToolStrip ();
			bar.Parent = this;
			bar.Visible = true;
			bar.Dock = DockStyle.Top;


			
			
			
			ToolStripDropDownButton AddNote = new ToolStripDropDownButton("Add Note");
			AddNote.DropDownOpening += HandleAddNoteDropDownOpening;
			bar.Items.Add (AddNote);
			
			


			//ToolStripLabel CurrentNote
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

			this.BackColor = Color.Pink;
			if (!GetIsChild && !GetIsSystemLayout)
				BuildFormatToolbar ();
			if (!GetIsSystemLayout) TabsBar ();
			if (!GetIsSystemLayout) LayoutToolbar ();






			NoteCanvas = new Panel();
			NoteCanvas.Dock = DockStyle.Fill;
			NoteCanvas.Parent = this;
			NoteCanvas.BringToFront();
			this.AutoScroll = true;
			//this.Enter += HandleGotFocus;
			//this.MouseEnter+=HandleGotFocus;



		}
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
				ToolStripButton AddNote = new ToolStripButton(LayoutDetails.Instance.GetNameFromType(t));

				AddNote.Tag = t;

				AddNote.Click += HandleAddNoteClick;
				(sender as ToolStripDropDownButton).DropDownItems.Add (AddNote);

			}





		}
		/// <summary>
		/// Counts the notes. Added for testing
		/// </summary>
		/// <returns>
		/// The notes.
		/// </returns>
		public int CountNotes()
		{
			return Notes.GetAllNotes().Count;
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
				NewMessage.Show (Loc.Instance.Cat.GetString("Load or create a note first"));
				return;
			}

			if ((sender as ToolStripButton).Tag == null) {
				Console.WriteLine ("LayoutPanel.HandleAddNoteClick", ProblemType.WARNING, "Unable to Add a Note of this Type Because Tag was Null");
			} else {
				//Type t = typeof(NoteDataXML);
				//Console.WriteLine (t.Assembly.FullName.ToString());
				//Console.WriteLine (t.AssemblyQualifiedName.ToString());
				//string TypeToTest =  (sender as ToolStripButton).Tag.ToString ();
				//NoteDataInterface note = (NoteDataInterface)Activator.CreateInstance ("LayoutPanels", TypeToTest);
				Type TypeTest = Type.GetType ( ((Type)(sender as ToolStripButton).Tag).AssemblyQualifiedName);

				//Type TypeTest = Type.GetType (t.AssemblyQualifiedName.ToString());

				if (null != TypeTest)
				{
				NoteDataInterface note = (NoteDataInterface)Activator.CreateInstance ( TypeTest);
			
				Notes.Add (note);
			
				note.CreateParent (this);
			
				UpdateListOfNotes ();
				SetSaveRequired (true); // TODO: Also need to go through and hook delegates/callbacks for when a note itself changes.
				}
				else
				{
					lg.Instance.Line("LayoutPanel.HandleAddNoteClick", ProblemType.ERROR, String.Format ("{0} Type not found", TypeTest.ToString()));
				}
			}
		}

	
	









		public override void SaveLayout ()
		{
			if (null != Notes) {
				lg.Instance.Line("LayoutPanel.SaveLayout", ProblemType.MESSAGE, "Saved");
				if (Notes.SaveTo() == true)
				{
				SetSaveRequired (false);
				}
				else
				{
					NewMessage.Show (Loc.Instance.Cat.GetString("This note is already being saved. Try again."));
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
		public override void LoadLayout (string _GUID, bool IsSubPanel)
		{

			//NewMessage.Show (_GUID);
			// super important to track parent child relationships
			GUID = _GUID;
			// ToDO: Check for save first!

			NoteCanvas.Controls.Clear ();
			// disable autoscroll because if an object is loaded off-screen before others it changes the centering of every object
			NoteCanvas.AutoScroll = false;


			if (Constants.BLANK == GUID) {NewMessage.Show ("You must specify a layout to load"); return;}


			Notes = new LayoutDatabase (GUID);
			if (Notes.LoadFrom (this) == false) {
				lg.Instance.Line("LayoutPanel.LoadLayout", ProblemType.MESSAGE, "This note is blank still.");
				//Notes = null;
				//NewMessage.Show ("That note does not exist");
			} else {
				if (this.header != null) header.UpdateHeader();
				UpdateListOfNotes ();
			//	NewMessage.Show (String.Format ("Name={0}, Status={1}", Notes.Name, Notes.Status));
			}
			Notes.IsSubPanel = IsSubPanel;
			NoteCanvas.AutoScroll = true;
			RefreshTabs();
			if (!GetIsChild && !GetIsSystemLayout) {
				if (header != null) header.Dispose();
				header = new HeaderBar(this, this.Notes);
				
			}
			SetSaveRequired(false);
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

			if (Notes.ShowTabs == true && tabsBar != null) {
				Console.WriteLine (String.Format (">>> refresh tabs for: {0}!<<<", this.Name));
				tabsBar.Visible = true;
				tabsBar.Items.Clear ();
				// redraw the list of tabs
				foreach (NoteDataInterface note in Notes.GetNotes()) {
					if (tabsBar.Items.Count > 0) {
						// insert sep
						ToolStripSeparator sep = new ToolStripSeparator ();
						tabsBar.Items.Add (sep);
					}
					ToolStripButton but = new ToolStripButton ();
					but.Text = note.Caption;
					but.Tag = note.GuidForNote;

					tabsBar.Items.Add (but);

				}
			} else {
				if (tabsBar != null) tabsBar.Visible = false;
			}
		}


		/// <summary>
		/// HACK TEMP: Only for converting ol files
		/// </summary>
		/// <param name='_GUID'>
		/// _ GUI.
		/// </param>
		public  void NewLayoutFromOldFile (string _GUID, string File)
		{
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

				((LayoutDatabase)Notes).LoadFromOld (File);
				foreach (NoteDataInterface note in Notes.GetNotes())
				{
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
		public override void NewLayout (string _GUID)
		{
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
				//	Notes = new LayoutDatabase("system");
				NoteDataXML newNote = new NoteDataXML();
				AddNote (newNote);
			
				if (!GetIsChild) {
					if (header != null) header.Dispose();
					header = new HeaderBar(this, this.Notes);
				

				}
			}
			NoteCanvas.AutoScroll = true;
		}


		public override List<NoteDataInterface> GetAvailableFolders ()
		{
			return Notes.GetAvailableFolders();

		}
		public void AddNote(NoteDataInterface note)
		{
			Notes.Add (note);
			// added to simplilfy things but need to test
			note.CreateParent(this);
			RefreshTabs ();
		}
		public override void MoveNote (string GUIDOfNoteToMove, string GUIDOfLayoutToMoveItTo)
		{
			// Take 1 was using LayoutDatabase

			// Take 2 - just manipulating the note arrays here and then saving?

			//  a. find note we want to move
			NoteDataXML_Panel newPanel = null;
			NoteDataInterface movingNote = null;

			foreach (NoteDataInterface note in Notes.GetNotes ()) {
				if (note.GuidForNote == GUIDOfLayoutToMoveItTo && note.IsPanel == true) {
					newPanel = (NoteDataXML_Panel)note;
				}
				if (note.GuidForNote == GUIDOfNoteToMove) {
					movingNote = note;

				}
			}
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

					
					}
				}
			} else {

				if (newPanel == null || movingNote == null) {
					NewMessage.Show (Loc.Instance.Cat.GetString ("Destination or note was null. No move"));
					return;
				}

				// remove note
			
				Notes.RemoveNote (movingNote);
				// add note
				newPanel.AddNote (movingNote);
			}
			try {
				movingNote.Location = new Point (0, 0);
			} catch (Exception) {
				lg.Instance.Line("LayoutPanel.MoveNote", ProblemType.WARNING, "problem setting location of note. Ok if this is unit testing");
			}
			SaveLayout ();
			// must save before calling this
			if (newPanel != null) {
				newPanel.Update (this);
			}

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


		public override void SetSaveRequired (bool NeedSave)
		{
			_saverequired = NeedSave;
			if (SetSubNoteSaveRequired != null) {
				SetSubNoteSaveRequired(NeedSave);
			}
		}
	}
}

