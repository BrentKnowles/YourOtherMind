// FindBarStatusStrip.cs
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
using System.Collections.Generic;

namespace Layout
{

	/// <summary>
	/// The new FindBar, for placement on Layouts
	/// </summary>
	public class FindBarStatusStrip : StatusStrip
	{

		#region GUI
		ToolStripLabel CurrentNote=null;
		ToolStripTextBox Searchbox=null;
		ToolStripTextBox ReplaceText=null;
		ToolStripLabel SearchMatchesFound=null;
		ToolStripLabel Words =null;
		ToolStripLabel Pages = null;
		ToolStripProgressBar Progress  = null;
		ToolStripButton Exact = null;
		ToolStripButton ReplaceAllOption =null;
		#endregion
		#region variables
		private bool supressMode=false; // if set to true hides confirmation messages, used only in testing


		public bool SupressMode {
			get {
				return supressMode;
			}
			set {
				supressMode = value;
			}
		}
		protected RichTextExtended LastRichText = null;
		protected List<int> PositionsFound = new List<int>();
		// if > -1 then we  ar ein the middle of a search. This iwhen DoFind is called
		protected int Position = -1; 
		string LastSearch=Constants.BLANK;
		#endregion
		private ToolStripSplitButton BuildFindMenu()
		{
			ToolStripSplitButton FindMenu = new ToolStripSplitButton();
			FindMenu.Text = Loc.Instance.GetString ("Find");
			FindMenu.Click+= HandleFindMenuClick;

			ToolStripButton ReplaceAll = new ToolStripButton();
			ReplaceAll.Text = Loc.Instance.GetString ("Replace With...");
			ReplaceAll.Click += HandleReplaceAllClick;

			ReplaceText = new ToolStripTextBox();

			 ReplaceAllOption = new ToolStripButton();
			ReplaceAllOption.Text = Loc.Instance.GetString ("Replace All");
			ReplaceAllOption.CheckOnClick = true;
			ReplaceAllOption.Checked= true;

			FindMenu.DropDown.Items.Add (ReplaceAll);
			FindMenu.DropDown.Items.Add (ReplaceText);
			FindMenu.DropDown.Items.Add (ReplaceAllOption);
			return FindMenu;

		}

		/// <summary>
		/// Resets the search. Called when setting a new text note to clear the search (to force it to search in the new note)
		/// </summary>
		public void ResetSearch()
		{
			PositionsFound = null;
			Position = 0;
		}
		/// <summary>
		/// Updates the search after editing interruption.
		/// 
		/// Basically if you search, then modify text, the positions are out of wack
		/// and need to be restored.
		/// 
		/// But we want to try and keep our position in the text properly
		/// 
		/// So:
		/// 
		///  - we empty the Position list butkeep the PositionIndex
		/// </summary>
		public void UpdateSearchAfterEditingInterruption ()
		{
			// we set to null so that Prev and Next know the list needs rebuiolding
			PositionsFound = null;


		}

		void HandleFindMenuClick (object sender, EventArgs e)
		{
			DoFindStandard (0);
		}

		protected void ReplaceTextNow (string oldText, string newText)
		{

			//NewMessage.Show (String.Format ("Replacing {0} to {1}", oldText, newText));
			if (LastRichText != null && newText != Constants.BLANK) {
				int nNumberToGet = 1;
				// DEFAULT: Only nex titem
				// can we use the search results?
				DoFindStandard (0);
				Cursor old = LastRichText.Cursor;
				LastRichText.Cursor = Cursors.WaitCursor;
				if (PositionsFound != null) {
					// DoNotUpdateSelection = true;
					try {
						// we found something.
						// now cycle through it, replacing each entity?
						if (ReplaceAllOption.Checked == true) {
							nNumberToGet = PositionsFound.Count;
						}
						LastRichText.BeginUpdate ();
						LastRichText.SuspendUpdateSelection ();
						// we set seleciton to 0 (and then use Selection location) to allow a word like Earth to be transformed into Earth2 (old system did not allow this)
						LastRichText.SelectionStart = 0;
						for (int i = 0; i <= nNumberToGet - 1; i++) {
							DoFindStandard (LastRichText.SelectionStart + 1);
							// always start over because we are deleting items as we go
							//FindNext ();
							if (LastRichText.SelectedText != "") {
								LastRichText.SelectedText = newText;
							}
						}
						LastRichText.EndUpdate ();
						LastRichText.ResumeUpdateSelection ();
						// make ti sure a ' nothing found' message, to indicate we have finished our replace
						DoFindStandard (0);
					}
					catch (Exception ex) {
						lg.Instance.Line ("HandleReplaceAllClick", ProblemType.EXCEPTION, ex.ToString ());
					}
					finally {
						if (supressMode == false)
							CoreUtilities.NewMessage.Show (Loc.Instance.GetStringFmt ("{0} words replaced", nNumberToGet.ToString ()));
						//    DoNotUpdateSelection = false;
					}
				}
				LastRichText.Cursor = old;
			}
		}

