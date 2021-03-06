// TimelineTest.cs
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
using NUnit.Framework;
using Layout;
using Timeline;

namespace Testing
{
	[TestFixture]
	public class TimelineTest
	{
		public TimelineTest ()
		{
		}

		[Test]
		public void TimelineAndTableAlwaysTogether()
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
			



			
			
			//panel.MoveNote(
			// create four panels A and B at root level. C inside A. D inside C
			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel ();
			panelA.Caption = "PanelA";
			panelA.GuidForNote = "panela";
			FAKE_NoteDataXML_Panel panelB = new FAKE_NoteDataXML_Panel ();
			panelB.Caption = "PanelB";
			panelB.GuidForNote = "panelb";
			FAKE_NoteDataXML_Panel panelC = new FAKE_NoteDataXML_Panel ();
			panelC.Caption = "PanelC";
			panelC.GuidForNote = "panelc";
			
			
			_w.output ("panels made");
			
			
			panel.AddNote (panelA);  // 1
			panel.AddNote (panelB);  // 2
			//panelA.CreateParent(panel); should not need to call this when doing LayoutPanel.AddNote because it calls CreateParent insid eof it
			
			basicNote = new NoteDataXML ();
			basicNote.Caption = "note2";
			
			
			
			panelA.AddNote (basicNote);  // Panel A has 1 note
			basicNote.CreateParent (panelA.myLayoutPanel ());  // DO need to call it when adding notes like this (to a subpanel, I think)
			panel.SaveLayout ();
			Assert.AreEqual (1, panelA.CountNotes (), "Panel A holds one note");  // So this counts as  + 2
			
			// so we have (1 + 1 note on it)panel A + (1)panelB + basicNote +DefaultNote = 5  + (NEW) LinkTable = 6
			_w.output ("STARTCOUNT");
			Assert.AreEqual (6, panel.CountNotes (), "Total notes SHOULD BE 6 :  (1 + 1 note on it)panel A + (1)panelB + basicNote +DefaultNote = 5  + (NEW) LinkTable = 6");
			
			_w.output ("ENDCOUNT");



			
			// add timeline into a PANEL
			// count 1 row
			NoteDataXML_Timeline MyTimeLine = new NoteDataXML_Timeline(100,100);
			MyTimeLine.Caption = "My Timeline!";
			panelA.AddNote(MyTimeLine);
			MyTimeLine.CreateParent(panelA.GetPanelsLayout());

			panel.SaveLayout(); // I needed this save else it would not work?

			Assert.AreEqual (8, panel.CountNotes (), "Total notes SHOULD BE 6 :  (1 + 1 note on it)panel A + (1)panelB + basicNote +DefaultNote = 5  + (NEW) LinkTable = 6");


			string guidOfTimeline = MyTimeLine.GuidForNote;
			string guidOfTimelineTable = guidOfTimeline + "table";

			NoteDataXML_Table FoundTable = (NoteDataXML_Table)panel.FindNoteByGuid(guidOfTimelineTable);
			Assert.NotNull(FoundTable);

			Assert.AreEqual (1, FoundTable.RowCount());
			FoundTable = null;
			
			// move the TABLE associated with the timeline OUT to parent
			// count 1 row


			panelA.myLayoutPanel ().MoveNote(guidOfTimelineTable, "up");

			// And for kicks add another timeline just to see if it messages anytnig up
			NoteDataXML_Timeline MyTimeLine2 = new NoteDataXML_Timeline(100,1020);
			MyTimeLine2.Caption = "My Timeline! #2";
			panel.AddNote(MyTimeLine2);

			panel.SaveLayout();
			Assert.AreEqual (10, panel.CountNotes (), "Total notes SHOULD BE 6 :  (1 + 1 note on it)panel A + (1)panelB + basicNote +DefaultNote = 5  + (NEW) LinkTable = 6");


			Assert.Null(FoundTable);
			FoundTable = (NoteDataXML_Table)panel.FindNoteByGuid(guidOfTimelineTable);
			Assert.NotNull(FoundTable);
			
			Assert.AreEqual (1, FoundTable.RowCount());

			// move the TABLE into ANOTHER panel
			// count 1 row
			_w.output("Moving into panelb now");
			panel.MoveNote(guidOfTimelineTable, "panelb");
			panel.SaveLayout();

			Assert.AreEqual (10, panel.CountNotes (), "Total notes SHOULD BE 6 :  (1 + 1 note on it)panel A + (1)panelB + basicNote +DefaultNote = 5  + (NEW) LinkTable = 6");
			_w.output("done counting");

			FoundTable = null;
			Assert.Null(FoundTable);
			FoundTable = (NoteDataXML_Table)panel.FindNoteByGuid(guidOfTimelineTable);
			Assert.NotNull(FoundTable);
			
			Assert.AreEqual (1, FoundTable.RowCount());
			form.Dispose ();
		}



		[Test]
		[ExpectedException]
		public void NullTimelineIsException()
		{
			NotePanelTimeline timeline = new NotePanelTimeline(null);
		}

	}
}

