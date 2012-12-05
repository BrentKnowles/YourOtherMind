using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

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
	public class LayoutPanel : Panel, LayoutPanelInterface
	{
		#region TEMPVARIABLES

		TextBox text = null;
		ListBox list = null;
		PropertyGrid grid = null;
		#endregion

		#region variables
		private string _guid = Constants.BLANK;
		private bool _saverequired = false;
		// storing reference to Interface, allowing a data swapout later
		LayoutInterface Notes = null;
		/// <summary>
		/// The GUID associated with this LayoutPanel. If blank there is no Layout loaded.
		/// </summary>
		/// <value>
		/// The GUI.
		/// </value>
		public string GUID {
			get { return _guid;}
			set { _guid = value;}
		}
		/// <summary>
		/// If true this layout needs to be saved
		/// </summary>
		/// <value>
		/// <c>true</c> if saved required; otherwise, <c>false</c>.
		/// </value>
		public bool SaveRequired {
			get{ return _saverequired;}
			set { _saverequired = value;}
		}
		#endregion
		public LayoutPanel ()
		{
			this.BackColor = Color.Pink;

			ToolStrip bar = new ToolStrip ();
			bar.Parent = this;
			bar.Visible = true;
			bar.Dock = DockStyle.Top;


			text = new TextBox();
			text.Parent = this;
			text.Visible = true;
			text.Dock = DockStyle.Bottom;

			ToolStripButton addNote = new ToolStripButton ("Add a Layout");
						addNote.Click += HandleAddClick;
						bar.Items.Add (addNote);

			ToolStripButton showNotes = new ToolStripButton("SaveLayout");
			showNotes.Click += SaveNoteClick;
			bar.Items.Add (showNotes);


			ToolStripButton EditSystem = new ToolStripButton("Add a Note to CURRENT Layout");
			EditSystem.Click += HandleEditNoteClick;
			bar.Items.Add (EditSystem);

			ToolStripButton LoadLayout = new ToolStripButton("Load Layout");
			LoadLayout.Click +=	LoadLayoutClick;
			bar.Items.Add (LoadLayout);


			list = new ListBox();
			list.SelectedIndexChanged += HandleSelectedIndexChanged;
			list.Parent = this;
			list.Width = 125;
			list.Dock = DockStyle.Left;

			grid = new PropertyGrid();
			grid.Parent = this;
			grid.Width = 125;
			grid.Dock = DockStyle.Right;
			grid.PropertyValueChanged += HandlePropertyValueChanged;

			Label label = new Label();
			label.Text = "Remember to save";
			label.Parent = this;
			label.Dock = DockStyle.Bottom;


			/*TODO:
			Thoughts: Does the NoteData actually create the Panel (i.e., flip the ownership around)
			 The NoteData will never REFERENCE the actual GUI elements however.
			 Also play with the Unit test example that StackOverflow had, this seems interest
			 */
		}

		void HandlePropertyValueChanged (object s, PropertyValueChangedEventArgs e)
		{

			((NoteDataXML)grid.SelectedObject).UpdateLocation();
			// when list is updated we are no longer the selected object
			UpdateListOfNotes();

		}

		void HandleSelectedIndexChanged (object sender, EventArgs e)
		{
			grid.SelectedObject = (NoteDataXML)list.SelectedItem;
		}

		void LoadLayoutClick (object sender, EventArgs e)
		{
			GUID = this.text.Text;
			if (Constants.BLANK == GUID) {NewMessage.Show ("You must specify a layout to load"); return;}
			LoadLayout (GUID);

		}

		void HandleEditNoteClick (object sender, EventArgs e)
		{
			// Modify the informatin in the SYSTEM NOTE
			if (Notes == null) {
				NewMessage.Show ("Load or create a note first");
				return;
			}

			AddNote ();

		}

		public void SaveLayout ()
		{
			if (null != Notes) {
				Notes.SaveTo();
				/*
				foreach (NoteDataInterface data in Notes.GetNotes()) {
					NewMessage.Show (data.Caption);
					Console.WriteLine (((NoteDataXML)data).JustXMLONLYTest);
					//data.CreateParent(); This is bad design though: use the itnerface.
				}*/
			}
			else
			{
				lg.Instance.Line("LayoutPanel.SaveNoteClick", ProblemType.MESSAGE,"No notes loaded");
			}
		}

		void SaveNoteClick (object sender, EventArgs e)
		{
			SaveLayout ();

		}
		private void UpdateListOfNotes ()
		{
			this.list.DataSource = null;
			this.list.Items.Clear ();
			this.list.DataSource = Notes.GetNotes();
			this.list.DisplayMember = "Caption";
			this.list.ValueMember = "GuidForNote";
		}
		public void LoadLayout (string GUID)
		{
			Notes = new LayoutDatabase(GUID);
			Notes.LoadFrom (this);
			UpdateListOfNotes();
	

		}

		public void AddNote ()
		{
			NoteDataXML xml = new NoteDataXML ();
			xml.Caption = "Blank Note";
			xml.GuidForNote = System.Guid.NewGuid().ToString();
			Notes.Add (xml);
			
			xml.CreateParent (this);

			UpdateListOfNotes ();
		}


		void HandleAddClick (object sender, EventArgs e)
		{
			// creates a new LAYOUT


			// if textbox is blank the GUID is generated autoamtically
			GUID = this.text.Text;
			if (Constants.BLANK == GUID) GUID = System.Guid.NewGuid().ToString();
			Notes = new LayoutDatabase(GUID);
		//	Notes = new LayoutDatabase("system");
			
			AddNote ();


			// Remove saving, make user do itNotes.SaveTo();

		}
	}
}