		void HandleReplaceAllClick (object sender, EventArgs e)
		{
			string oldText = Searchbox.Text;
			string newText = ReplaceText.Text;
			ReplaceTextNow (oldText, newText);


		}
		public void SetCurrentNoteText(string Caption)
		{
			CurrentNote.Text = Caption;
		}

		public FindBarStatusStrip()
		{



			CurrentNote = new ToolStripLabel();
			CurrentNote.BackColor = this.BackColor;
			CurrentNote.Text = "*";





			Searchbox = new ToolStripTextBox();
			Searchbox.TextChanged+= HandleSearchBoxTextChanged;
			Searchbox.KeyPress+= HandleSearchBoxKeyPress;

			 Exact = new ToolStripButton();
			Exact.Text = Loc.Instance.GetString ("Exact?");
			Exact.CheckOnClick = true;
			Exact.Click+= HandleExactClick;

			 Progress = new ToolStripProgressBar();

			ToolStripButton Prev  =new ToolStripButton();
			Prev.Text = "<";
			Prev.Click += HandlePreviousClick;

			ToolStripButton Next= new ToolStripButton();
			Next.Text = ">";
			Next.Click += HandleNextClick;

			SearchMatchesFound = new ToolStripLabel();
			SearchMatchesFound.BackColor = this.BackColor;
			UpdateSearchMatches(0,0);

			Words = new ToolStripLabel();
			Words.BackColor = this.BackColor;
			UpdateWords(0,0);

			 Pages = new ToolStripLabel();
			Pages.BackColor = this.BackColor;
			UpdatePages(0,0);

			this.Items.Add (BuildFindMenu());
		
			this.Items.Add (Searchbox);
			this.Items.Add (Exact);
			this.Items.Add (new ToolStripSeparator());
			this.Items.Add (Prev);
			this.Items.Add (Next);
			this.Items.Add (new ToolStripSeparator());
			this.Items.Add (SearchMatchesFound);
			this.Items.Add (Progress);
			this.Items.Add (Words);
			this.Items.Add (new ToolStripSeparator());
			this.Items.Add (Pages);
			this.Items.Add (new ToolStripSeparator());
			this.Items.Add (CurrentNote);
		}

		void HandleSearchBoxTextChanged (object sender, EventArgs e)
		{
		// was going to reset position here but I need to do it in keypress
		}

		void HandleExactClick (object sender, EventArgs e)
		{
			DoFindStandard(0);
		}

		void HandleSearchBoxKeyPress (object sender, KeyPressEventArgs e)
		{
	
			
			//Keys.Enter
			if (e.KeyChar == 13) {
				// supress beeping
				//e.KeyChar = 0;
				e.Handled = true;
			//	NewMessage.Show ("Deal with enter pressed");
				if (-1 == Position)
				{
					DoFindStandard(0);
				}
				else
				{
					FindNext();
				}
				
//				if (toolFindText.Text.IndexOf("~") > -1)
//				{
//					HandleConsoleCommand(toolFindText.Text);
//				}
//				else
//				{
//					if (founditems == null)
//					{
//						DoFind();
//					}
//					else
//					{
//						// we already started a search, pressing enter key goes to next entry
//						GoToNext();
//					}
//				}
				
			} 
		}

