// HeaderBar.cs
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
using Layout;
using CoreUtilities;
using System.Drawing;
using System.Collections.Generic;
namespace LayoutPanels
{
	/// <summary>
	/// This is the uppermost Toolbar on a Layout and is only visible at the 'root' level of a Layout.
	/// 
	/// </summary>
	public class HeaderBar : IDisposable
	{
		#region interfaceelements
		LayoutPanel Layout;
		LayoutInterface Notes;
		public ToolStrip headerBar = null;
		ContextMenuStrip Notebook;
		ContextMenuStrip Sections;
		ContextMenuStrip Subtypes;
		ContextMenuStrip Status;
		ToolStripMenuItem LocationNotebook;
		ToolStripMenuItem SectionsItem;
		ToolStripMenuItem SubtypeItem;
		ToolStripMenuItem StatusItem;
		ToolStripTextBox changeName;
		Stars starControl;
		ToolStripLabel NameOfLayout ;

		ContextMenuStrip Source;
		ToolStripMenuItem SourceItem;

		ContextMenuStrip Words;
		ToolStripMenuItem WordItem;

		#endregion
		public HeaderBar (LayoutPanel layout, LayoutInterface notes)
		{
			if (null == layout || null == notes) {
				throw new Exception("Must pass in a valid layout");
			}
			Layout = layout;
			Notes = notes;
			HeaderToolbar();
			UpdateHeader ();

		
		}

		public int height {
			get { return headerBar.Height;}
		}

		private void HeaderToolbar ()
		{
			headerBar = new ToolStrip();
			headerBar.Parent = (Control)Layout;
			headerBar.Dock = DockStyle.Top;
			headerBar.Visible = true;
			
			
		}
		/// <summary>
		/// Releases all resource used by the <see cref="LayoutPanels.HeaderBar"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the <see cref="LayoutPanels.HeaderBar"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="LayoutPanels.HeaderBar"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="LayoutPanels.HeaderBar"/> so the garbage
		/// collector can reclaim the memory that the <see cref="LayoutPanels.HeaderBar"/> was occupying.
		/// </remarks>
		public void Dispose()
		{
			Layout.Controls.Remove (headerBar);
			headerBar = null;
		}
		void HandlePropertiesDropDownOpening (object sender, EventArgs e)
		{
			(sender as ToolStripDropDownButton).DropDownItems.Clear ();
			changeName = new ToolStripTextBox();
			changeName.Font = new Font(changeName.Font.FontFamily, 12);
			changeName.Text = Notes.Name;
			changeName.KeyDown += HandleChangeNameKeyDown;
			changeName.TextChanged+= HandleChangeNameClick;
			////////////////////////////


			ToolStripMenuItem Blurb = LayoutDetails.BuildMenuPropertyEdit (Loc.Instance.GetString ("Blurb: {0}"),Notes.Blurb,
			                                     Loc.Instance.GetString ("A brief blurb discussing the purpose of this layout."),
			                                     HandleBlurbEdit );



			///////////////////////////
			SourceItem = new ToolStripMenuItem();
			if (Notes.Source == Constants.BLANK) Notes.Source = Loc.Instance.GetString("none");
			SourceItem.Text = Notes.Source;
			SourceItem.ToolTipText = Loc.Instance.GetString("Source: If a reference, indicate where the reference originated.");


			Source = new ContextMenuStrip();
			ToolStripTextBox sourceText = new ToolStripTextBox();
			sourceText.Text = Notes.Source;
			sourceText.KeyDown+= HandleSourceKeyDown;;
			Source.Items.Add (sourceText);


			// - words
			WordItem = new ToolStripMenuItem();

			WordItem.Text = Loc.Instance.GetStringFmt("Words: {0}",Notes.Words.ToString());
			WordItem.ToolTipText = Loc.Instance.GetString("Indicate the length of this piece, in words.");
			
			
			Words = new ContextMenuStrip();
			ToolStripTextBox sourceWords = new ToolStripTextBox();
			sourceWords.Text = Notes.Words.ToString ();
			sourceWords.KeyDown+= HandleWordsKeyDown;;
			Words.Items.Add (sourceWords);

			//tabMenu.DropDownItems.Remove (tabMenu.DropDownItems.Add ("empty"));


			//////////
			ToolStripButton backColor = new ToolStripButton();
			backColor.Text = Loc.Instance.GetString("Background Color");
			backColor.BackColorChanged+= HandleBackColorChanged;
			backColor.BackColor = Notes.BackgroundColor;
			backColor.Click+= HandleBackGroundColorClick;



			//////////

			// setup contextmenus
			SourceItem.DropDown = Source;
			WordItem.DropDown = Words;

			(sender as ToolStripDropDownButton).DropDownItems.Add (changeName);
			(sender as ToolStripDropDownButton).DropDownItems.Add (Blurb);
			(sender as ToolStripDropDownButton).DropDownItems.Add (SourceItem);
			(sender as ToolStripDropDownButton).DropDownItems.Add (WordItem);
			(sender as ToolStripDropDownButton).DropDownItems.Add (backColor);
			//(sender as ToolStripDropDownButton).DropDownItems.Add (tabMenu);
		}
		Color old = Color.Black;
		// used to show the user that the layout they are in is no longer 'active'
		public void Disable (bool off)
		{
			if (true == off) {
				old = NameOfLayout.ForeColor;
				NameOfLayout.ForeColor = Color.Red;
			} else {
				NameOfLayout.ForeColor = old;
			}
		}

