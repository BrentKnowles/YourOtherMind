// PlugInAction.cs
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
using CoreUtilities;
namespace MefAddIns.Extensibility
{
	public class PlugInAction
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
		public string NoteActionMenuOverride=Constants.BLANK;

	

	


	}
}

