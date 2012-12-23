using System;
using System.Windows.Forms;
using CoreUtilities;
using System.Collections.Generic;

namespace Layout
{
	public abstract class LayoutPanelBase : Panel, LayoutPanelInterface
	{
		public LayoutPanelBase ()
		{
		}
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
			get{ return _saverequired;}
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
		abstract public NotePanelInterface CurrentNote{ get; set; }
		abstract public string Caption {get;}

#endregion

		#region gui
		public virtual Panel NoteCanvas {get;set;}
#endregion
		public abstract void SaveLayout();
		public abstract  void LoadLayout(string GUID);
		
	//	public abstract  void AddNote();

		public abstract System.Collections.Generic.List<NoteDataInterface> GetAvailableFolders();
		public abstract void MoveNote (string GUIDOfNoteToMove, string GUIDOfLayoutToMoveItTo);
		//public abstract string Backup();
		public abstract void SetSaveRequired(bool NeedSave);
		public abstract void UpdateListOfNotes ();
		public abstract void NewLayout (string _GUID);
		public abstract void RefreshTabs ();
		public abstract void DeleteNote(NoteDataInterface NoteToDelete);
	}
}

