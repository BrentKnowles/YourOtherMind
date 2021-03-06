// LayoutPanelTests.cs
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
using LayoutPanels;
using database;
using CoreUtilities;
using YourOtherMind;

namespace Testing
{
	[TestFixture]
	public class LayoutPanelTests
	{
		public LayoutPanelTests ()
		{
		}

//		protected void _SetupForLayoutPanelTests ()
//		{
//			LayoutDetails.Instance.YOM_DATABASE = "yom_test_database.s3db";
//			LayoutDetails.Instance.AddToList(typeof(FAKE_NoteDataXML_Panel),"testingpanel");
//			LayoutDetails.Instance.AddToList(typeof(FAKE_NoteDataXML_Text),"testingtext");
//
//			FakeLayoutDatabase layout = new FakeLayoutDatabase("testguid");
//			FAKE_SqlLiteDatabase db = new FAKE_SqlLiteDatabase(layout.GetDatabaseName ());
//			db.DropTableIfExists(Layout.data.dbConstants.table_name);
//			_w.output ("dropping table " + Layout.data.dbConstants.table_name);
//		}

//		[Test]
//		public void NoteListFailsWithoutASystemLayout()
//		{
//			_TestSingleTon.Instance._SetupForLayoutPanelTests();
//			
//			
//			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
//
//			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
//			form.Controls.Add (panel);
//			// needed else DataGrid does not initialize
//			
//			form.Show ();
//			//form.Visible = false;
//			
//			//NOTE: For now remember that htis ADDS 1 Extra notes
//			string panelname = System.Guid.NewGuid().ToString();
//			panel.NewLayout (panelname,true, null);
//			LayoutDetails.Instance.AddToList (typeof(FAKE_NoteDataXML_Panel), "testingpanel");
//
//			panel.SaveLayout();
//
//			NoteDataXML_NoteList lister = new NoteDataXML_NoteList();
//			panel.AddNote(lister);
//			lister.CreateParent(panel);
//
//			panel.SaveLayout();
//		}

		[Test]
		public void Quicker_UpdateAfterLoadTest()
		{

				_TestSingleTon.Instance._SetupForLayoutPanelTests();
				
				
				System.Windows.Forms .Form form = new System.Windows.Forms.Form();
				
				
				
				
				FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
				
				form.Controls.Add (panel);
				
				// needed else DataGrid does not initialize
				
				form.Show ();
				//form.Visible = false;
				_w.output("boom");
				// March 2013 -- notelist relies on having this
				YOM2013.DefaultLayouts.CreateASystemLayout(form,null);
				
				
				//NOTE: For now remember that htis ADDS 1 Extra notes
				string panelname = System.Guid.NewGuid().ToString();
				panel.NewLayout (panelname,true, null);
				LayoutDetails.Instance.AddToList (typeof(FAKE_NoteDataXML_Panel), "testingpanel");
				_w.output ("herefirst");
				
				// ADD 1 of each type
				foreach (Type t in LayoutDetails.Instance.ListOfTypesToStoreInXML()) {
					for (int i = 0; i < 5; i++) {
						NoteDataInterface note = (NoteDataInterface)Activator.CreateInstance (t);
						panel.AddNote (note);
						note.CreateParent(panel);

					note.UpdateAfterLoad();
					}
				}

			panel.SaveLayout();
			form.Dispose ();
		}
		[Ignore]
		[Test]
		public void SpeedTest ()
		{
		// this will be a benchmarking test that will create a complicated Layout
			// Then it will time and record the results of LOADING and SAVING that layout into a 
			// table saved in my backup paths
			// will also output a DAAbackup file (text readable) format too
			_TestSingleTon.Instance._SetupForLayoutPanelTests();


			System.Windows.Forms .Form form = _TestSingleTon.Instance.FORM;//new System.Windows.Forms.Form();
		
	


			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);

			form.Controls.Add (panel);
			
			// needed else DataGrid does not initialize
			
			form.Show ();
			//form.Visible = false;
			_w.output("boom");
			// March 2013 -- notelist relies on having this
			YOM2013.DefaultLayouts.CreateASystemLayout(form,null);