		void UpdatePages (int i, int i2)
		{
			Pages.Text = Loc.Instance.GetStringFmt("Pages: {0}/{1}", i.ToString (), i2.ToString ());
		}

		void UpdateWords (int i, int i2)
		{
			Words.Text = Loc.Instance.GetStringFmt("Words: {0}/{1}", i.ToString (), i2.ToString ());
		}

		void UpdateSearchMatches (int i, int i2)
		{
			SearchMatchesFound.Text = Loc.Instance.GetStringFmt("{0}/{1} Found", i.ToString (), i2.ToString ());
		}

		void HandleNextClick (object sender, EventArgs e)
		{
			FindNext();

		}

		void HandlePreviousClick (object sender, EventArgs e)
		{
			FindPrev();
		}

	

		/// <summary>
		/// Returns the total number of pages
		/// Jan 2010 - changed this to 250 words per page to match 'print'
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static int CountPages(int nWordCount)
		{
			int nPages = (nWordCount + 249) / 250;
			if (nPages <= 0) nPages = 1; // oct 30 2009 - base page is 1
			
			return nPages;
			
			

		}

		private int WordCount(string Text)
		{
			if (LayoutDetails.Instance.WordSystemInUse == null)
			{
				NewMessage.Show (Loc.Instance.GetString ("Major error: No Word System was assigned. Programming error."));
				return 0;
			}
			return LayoutDetails.Instance.WordSystemInUse.CountWords(Text);
		}
		// This is kept in memory to avoid unnecessary updates
		int nCurrentPageNumberWords = 0;
		
		
		//   bool DoNotUpdateSelection = false; // this is set on/off when doing replaceall
		/// <summary>
		/// updates the word count information, in addition to the timer stuff
		/// 
		/// when the user selects text
		/// </summary>
		/// 
		/// <param name="bSlowRefresh">if true there will only be a random chance of updating -- a cheap speed improvement</param>
		public void UpdateSelection (string sSelection, bool bSlowRefresh)
		{
			if (LayoutDetails.Instance.CurrentLayout != null && LayoutDetails.Instance.CurrentLayout.CurrentTextNote != null) {
				//if (RichText.bSuspendUpdateSelection == true) return;
				//   if (DoNotUpdateSelection == true) return;
			
			
				// april 5 2009
				// Update the FindResults (i.e., in case you are editing after finding a word so that when you press Find again, it goes to 
				// the correct location
				//if (nPosition != -1)
				//	DoFind(true);
			
			
			
			
			
				int nWords = 0;  
				int nSelection = 0;
			
				// March 9 2009 - only update 25% of the time?
				if ((bSlowRefresh == false) || (new Random ().Next (1, 100) > 75)) {
					nWords = WordCount (LayoutDetails.Instance.CurrentLayout.CurrentTextNote.GetAsText());
					nSelection = WordCount (sSelection);
				
				
					// we seldom update the current page count
					if (bSlowRefresh == false || new Random ().Next (1, 100) > 94) {
						nCurrentPageNumberWords = 1;
						// grab the number of words to the current selection?
						// to get current page number
					
						// Sep 2009 - checked for selection length to AVOID crashes
						// Oct 30 2009 - we are not updating the Page sproperly now, doing an 
						//       experiment... selectionstart should always be valid, no??
					
					
						// Nov 5 2009
						// There is a bug with this, stillthe occasional crash but never when I am debugging
						// So I'm wrapping thi sall with exception handling even though I don't think its necessary
					
						try {
						
						
							if (LayoutDetails.Instance.CurrentLayout.CurrentTextNote.SelectionStart > 0) {
								string sCurrentPageNumberString = LayoutDetails.Instance.CurrentLayout.CurrentTextNote.GetAsText ()
									.Substring (0,LayoutDetails.Instance.CurrentLayout.CurrentTextNote.SelectionStart);
								if (sCurrentPageNumberString != null && sCurrentPageNumberString != "") {
									nCurrentPageNumberWords = WordCount (sCurrentPageNumberString);
								}
							} else {
								nCurrentPageNumberWords = 1;
							
							}
						} catch (Exception) {
							nCurrentPageNumberWords = 1;
						}
					}
				
				
					UpdatePages( CountPages (nCurrentPageNumberWords), CountPages (nWords));
					//toolLabelPages.Text = String.Format ("Pages: {0}/{1}", CountPages (nCurrentPageNumberWords), CountPages (nWords));
				
					UpdateWords( nSelection, nWords);
					// updates both the text and the progress bar
					//toolLabelWords.Text = String.Format ("Words: {0}/{1}", nSelection.ToString (), nWords.ToString ());
				
					Progress.Maximum = nWords;
					if (nSelection > Progress.Maximum) {
						nSelection = Progress.Maximum;
					}
					Progress.Value = nSelection;
				}
			}
		}
		public void FocusOnSearchEdit ()
		{
			if (Searchbox == null) {
				throw new Exception("No searchbox defined in Findbar");
			}

			Searchbox.Focus();
		}

