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




			//Type[] typeList = LayoutDetails.Instance.TypeList;
			LayoutDetails.Instance.AddToList(typeof(NoteDataXML_Panel), "Panel");

			/*Type[] newTypeList = new Type[typeList.Length + 1];

			for (int i = 0; i < typeList.Length; i++)
			{
				newTypeList[i] = typeList[i];
			}

			newTypeList[newTypeList.Length-1] = typeof(NoteDataXML_Panel);
			LayoutDetails.Instance.TypeList = newTypeList;
*/


			this.BackColor = Color.Pink;


			ToolStrip bar = new ToolStrip ();
			bar.Parent = this;
			bar.Visible = true;
			bar.Dock = DockStyle.Top;


			text = new TextBox();
			text.Parent = this;
			text.Visible = true;
			text.Dock = DockStyle.Bottom;

			ToolStripButton addNote = new ToolStripButton ("Add a Layout (remove me)");
						addNote.Click += HandleAddClick;
						bar.Items.Add (addNote);



			ToolStripDropDownButton AddNote = new ToolStripDropDownButton("Add Note");
			AddNote.DropDownOpening += HandleAddNoteDropDownOpening;
			bar.Items.Add (AddNote);
		

			ToolStripButton LoadLayout = new ToolStripButton("Load Layout");
			LoadLayout.Click +=	LoadLayoutClick;
			bar.Items.Add (LoadLayout);





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
				}
				else
				{
					lg.Instance.Line("LayoutPanel.HandleAddNoteClick", ProblemType.ERROR, String.Format ("{0} Type not found", TypeTest.ToString()));
				}
			}
		}

		void HandlePropertyValueChanged (object s, PropertyValueChangedEventArgs e)
		{

			((NoteDataXML)grid.SelectedObject).UpdateLocation();
			((NoteDataXML)grid.SelectedObject).Update(this);
			// when list is updated we are no longer the selected object
			UpdateListOfNotes();

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


			Notes.UpdateListOfNotes();
		
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
		public override List<NoteDataInterface> GetAvailableFolders ()
		{
			return Notes.GetAvailableFolders();

		}

		public override void MoveNote (string GUIDOfNoteToMove,  string GUIDOfLayoutToMoveItTo)
		{
			Notes.MoveNote(GUIDOfNoteToMove, GUIDOfLayoutToMoveItTo);
		}
	}
}

