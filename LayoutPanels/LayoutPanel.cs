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
		ListBox list = null;
		PropertyGrid grid = null;
		#endregion

		LayoutInterface Notes = null;
		#region gui
		private Panel noteCanvas;
		override public Panel NoteCanvas {
			get { return noteCanvas;}
			set { noteCanvas = value;}
		}

		#endregion
		public LayoutPanel ()
		{


			//TO DO  Adding a Dyanmic Type: THIS SHOULD ONLY HAPPEN ONCE. For this I could use a variable on the singleton but for
			// others, it has to happen as part of the Plugin registration??
			Type[] typeList = LayoutDetails.Instance.TypeList;
			
			Type[] newTypeList = new Type[typeList.Length + 1];

			for (int i = 0; i < typeList.Length; i++)
			{
				newTypeList[i] = typeList[i];
			}

			newTypeList[newTypeList.Length-1] = typeof(NoteDataXML_Panel);
			LayoutDetails.Instance.TypeList = newTypeList;



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



			ToolStripButton EditSystem = new ToolStripButton("Add a Note to CURRENT Layout");
			EditSystem.Click += HandleEditNoteClick;
			bar.Items.Add (EditSystem);

			ToolStripButton LoadLayout = new ToolStripButton("Load Layout");
			LoadLayout.Click +=	LoadLayoutClick;
			bar.Items.Add (LoadLayout);

			ToolStripButton AddText = new ToolStripButton("Add TExtbox");
			AddText.Click +=	TextBoxClick;
			bar.Items.Add (AddText);


			
			ToolStripButton AddPanel = new ToolStripButton("Add Panel");
			AddPanel.Click +=	PanelBoxClick;
			bar.Items.Add (AddPanel);

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

		void HandlePropertyValueChanged (object s, PropertyValueChangedEventArgs e)
		{

			((NoteDataXML)grid.SelectedObject).UpdateLocation();
			((NoteDataXML)grid.SelectedObject).Update(this);
			// when list is updated we are no longer the selected object
			UpdateListOfNotes();

		}
		void TextBoxClick(object sender, EventArgs e)
		{
			NoteDataXML_RichText xml = new NoteDataXML_RichText ();

			Notes.Add (xml);
			
			xml.CreateParent (this);
			
			UpdateListOfNotes ();
		}

		void PanelBoxClick(object sender, EventArgs e)
		{
			NoteDataXML_Panel xml = new NoteDataXML_Panel ();
			
			Notes.Add (xml);
			
			xml.CreateParent (this);
			
			UpdateListOfNotes ();
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

		public override void SaveLayout ()
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


		private void UpdateListOfNotes ()
		{
			this.list.DataSource = null;
			this.list.Items.Clear ();
			this.list.DataSource = Notes.GetNotes();
			this.list.DisplayMember = "Caption";
			this.list.ValueMember = "GuidForNote";
		}
		public override void LoadLayout (string GUID)
		{
			// disable autoscroll because if an object is loaded off-screen before others it changes the centering of every object
			NoteCanvas.AutoScroll = false;

			Notes = new LayoutDatabase (GUID);
			if (Notes.LoadFrom (this) == false) {
				lg.Instance.Line("LayoutPanel.LoadLayout", ProblemType.MESSAGE, "This note is blank still.");
				//Notes = null;
				//NewMessage.Show ("That note does not exist");
			} else {
				UpdateListOfNotes ();
			//	NewMessage.Show (String.Format ("Name={0}, Status={1}", Notes.Name, Notes.Status));
			}

			NoteCanvas.AutoScroll = true;
		}

		public override void AddNote ()
		{
			NoteDataXML xml = new NoteDataXML ();

			Notes.Add (xml);
			
			xml.CreateParent (this);

			UpdateListOfNotes ();
		}


		void HandleAddClick (object sender, EventArgs e)
		{
			// creates a new LAYOUT


			// if textbox is blank the GUID is generated autoamtically
			GUID = this.text.Text;
			// check to see if exists already
			if (Notes != null && Notes.Exists (GUID)) {
				NewMessage.Show("that layout exists already");
			} else {
				if (Constants.BLANK == GUID)
					GUID = System.Guid.NewGuid ().ToString ();
				Notes = new LayoutDatabase (GUID);
				//	Notes = new LayoutDatabase("system");
			
				AddNote ();

			}
			// Remove saving, make user do itNotes.SaveTo();

		}
	}
}