			//NOTE: For now remember that htis ADDS 1 Extra notes
			string panelname = System.Guid.NewGuid().ToString();
			panel.NewLayout (panelname,true, null);
			LayoutDetails.Instance.AddToList (typeof(FAKE_NoteDataXML_Panel), "testingpanel");
			_w.output ("herefirst");

			// ADD 1 of each type
			foreach (Type t in LayoutDetails.Instance.ListOfTypesToStoreInXML()) {
				for (int i = 0; i < 10; i++) {
					NoteDataInterface note = (NoteDataInterface)Activator.CreateInstance (t);
					panel.AddNote (note);
					note.CreateParent(panel);
				}
			}
			_w.output("here");
			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel ();
			panelA.Caption = "PanelA";
			panel.AddNote (panelA);
			string stringoftypes = "";
			foreach (Type t in LayoutDetails.Instance.ListOfTypesToStoreInXML()) {
				NoteDataInterface note = (NoteDataInterface)Activator.CreateInstance (t);
				panelA.AddNote (note);
				stringoftypes = stringoftypes + " " + t.ToString();
			}
			panel.SaveLayout();
			string base_path = _TestSingleTon.PATH_TO_SpeedTestFiles;//@"C:\Users\BrentK\Documents\Keeper\Files\yomspeedtests2013\";
			_w.output ("here");
			NoteDataXML_RichText richy;
			for (int i = 0; i < 20; i ++) {
				richy = new NoteDataXML_RichText ();
				richy.Caption = "richtext";
				panel.AddNote (richy);
				Assert.True (richy.GetIsRichTextBlank ());
				richy.DoOverwriteWithRTFFile (System.IO.Path.Combine (base_path,"speedtest.rtf"));
				Assert.False (richy.GetIsRichTextBlank ());
			}
			_w.output("First save");
			panel.SaveLayout();
			string table = "layoutpanelsaveload";
			// Now try and write this data out.
			SqlLiteDatabase timetracking = new SqlLiteDatabase(System.IO.Path.Combine (base_path,"speedtests.s3db"));
			timetracking.CreateTableIfDoesNotExist(table, new string [5] {"id", "datetime", "timetook", "types","saveorload"},
			new string[5] {"INTEGER", "TEXT", "FLOAT", "TEXT", "TEXT"}, "id");


			// * Now start the Load Test
			TimeSpan time;
			CoreUtilities.TimerCore.TimerOn = true;
			time = CoreUtilities.TimerCore.Time (() => {
			panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
				form.Controls.Add (panel);
				panel.LoadLayout(panelname, false,null);
			});
			_w.output("TIME " + time);

	

			timetracking.InsertData(table, new string[4] {"datetime", "timetook", "types","saveorload"},new object[4] {DateTime.Now.ToString (),
				time.TotalSeconds, stringoftypes,"load"});

			time = CoreUtilities.TimerCore.Time (() => {
				// We keep the PANEL from above! Don't recreate it.
				//panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK);
				panel.SaveLayout();
			});


			Console.WriteLine("TIME " + time);

			timetracking.InsertData(table, new string[4] {"datetime", "timetook", "types","saveorload"},new object[4] {DateTime.Now.ToString (),
				time.TotalSeconds, stringoftypes,"save"});

			string backup = timetracking.BackupDatabase();

			System.IO.TextWriter write = new System.IO.StreamWriter(System.IO.Path.Combine (base_path,"timeresults.txt"));
			write.WriteLine (backup);

			write.Close();

			FakeLayoutDatabase layout = new FakeLayoutDatabase("testguid");
			FAKE_SqlLiteDatabase db = new FAKE_SqlLiteDatabase(layout.GetDatabaseName ());
			_w.output("Backup of stored database: " + db.BackupDatabase());

			timetracking.Dispose();
			db.Dispose();

		}

		/// <summary>
		/// Tests the moving notes.
		/// </summary>
		[Test]
		public void TestMovingNotes ()
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

