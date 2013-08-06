// NoteDataXML_SystemOnly.cs
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
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Layout
{
	/// <summary>
	/// Note data XM l_ system only.
	/// 
	/// This is a special note type that is only allowed on the System note. It 'holds' other notes and is the target of the NoteList (when in Layout List mode)
	/// 
	/// </summary>
	public class NoteDataXML_SystemOnly: NoteDataXML
	{
		public NoteDataXML_SystemOnly () : base()
		{
			// do not want system layouts to have captions
			Caption = Constants.BLANK;
		}
		// this delegate is setup when the window is open and fired if user closes the note
		public Action<LayoutPanelBase> AlertWhenClosed;
		public NoteDataXML_SystemOnly(int _height, int _width) : base(_height, _width)
		{
			// do not want system layouts to have captions
			Caption = Constants.BLANK;
		}
		[XmlIgnore]
		public override bool IsSystemNote{
			get {return true;}
		}

	

		protected override void DoBuildChildren (LayoutPanelBase Layout)
		{
			//Width = 500;
			base.DoBuildChildren (Layout);
		//	GetChildLayout().SystemNote = true;
			CaptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
			//CaptionLabel.Visible = false;

		//	Maximize(true);
			ParentNotePanel.Leave+= HandleLeave;
			//Parent.Enter+= HandleEnter;
			//ParentNotePanel.GotFocus+= HandleGotFocus;
			//ParentNotePanel.MouseEnter+= HandleMouseEnter;
			ParentNotePanel.BringToFront();
			//Parent.Dock = System.Windows.Forms.DockStyle.Left;
			Dock = DockStyle.Fill;
			UpdateLocation();
		//	Parent.BringToFront();
		}

		void HandleLeave (object sender, EventArgs e)
		{
			//NewMessage.Show ("leave");
			// This is still not fool proof (July 2013)
			Save ();
			GetChildLayout().DisableLayout(true);
		//	ParentNotePanel.Enabled = false;

		}

		void HandleGotFocus (object sender, EventArgs e)
		{
			// never worked the way I needed
			//NewMessage.Show ("Got Focus for " + Caption);
		}

		/// <summary>
		/// Responds to note selection.  If we click on the note it must work very similar to how SELECTING the note in the Windows Menu works
		/// Especially the setting of the current layout so that findbars and whatnot work.
		/// </summary>
		public override void RespondToNoteSelection ()
		{
			base.RespondToNoteSelection ();
			if (this.LayoutAssociatedWithThisSystemNote != null) {
				bool AlreadyAmCurrent = false;
				if (LayoutDetails.Instance.CurrentLayout != null) {
					if (LayoutDetails.Instance.CurrentLayout != this.LayoutAssociatedWithThisSystemNote)
					{
					LayoutDetails.Instance.CurrentLayout.SaveLayout ();
					}
					else
					{
						AlreadyAmCurrent = true;
					}
				}

				if (false == AlreadyAmCurrent) {
					LayoutDetails.Instance.CurrentLayout = this.LayoutAssociatedWithThisSystemNote;
				}
			}


		}
		LayoutPanelBase LayoutAssociatedWithThisSystemNote = null;
		/// <summary>
		/// Adds A layout reference. This is set in the mainform when a layout is loaded.
		/// We hold the LayoutPanel for the situation when the user CLICKS on the note
		/// (instead of moving between notes via a menu
		/// </summary>
		public void AddALayout (LayoutPanelBase _LayoutAssociatedWithThisSystemNote)
		{
			LayoutAssociatedWithThisSystemNote = _LayoutAssociatedWithThisSystemNote;
			ParentNotePanel.Controls.Add (LayoutAssociatedWithThisSystemNote);
		}

		/// <summary>
		/// Gets the child layout.
		/// 
		/// called when needing to remove the Layout from the Window List (i.e., X clicked to close it)
		/// </summary>
		/// <returns>
		/// The child layout.
		/// </returns>
		public LayoutPanelBase GetChildLayout()
		{
			foreach (Control control in ParentNotePanel.Controls) {
				if (control.GetType ().BaseType == typeof(LayoutPanelBase))
				{
					//NewMessage.Show ("FOUND : " + ((LayoutPanelBase)control).GUID);
					return ((LayoutPanelBase)control);
					//LayoutDetails.Instance.CurrentLayout =((LayoutPanelBase)control);

				}
			}
			return null;
		}
		public string GetLayoutName()
		{
			return GetChildLayout().Caption;

		}
		public string GetLayoutGUID()
		{
			return GetChildLayout().GUID;
		}
		/// <summary>
		/// Handles the mouse enter.
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleMouseEnter (object sender, EventArgs e)
		{

			// July 2013 - Disabled this entirely Move this to a click on the NAME label... Document

			// do not worry about lack of return value here or anything because we are setting the layout in the loop
			// July 2013 - Update. Yes, do worry about the lack of return. At some point I changed the behavior on this and it broke things.
//			LayoutDetails.Instance.CurrentLayout = GetChildLayout ();
//			if (null == LayoutDetails.Instance.CurrentLayout) {
//				NewMessage.Show (Loc.Instance.GetString ("No Window defined"));
//			}
		}

		void HandleEnter (object sender, EventArgs e)
		{

		}
	

		/// <summary>
		/// Registers the type.
		/// </summary>
		public override string RegisterType()
		{
			return Loc.Instance.Cat.GetString("System");
		}

		public void DoCloseNote (bool save)
		{
			if (true == save) {
				//NewMessage.Show ("saving when note closes not implemented yet");
				Save ();
			}

			if (AlertWhenClosed != null) {
				// gets removed from the Windows list
				AlertWhenClosed (GetChildLayout ());
			}
			
			
			
			
			if (GetChildLayout().GetSaveRequired == true) {
				// March 7 2013
				// Found this message still here WITHOUT an explict save.
				// Added the save
				GetChildLayout().SaveLayout ();
			//	NewMessage.Show("HandleCloseClick Save is required ");
			}
			
			//			if (CloseNoteDelegate != null) {
			//				CloseNoteDelegate(true);
			//			}
			
			DeleteNote(this);
		}
		protected override void HandleCloseClick (object sender, EventArgs e)
		{

			DoCloseNote (false);

		}
	}
}

