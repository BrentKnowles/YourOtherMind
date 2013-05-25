// NoteDataInterface.cs
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
using System.Drawing;
using System.Windows.Forms;

namespace Layout
{

	public interface NoteDataInterface
	{
		#region variables
#endregion
		#region UserSet
		string Caption {get; set;}
		Point Location {get; set;}
		int Height {get;set;}
		int Width{get;set;}
		bool Visible{get;set;}
		DockStyle Dock {get; set;}
		bool ReadOnly { get; set; }

		/// <summary>
		/// This field will be used to store RTF text in the TextBox, A Link to the picture file for Picture Notes, et cetera.
		/// </summary>
		/// <value>
		/// The data1.
		/// </value>
		string Data1 {get;set;}

		#endregion
		#region System
		string GuidForNote {get;set;}
		NotePanel ParentNotePanel {get;set;}

		// I did not want to do this but because panels are implemented "further down the chain" I need these two functions here
		bool IsPanel{get;} 
		// if true means a NoteLink can link to this
		bool IsLinkable {get;}
		bool IsSystemNote{get;}
		System.Collections.ArrayList   GetChildNotes();
		string GetStoryboardPreview{get;}
		void RespondToNoteSelection();
		#endregion

		void GetStoryboardData (out string sCaption, out string sValue, out int type, out string extraField);

		string GetLinkData ();




		// Methods
		#region methods
		/// <summary>
		/// Creates the parent. Called when creating a new Note, or Loading a Note file
		/// </summary>
		/// <param name='Layout'>
		/// Layout.
		/// </param>
		void CreateParent(LayoutPanelBase Layout);
		void Update(LayoutPanelBase Layout);
		void Destroy();

		Action<bool> SetSaveRequired { get; set; }
		void Save();
		 void BringToFrontAndShow();
		 void Flash();
		void Maximize (bool Maximize);
		void Minimize();
		string RegisterType();

		// for use with the F6 command in mainform (hiding the system panel)
		void ToggleTemporaryVisibility();
		/// <summary>
		/// Updates the location, called when location is set outside actual movement.
		/// </summary>
		void UpdateLocation ();

		// returns the GUID of the master containing object for this form
		string GetAbsoluteParent();
		void EndDrag ();
		void UpdateAfterLoad();
		// names of subnotes, if a panel or equivalent notes (used for accessiblity as NoteDataXML_Panel not alwasy available)
		System.Collections.Generic.List<string> ListOfSubnotes();
		#endregion;
	}

}

