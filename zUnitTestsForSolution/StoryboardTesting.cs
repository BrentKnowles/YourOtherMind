// StoryboardTesting.cs
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
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Storyboards;
using Layout;
namespace Testing
{
	[TestFixture]
	public class StoryboardTesting
	{
		public StoryboardTesting ()
		{
		}
		[Test]
		[ExpectedException("System.Exception")]
		public void testExceptionAddItem()
		{
			Storyboard groupEm = new Storyboard();
		//	groupEm.AllowDrop = false;
			groupEm.AddItem("test", "test", 0,"");
			
		}
		[Test]
		[ExpectedException("System.Exception")]
		public void testExceptionEditItem()
		{
			Storyboard groupEm = new Storyboard();
			groupEm.EditItem(null);
			
		}
		
		[Test]
		[ExpectedException("System.Exception")]
		public void testExceptionDeleteItem()
		{
			Storyboard groupEm = new Storyboard();
			groupEm.DeleteItem(null);
			
		}
		[Test]
		public void TestAddItemsToStoryboard()
		{
			_w.output ("START");
			System.Windows.Forms .Form form = new System.Windows.Forms.Form ();
			_TestSingleTon.Instance._SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			form.Controls.Add (panel);
			form.Show ();
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout ("mynewpanel", true, null);
			NoteDataXML basicNote = new NoteDataXML ();
			basicNote.Caption = "note1";
			
			panel.AddNote (basicNote);
			//basicNote.CreateParent(panel);
			
			
			NoteDataXML_GroupEm StoryBoard = new NoteDataXML_GroupEm();
			panel.AddNote (StoryBoard);
			
			//panel.MoveNote(
			// create four panels A and B at root level. C inside A. D inside C
			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel ();
			panelA.Caption = "PanelA";
			panel.AddNote (panelA);  // 1



			StoryBoard = new NoteDataXML_GroupEm();
			StoryBoard.GuidForNote = "storyboard2";
			StoryBoard.Caption = "storyboard";
			panelA.AddNote (StoryBoard);
			StoryBoard.CreateParent(panelA.GetPanelsLayout());

			Assert.AreEqual (0, StoryBoard.CountStoryBoardItems());


			CoreUtilities.Links.LinkTableRecord record = new CoreUtilities.Links.LinkTableRecord();
			record.sFileName = "PanelA";//StoryBoard.GuidForNote;
			record.sKey = "*";
			record.sText="*";
			record.sSource = StoryBoard.GuidForNote;
			panel.GetLinkTable().Add (record);
			Assert.AreEqual (1,panel.GetLinkTable ().Count());
			StoryBoard.Refresh();
			Assert.AreEqual (1, StoryBoard.CountStoryBoardItems());
		}
	}
}



