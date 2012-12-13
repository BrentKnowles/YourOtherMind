using System;
using System.Windows.Forms;

namespace Layout
{
	public interface LayoutPanelInterface
	{   
		#region variables
		string GUID { get; set; }
		bool GetSaveRequired {get;}
		#endregion
		#region gui
		Panel NoteCanvas {get;set;}
		#endregion
		void SaveLayout();
		void LoadLayout(string GUID);
		string Backup();
		void AddNote();
	}

}

