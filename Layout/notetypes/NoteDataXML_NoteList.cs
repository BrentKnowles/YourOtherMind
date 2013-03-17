using System;
using System.Windows.Forms;
using CoreUtilities;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace Layout
{
	/// <summary>
	/// Note data XM l_ note list.
	/// 
	/// This will be both:
	/// 
	/// 1. The list of Notes on a Layout
	/// 2. The list of Layouts
	/// 
	/// Shared filtering between them.
	/// </summary>
	public class NoteDataXML_NoteList : NoteDataXML
	{
		#region constants
		public enum Modes  {NOTES, LAYOUTS};

		public override int defaultHeight { get { return 600; } }
		public override int defaultWidth { get { return 200; } }

		#endregion
		#region variables

		#endregion

		#region variables_saved_in_xml
		Modes _mode;
		/// <summary>
		/// Gets or sets the mode. (Showing the list of notes or the list of layouts)
		/// </summary>
		/// <value>
		/// The mode.
		/// </value>
		public Modes Mode {
			get { return _mode;}
			set { _mode = value;}
		}

		// this is the INDEX (name column) into the system table that contains the queries
		// it will then lookup the appropriate query as required
		private string currentFilterName = Constants.BLANK;

		public string CurrentFilterName {
			get {
				return currentFilterName;
			}
			set {
				currentFilterName = value;
			}
		}

		#endregion

		#region formcontrols
		ListBox list;
		Label count;
		Label blurb;
		TextBox TextEditor = null;
		ComboBox  CurrentFilterDropDown  = null;
		CheckBox FullTextSearch = null;
		Panel SearchDetails = null;
		ComboBox mode = null;
		#endregion

		public NoteDataXML_NoteList () : base()
		{
			Caption = Loc.Instance.Cat.GetString("Note List");
			_mode = Modes.NOTES;
		}
		public NoteDataXML_NoteList(int height, int width) : base(height, width)
		{
			Caption = Loc.Instance.Cat.GetString("Note List");
			_mode = Modes.NOTES;
		}

		protected override void DoBuildChildren (LayoutPanelBase Layout)
		{
			base.DoBuildChildren (Layout);
			CaptionLabel.Dock = DockStyle.Top;

			 mode = new ComboBox ();
			mode.Parent = ParentNotePanel;
			mode.DropDownStyle = ComboBoxStyle.DropDownList;
			mode.Dock = DockStyle.Top;
			mode.BringToFront ();

			mode.Items.Add (Loc.Instance.GetString ("Notes on This Layout"));
			mode.Items.Add (Loc.Instance.GetString ("All Layouts"));
			mode.Items.Add (Loc.Instance.GetString ("Notes on Current Layout"));


			mode.SelectedIndexChanged += HandleDropDownSelectedIndexChanged;
		

			SearchDetails = new Panel();
			SearchDetails.Dock = DockStyle.Bottom;


			CurrentFilterDropDown = new ComboBox();
			CurrentFilterDropDown.Dock = DockStyle.Bottom;

			// because we need the tables to be loaded we CANNOT load this data now
			LayoutDetails.Instance.UpdateAfterLoadList.Add (this);


		



			TextEditor = new TextBox();
			TextEditor.Dock = DockStyle.Bottom;
		
			FullTextSearch = new CheckBox();
			FullTextSearch.Checked = false;
			FullTextSearch.Text = Loc.Instance.GetString ("Search All Text Fields");
			FullTextSearch.Dock = DockStyle.Bottom;

			SearchDetails.Controls.Add (TextEditor);
			SearchDetails.Controls.Add (CurrentFilterDropDown);
			SearchDetails.Controls.Add (FullTextSearch);

			list = new ListBox();
			//list.SelectedIndexChanged += HandleDropDownSelectedIndexChanged;
			list.Parent = ParentNotePanel;
			list.Width = 125;
			list.Dock = DockStyle.Fill;
			list.BringToFront();
			list.BindingContextChanged+= HandleBindingContextChanged;
			list.DoubleClick += HandleListBoxDoubleClick;
			list.Click+= HandleNoteListClick;

			ParentNotePanel.Controls.Add (SearchDetails);

			count = new Label();
			count.Parent = ParentNotePanel;
			count.Dock = DockStyle.Bottom;
			
			
			blurb = new Label();
			blurb.Parent = ParentNotePanel;
			blurb.Dock = DockStyle.Bottom;


			Button refresh = new Button();
			refresh.Text = Loc.Instance.GetString("Refresh");
			refresh.Dock = DockStyle.Bottom;
			refresh.Parent = ParentNotePanel;
			refresh.Click += HandleRefreshClick;



	

		//	AdjustHeightOfLayoutSearchPanel (); This already gets called when the note type is chosen
		}
		public override void UpdateAfterLoad ()
		{
			System.Collections.Generic.List<string> queries = new List<string> ();
			//how and when to load this 
			// because we need the tables to be loaded we CANNOT load this data now
			if (LayoutDetails.Instance.TableLayout != null) {

				queries = LayoutDetails.Instance.TableLayout.GetListOfStringsFromSystemTable (LayoutDetails.SYSTEM_QUERIES, 1);
			} else {
				// first load The SYstem Table won't be ready so we grab it directly FROM US
				queries = Layout.GetListOfStringsFromSystemTable(LayoutDetails.SYSTEM_QUERIES, 1);
				//Layout.GetNoteOnSameLayout(LayoutDetails.SYSTEM_QUERIES, false);
			}
			queries.Sort ();
			CurrentFilterDropDown.DropDownStyle = ComboBoxStyle.DropDownList;
			foreach (string s in queries) {
				CurrentFilterDropDown.Items.Add (s);
			}
			int lastQueryIndex =queries.IndexOf(CurrentFilterName);// queries.Find(s=>s==CurrentFilter);
			CurrentFilterDropDown.SelectedIndex = lastQueryIndex;
			CurrentFilterDropDown.SelectedIndexChanged+= HandleSelectedIndexLastQueryChanged;


			// All other elements need to be made before this code
			switch (Mode) {
			case Modes.NOTES: mode.SelectedIndex = 0; break;
			case Modes.LAYOUTS: mode.SelectedIndex = 1; break;
			}
		}
		void HandleSelectedIndexLastQueryChanged (object sender, EventArgs e)
		{

			SetSaveRequired(true);
			if ((sender as ComboBox).SelectedItem != null && (sender as ComboBox).SelectedItem.ToString () != Constants.BLANK) {
				//NewMessage.Show ("Set to " + (sender as ComboBox).SelectedItem.ToString ());
				CurrentFilterName = (sender as ComboBox).SelectedItem.ToString ();
				UpdateLists ();
			} else {
				//NewMessage.Show ("Did not set");
			}
		}
		void HandleNoteListClick (object sender, EventArgs e)
		{
			switch (Mode) {
			case Modes.NOTES: blurb.Text ="";break;
			case Modes.LAYOUTS: 
				if (list.SelectedItem != null)
				{
					MasterOfLayouts.NameAndGuid record = (MasterOfLayouts.NameAndGuid)this.list.SelectedItem;
					blurb.Text =(record.Blurb);
				}
				break;
			}
		}

		void HandleBindingContextChanged (object sender, EventArgs e)
		{
			count.Text = Loc.Instance.GetStringFmt("Count = {0}", this.list.Items.Count);
		}

	


		public void Refresh()
		{
			if (Modes.NOTES == _mode) {
				UpdateListOfNotesOnLayout ();
			} else
			if (Modes.LAYOUTS == _mode) {
				UpdateListOfLayouts();
				
			}
		}

		/// <summary>
		/// Redraws the lists
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleRefreshClick (object sender, EventArgs e)
		{
			Refresh ();
		}
		/// <summary>
		/// Handles the list box double click.
		/// 
		/// Will go to the note (if in OnLayout mode) or open a new note
		/// 
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleListBoxDoubleClick (object sender, EventArgs e)
		{

			if (Modes.NOTES == _mode) {


					NoteDataInterface note =  (NoteDataInterface)this.list.SelectedItem;

				if (note != null)
				{

					Layout.GoToNote(note);
				}
				else
				{
					lg.Instance.Line ("LayoutPanel->HandleTabButtonClick", ProblemType.WARNING, String.Format ("Note with guid = {0} not found",note.GuidForNote));
				}

			} else
				if (Modes.LAYOUTS == _mode) {
				if (this.list.SelectedItem != null)
				{
					MasterOfLayouts.NameAndGuid record = (MasterOfLayouts.NameAndGuid)this.list.SelectedItem;
				LayoutDetails.Instance.LoadLayout(record.Guid);
				}

			}
		}

		void UpdateListOfLayouts ()
		{
			this.list.DataSource = null;
			this.list.Items.Clear ();
		
//			MasterOfLayouts master = new MasterOfLayouts ();
//			this.list.DataSource = master.GetListOfLayouts ("filternotdone");
			//DataView view = new DataView(MasterOfLayouts.GetListOfLayouts (CurrentFilterName));

			// if we are the system note we need to override this
			// to use ourself because the table lookup is not yet loaded
			LayoutPanelBase OverridePanelToUse = null;//LayoutDetails.Instance.TableLayout;
			if (LayoutDetails.Instance.TableLayout == null)
			{
				OverridePanelToUse = Layout;
			}

			List<MasterOfLayouts.NameAndGuid> ListOfItems = MasterOfLayouts.GetListOfLayouts (CurrentFilterName, TextEditor.Text, FullTextSearch.Checked, OverridePanelToUse);
//			for (int i = ListOfItems.Count - 1; i >= 0; i--)
//			{
//				if (TextEditor.Text != Constants.BLANK)
//				{
//					if (ListOfItems[i].Caption.IndexOf(TextEditor.Text) > -1)
//					{
//
//					}
//					else
//					{
//						ListOfItems.Remove (ListOfItems[i]);
//					}
//				}
//			}
			this.list.DataSource = ListOfItems;

			this.list.DisplayMember = "Caption";
			this.list.ValueMember = "Guid";


			///master.Dispose ();

			count.Text = Loc.Instance.GetStringFmt("Count = {0}", this.list.Items.Count);
		}

		void UpdateLists ()
		{
			switch (_mode) {
			case Modes.LAYOUTS:

				FullTextSearch.Visible = true;
				CurrentFilterDropDown.Visible = true;

				UpdateListOfLayouts ();

				break;
			case Modes.NOTES: 

				
				FullTextSearch.Visible = false;
				CurrentFilterDropDown.Visible = false;

				UpdateListOfNotesOnLayout ();

				break;

			}
		
		}

		void HandleDropDownSelectedIndexChanged (object sender, EventArgs e)
		{
			if ((sender as ComboBox).SelectedIndex == 0) {
				// just the notes in this layout
				_mode = Modes.NOTES;

			} else
			if ((sender as ComboBox).SelectedIndex == 1) {
				// all notes
				_mode = Modes.LAYOUTS;




			}
		
			UpdateLists ();
			AdjustHeightOfLayoutSearchPanel();
		}

		void AdjustHeightOfLayoutSearchPanel ()
		{
			//SearchDetails.BringToFront();
			int totalheight = 0;
			foreach (Control control in SearchDetails.Controls)
			{
				if (control.Visible == true)
				{
					totalheight = totalheight + control.Height ;
				}
			}
			SearchDetails.Height = totalheight;
		}

		public void UpdateListOfNotesOnLayout ()
		{
			// if find one then we call its update function
			if (_mode == Modes.NOTES) {
				this.list.DataSource = null;
				this.list.Items.Clear ();



				ArrayList Notes = Layout.GetAllNotes();
				for (int i = Notes.Count - 1; i >= 0; i--)
				{
					if (TextEditor.Text != Constants.BLANK)
					{
						if ( ((NoteDataInterface)Notes[i]).Caption.IndexOf(TextEditor.Text) > -1)
						{
							
						}
						else
						{
							Notes.Remove (Notes[i]);
						}
					}
				}


				this.list.Sorted = true;
				this.list.DataSource = Notes;
				this.list.DisplayMember = "Caption";
				this.list.ValueMember = "GuidForNote";
			} else {

			}

			count.Text = Loc.Instance.GetStringFmt("Count = {0}", this.list.Items.Count);
		}

		/// <summary>
		/// Registers the type.
		/// </summary>
		public override string RegisterType()
		{
			return Loc.Instance.Cat.GetString("List");
		}
		
	
	}

}