		/// <summary>
		/// Dos the find. This is the external facing find
		/// </summary>
		/// <returns>
		/// <c>true</c>, if find was done, <c>false</c> otherwise.
		/// </returns>
		/// <param name='SearchText'>
		/// Search text.
		/// </param>
		/// <param name='Exact'>
		/// If set to <c>true</c> exact.
		/// </param>
		/// <param name='RichBox'>
		/// Rich box.
		/// </param>
		public bool DoFind (string SearchText, bool Exact, RichTextExtended RichBox, int StartingLocation)
		{
			LastRichText = RichBox;
			Position = -1;
			Searchbox.Text = SearchText;

			PositionsFound = DoFind_BuildListOfFoundPositions (SearchText, Exact, RichBox.Text, StartingLocation);
			if (PositionsFound != null && PositionsFound.Count > 0) {
				//	UpdateSearchMatches(1, PositionsFound.Count);
				FindFirst ();
				LastSearch = SearchText;
				return true;
			}
			UpdateSearchMatches(0,0);

			return false;
		}
	
		void GoTo ()
		{
			if (null == LastRichText) {
				NewMessage.Show (Loc.Instance.GetString ("Please select a note first."));
				return;
			}
			if (PositionsFound != null && Position >-1  && PositionsFound.Count >= Position ) {
				int PositionToGoTo = PositionsFound [Position];
				LastRichText.SelectionStart = PositionToGoTo;
				LastRichText.SelectionLength = Searchbox.Text.Length;
				LastRichText.ScrollToCaret();
				UpdateSearchMatches(Position+1, PositionsFound.Count);
			}
		}
		void FindFirst ()
		{
			// selects the first occurence of the word. Called from DoFind
			Position = 0;
				GoTo();


		}


		protected void FindNext ()
		{
			if (PositionsFound != null) {
				// Sep 2009 - if we somehow changed the word we are searching for we need to reset the serach
				if (LastSearch != Constants.BLANK && LastSearch != Searchbox.Text && Searchbox.Text != Constants.BLANK) {
					DoFindStandard (0);
					return;
				}

				if (Position < PositionsFound.Count - 1) {
					Position++;
					GoTo ();
				}
			} else {
				HandleNextOrPreviousAfterTextHasBeenChanged();

			}
		}

		void HandleNextOrPreviousAfterTextHasBeenChanged ()
		{
			int oldPosition = Position;
			// perhaps text was changed and so we try to rebuild the list AND keep our position
			DoFindStandard(0);
			Position = oldPosition;
			if (PositionsFound != null && PositionsFound.Count > 0)
			{
				// fix Position, in case list size has changed
				if (Position > PositionsFound.Count - 1)
				{
					Position = PositionsFound.Count -1;
				}
				GoTo();
			}
		}