			//COUNT SHOULD BE: panelA has 1 note
			//COUNT Total should be: Default Note + Note + PanelA + Panel B  + (Note Inside Panel A) = 5

			panelA.AddNote (panelC);  // panelC is now part of Panel 2
			panelC.CreateParent (panelA.myLayoutPanel ());
			panel.SaveLayout (); // NEED TO SAVE before conts will be accurated



			//COUNT SHOULD BE: panelA has 2 notes (Panel C + the Note I add)

			_w.output ("START COUNT SUB");
			Assert.AreEqual (2, panelA.CountNotes (), "two notes in panelA");
			_w.output ("END COUNT SUB");

			Assert.AreEqual (7, panel.CountNotes (), "total of SEVEN notes");

			_w.output ("START COUNT SUB2");
			Assert.AreEqual (0, panelB.CountNotes (), "0 count worked?");
			_w.output ("END COUNT SUB2");
			//COUNT Total should be: Default Note + Note + PanelA + Panel B + (NoteInside Panel A) + (Panel C Inside Panel A) = 6

			FAKE_NoteDataXML_Panel panelD = new FAKE_NoteDataXML_Panel ();
			panelD.Caption = "PanelD";
			panelC.AddNote (panelD); // panelC which is inside PANELA now adds panelD (which is empty?)
			panelD.CreateParent (panelC.myLayoutPanel ());




			panel.SaveLayout ();
			_w.output ("START COUNT SUB3");
			Assert.AreEqual (0, panelD.CountNotes (), "No notes in panelD");
			_w.output ("START COUNT SUB3");
			/*
			NoteDataXML_RichText extra = new NoteDataXML_RichText();
			panelD.AddNote (extra); //this was here only to test that there is an error with counting EMPTY panels
			extra.CreateParent(panelD.myLayoutPanel());
			panel.SaveLayout();*/
			// update on ERROR: Every note in D is being counted DOUBLE


			// weird error: If a panel is empty it will register as +1 (i.e, counted twice)

			// PanelC and PanelA are adding this note
//			AddChildrenToList, TYPE: TEMPORARY, scanning children...47d43f01-a031-42d1-8539-bb7ae284a9e1 from PanelC
		
//			AddChildrenToList, TYPE: TEMPORARY, scanning children...47d43f01-a031-42d1-8539-bb7ae284a9e1 from PanelA

			//OK: Every note is claiming ownership of the TEXT NOTE, which is the problem, I think.
			_w.output ("START COUNT SUB4");
			Assert.AreEqual (1, panelC.CountNotes (), "1 notes in panelC");
			_w.output ("E COUNT SUB4");
			Assert.AreEqual (0, panelD.CountNotes (), "1 note in panelD");
			_w.output ("_------------------------------");
			_w.output ("panel.CountNotes=" +panel.CountNotes ());

			_w.output ("_------------------------------");
			Assert.AreEqual (8, panel.CountNotes (), "We have only added one panel so we should be up to 8");

			Assert.AreEqual (3, panelA.CountNotes ());

			Assert.AreEqual (8, panel.CountNotes (), "number of notes in main panel");
			Assert.AreEqual (0, panelB.CountNotes (), "testt");
			Assert.AreEqual (1, panelC.CountNotes (), "testt");

			// add a note to panel d (we want to make sure this note does not disappear while D is being moved around
			NoteDataXML noteford = new NoteDataXML ();
			noteford.Caption = "note for d";
			panelD.AddNote (noteford);


			// Move panel D from panelC into PANEL A
			panelC.myLayoutPanel ().MoveNote (panelD.GuidForNote, "up");
			panel.SaveLayout ();
			Assert.AreEqual (4, panelA.CountNotes ()); // 4 because I added a note to D which is inside A
			Assert.AreEqual (9, panel.CountNotes ());
			Assert.AreEqual (0, panelC.CountNotes ());
			Assert.AreEqual (0, panelB.CountNotes ());
			Assert.AreEqual (1, panelD.CountNotes ());
			// Move panel D from panelA into ROOT
			panelA.myLayoutPanel ().MoveNote (panelD.GuidForNote, "up");
			panel.SaveLayout ();
			Assert.AreEqual (2, panelA.CountNotes ());
			Assert.AreEqual (9, panel.CountNotes ());
			Assert.AreEqual (0, panelC.CountNotes ()); // ** FINE HERE
			_w.output ("do c twice, what happens?");
			Assert.AreEqual (0, panelC.CountNotes ()); 
			Assert.AreEqual (0, panelB.CountNotes ());
			Assert.AreEqual (1, panelD.CountNotes ());


