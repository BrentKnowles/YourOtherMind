// ADDIN_Archive_Test.cs
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
using System.IO;
using CoreUtilities;
using System.Windows.Forms;
namespace Testing
{
	[TestFixture]
	public class ADDIN_Archive_Test
	{
		public ADDIN_Archive_Test ()
		{
		}
		[Test]
		public void ArchiveAllAndCount()
		{
			lg.Instance.OutputToConstoleToo = true;
			Form form = new Form();
			string ThisLayoutGUID = "mynewpanelXA";
			// create a layout
			_TestSingleTon.Instance._SetupForLayoutPanelTests ();
			//	_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			form.Controls.Add (panel);
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout (ThisLayoutGUID, true, null);
			
			NoteDataXML basicNote = new NoteDataXML ();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote (basicNote);
			//basicNote.CreateParent (panel);
			panel.SaveLayout ();
			
			
			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel ();
			panelA.Caption = "PanelA";
			panelA.GuidForNote = "panela";
			panel.AddNote (panelA);
			panelA.CreateParent (panel);
			
			
			FAKE_NoteDataXML_Panel panelB = new FAKE_NoteDataXML_Panel ();
			panelB.Caption = "PanelBBBBB2";
			panelB.GuidForNote = "panelBB";
			panelA.AddNote (panelB);
			panelB.CreateParent (panelA.GetPanelsLayout());
			FAKE_NoteDataXML_Text textNoteA = new FAKE_NoteDataXML_Text ();
			
			panelB.AddNote (textNoteA);
			textNoteA.GuidForNote = "My Text Note For the B Panel";
			textNoteA.CreateParent (panelB.GetPanelsLayout ());
			textNoteA.Caption = "My text B";
			
			
			
			FAKE_NoteDataXML_Text textNote = new FAKE_NoteDataXML_Text ();
			
			panelA.AddNote (textNote);
			textNote.GuidForNote = "Text Note For Panel A";
			textNote.CreateParent (panelA.GetPanelsLayout ());
			textNote.Caption = "My text A";
			textNote.GetRichTextBox ().Text = "Hello there." +Environment.NewLine +"I am still here, are you?" + Environment.NewLine+"Yep!";
			panel.SaveLayout ();
			Assert.AreEqual (7, panel.CountNotes (), "Count1");
			
			// Note count: Default Note + BasicNote+ PanelA + LinkTable + MyText  + PanbelB + My Text B  =7
			



			//test existence
			Assert.True (MasterOfLayouts.ExistsByGUID (ThisLayoutGUID));
			// counting our subpanel we have 2
			Assert.AreEqual (3, MasterOfLayouts.Count (true));
			// NOT counting our subpanel we have 1
			Assert.AreEqual (1, MasterOfLayouts.Count (false));
			
			LayoutDetails.Instance.CurrentLayout = panel;
			MefAddIns.Addin_Archive Archiver = new MefAddIns.Addin_Archive();

			// delete existing files
			string path = Archiver.BuildArchiveDepotPath();
			string[] files = Directory.GetFiles(path);
			foreach (string s in files)
			{
			File.Delete (s);
			}
			files = Directory.GetFiles(path);

			Assert.AreEqual (0, files.Length);


			Archiver.ArchiveAll(panel, "automatedtesting");
			files = Directory.GetFiles(path);
			Assert.AreEqual (3, files.Length);
		}
	}
}

