// ADDIN_Fact.cs
//
// Unit test
//
// Copyright (c) 2013-2014 Brent Knowles (http://www.brentknowles.com)
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
using CoreUtilities;
using SendTextAway;
using System.IO;
using System.Collections.Generic;
using Layout;

namespace Testing
{
	[TestFixture]
	public class ADDIN_Fact
	{
		public ADDIN_Fact ()
		{
		}
		[Test]
		public void TestRemoteFactParse ()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			LayoutDetails.Instance.AddToList(typeof(ADD_Facts.NoteDataXML_Facts), "factsfromtest");


			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);


			//
			// #1 - Creating SOURCE layout
			//

			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);
			panel.SetCaption("SourcePanel");
			Assert.AreEqual("SourcePanel", panel.Caption);

			NoteDataXML_RichText basicNote = new NoteDataXML_RichText();
			basicNote.GuidForNote = "chapter1";
			basicNote.Caption = "chapter1";

			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			basicNote.SelectedText = "The dog runs[[dog]] fast.\n\t But we don't love him[[snake]]. We love aliens[[alien]].";
			//Assert.AreEqual(basicNote.GetAsText(), "The dog runs[[dog]] fast.\n But we don't love him. We love aliens[[alien]].");

			NoteDataXML_RichText basicNote2 = new NoteDataXML_RichText();
			basicNote2.GuidForNote = "chapter2";
			basicNote2.Caption = "chapter2";
			
			panel.AddNote(basicNote2);
			basicNote2.CreateParent(panel);
			basicNote2.SelectedText = "The cat runs[[cat]] fast.\n But we don't love him. \n\n\tWe love dogs[[dog]].";
			Assert.AreEqual(basicNote2.GetAsText(), "The cat runs[[cat]] fast.\n But we don't love him. \n\n\tWe love dogs[[dog]].");
			panel.SaveLayout();

			NoteDataXML_GroupEm storyboard = new NoteDataXML_GroupEm();
			storyboard.GuidForNote="storyboard";
			storyboard.Caption ="Storyboard";
			panel.AddNote (storyboard);
			storyboard.CreateParent(panel);

			panel.SaveLayout();
			Assert.AreEqual(5, panel.Count(), "why 5? Linktable + 3 should be 4? Nope: Rmemeber there is always a default note created too");
			Assert.AreEqual (0, storyboard.GetGroups ().Count);
			Assert.AreEqual(0, storyboard.CountStoryBoardItems());
			storyboard.AddRecordDirectly ("chapter1", basicNote.GuidForNote, "Chapters 1");
			Assert.AreEqual(1, storyboard.CountStoryBoardItems(), "We only added 1 record but group counts as a second?");
			storyboard.AddRecordDirectly ("chapter2", basicNote2.GuidForNote, "Chapters 1");
			Assert.AreEqual(2, storyboard.CountStoryBoardItems(), "We only added 1 record but group counts as a second?");

			Assert.AreEqual (1, storyboard.GetGroups ().Count);

			panel.SaveLayout();

			
			//
			// #2 - Creating DESTINATION layout -- this is where we invoke the FACT search on Layout #1
			//

			FAKE_LayoutPanel panel2 = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			panel2.NewLayout("mynewpanel2", true, null);

			ADD_Facts.NoteDataXML_Facts fact = new ADD_Facts.NoteDataXML_Facts();
			fact.GuidForNote="factnote";
			fact.Caption="Fact";
			panel2.AddNote (fact);
			fact.CreateParent(panel2);

			panel2.SaveLayout();
			Assert.AreEqual(3, panel2.Count(), "Default note + fact + linktable = 3");

			NoteDataXML_RichText basicNote3 = new NoteDataXML_RichText();
			basicNote3.GuidForNote = "factsource";
			basicNote3.Caption = "Fact Source";

			panel2.AddNote(basicNote3);
			basicNote3.CreateParent(panel2);
			basicNote3.SelectedText = "SourcePanel;[[Group,Storyboard,Chapters*,*]]";


			panel2.SaveLayout();
			Assert.AreEqual(4, panel2.Count(), "Default note + fact + linktable = 3");


			// intentionally testing this from the previous layout just incase somethingw eird has happened
			Assert.AreEqual(basicNote2.GetAsText(), "The cat runs[[cat]] fast.\n But we don't love him. \n\n\tWe love dogs[[dog]].");


//			06Anobreak;[[Group,Storyboard,Chapters*,*]]
//			06Bnobreak;[[Group,Storyboard,Chapters*,*]]

			string FactParseNote = "Fact Source";
			NoteDataXML_RichText note = (NoteDataXML_RichText)panel2.FindNoteByName (FactParseNote);
			Assert.NotNull(note);
			string textFromNote = note.GetAsText ();
			Assert.AreEqual("SourcePanel;[[Group,Storyboard,Chapters*,*]]", textFromNote);
			// now attempt the Fact Parse
			List<string> results = new List<string>();
			int error = fact.StartFactGathering(ref results, textFromNote); 
			Assert.AreEqual(0, error, "first pass");
			Assert.NotNull (results);

			Assert.AreEqual(0,results.Count, "How many matching facts found");


			fact.SetTag("dog");
			Assert.AreEqual("dog", fact.Token);
			fact.SetSaveRequired(true);
			panel2.SaveLayout();
			Assert.AreEqual("dog", fact.Token);



			//Assert.AreEqual(2,results.Count, "How many matching facts found. THIS SHOULD FAIL -- we have no markup assigned");
			YourOtherMind.iMarkupYourOtherMind markup = new YourOtherMind.iMarkupYourOtherMind();
			LayoutDetails.Instance.AddMarkupToList(markup);
			LayoutDetails.Instance.SetCurrentMarkup(markup);
			Assert.AreEqual("YourOtherMind", LayoutDetails.Instance.GetCurrentMarkup().ToString ());

			
			results = new List<string>();
			error = fact.StartFactGathering(ref results, textFromNote); 
			Assert.AreEqual(0, error, "second pass");

			Assert.AreEqual(2,results.Count, "How many matching facts found. Now that we have setup tag");

			fact.SetTag ("cat");
			results = new List<string>();
			error = fact.StartFactGathering(ref results, textFromNote); 
			Assert.AreEqual(1,results.Count, "How many matching facts found. Now that we have setup tag");

			fact.SetTag ("fish");
			results = new List<string>();
			error = fact.StartFactGathering(ref results, textFromNote); 
			Assert.AreEqual(0,results.Count, "How many matching facts found. Now that we have setup tag");

			fact.SetTag ("alien");
			results = new List<string>();
			error = fact.StartFactGathering(ref results, textFromNote); 
			Assert.AreEqual(1,results.Count, "How many matching facts found. Now that we have setup tag");
		}
	}
}

