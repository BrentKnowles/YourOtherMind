using System;
using System.Windows.Forms;

namespace Layout
{
	public interface LayoutPanelInterface
	{   
		#region variables
		string GUID { get; set; }
		bool GetSaveRequired {get;}
		 string ParentGUID {get;set;}
		bool GetIsChild { get; }
		#endregion
		#region gui
		Panel NoteCanvas {get;set;}
		#endregion

		void SaveLayout();
		void LoadLayout(string GUID);
		void SetSaveRequired(bool NeedSave);
		string Backup();
		void RefreshTabs ();
		//void AddNote();

	}

}