		void HandleBackColorChanged (object sender, EventArgs e)
		{
			(sender as ToolStripButton).ForeColor = TextUtils.InvertColor((sender as ToolStripButton).BackColor);
		}

		void HandleBackGroundColorClick (object sender, EventArgs e)
		{
			ColorDialog color = new ColorDialog ();
			if (color.ShowDialog () == DialogResult.OK) {
				(sender as ToolStripButton).BackColor = color.Color;
				Notes.BackgroundColor = color.Color;
				Layout.BackColor = color.Color;
				
			}
		}

		void HandleBlurbEdit (object sender, KeyEventArgs e)
		{
		
				string tablecaption = Notes.Blurb;
				LayoutDetails.HandleMenuLabelEdit (sender, e, ref tablecaption, Layout.SetSaveRequired);
				Notes.Blurb = tablecaption;

		}
	
		void HandleWordsKeyDown (object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter) {
				// the header is not updated unti enter pressed but the NAME is being updated
				int words = 0;
				if (Int32.TryParse((sender as ToolStripTextBox).Text, out words) == true)
				{
					Notes.Words = words;
					WordItem.Text = Loc.Instance.GetStringFmt("Words: {0}",Notes.Words.ToString());
					Layout.SetSaveRequired(true);
				}
				else
				{
					Notes.Words = 0;
				}


				// silenece beep
				e.SuppressKeyPress = true;
			}
		}