			lg.Instance.Loudness = Loud.ACRITICAL;

			_w.output ("START COUNT SUB5");
			NoteDataXML_RichText richy = new NoteDataXML_RichText ();
			richy.GuidForNote = "richy";
			panel.AddNote (richy);
			richy.CreateParent (panel);
			panel.SaveLayout ();
			_w.output ("do c THRICE, what happens?");
			Assert.AreEqual (0, panelC.CountNotes ()); 
			_w.output ("E COUNT SUB5");
			// now move note into A
			_w.output ("move richy FROM PANEL into PanelA (PanelA also holds PanelC). The exact logic seems to be that IN THE FOLLOWING MOVE, PANELA Gets Disposed of??!?");
			panel.MoveNote (richy.GuidForNote, panelA.GuidForNote);
			panel.SaveLayout ();
			_w.output ("do c 4x, what happens?");
			Assert.AreEqual (0, panelC.CountNotes ()); 


			_w.output ("done move richy");
			Assert.AreEqual (3, panelA.CountNotes ());
			Assert.AreEqual (10, panel.CountNotes ());
			_w.output ("countc");
			Assert.AreEqual (0, panelC.CountNotes ());  // * Jan 19 2013 - brok ehere
			_w.output ("e countc");
			_w.output ("countb");

			Assert.AreEqual (0, panelB.CountNotes ());
			_w.output ("e countb");

			_w.output ("Panel A Notes " + panelA.CountNotes ().ToString ());
			_w.output ("Total Notes " + panel.CountNotes ());
			// also see if this test or another could replicate the problems I had in previous version with RefhresTabs
			panel.SaveLayout ();

			// now do a test to load it
			panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			//form.Controls.Add (panel);
			//form.Show ();

			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.LoadLayout ("mynewpanel", false, null);
			_w.output ("getting notes for " + panel.Caption);
			System.Collections.ArrayList TheNotes = panel.GetAllNotes ();

		//	System.Collections.Generic.List<NoteDataInterface> list = new System.Collections.Generic.List<NoteDataInterface> ();
			//list.AddRange ((NoteDataInterface[])TheNotes.ToArray ());
			int count = 0;
			foreach (NoteDataInterface note in TheNotes) {
				if (note.GuidForNote == CoreUtilities.Links.LinkTable.STICKY_TABLE)
				{
					count++;
				}
			}
			// make sure there is only one linktable
			Assert.AreEqual(1, count);

