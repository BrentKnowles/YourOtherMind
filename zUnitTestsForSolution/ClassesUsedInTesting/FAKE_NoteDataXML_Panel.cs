// FAKE_NoteDataXML_Panel.cs
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

namespace Testing
{
	public class FAKE_NoteDataXML_Panel : LayoutPanels.NoteDataXML_Panel
	{
		public FAKE_NoteDataXML_Panel ()
		{
		}
		public FAKE_NoteDataXML_Panel (int _height, int _width):base (_height, _width)
		{
		}

//		public Layout.LayoutDatabase TheNotesDatabase()
//		{
//			return this.panelLayout.Notes
//		}

		public Layout.LayoutPanel myLayoutPanel()
		{
			return (Layout.LayoutPanel)this.panelLayout;
		}
		public int CountNotes ()
		{
			int count = 0;
			System.Collections.ArrayList list = this.panelLayout.GetAllNotes();
			foreach (Layout.NoteDataInterface note in list) {
				count++;
				_w.output(String.Format ("**{0}** GUID = {1}", count, note.GuidForNote));
			}
			return count;
			//return this.panelLayout.CountNotes();
		}
		public string GetParent_Section()
		{
			return this.panelLayout.Section;
		}
		public string GetParent_Keywords()
		{
			return this.panelLayout.Keywords;
		}
		public string GetParent_Notebook()
		{
			return this.panelLayout.Notebook;
		}
		public string GetParent_Subtype()
		{
			return this.panelLayout.Subtype;
		}
		public string GetParent_ParentGuid()
		{
			return this.panelLayout.ParentGUID;
		}
	}
}

