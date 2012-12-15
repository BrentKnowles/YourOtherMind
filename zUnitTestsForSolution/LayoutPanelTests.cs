using System;
using NUnit.Framework;
using Layout;
using LayoutPanels;
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
		}

		/// <summary>
		/// Tests the moving notes.
		/// </summary>
		[Test]
		public void TestMovingNotes()
		{
		
			_SetupForLayoutPanelTests ();

			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK);

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

			FAKE_NoteDataXML_Panel panelD = new FAKE_NoteDataXML_Panel();
			panelD.Caption = "PanelD";
			_w.output("panels made");


			panel.AddNote(panelA);
			panel.AddNote(panelB);
			//panelA.CreateParent(panel); should not need to call this when doing LayoutPanel.AddNote because it calls CreateParent insid eof it

			basicNote = new NoteDataXML();
			basicNote.Caption = "note2";



			panelA.AddNote(basicNote);
			basicNote.CreateParent(panelA.myLayoutPanel());  // DO need to call it when adding notes like this (to a subpanel, I think)
			panel.SaveLayout();
			Assert.AreEqual(1, panelA.CountNotes());
	  	Assert.AreEqual (5, panel.CountNotes());
			//COUNT SHOULD BE: panelA has 1 note
			//COUNT Total should be: Default Note + Note + PanelA + Panel B  + (Note Inside Panel A) = 5

			 panelA.AddNote (panelC);
			 panelC.CreateParent(panelA.myLayoutPanel());
			panel.SaveLayout(); // NEED TO SAVE before conts will be accurated
			//COUNT SHOULD BE: panelA has 2 notes (Panel C + the Note I add)
			Assert.AreEqual(2, panelA.CountNotes());
			Assert.AreEqual (6, panel.CountNotes());
			Assert.AreEqual (0, panelB.CountNotes(),"0 count worked?");
			//COUNT Total should be: Default Note + Note + PanelA + Panel B + (NoteInside Panel A) + (Panel C Inside Panel A) = 6


			panelC.AddNote(panelD);
			panelD.CreateParent(panelC.myLayoutPanel());
			panel.SaveLayout();
			Assert.AreEqual(3, panelA.CountNotes());
			Assert.AreEqual (7, panel.CountNotes());
			Assert.AreEqual (0, panelB.CountNotes(),"testt");
			Assert.AreEqual (1, panelC.CountNotes(),"testt");

			// Move panel D from panelC into PANEL A
			panelC.myLayoutPanel().MoveNote(panelD.GuidForNote, "up");
			panel.SaveLayout();
			Assert.AreEqual(3, panelA.CountNotes());
			Assert.AreEqual (7, panel.CountNotes());
			Assert.AreEqual (0, panelC.CountNotes());
			Assert.AreEqual (0, panelB.CountNotes());
			// Move panel D from panelA into ROOT
			panelA.myLayoutPanel().MoveNote(panelD.GuidForNote, "up");
			panel.SaveLayout();
			Assert.AreEqual(2, panelA.CountNotes());
			Assert.AreEqual (7, panel.CountNotes());
			Assert.AreEqual (0, panelC.CountNotes());
			Assert.AreEqual (0, panelB.CountNotes());


			NoteDataXML_RichText richy = new NoteDataXML_RichText();
			panel.AddNote(richy);
			richy.CreateParent(panel);
			panel.SaveLayout();
			// now move note into A
			panel.MoveNote(richy.GuidForNote, panelA.GuidForNote);
			panel.SaveLayout();
			Assert.AreEqual(3, panelA.CountNotes());
			Assert.AreEqual (8, panel.CountNotes());
			Assert.AreEqual (0, panelC.CountNotes());
			Assert.AreEqual (0, panelB.CountNotes());

			_w.output ("Panel A Notes "+ panelA.CountNotes().ToString());
			_w.output("Total Notes " + panel.CountNotes());
			// also see if this test or another could replicate the problems I had in previous version with RefhresTabs


				//	Assert.True (false);
		}
			
	}
}

