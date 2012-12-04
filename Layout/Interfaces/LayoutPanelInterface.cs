using System;

namespace Layout
{
	public interface LayoutPanelInterface
	{   
		#region variables
		string GUID { get; set; }
		bool SaveRequired {get;set;}
		#endregion

		void SaveLayout();
		void LoadLayout(string GUID);

		void AddNote();
	}

}