		void FindPrev ()
		{


			if (PositionsFound != null) {
				// Sep 2009 - if we somehow changed the word we are searching for we need to reset the serach
				if (LastSearch != Constants.BLANK && LastSearch != Searchbox.Text && Searchbox.Text != Constants.BLANK)
				{
					DoFindStandard(0);
					return;
				}
				
				if (Position > 0) {
					Position--;
					GoTo ();
				}
			}
			else {
				HandleNextOrPreviousAfterTextHasBeenChanged();
				
			}
		}
		private void DoFindStandard (int StartingLocation)
		{
			// called when FindButton pressed

			// grabs the text from controls
			if (LayoutDetails.Instance.CurrentLayout != null && LayoutDetails.Instance.CurrentLayout.CurrentTextNote != null) {
				DoFind (Searchbox.Text, Exact.Checked, LayoutDetails.Instance.CurrentLayout.CurrentTextNote.GetRichTextBox(),StartingLocation);
			} else {
				NewMessage.Show (Loc.Instance.GetString ("Please select a text note first."));
			}
		}

		// changing how find works. DoFind is a straight find.
		// it fills the list with Found items
		protected List<int> DoFind_BuildListOfFoundPositions(string sFind, bool bExact, string TextToSearch, int StartingLocation)
			{
			
		
			if (sFind == null || sFind == "") return null;
		
		
			
			// just doing a test right now
			string SearchPhrase = Constants.BLANK;

			
			
			// even an exact search must be lower because we tolower all the other text
			SearchPhrase  = sFind.ToLower();
			
			////// Exact /////////////
			if (bExact == true)
			{
				// pad the word so only ones with whitespace come up
				SearchPhrase = " " + SearchPhrase + " ";
			}



			
			
			// step 1 - get an array of positions for each time the word appears
			// only keep the word being searched for
			
			List<int> positionList = new List<int>();

			// do search after search? Replacing the text with ****=length
			bool done = false;
			TextToSearch = TextToSearch.ToLower();


			int nCount = StartingLocation; // Feb 2013 - originally this was 0 but to allow specifying a start location made this a passed in parameter
			
			while (done == false)
			{
				bool adverbsearch = false;
				// kinda hack, if the searchtext = ly then we konw we are doing an adverb search 
				if (SearchPhrase == "ly")
				{
					adverbsearch = true;
				}
				
				int lastcount = nCount;
				nCount = TextToSearch.IndexOf(SearchPhrase, nCount);
				if (true == adverbsearch)
				{
					string[] adverbs = new string[4] { "ly,", "ly.", "ly!", "ly?" };
					int count2 = 0;
					while (-1 == nCount && count2 < adverbs.Length)
					{
						
						nCount = TextToSearch.IndexOf(adverbs[count2], lastcount);
						count2++;
						
					}
				}
				
				
				if (nCount > -1)
				{
					// add word position
					positionList.Add(nCount);
					nCount++;
					
				}
				else
				{
					done = true;
				}
			}
			
			//MessgeBox.Show(positionList.Count.ToString());
			return positionList;
		}
		/// <summary>
		/// this is for finding text but using a specific position from SpellChecker
		/// </summary>
		/// <param name="sText"></param>
		/// <param name="nPosition"></param>
		public void SetupForFindSpecificPosition(string sText, int nPosition)
		{
			Searchbox.Text = sText;
			Searchbox.SelectAll();
			DoFindStandard(nPosition);
			//GoToNextSpecificPosition(nPosition);
		}
	}


}



/// <summary>
/// tries to get to the passed in Position (spellchecker).
/// Looks for closet match 
/// </summary>
/// <param name="Position"></param>
//private void GoToNextSpecificPosition(int Position)
//{
//	int nPositionIdx = -1;
//	
//	foreach (object position in founditems)
//	{
//		nPositionIdx++;
//		if ((int)position == Position)
//		{
//			break;
//		}
//	}
//	
//	//always increment by one
//	nPositionIdx++;
//	
//	toolLabelOccurences.Text = String.Format("{0}/{1} Found", nPositionIdx, founditems.Length);
//	RichText.SelectionStart = Position;
//	RichText.SelectionLength = toolFindText.Text.Length;
//	RichText.ScrollToCaret();
//	
//	
//}

