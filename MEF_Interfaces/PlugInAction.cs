using System;

namespace MefAddIns.Extensibility
{
	public struct PlugInAction
	{
		
		public bool IsOnAMenu;

		// Have to remove this because there will be multiple Toolbars, not able to remove it. Will have to suffice on a Menu
		//public bool IsOnAToolbar;

		// Optional. Some elements will display an image
		public System.Drawing.Bitmap Image; 
		public bool IsNoteAction;
		public bool IsANote;
		// Will show a quicklink on the Footer, while this is running
		public bool QuickLinkShows;
		// If ParentMenuName = "" then will appear at toplevel
		public string ParentMenuName;
		// If "" then *ONLY* hotkey functions
		public string MyMenuName;
		// if true then ParentMenuName must be the name of a valid ContextMenuStrip
		public bool IsOnContextStrip;
		// TODO Will tie into the Hotkey system later
		public int HotkeyNumber;
		public string ToolTip;
		public string GUID;

	


	}
}

