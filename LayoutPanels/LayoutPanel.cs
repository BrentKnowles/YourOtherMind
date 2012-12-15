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

		TextBox text = null;

		#endregion

		protected LayoutInterface Notes = null;
		#region gui
		private Panel noteCanvas;
		override public Panel NoteCanvas {
			get { return noteCanvas;}
			set { noteCanvas = value;}
		}



		ToolStrip tabsBar = null;
		ToolStrip headerBar = null;

		#endregion
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
		public void HeaderToolbar ()
		{
			headerBar = new ToolStrip();
			headerBar.Parent = this;
			headerBar.Dock = DockStyle.Top;

		}


		public void TabsBar()
		{
			tabsBar = new ToolStrip();
			tabsBar.Parent = this;
			tabsBar.Dock = DockStyle.Top;

			// RefreshTabs(); don't need to call it until after load
		}
		public void LayoutToolbar ()
		{
			ToolStrip bar = new ToolStrip ();
			bar.Parent = this;
			bar.Visible = true;
			bar.Dock = DockStyle.Top;

			ToolStripButton addNote = new ToolStripButton ("Add a Layout (remove me)");
			addNote.Click += HandleAddClick;
			bar.Items.Add (addNote);
			
			
			
			ToolStripDropDownButton AddNote = new ToolStripDropDownButton("Add Note");
			AddNote.DropDownOpening += HandleAddNoteDropDownOpening;
			bar.Items.Add (AddNote);
			
			
			ToolStripButton LoadLayout = new ToolStripButton("Load Layout");
			LoadLayout.Click +=	LoadLayoutClick;
			bar.Items.Add (LoadLayout);
		}

		public LayoutPanel (string GUID)
		{
			ParentGUID = GUID;



			//Type[] typeList = LayoutDetails.Instance.TypeList;


			LayoutDetails.Instance.AddToList(typeof(NoteDataXML_Panel), new NoteDataXML_Panel().RegisterType());

			/*Type[] newTypeList = new Type[typeList.Length + 1];

			for (int i = 0; i < typeList.Length; i++)
			{
				newTypeList[i] = typeList[i];
			}

			newTypeList[newTypeList.Length-1] = typeof(NoteDataXML_Panel);
			LayoutDetails.Instance.TypeList = newTypeList;
*/


			this.BackColor = Color.Pink;
			if (!GetIsChild) BuildFormatToolbar();
			TabsBar ();
			LayoutToolbar();
			if (!GetIsChild) HeaderToolbar();






			text = new TextBox();
			text.Parent = this;
			text.Visible = true;
			text.Dock = DockStyle.Bottom;









			Label label = new Label();
			label.Text = "Remember to save";
			label.Parent = this;
			label.Dock = DockStyle.Bottom;


			NoteCanvas = new Panel();
			NoteCanvas.Dock = DockStyle.Fill;
			NoteCanvas.Parent = this;
			NoteCanvas.BringToFront();
			this.AutoScroll = true;

			/*TODO:
			Thoughts: Does the NoteData actually create the Panel (i.e., flip the ownership around)
			 The NoteData will never REFERENCE the actual GUI elements however.
			 Also play with the Unit test example that StackOverflow had, this seems interest
			 */
		}
		public override string Backup ()
		{
			if (Notes == null) {
				NewMessage.Show (Loc.Instance.Cat.GetString("Please load a layout first"));
				return "";
			} else {
				return Notes.Backup();
			}
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

	
	





		void LoadLayoutClick (object sender, EventArgs e)
		{

				LoadLayout (this.text.Text);

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
		private void UpdateHeader ()
		{
			if (GetIsChild == false) {
				ToolStripLabel bold = new ToolStripLabel ();
				bold.Text = Notes.Name;
				headerBar.Items.Add (bold);
			}
		}
		public override void LoadLayout (string _GUID)
		{


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
				UpdateHeader();
				UpdateListOfNotes ();
			//	NewMessage.Show (String.Format ("Name={0}, Status={1}", Notes.Name, Notes.Status));
			}

			NoteCanvas.AutoScroll = true;
			RefreshTabs();
		}
		/// <summary>
		/// Refreshs the tabs.
		/// </summary>
		public void RefreshTabs ()
		{
			Console.WriteLine (">>> refresh tabs <<<");
			if (Notes.ShowTabs == true) {
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
				tabsBar.Visible = false;
			}
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
				if (Constants.BLANK == GUID)
					GUID = System.Guid.NewGuid ().ToString ();
				Notes = new LayoutDatabase (GUID);
				//	Notes = new LayoutDatabase("system");
				NoteDataXML newNote = new NoteDataXML();
				AddNote (newNote);
				
			}
		}

		void HandleAddClick (object sender, EventArgs e)
		{
			// creates a new LAYOUT


			// if textbox is blank the GUID is generated autoamtically
			//GUID = this.text.Text;
			NewLayout (this.text.Text);

			// Remove saving, make user do itNotes.SaveTo();

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

