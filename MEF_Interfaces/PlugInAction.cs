using System;

namespace MefAddIns.Extensibility
{
	public struct PlugInAction
	{
		
		public bool IsOnAMenu;
		public bool IsOnAToolbar;
		public bool IsANote;
		
		// If ParentMenuName = "" then will appear at toplevel
		public string ParentMenuName;
		// If "" then *ONLY* hotkey functions
		public string MyMenuName;
		
		// TODO Will tie into the Hotkey system later
		public int HotkeyNumber;
		public string GUID;

	


	}
}

