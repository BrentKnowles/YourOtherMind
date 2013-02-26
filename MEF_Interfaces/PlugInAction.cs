using System;
using CoreUtilities;
namespace MefAddIns.Extensibility
{
	public struct PlugInAction
	{
		// Jan 2013 - Considered removing this struct, but one benefit (though a lazy one) is that I don't have to create get;set; for everything I pass arond
		public bool IsOnAMenu;

		// Have to remove this because there will be multiple Toolbars, not able to remove it. Will have to suffice on a Menu
		//public bool IsOnAToolbar;
		// Optional. Some elements will display an image
		public System.Drawing.Bitmap Image; 
	
		public bool IsNoteAction;
		public bool IsANote;
	

		/// <summary>
		/// 	// Will show a quicklink on the Footer, while this is running. You must override ACTIVEFORM for this to work.
		/// </summary>
		public bool QuickLinkShows;
		// If ParentMenuName = "" then will appear at toplevel
		public string ParentMenuName;
		// If "" then *ONLY* hotkey functions
		public string MyMenuName;
		// if true then ParentMenuName must be the name of a valid ContextMenuStrip
		public bool IsOnContextStrip;

		public string ToolTip;
		public string GUID;

		// if blank NoteActions use the standard menu label otherwise they use this
		public string NoteActionMenuOverride;

	


	}
}

