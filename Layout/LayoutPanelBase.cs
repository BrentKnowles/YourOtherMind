using System;
using System.Windows.Forms;
namespace Layout
{
	public abstract class LayoutPanelBase : Panel, LayoutPanelInterface
	{
		public LayoutPanelBase ()
		{
		}
		#region variables
		private string _guid = CoreUtilities.Constants.BLANK;
		private bool _saverequired = false;
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
		public bool SaveRequired {
			get{ return _saverequired;}
			set { _saverequired = value;}
		}
#endregion

		#region gui
		public virtual Panel NoteCanvas {get;set;}
#endregion
		public abstract void SaveLayout();
		public abstract  void LoadLayout(string GUID);
		
		public abstract  void AddNote();

		public abstract System.Collections.Generic.List<NoteDataInterface> GetAvailableFolders();
		public abstract void MoveNote (string GUIDOfNoteToMove, string GUIDOfLayoutToMoveItTo);
	}
}