		void HandleSourceKeyDown (object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter) {
				// the header is not updated unti enter pressed but the NAME is being updated
				Notes.Source = (sender as ToolStripTextBox).Text;
				SourceItem.Text = Notes.Source;
				// silenece beep
				e.SuppressKeyPress = true;
				Layout.SetSaveRequired(true);
			}
		}




	
		void HandleChangeNameClick (object sender, EventArgs e)
		{
			Notes.Name = (sender as ToolStripTextBox).Text;
			Layout.SetSaveRequired(true);
			
		}
		void HandleChangeNameKeyDown (object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter) {
				// the header is not updated unti enter pressed but the NAME is being updated
				//UpdateHeader ();
				 NameOfLayout.Text= changeName.Text;
				LayoutDetails.Instance.UpdateTitle(changeName.Text);
				// silenece beep
				e.SuppressKeyPress = true;
			}
		}
		
		public void UpdateHeader ()
		{
			if (false == Layout.GetIsChild && false == Layout.GetIsSystemLayout ) {
				headerBar.Items.Clear ();



				 NameOfLayout = new ToolStripLabel ();
				NameOfLayout.Click+= NameOfLayoutClick;
				NameOfLayout.Text = Notes.Name;
				int DaysSinceLastEdit = (DateTime.Now - Notes.DateEdited).Days;
				string an_S = "";
				if (DaysSinceLastEdit != 1) an_S = "s";
				NameOfLayout.ToolTipText = Loc.Instance.GetStringFmt("{0} day{1} since this layout last edited", DaysSinceLastEdit,an_S);
			

				 starControl = new Stars();
				starControl.SetStars("1", Notes.Stars);
				starControl.RatingChanged+= HandleRatingChanged;
				//starControl.UpdateStars();


				headerBar.Items.Add (NameOfLayout);
				headerBar.Items.Add (starControl);
				string[] keywords_list = GetKeywords();
				foreach (string keyword in keywords_list)
				{

					// build floating labels
					ToolStripLabel keylabel = new ToolStripLabel();
					keylabel.LinkBehavior = LinkBehavior.AlwaysUnderline;
					keylabel.IsLink = true;
					keylabel.Text = keyword;
					keylabel.Click+= HandleKeyLabelClick;
					headerBar.Items.Add(keylabel);
				}


				ToolStripLabel keywords = new ToolStripLabel();
				keywords.Font = new Font(keywords.Font.FontFamily, 8);
				keywords.LinkBehavior = LinkBehavior.AlwaysUnderline;
				keywords.IsLink = true;
				keywords.Text = Loc.Instance.GetString("(Edit)");
				keywords.ToolTipText = Loc.Instance.GetString ("Click here to adjust the keywords associated with this layout.");
				keywords.Click+= HandleKeywordsClick;

				
				ToolStripDropDownButton properties = new ToolStripDropDownButton();
				properties.Text = Loc.Instance.GetString("Properties");
				properties.DropDownOpening+= HandlePropertiesDropDownOpening;
				
				ToolStripButton Info = new ToolStripButton();

				Info.Text = Loc.Instance.GetString ("Info"); 
				Info.Click += HandleInfoClick;

				//
				///////// - Location settings
				/// 
				Notebook = new ContextMenuStrip();
				Sections = new ContextMenuStrip();
				Subtypes = new ContextMenuStrip();
				Status = new ContextMenuStrip();


				if (Notes.Notebook == Constants.BLANK) Notes.Notebook = Loc.Instance.GetString("All");
				if (Notes.Subtype == Constants.BLANK) Notes.Subtype = Loc.Instance.GetString ("None");
				if (Notes.Status == Constants.BLANK) Notes.Status = Loc.Instance.GetString("None");
				//Notebook.Text = Notes.Notebook;
	//			Notebook.Opening += HandleDropDownOpening;;

				//ToolStripMenuItem Location = new ToolStripMenuItem();
				ToolStripDropDownButton Location = new ToolStripDropDownButton();
				Location.AutoSize = true;
				// this title will be replaced by the actual Notebook|Section when loaded, this only appears if blank
				Location.Text = Loc.Instance.GetString("Filters");
				//Location.ShowItemToolTips = Loc.Instance.GetString ("Set the Notebook and Section for this Layout");


				LocationNotebook = new ToolStripMenuItem();
				LocationNotebook.Name = "locationnotebook";
				LocationNotebook.Text=Notes.Notebook;
				LocationNotebook.AutoSize = true;
				LocationNotebook.DropDown = Notebook;
				LocationNotebook.ToolTipText = Loc.Instance.GetString("Choose the notebook for this layout");


				SectionsItem = new ToolStripMenuItem();
				SectionsItem.Name = "sectionsitem";
				SectionsItem.AutoSize = true;
				SectionsItem.Text = Notes.Section;
				SectionsItem.DropDown = Sections;
				SectionsItem.ToolTipText = Loc.Instance.GetStringFmt("Choose the section of the notebook {0} for this layout", Notes.Notebook);


				SubtypeItem = new ToolStripMenuItem();
				SubtypeItem.Name = "subtypeitem";
				SubtypeItem.AutoSize = true;
				SubtypeItem.Text = Notes.Subtype;
				SubtypeItem.DropDown = Subtypes;
				SubtypeItem.ToolTipText = Loc.Instance.GetString("Choose the subtype for this layout");


				StatusItem = new ToolStripMenuItem();
				StatusItem.Name = "statusitem";
				StatusItem.AutoSize = true;
				StatusItem.Text = Notes.Status;
				StatusItem.DropDown = Status;
				StatusItem.ToolTipText = Loc.Instance.GetString("Choose the status for this layout");

				Location.DropDownItems.Add (LocationNotebook);
				Location.DropDownItems.Add (SectionsItem);
				Location.DropDownItems.Add (SubtypeItem);
				Location.DropDownItems.Add (StatusItem);
				Location.DropDownOpening+= HandleDropDownOpening;
				//Location.DropDown = Notebook;
				//Location.DropIItems.Add (LocationNotebook);





				//
				// - General settings
				//
				headerBar.TabIndex = 0;
				//headerBar.BringToFront();
				headerBar.Font = new Font(headerBar.Font.FontFamily, 12);
				lg.Instance.Line("HeaderBar.UpdateHeader", ProblemType.MESSAGE, "Header should be visible");

				// Adding

				headerBar.Items.Add (keywords);
				headerBar.Items.Add (Location);
				headerBar.Items.Add (properties);
				headerBar.Items.Add (Info);
			}
		}

		void NameOfLayoutClick (object sender, EventArgs e)
		{
			LayoutDetails.Instance.CurrentLayout = this.Layout;
			// we do not want to set Subpanels as the CurrentLayout!
			if (this.Layout.GetIsSystemLayout == true) {
				if (null == LayoutDetails.Instance.CurrentLayout) {
					NewMessage.Show (Loc.Instance.GetString ("No Window defined"));
				}
			}
			Disable(false);
		}

		void HandleKeyLabelClick (object sender, EventArgs e)
		{
			LayoutDetails.Instance.FilterByKeyword ((sender as ToolStripLabel).Text);
		
//			NewMessage.Show ("Filter me");
		}

		/// <summary>
		/// Gets the keywords and does basic error checking
		/// </summary>
		/// <returns>
		/// The keywords.
		/// </returns>
		string[] GetKeywords ()
		{
			string[] result =Notes.Keywords.Split (new char[1] {'|'});
			return result;
		}

		void HandleKeywordsClick (object sender, EventArgs e)
		{
			List<string> allitems = LayoutDetails.Instance.TableLayout.GetListOfStringsFromSystemTable(LayoutDetails.SYSTEM_KEYWORDS,1);


			List<string> checkeditems = new List<string> ();
			string[] temp = GetKeywords();
			if (temp != null && temp.Length > 0) {
				checkeditems = new List<string> (temp);
			}

			CheckBoxForm checkers = new CheckBoxForm (allitems, checkeditems, Loc.Instance.GetString ("Keywords"), appframe.MainFormBase.MainFormIcon, LayoutDetails.ButtonHeight);
			checkers.Height = 600;
			if (checkers.ShowDialog () == DialogResult.OK) {
				string result = Constants.BLANK;
				foreach (string s in checkers.GetItems())
				{
					string delim = Constants.BLANK;
					if (result != Constants.BLANK)
					{
						delim = "|";
					}
					result = String.Format ("{0}{1}{2}", result, delim, s);
				}
				Notes.Keywords = result;
				// need to redraw the keywords
				UpdateHeader();
				Layout.SetSaveRequired(true);
			}
		}

		string HandleRatingChanged ()
		{
			Notes.Stars = starControl.GetStars(); 
			return "";
		}
		// list of notebooks
	
		/// <summary>
		/// The "LOCATION" option has been selected, we rebuild the submenus
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleDropDownOpening (object sender, EventArgs e)
		{
			// 
			Notebook.Items.Clear ();

			foreach (string s in LayoutDetails.Instance.TableLayout.GetListOfStringsFromSystemTable(LayoutDetails.SYSTEM_NOTEBOOKS,1)) {

				ToolStripItem item = Notebook.Items.Add (s);
				//NewMessage.Show (Notebook.OwnerItem.ToString ());
				//if (Notebook.OwnerItem == null) NewMessage.Show ("null");
				//lg.Instance.Line ("HeaderBar->HandleDropDownOpening", ProblemType.MESSAGE, Notebook.OwnerItem.ToString (), Loud.CTRIVIAL);
				//item.Tag = Notebook.SourceControl;
				item.Click += HandleNotebookClick;

			}

			Sections.Items.Clear ();
			System.Collections.Generic.List<string> basestrings = LayoutDetails.Instance.TableLayout.GetListOfStringsFromSystemTable (LayoutDetails.SYSTEM_NOTEBOOKS, 2, 
			                                                                                                                           String.Format ("1|{0}",Notes.Notebook));
			System.Collections.Generic.List<string> truelist = new System.Collections.Generic.List<string>();

			foreach (string listofstrings in basestrings) {
				string[] holder = listofstrings.Split(new char[1]{'|'});
				foreach (string sub in holder)
				{
					truelist.Add(sub);
				}
			}
			foreach (string s in truelist) {

				ToolStripItem item =  Sections.Items.Add(s);
				//lg.Instance.Line("HeaderBar->HandleDropDownOpening", ProblemType.MESSAGE, Sections.OwnerItem.ToString(), Loud.CTRIVIAL);
				//item.Tag = Sections.SourceControl;
				item.Click+= HandleSectionsClick;;
				
			}

			Subtypes.Items.Clear();

			foreach (string s in LayoutDetails.Instance.TableLayout.GetListOfStringsFromSystemTable(LayoutDetails.SYSTEM_SUBTYPE,1)) {

					ToolStripItem item = Subtypes.Items.Add (s);
					item.Click+= HandleSubtypeClick;
				}

			Status.Items.Clear ();
			
			foreach (string s in LayoutDetails.Instance.TableLayout.GetListOfStringsFromSystemTable(LayoutDetails.SYSTEM_STATUS,1)) {
				
				ToolStripItem item = Status.Items.Add (s);
				//NewMessage.Show (Notebook.OwnerItem.ToString ());
				//if (Notebook.OwnerItem == null) NewMessage.Show ("null");
				//lg.Instance.Line ("HeaderBar->HandleDropDownOpening", ProblemType.MESSAGE, Notebook.OwnerItem.ToString (), Loud.CTRIVIAL);
				//item.Tag = Notebook.SourceControl;
				item.Click += HandleStatusClick;;
				
			}

		}

		void HandleStatusClick (object sender, EventArgs e)
		{
			Notes.Status = (sender as ToolStripItem).Text;
			StatusItem.Text = Notes.Status;
			if (Notes.Status.IndexOf (Loc.Instance.GetString ("Retired")) > -1) {
				LayoutDetails.Instance.TransactionsList.AddEvent (new Transactions.TransactionRetired (DateTime.Now, this.Layout.GUID));
			} else
			if (Notes.Status.IndexOf (Loc.Instance.GetString ("Complete")) > -1) {
				LayoutDetails.Instance.TransactionsList.AddEvent (new Transactions.TransactionFinishedLayout (DateTime.Now, this.Layout.GUID));
			} else {
				// if neither Retired or Complete then flag a generic transaction
				LayoutDetails.Instance.TransactionsList.AddEvent (new Transactions.TransactionGenericStatus (DateTime.Now,this.Layout.GUID,Notes.Status));
			}
			Layout.SetSaveRequired(true);
		}

		void HandleSectionsClick (object sender, EventArgs e)
		{
			Notes.Section = (sender as ToolStripItem).Text;
			SectionsItem.Text = Notes.Section;
			Layout.SetSaveRequired(true);
		}
		/// <summary>
		/// Handles the notebook click. (When selecting a new notebook)
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>

		void HandleNotebookClick (object sender, EventArgs e)
		{
			Notes.Notebook = (sender as ToolStripItem).Text;
			//NewMessage.Show ( (sender as ToolStripItem).Tag.GetType().ToString());
			//Notebook.OwnerItem.Text = Notes.Notebook;
			LocationNotebook.Text = Notes.Notebook;
			//((ToolStripItem)(sender as ToolStripItem).Tag).Text = Notes.Notebook;
			Layout.SetSaveRequired(true);
		//	fail(sender as ToolStripItem).OwnerItem.GetCurrentParent().Text = Notes.Notebook;
		}

	

		public void SendToBack()
		{
			headerBar.SendToBack();
		}
		void GetInfo()
		{
			string messagestring = Loc.Instance.GetStringFmt ("Created On={0}{4}Last Edited={1}{4}Visited={2}{4}ID={3}", Notes.DateCreated,Notes.DateEdited,Notes.Hits, Layout.GUID, Environment.NewLine);
			info_form info = new info_form(messagestring, Layout.GUID);
			info.ShowDialog();




		}
		void HandleInfoClick (object sender, EventArgs e)
		{
			GetInfo();

		}
		
			void HandleSubtypeClick (object sender, EventArgs e)
			{
				Notes.Subtype = (sender as ToolStripItem).Text;
			SubtypeItem.Text = Notes.Subtype;
				Layout.SetSaveRequired(true);
			}
	}

	
}

