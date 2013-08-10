// NoteDataXML_NoteList.cs
//
// Copyright (c) 2013 Brent Knowles (http://www.brentknowles.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Review documentation at http://www.yourothermind.com for updated implementation notes, license updates
// or other general information/
// 
// Author information available at http://www.brentknowles.com or http://www.amazon.com/Brent-Knowles/e/B0035WW7OW
// Full source code: https://github.com/BrentKnowles/YourOtherMind
//###
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
		public enum Modes  {NOTES, LAYOUTS, LAYOUTSONCURRENTLAYOUT};

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

		List<string> history = new List<string>();
		// Limited list of history items
		public List<string> History {
			get {
				return history;
			}
			set {
				history = value;
			}
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
		ComboBox TextEditor = null;
		ComboBox  CurrentFilterDropDown  = null;
		CheckBox FullTextSearch = null;
		Panel SearchDetails = null;
		ComboBox mode = null;
		Button  refresh = null;
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
		

			SearchDetails = new Panel ();
			SearchDetails.Dock = DockStyle.Bottom;


			CurrentFilterDropDown = new ComboBox ();
			CurrentFilterDropDown.Dock = DockStyle.Bottom;

			// because we need the tables to be loaded we CANNOT load this data now
			LayoutDetails.Instance.UpdateAfterLoadList.Add (this);


		



			TextEditor = new ComboBox ();
			TextEditor.Dock = DockStyle.Bottom;
			TextEditor.KeyPress += HandleTextEditKeyPress;
			TextEditor.KeyUp += HandleKeyUp;

			// Do some cleanup on history item to keep the list reasonable.
			// This happens only on load to keep things simple
			if (History.Count > 10) {
				History.RemoveRange(9, (History.Count)-9);
			}


			foreach (string s in History) {
				TextEditor.Items.Add (s);
			}


			TextEditor.SelectedIndexChanged+= (object sender, EventArgs e) => 	Refresh ();;
		
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


			refresh = new Button();
			refresh.Text = Loc.Instance.GetString("Refresh");
			refresh.Dock = DockStyle.Bottom;
			refresh.Parent = ParentNotePanel;
			refresh.Click += HandleRefreshClick;



	

		//	AdjustHeightOfLayoutSearchPanel (); This already gets called when the note type is chosen
		}
	
		// must be called in the Refresh Method
		void StoreHistoryText ()
		{

			string Value = TextEditor.Text;
			if (Value != Constants.BLANK) {

				int CurrentIndex = TextEditor.Items.IndexOf (Value);
				if (CurrentIndex == -1)
					TextEditor.Items.Insert (0, (Value));
				else
				{
					// August 2013
					// Remove the old entry and add the new entry
					// this will mean that entries we go back to often, will bump back to the 
					// top of the list (and stay on it)
					TextEditor.Items.Remove (Value);
					TextEditor.Items.Insert (0, (Value));
				}
				SetSaveRequired(true);
			}
			// we do not add if the text is already there

		}

		void HandleTextEditKeyPress (object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter) {
				Refresh ();
				LayoutDetails.SupressBeep (e);

			}
		}

		void HandleKeyUp (object sender, KeyEventArgs e)
		{
//			if (e.KeyCode == Keys.Enter) {
//
//			
//			}
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
			case Modes.LAYOUTSONCURRENTLAYOUT: mode.SelectedIndex = 2; break;
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
			case Modes.LAYOUTSONCURRENTLAYOUT:
				//NewMessage.Show ("notes on other click");
				break;
			}
		}

		void HandleBindingContextChanged (object sender, EventArgs e)
		{
			count.Text = Loc.Instance.GetStringFmt("Count = {0}", this.list.Items.Count);
		}

	
	


		public void Refresh ()
		{
			if (Modes.NOTES == _mode) {
				UpdateListOfNotesOnLayout ();
			} else
			if (Modes.LAYOUTS == _mode) {
				UpdateListOfLayouts (CurrentFilterName);
				
			} else
				if (Modes.LAYOUTSONCURRENTLAYOUT == _mode) {
				UpdateListOfNotesFromCurrentLayout();
			}

			StoreHistoryText ();
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
			if (this.list.SelectedItem != null) {


				if (Modes.NOTES == _mode) {


					NoteDataInterface note = (NoteDataInterface)this.list.SelectedItem;

					if (note != null) {

						Layout.GoToNote (note);
					} else {
						lg.Instance.Line ("LayoutPanel->HandleTabButtonClick", ProblemType.WARNING, String.Format ("Note with guid = {0} not found", note.GuidForNote));
					}

				} else
				if (Modes.LAYOUTS == _mode) {

					MasterOfLayouts.NameAndGuid record = (MasterOfLayouts.NameAndGuid)this.list.SelectedItem;
					LayoutDetails.Instance.LoadLayout (record.Guid);
				} else
					if (Modes.LAYOUTSONCURRENTLAYOUT == _mode) {
					if (LayoutDetails.Instance.CurrentLayout != null) {
						NoteDataInterface note = (NoteDataInterface)this.list.SelectedItem;
						if (note != null) {
							
							LayoutDetails.Instance.CurrentLayout.GoToNote (note);
						} else {
							lg.Instance.Line ("LayoutPanel->HandleTabButtonClick", ProblemType.WARNING, String.Format ("Note with guid = {0} not found", note.GuidForNote));
						}

					}
				
				}

			}
		}

		void UpdateListOfLayouts (string _CurrentFilter)
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

			List<MasterOfLayouts.NameAndGuid> ListOfItems = MasterOfLayouts.GetListOfLayouts (_CurrentFilter, TextEditor.Text, FullTextSearch.Checked, OverridePanelToUse);
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

			list.BeginUpdate();
			ParentNotePanel.Cursor = Cursors.WaitCursor;

			switch (_mode) {
			case Modes.LAYOUTS:

				FullTextSearch.Visible = true;
				CurrentFilterDropDown.Visible = true;

				UpdateListOfLayouts (CurrentFilterName);

				break;
			case Modes.NOTES: 

				
				FullTextSearch.Visible = false;
				CurrentFilterDropDown.Visible = false;

				UpdateListOfNotesOnLayout ();

				break;
			case Modes.LAYOUTSONCURRENTLAYOUT:

				FullTextSearch.Visible = false;
				CurrentFilterDropDown.Visible = false;
				UpdateListOfNotesFromCurrentLayout();
				break;
			}
			list.EndUpdate();
			ParentNotePanel.Cursor = Cursors.Default;
		
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
			else if ((sender as ComboBox).SelectedIndex == 2)
			{
				_mode = Modes.LAYOUTSONCURRENTLAYOUT;
			}
			// blank the search text when switching modes. Just confusing otherwise. May 2013
			TextEditor.Text = Constants.BLANK;
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
		void UpdateListOfNotesFromCurrentLayout ()
		{
			if (LayoutDetails.Instance.CurrentLayout != null) {
				ArrayList Notes = LayoutDetails.Instance.CurrentLayout.GetAllNotes ();
			
				BuildListOfNotesOnALayout (Notes);
			}
		}

		private void BuildListOfNotesOnALayout (ArrayList Notes)
		{
			this.list.DataSource = null;
			this.list.Items.Clear ();
			
			string textToLookFor = TextEditor.Text.ToLower();
			for (int i = Notes.Count - 1; i >= 0; i--) {
				if (TextEditor.Text != Constants.BLANK) {
					if (((NoteDataInterface)Notes [i]).Caption.ToLower ().IndexOf (textToLookFor) > -1) {
						// keeping
					} else {
						Notes.Remove (Notes [i]);
					}
				}
			}

			if (Notes.Count > 0) {
			
				this.list.Sorted = true;
				this.list.DataSource = Notes;
				this.list.DisplayMember = "Caption";
				try {
					this.list.ValueMember = "GuidForNote";
				} catch (Exception ex) {
					NewMessage.Show (ex.ToString ());
				}
			}
			count.Text = Loc.Instance.GetStringFmt("Count = {0}", this.list.Items.Count);
		}
		public void UpdateListOfNotesOnLayout ()
		{
			// if find one then we call its update function
			if (_mode == Modes.NOTES) {

				ArrayList Notes = Layout.GetAllNotes();

				BuildListOfNotesOnALayout(Notes);
			} else {

			}

		
		}

		/// <summary>
		/// Registers the type.
		/// </summary>
		public override string RegisterType()
		{
			return Loc.Instance.Cat.GetString("List");
		}
		public NoteDataXML_NoteList(NoteDataInterface Note) : base(Note)
		{
			
		}
		public override void CopyNote (NoteDataInterface Note)
		{
			base.CopyNote (Note);
		}	

		public void FilterByKeyword (string text)
		{
			mode.SelectedIndex = 1;
			string tempfilter=String.Format("CODE_KEYWORD_AUTO,{0}", text);
			UpdateListOfLayouts(tempfilter);
		}
		public override void Save ()
		{

			History = new List<string>();
			foreach (string s in TextEditor.Items) {
				History.Add (s);
			}
		

			base.Save ();
		
		}

		protected override AppearanceClass UpdateAppearance ()
		{
			AppearanceClass app = base.UpdateAppearance ();
			if (null != app) {
				ParentNotePanel.BackColor = app.mainBackground;
				count.ForeColor = app.secondaryForeground;
				blurb.ForeColor = app.secondaryForeground;
				refresh.ForeColor = app.secondaryForeground;
				FullTextSearch.ForeColor = app.secondaryForeground;


				count.Font = app.captionFont;
				blurb.Font = app.captionFont;
				refresh.Font = app.captionFont;
				FullTextSearch.Font = app.captionFont;

				list.BackColor = app.mainBackground;
				list.ForeColor = app.secondaryForeground;
				list.Font = app.captionFont;
			}
			return app;
		}
	}

}

