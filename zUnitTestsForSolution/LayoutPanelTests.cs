using System;
using NUnit.Framework;
using Layout;
using LayoutPanels;
using database;
using CoreUtilities;

namespace Testing
{
	[TestFixture]
	public class LayoutPanelTests
	{
		public LayoutPanelTests ()
		{
		}

		private void _SetupForLayoutPanelTests ()
		{
			LayoutDetails.Instance.YOM_DATABASE = "yom_test_database.s3db";


			FakeLayoutDatabase layout = new FakeLayoutDatabase("testguid");
			FAKE_SqlLiteDatabase db = new FAKE_SqlLiteDatabase(layout.GetDatabaseName ());
			db.DropTableIfExists(Layout.data.dbConstants.table_name);
			_w.output ("dropping table " + Layout.data.dbConstants.table_name);
		}

		[Test]
		public void SpeedTest ()
		{

			// TODO: need to delete OLD table. THis is why speedtest fails when ran as part of a group


			// this will be a benchmarking test that will create a complicated Layout
			// Then it will time and record the results of LOADING and SAVING that layout into a 
			// table saved in my backup paths
			// will also output a DAAbackup file (text readable) format too
			_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			//NOTE: For now remember that htis ADDS 1 Extra notes
			string panelname = System.Guid.NewGuid().ToString();
			panel.NewLayout (panelname);
			LayoutDetails.Instance.AddToList (typeof(FAKE_NoteDataXML_Panel), "testingpanel");

			// ADD 1 of each type
			foreach (Type t in LayoutDetails.Instance.ListOfTypesToStoreInXML()) {
				for (int i = 0; i < 10; i++) {
					NoteDataInterface note = (NoteDataInterface)Activator.CreateInstance (t);
					panel.AddNote (note);
				}
			}
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
			string base_path = @"C:\Users\BrentK\Documents\Keeper\Files\yomspeedtests2013\";
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
			time = CoreUtilities.TimerCore.Time (() => {
			panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
				panel.LoadLayout(panelname, false);
			});
			Console.WriteLine("TIME " + time);

	

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
		public void TestMovingNotes()
		{
		
			_SetupForLayoutPanelTests ();

			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);

			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel");
			NoteDataXML basicNote = new NoteDataXML();
			basicNote.Caption = "note1";

			panel.AddNote(basicNote);
			//basicNote.CreateParent(panel);


			LayoutDetails.Instance.AddToList(typeof(FAKE_NoteDataXML_Panel),"testingpanel");

			//panel.MoveNote(
			// create four panels A and B at root level. C inside A. D inside C
			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel();
			panelA.Caption = "PanelA";

			FAKE_NoteDataXML_Panel panelB = new FAKE_NoteDataXML_Panel();
			panelB.Caption = "PanelB";

			FAKE_NoteDataXML_Panel panelC = new FAKE_NoteDataXML_Panel();
			panelC.Caption = "PanelC";

		
			_w.output("panels made");


			panel.AddNote(panelA);  // 1
			panel.AddNote(panelB);  // 2
			//panelA.CreateParent(panel); should not need to call this when doing LayoutPanel.AddNote because it calls CreateParent insid eof it

			basicNote = new NoteDataXML();
			basicNote.Caption = "note2";



			panelA.AddNote(basicNote);  // Panel A has 1 note
			basicNote.CreateParent(panelA.myLayoutPanel());  // DO need to call it when adding notes like this (to a subpanel, I think)
			panel.SaveLayout();
			Assert.AreEqual(1, panelA.CountNotes(), "Panel A holds one note");
	  	Assert.AreEqual (5, panel.CountNotes(), "Total notes now is 5");
			//COUNT SHOULD BE: panelA has 1 note
			//COUNT Total should be: Default Note + Note + PanelA + Panel B  + (Note Inside Panel A) = 5

			 panelA.AddNote (panelC);  // panelC is now part of Panel 2
			 panelC.CreateParent(panelA.myLayoutPanel());
			panel.SaveLayout(); // NEED TO SAVE before conts will be accurated



			//COUNT SHOULD BE: panelA has 2 notes (Panel C + the Note I add)
			Assert.AreEqual(2, panelA.CountNotes(), "two notes in panelA");
			Assert.AreEqual (6, panel.CountNotes(), "total of six notes");
			Assert.AreEqual (0, panelB.CountNotes(),"0 count worked?");
			//COUNT Total should be: Default Note + Note + PanelA + Panel B + (NoteInside Panel A) + (Panel C Inside Panel A) = 6

			FAKE_NoteDataXML_Panel panelD = new FAKE_NoteDataXML_Panel();
			panelD.Caption = "PanelD";
			panelC.AddNote(panelD); // panelC which is inside PANELA now adds panelD (which is empty?)
			panelD.CreateParent(panelC.myLayoutPanel());
			panel.SaveLayout();

			Assert.AreEqual (0, panelD.CountNotes (), "No notes in panelD");
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

			Assert.AreEqual (1,panelC.CountNotes(), "1 notes in panelC");
			Assert.AreEqual (0,panelD.CountNotes(), "1 note in panelD");
			_w.output("_------------------------------");
			_w.output (panel.CountNotes());
			_w.output("_------------------------------");
			Assert.AreEqual (7, panel.CountNotes(), "We have only added one panel so we should be up to 7");

			Assert.AreEqual(3, panelA.CountNotes());

			Assert.AreEqual (7, panel.CountNotes(),"number of notes in main panel");
			Assert.AreEqual (0, panelB.CountNotes(),"testt");
			Assert.AreEqual (1, panelC.CountNotes(),"testt");

			// add a note to panel d (we want to make sure this note does not disappear while D is being moved around
			NoteDataXML noteford = new NoteDataXML();
			noteford.Caption = "note for d";
			panelD.AddNote(noteford);


			// Move panel D from panelC into PANEL A
			panelC.myLayoutPanel().MoveNote(panelD.GuidForNote, "up");
			panel.SaveLayout();
			Assert.AreEqual(4, panelA.CountNotes()); // 4 because I added a note to D which is inside A
			Assert.AreEqual (8, panel.CountNotes());
			Assert.AreEqual (0, panelC.CountNotes());
			Assert.AreEqual (0, panelB.CountNotes());
			Assert.AreEqual (1, panelD.CountNotes());
			// Move panel D from panelA into ROOT
			panelA.myLayoutPanel().MoveNote(panelD.GuidForNote, "up");
			panel.SaveLayout();
			Assert.AreEqual(2, panelA.CountNotes());
			Assert.AreEqual (8, panel.CountNotes());
			Assert.AreEqual (0, panelC.CountNotes());
			Assert.AreEqual (0, panelB.CountNotes());
			Assert.AreEqual (1, panelD.CountNotes());

			NoteDataXML_RichText richy = new NoteDataXML_RichText();
			panel.AddNote(richy);
			richy.CreateParent(panel);
			panel.SaveLayout();
			// now move note into A
			panel.MoveNote(richy.GuidForNote, panelA.GuidForNote);
			panel.SaveLayout();
			Assert.AreEqual(3, panelA.CountNotes());
			Assert.AreEqual (9, panel.CountNotes());
			Assert.AreEqual (0, panelC.CountNotes());
			Assert.AreEqual (0, panelB.CountNotes());

			_w.output ("Panel A Notes "+ panelA.CountNotes().ToString());
			_w.output("Total Notes " + panel.CountNotes());
			// also see if this test or another could replicate the problems I had in previous version with RefhresTabs


				//	Assert.True (false);
		}
			[Test]
		public void AdvancedSearchingTests()
		{
			// find notes inside of notes with the GUI code and such
			Assert.True (false);
		}
	}
}

