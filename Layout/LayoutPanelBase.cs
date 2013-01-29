using System;
using System.Windows.Forms;
using CoreUtilities;
using System.Collections.Generic;

namespace Layout
{
	public abstract class LayoutPanelBase : Panel
	{
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
		private string parentGUID = Constants.BLANK;
		/// <summary>
		/// Gets or sets the parent GUI. (Will be set by NoteDataXML_Panel) and refers to the Layout that owns this layout. That value in turn is used in the MoveNote code
		/// </summary>
		/// <value>
		/// The parent GUI.
		/// </value>
		public string ParentGUID {
			get { return parentGUID;}
			set { parentGUID = value;}
		}
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

		abstract public void SetParentFields (string section, string keywords, string subtype, string notebook);
#endregion

		#region gui
		public virtual Panel NoteCanvas {get;set;}
		// passed in front MainForm and then used by any text box
		protected ContextMenuStrip TextEditContextStrip; 
		public ContextMenuStrip GetLayoutTextEditContextStrip()
		{
			return TextEditContextStrip;
		}
#endregion
		public abstract void SaveLayout();
		//public abstract  void LoadLayout(string GUID);
		public abstract void LoadLayout (string _GUID, bool IsSubPanel, ContextMenuStrip textEditorContextStrip);

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
		public abstract NoteDataInterface FindNoteByGuid (string guid);
		public abstract void GoToNote(NoteDataInterface note);
		public abstract NoteDataInterface FindSubpanelNote (NoteDataInterface note);
		public abstract void SetCurrentTextNote (NoteDataXML_RichText note);
		public abstract List<string> GetListOfStringsFromSystemTable (string tableName, int Column, string filter);
		public abstract List<string> GetListOfStringsFromSystemTable (string tableName, int Column);
		public abstract NoteDataXML_SystemOnly GetSystemPanel ();
		public abstract void NewLayout (string _GUID, bool AddDefaultNote, ContextMenuStrip textEditorContextStrip);
		public abstract CoreUtilities.Links.LinkTable GetLinkTable ();
		public abstract int CountNotes();
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
				while (layoutParent == null || layoutParent.GetIsChild == true)
				{
					if (looker is LayoutPanelBase)
					{
						layoutParent= (looker as LayoutPanelBase);

						//NewMessage.Show ("Setting Absolute to " + layoutParent.GUID);
					}
					else
					{

					}
					// because we want to go all the way to the top we keep looking
					looker = looker.Parent;
				}
				
			} else {
				NewMessage.Show (Loc.Instance.GetStringFmt("No parent for this layout {0} with ParentGUID = {1}", GUID, this.ParentGUID));
			}
			return layoutParent;
			
		}
	}
}