			//.NoteDataInterface[] found = (NoteDataInterface)list.Find (NoteDataInterface=>NoteDataInterface.GuidForNote == CoreUtilities.Links.LinkTable.STICKY_TABLE );
				//	Assert.True (false);
		}
			[Test]
		public void AdvancedSearchingForNotesTests()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			//_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);
			NoteDataXML basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();


			FAKE_NoteDataXML_Panel DiversionPanel = new FAKE_NoteDataXML_Panel();
			DiversionPanel.Caption ="WeAdd this to make the FindFail later by having an empty layout that sends buggy searches down the wrong corridor";
			panel.AddNote(DiversionPanel);
			DiversionPanel.CreateParent(panel);


			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel();
			panelA.Caption = "PanelA";
			panel.AddNote(panelA);
			panelA.CreateParent(panel);

			basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid2";
			basicNote.Caption = "note2";

			panelA.AddNote(basicNote);  // Panel A has 1 note

			basicNote.CreateParent(panelA.myLayoutPanel());  // DO need to call it when adding notes like this (to a subpanel, I think)
			panel.SaveLayout();

			NoteDataInterface finder = null;
			finder = panel.FindNoteByGuid("thisguid1");
			Assert.NotNull(finder.ParentNotePanel);
			_w.output("C: " +finder.Caption);
			Assert.AreEqual(finder.Caption, "note1");
			Assert.NotNull (finder.ParentNotePanel );
			finder = null;

			// this note is on a subpanel
			finder = panel.FindNoteByGuid("thisguid2");

			Assert.NotNull(finder);
			Assert.AreEqual (finder.Caption,"note2");

			finder= panel.GoToNote(finder);
			// added this February 2013 because this code stopped finding notes within subpanels for some reason
			Assert.NotNull(finder.ParentNotePanel);
			// find notes inside of notes with the GUI code and such
		
			// now test the fast search function
			NoteDataInterface foundNote2 = panel.GetNoteOnSameLayout(finder.GuidForNote, false);
			Assert.NotNull (foundNote2);

			Assert.AreEqual (finder.GuidForNote, foundNote2.GuidForNote);
			Assert.NotNull (foundNote2.ParentNotePanel);
		}
		[Test]
		public void SetCurrentTextNoteAndDetectThisInParentLayouts()
		{

			// nest several panels
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);
			NoteDataXML basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();
			
			
			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel();
			panelA.Caption = "PanelA";
			panel.AddNote(panelA);
			panelA.CreateParent(panel);
			
			basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid2";
			basicNote.Caption = "note2";
			
			panelA.AddNote(basicNote);  // Panel A has 1 note
			
			basicNote.CreateParent(panelA.myLayoutPanel());  // DO need to call it when adding notes like this (to a subpanel, I think)
			panel.SaveLayout();



			FAKE_NoteDataXML_Panel panelB = new FAKE_NoteDataXML_Panel();
			panelB.Caption = "PanelB";
			panelA.AddNote(panelB);
			panelB.CreateParent(panelA.myLayoutPanel());

			FAKE_NoteDataXML_Panel panelC = new FAKE_NoteDataXML_Panel();
			panelC.Caption = "PanelC";
			panelB.AddNote(panelC);
			panelC.CreateParent(panelB.myLayoutPanel());

			FAKE_NoteDataXML_Panel panelD = new FAKE_NoteDataXML_Panel();
			panelD.Caption = "PanelD";
			panelC.AddNote(panelC);
			panelD.CreateParent(panelC.myLayoutPanel());

			//Add a text note
			FAKE_NoteDataXML_Text texter = new FAKE_NoteDataXML_Text();
			panelD.AddNote(texter);
			texter.CreateParent(panelD.myLayoutPanel());

			// Set this text note as active
			texter.SetActive();

			// Go back to each of the owner panels and make sure their CurrentTextNote is equal to this one
			Assert.AreEqual (panelD.myLayoutPanel().CurrentTextNote, texter);
			Assert.AreEqual (panelC.myLayoutPanel().CurrentTextNote, texter);
			Assert.AreEqual (panelB.myLayoutPanel().CurrentTextNote, texter);
			Assert.AreEqual (panelA.myLayoutPanel().CurrentTextNote, texter);
			Assert.AreEqual (panel.CurrentTextNote, texter);

		}
		[Test]
		[ExpectedException]
		public void CallingSetFieldsOnAParent()
		{
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
			panel.SetParentFields("","","","");
		}
		[Test]
		public void TestThatChildInheritsKeywordsAndOtherDetailsOfParent()
		{
		
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
			panelA.GuidForNote ="panela";
				
				FAKE_NoteDataXML_Panel panelB = new FAKE_NoteDataXML_Panel ();
				panelB.Caption = "PanelB";
			panelB.GuidForNote = "panelb";
				
				FAKE_NoteDataXML_Panel panelC = new FAKE_NoteDataXML_Panel ();
				panelC.Caption = "PanelC";
				
				
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
				
				Assert.AreEqual (6, panel.CountNotes (), "Total notes SHOULD BE 6 :  (1 + 1 note on it)panel A + (1)panelB + basicNote +DefaultNote = 5  + (NEW) LinkTable = 6");
				//COUNT SHOULD BE: panelA has 1 note
				//COUNT Total should be: Default Note + Note + PanelA + Panel B  + (Note Inside Panel A) = 5
				
				panelA.AddNote (panelC);  // panelC is now part of Panel 2
				panelC.CreateParent (panelA.myLayoutPanel ());
				panel.SaveLayout (); // NEED TO SAVE before conts will be accurated
				
				
				
				//COUNT SHOULD BE: panelA has 2 notes (Panel C + the Note I add)
				Assert.AreEqual (2, panelA.CountNotes (), "two notes in panelA");
				Assert.AreEqual (7, panel.CountNotes (), "total of SEVEN notes");
				Assert.AreEqual (0, panelB.CountNotes (), "0 count worked?");

			// The Preamble Above is just standard creation
			// Now we want to test adjusting some fields
			panel.NotesForYou().Section ="thepanelsection";
			panel.NotesForYou ().Keywords = "fish,fries,in,the,sky";
			panel.NotesForYou ().Notebook="freshnote";
			panel.NotesForYou().Subtype = "thebestsub";



			panel.SaveLayout ();
			// now reload
			panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			panel.LoadLayout("mynewpanel", false, null);
			panelA = null;
			panelB = null;

			panelA = (FAKE_NoteDataXML_Panel)panel.FindNoteByGuid("panela");
			panelB = (FAKE_NoteDataXML_Panel)panel.FindNoteByGuid("panelb");

			Assert.AreEqual (panel.GUID, panelA.GetParent_ParentGuid());
			Assert.AreEqual (panel.GUID, panelB.GetParent_ParentGuid());

			Assert.AreEqual (2, panelA.CountNotes (), "two notes in panelA");
			Assert.AreEqual (7, panel.CountNotes (), "total of SEVEN notes");
			Assert.AreEqual (0, panelB.CountNotes (), "0 count worked?");




			// just make sure that the Parent itself got the changes we made!!
			Assert.AreEqual ("thepanelsection", panel.NotesForYou().Section);
			Assert.AreEqual ("fish,fries,in,the,sky", panel.NotesForYou().Keywords);
			Assert.AreEqual ("freshnote", panel.NotesForYou().Notebook);
			Assert.AreEqual ("thebestsub", panel.NotesForYou().Subtype);



			// now test the Children 
			// this should FAIL utnil I write the needed code
			Assert.AreEqual ("thepanelsection", panelA.GetParent_Section());
			Assert.AreEqual ("thepanelsection", panelB.GetParent_Section());


			Assert.AreEqual ("fish,fries,in,the,sky", panelA.GetParent_Keywords());
			Assert.AreEqual ("fish,fries,in,the,sky", panelB.GetParent_Keywords());


			Assert.AreEqual ("freshnote", panelA.GetParent_Notebook());
			Assert.AreEqual ("freshnote", panelB.GetParent_Notebook());

			Assert.AreEqual ("thebestsub", panelA.GetParent_Subtype());
			Assert.AreEqual ("thebestsub", panelB.GetParent_Subtype());

		}

		[Test]
		public void MoveNoteOutOfParent_SaveShouldHappenAutomatically()
		{

				_w.output ("START");
				System.Windows.Forms .Form form = new System.Windows.Forms.Form ();
				_TestSingleTon.Instance._SetupForLayoutPanelTests ();
				
				FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
		
			LayoutDetails.Instance.CurrentLayout = panel;
				form.Controls.Add (panel);
				form.Show ();
				
				//NOTE: For now remember that htis ADDS 1 Extra notes
				panel.NewLayout ("mynewpanel", true, null);
			panel.SetCaption("TheMainPanel");
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
			basicNote.BringToFrontAndShow();
			NoteDataXML_RichText textNote = new NoteDataXML_RichText();
			textNote.Caption = "howdy";
		//	((FAKE_LayoutPanel)panelA.GetPanelsLayout()).SetCaption("boohowcrashmenow");
			panelA.AddNote (textNote);
			textNote.CreateParent(panelA.myLayoutPanel());

			textNote.BringToFrontAndShow();
			textNote.TestEnter ();
			panelA.GetPanelsLayout().TestForceError();
			// the current layout can never be set to a subplayout
			Assert.AreNotEqual(panelA.GetPanelsLayout(), LayoutDetails.Instance.CurrentLayout, "the current layout can never be set to a subplayout");
				panel.SaveLayout ();
				Assert.AreEqual (2, panelA.CountNotes (), "Panel A holds TWo note");  // So this counts as  + 2
				
				// so we have (1 + 1 note on it)panel A + (1)panelB + basicNote +DefaultNote = 5  + (NEW) LinkTable = 6
				_w.output ("STARTCOUNT");
				Assert.AreEqual (7, panel.CountNotes (), "Total notes SHOULD BE 7 :  (1 + 1 note on it)panel A + (1+1textnote)panelB + basicNote +DefaultNote = 5  + (NEW) LinkTable = 6");
				
				_w.output ("ENDCOUNT");


				// now we move basicNote out
				// WITOUT calling save  ****
				// it should still ahve saved

			panelA.myLayoutPanel ().MoveNote(basicNote.GuidForNote, "up");
			Assert.AreEqual (1, panelA.CountNotes (), "Panel A holds 1 note -- because we moved one out");
			Assert.AreEqual (7, panel.CountNotes (), "Total notes SHOULD BE 6 :  (1 + 1 note on it)panel A + (1)panelB + basicNote +DefaultNote = 5  + (NEW) LinkTable = 6");
			panel = null;
			panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			panel.LoadLayout("mynewpanel", false, null);
			//Assert.AreEqual (0, panelA.CountNotes (), "Panel A holds one note");
			Assert.AreEqual (7, panel.CountNotes (), "Total notes SHOULD BE 6 :  (1 + 1 note on it)panel A + (1)panelB + basicNote +DefaultNote = 5  + (NEW) LinkTable = 6");

		}
		[Test]
		public void TestNavigationNote()
		{
			NoteDataXML_RichText texter = new NoteDataXML_RichText(100, 100);

			System.Windows.Forms .Form form = new System.Windows.Forms.Form ();
			_TestSingleTon.Instance._SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			form.Controls.Add (panel);
			form.Show ();
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout ("mynewpanel", true, null);

			panel.AddNote(texter);

			texter.GetRichTextBox().Text = "";

			NoteNavigation bookMarkView = new NoteNavigation (texter);

			// Test 1: Tests that no crash happens when we don't have a proper markup assigned
			bookMarkView.UpdateListOfBookmarks();


			LayoutDetails.Instance.SetCurrentMarkup(new iMarkupYourOtherMind());
			// Test 2: Same test. No crash still.
			bookMarkView.UpdateListOfBookmarks();
			string TestText = @"7 minutes
Bus crash over cliff in Ecuador
Lost style flash leading to salvation
ddd[[f]]
=Dog=

=Dog 2=
The |whatis|this|
===error===
The |firstssafdfsf|day of the ghoose|
The |linkfdfgdfgdfg|dfdgdfgedfgdnded|";
				texter.GetRichTextBox().Text = TestText;
			// test 3: some text
			bookMarkView.UpdateListOfBookmarks();

			// Main Level Node +1
			// Dog +1
			// Dog 2 + 1
			// <Make Up Node to insert between Dog 2 and Error> +1
			// Error + 1
			// Current Position (Automated Node) + 1
			// = 6
			Assert.AreEqual (6, bookMarkView.NumberOfNodes());

			texter.GetRichTextBox().Text = "[[~scene]]";
			bookMarkView.UpdateListOfBookmarks();
			Assert.AreEqual (3, bookMarkView.NumberOfNodes());
			texter.GetRichTextBox().Text = @"Hello.
=h1=
=h2=
=h3=
=h4=
=h1=
";
			bookMarkView.UpdateListOfBookmarks();
			Assert.AreEqual (7, bookMarkView.NumberOfNodes());
		}

		[TearDown] public void Cleanup()
		{ 
			lg.Instance.WriteLog();
		//	lg.Instance.Dispose(); this caused an mdihost failure
		
		}
	}
}

