// LayoutPanelBase.cs
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
	public abstract class LayoutPanelBase : Panel
	{
		public const string SYSTEM_LAYOUT ="system";

		public abstract void DisableLayout (bool off);

		bool systemNote = false;

		// set in NoteDataXML_SystemONy to true so I can use it to figure out if it deserves a findbar
		public bool SystemNote {
			get {
				return systemNote;
			}
			set {
				if (value == true)
				{
					//if (FindBar == null)
					{
						FindBar = new FindBarStatusStrip ();
						FindBar.Dock = DockStyle.Bottom;
						this.Controls.Add (FindBar);
					}
				}
				systemNote = value;
			}
		}
		// used for auto window assignment
		public abstract int HeightOfToolbars ();
		
		public LayoutPanelBase ()
		{
		

		}

		#region delegates
		// This delgate set in NoteDataXML_Panel, hooking the parent Layout up with the Child Layout, for the purposes of SettingCurrentTextNote accurately
		public Action<NoteDataXML_RichText> SetParentLayoutCurrentNote;
		// set in NoteDataXML_Panel so that a child Layout will tell a higher level to save, if needed
		public Action<bool> SetSubNoteSaveRequired = null;

#endregion

		#region variables
		private string _guid = CoreUtilities.Constants.BLANK;
		protected bool _saverequired = false;
	
		private bool isDraggingANote=false;
		/// <summary>
		/// Is set to true if occupied by dragging a note around
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is dragging A note; otherwise, <c>false</c>.
		/// </value>
		public bool IsDraggingANote {
			get {
				return isDraggingANote;
			}
			set {
				isDraggingANote = value;
			}
		}
/// <summary>
/// Clears the drag. Called from ESC is presed
/// </summary>
		public virtual void ClearDrag ()
		{
			IsDraggingANote = false;
		}
		public abstract void SetParentGuidForNotesFromExternal(string newGUID);

		public abstract string ParentGuidFromNotes { get; }
		public abstract string ParentGUID { get; set; }
		public abstract void CopyNote (NoteDataInterface Note);
		/// <summary>
		/// Wraps the lookup to see if I am a child (by checking my parentGUID)
		/// </summary>
		/// <value>
		/// <c>true</c> if get is child; otherwise, <c>false</c>.
		/// </value>
		public  bool GetIsChild {
			get {
				if (ParentGUID == Constants.BLANK)
				{
					return false;
				}
				return true;
			}
		}
		// storing reference to Interface, allowing a data swapout later

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
		public bool GetSaveRequired {
			get{ 
				lg.Instance.Line("LayoutPanelBase->GetSaveRequired", ProblemType.MESSAGE, String.Format ("{0} GetSaveRequired for {1}",
				                                                                                         _saverequired, this.GUID),Loud.CTRIVIAL);
				return _saverequired;}
		//	set { _saverequired = value;} Must set this through function
		}

		/// <summary>
		/// Wrapper around the show tab variable for access from individual notes (layoutpanel notes)
		/// </summary>
		/// <value>
		/// <c>true</c> if show tabs; otherwise, <c>false</c>.
		/// </value>
		abstract public bool ShowTabs { get; set; }
		abstract public bool GetIsSystemLayout { get; set; }	
		abstract public NoteDataXML_RichText CurrentTextNote{ get; set; }
		abstract public string Caption {get;}
		abstract public string Section { get; }
		abstract public string Keywords { get; }
		abstract public string Subtype { get; }
		abstract public string Notebook { get; }

		// set when a richtext box has been updated so that when the mainform update timer runs
		// we know to update the word count and such on the findbar
		private bool needsTextBoxUpdate = false;
		public bool NeedsTextBoxUpdate {
			get { return needsTextBoxUpdate;}
			set { needsTextBoxUpdate = value;}
		}

		abstract public void SetParentFields (string section, string keywords, string subtype, string notebook);


		private bool isLoaded=false;
		/// <summary>
		/// Gets or sets a value indicating whether this instance is loaded.
		/// 
		/// This is used when painting something expensive, or something that can't paint untl all the notes exist
		/// 
		/// re: Timeline
		/// 
		/// It is set to true in NewLayout and LoadLayout
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is loaded; otherwise, <c>false</c>.
		/// </value>
		public bool IsLoaded {
			get {
				return isLoaded;
			}
			set {
				isLoaded = value;
			}
		}

#endregion

		#region gui
		public virtual Panel NoteCanvas {get;set;}
		// passed in front MainForm and then used by any text box
		protected ContextMenuStrip TextEditContextStrip; 
		public ContextMenuStrip GetLayoutTextEditContextStrip()
		{
			return TextEditContextStrip;
		}
		public FindBarStatusStrip GetFindbar ()
		{
			if (GetIsChild) {
				LayoutPanelBase absoluteParent = (LayoutPanelBase)GetAbsoluteParent ();
				if (absoluteParent != null) {
					return absoluteParent.GetFindbar () ;
				}
			}

			return FindBar;
		}

		protected FindBarStatusStrip FindBar=null;
#endregion
		public abstract void SaveLayout();
		//public abstract  void LoadLayout(string GUID);
		public abstract void LoadLayout (string _GUID, bool IsSubPanel, ContextMenuStrip textEditorContextStrip);
		public abstract void LoadLayout (string _GUID, bool IsSubPanel, ContextMenuStrip textEditorContextStrip, bool NoSaveMode);

	//	public abstract  void AddNote();
		public abstract void AddNote(NoteDataInterface note);

		public abstract System.Collections.Generic.List<NoteDataInterface> GetAvailableFolders();
		public abstract void MoveNote (string GUIDOfNoteToMove, string GUIDOfLayoutToMoveItTo);
		//public abstract string Backup();
		public abstract void SetSaveRequired(bool NeedSave);
		public abstract void UpdateListOfNotes ();

		public abstract void RefreshTabs ();
		public abstract void DeleteNote(NoteDataInterface NoteToDelete);
		public abstract System.Collections.ArrayList GetAllNotes();
		public abstract NoteDataInterface FindNoteByName(string name);
		public abstract  NoteDataInterface FindNoteByName(string name, ref System.Collections.ArrayList notes_tmp);
		public abstract NoteDataInterface FindNoteByGuid (string guid);
		public abstract NoteDataInterface GoToNote(NoteDataInterface note);
		public abstract  NoteDataInterface GetNoteOnSameLayout(string GUID, bool GoTo);
		public abstract NoteDataInterface GetNoteOnSameLayout (string GUID, bool GoTo, string TextToFindInRichEdit);
		public abstract NoteDataInterface FindSubpanelNote (NoteDataInterface note);
		public abstract void SetCurrentTextNote (NoteDataXML_RichText note);
		public abstract List<string> GetListOfStringsFromSystemTable (string tableName, int Column, string filter, bool sort);
		public abstract List<string> GetListOfStringsFromSystemTable (string tableName, int Column, string filter);
		public abstract List<string> GetListOfStringsFromSystemTable (string tableName, int Column);
		public abstract NoteDataXML_SystemOnly CreateSystemPanel ();
		public abstract void NewLayout (string _GUID, bool AddDefaultNote, ContextMenuStrip textEditorContextStrip);
		public abstract CoreUtilities.Links.LinkTable GetLinkTable ();
		public abstract int CountNotes();
		public abstract string[] GetListOfGroupEmNameMatching (string sStoryboardName, string sGroupMatch);
		
	//	public abstract void SystemNoteHasClosedDown (bool closed);

		/// <summary>
		/// Gets the absolute parent (the LayoutPanel that contains me or the subpanels I am inside)
		/// </summary>
		/// <returns>
		/// The absolute parent.
		/// </returns>
		public LayoutPanelBase GetAbsoluteParent()
		{
			LayoutPanelBase layoutParent = null;
			
			if (this.Parent != null) {
				//	NewMessage.Show (this.Parent.Name);
				
				Control looker = this.Parent;
				if (looker != null)
				{
				while ( (layoutParent == null || layoutParent.GetIsChild == true) && (looker != null) /*not sure&& (looker != null)*/)
				{
					if (looker is LayoutPanelBase )
					{
						layoutParent= (looker as LayoutPanelBase);

						//NewMessage.Show ("Setting Absolute to " + layoutParent.GUID);
					}
					else
					{

					}
					// because we want to go all the way to the top we keep looking

						if (null == looker.Parent)
						{
							looker = null;
						}
						else
							looker = looker.Parent;
				}
				}//if looker not null
				
			} else {
				NewMessage.Show (Loc.Instance.GetStringFmt("No parent for this layout {0} with ParentGUID = {1}", GUID, this.ParentGUID));
			}
			return layoutParent;
			
		}

		/// <summary>
		/// Focuses the on find bar.
		/// </summary>
		public void FocusOnFindBar ()
		{
			if (FindBar != null) {
				FindBar.FocusOnSearchEdit();
			}
		}
		public virtual void BringNoteToFront(string guid)
		{

		}
	

		public void DoFormatOnText (NoteDataXML_RichText.FormatText format)
		{
			if (CurrentTextNote != null) {

				switch (format) {
				case NoteDataXML_RichText.FormatText.BOLD:
					CurrentTextNote.Bold();
					break;
				case NoteDataXML_RichText.FormatText.STRIKETHRU:
					CurrentTextNote.Strike();
					break;
				case NoteDataXML_RichText.FormatText.ZOOM:
					CurrentTextNote.ZoomToggle();
					break;
				case NoteDataXML_RichText.FormatText.LINE:
					CurrentTextNote.GetRichTextBox().DrawBlackLine();
					break;
				case NoteDataXML_RichText.FormatText.BULLET:
					CurrentTextNote.GetRichTextBox().Bullet(false);
					break;
				case NoteDataXML_RichText.FormatText.BULLETNUMBER:
					CurrentTextNote.GetRichTextBox().Bullet(true);
					break;
				case NoteDataXML_RichText.FormatText.DEFAULTFONT:
					if (LayoutDetails.Instance.GetDefaultFont != null)
					{
						CurrentTextNote.GetRichTextBox().SelectionFont = LayoutDetails.Instance.GetDefaultFont();
					}

					break;
				case NoteDataXML_RichText.FormatText.DATE:
					CurrentTextNote.GetRichTextBox().InsertDate();
					break;
				case NoteDataXML_RichText.FormatText.UNDERLINE:
					CurrentTextNote.Underline();
					break;
				case NoteDataXML_RichText.FormatText.ITALIC:
					CurrentTextNote.Italic();
					break;
				}
			}

		}
	
		// looks for a notelist and will filter that notelist by the specified text
		public abstract void FilterByKeyword (string text);

		public abstract void ColorToolBars (AppearanceClass app);
		public abstract void TestForceError();
	}


}

