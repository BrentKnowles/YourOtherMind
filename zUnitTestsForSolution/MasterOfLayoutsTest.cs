using System;
using NUnit.Framework;
using Layout;
using System.IO;
using CoreUtilities;
using System.Windows.Forms;
namespace Testing
{
	[TestFixture]
	public class MasterOfLayoutsTest 
	{
		public MasterOfLayoutsTest ()
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
		[Test]
		public void TestExistsByName()
		{
			//_SetupForLayoutPanelTests ();
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel3", true, null);
			panel.SetName("frank");
			NoteDataXML basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();
			Assert.False (MasterOfLayouts.ExistsByName("dasdfasd"));
			Assert.True (MasterOfLayouts.ExistsByName("frank"));
		}
		[Test]
		public void TestExistsByGUID()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			//	_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);
			NoteDataXML basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();
			Assert.False (MasterOfLayouts.ExistsByGUID("mynewpanel2"));
			Assert.True (MasterOfLayouts.ExistsByGUID("mynewpanel"));
		}
		[Test]
		public void DeleteTests()
		{
				
				// nest several panels
				//_SetupForLayoutPanelTests ();
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

			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel ();
			panelA.Caption = "PanelA";
			panelA.GuidForNote = "panela";
			panel.AddNote (panelA);
			panelA.CreateParent (panel);
			
			
			FAKE_NoteDataXML_Text textNote = new FAKE_NoteDataXML_Text ();
			
			panelA.AddNote (textNote);
			textNote.CreateParent (panelA.GetPanelsLayout ());
			textNote.Caption = "My text";
			textNote.GetRichTextBox ().Text = "Hello there.";
			panel.SaveLayout ();
			Assert.AreEqual (2, MasterOfLayouts.Count (true));
			Assert.True (MasterOfLayouts.ExistsByGUID("mynewpanel"));
			MasterOfLayouts.DeleteLayout("mynewpanel");
			Assert.False (MasterOfLayouts.ExistsByGUID("mynewpanel"));
			Assert.AreEqual (0, MasterOfLayouts.Count (true));
				
				

		}
		[Test]
		public void BuildReciprocalLinks()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			//	_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);

			NoteDataXML basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();

			// #2



			panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			panel.NewLayout("mynewpanel2", true, null);
			basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();

			NoteDataXML_LinkNote note = new NoteDataXML_LinkNote();




			panel.AddNote (note);
			note.CreateParent(panel);
			note.SetLink("mynewpanel.thisguid1");
			panel.SaveLayout();


		

			System.Collections.Generic.List<string> linkstome = MasterOfLayouts.ReciprocalLinks("mynewpanel2");
			Assert.AreEqual(0, linkstome.Count);
			linkstome = MasterOfLayouts.ReciprocalLinks("mynewpanel");
			Assert.AreEqual(1, linkstome.Count);

		}

		[Test]
		public void CountSizeOfCombinedTableAfterReciprocalLinks()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			//	_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);
			
			NoteDataXML basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();
			
			// #2
			
			
			
			panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			panel.NewLayout("mynewpanel2", true, null);
			basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();
			
			NoteDataXML_LinkNote note = new NoteDataXML_LinkNote();
			
			
			
			
			panel.AddNote (note);
			note.CreateParent(panel);
			note.SetLink("mynewpanel.thisguid1");
			panel.SaveLayout();
			
			


			
			// #3
			
			
			
			panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			panel.NewLayout("mynewpanel3", true, null);
			basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();
			
			note = new NoteDataXML_LinkNote();
			
			
			
			
			panel.AddNote (note);
			note.CreateParent(panel);
			note.SetLink("mynewpanel2.thisguid1");
			panel.SaveLayout();


			Assert.AreEqual(2, Fake_MasterOfLayouts.GetRowCountOfCombinedLinkTableAfterCallToReciprocalLinks());



			
		}
		[Test]
		public void AddLinkToSubpanel()
		{
			// get the link into a subpanel and make the counts are as expected
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			//	_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);
			
			NoteDataXML basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();


			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel ();
			panelA.Caption = "PanelA";
			panel.AddNote(panelA);
			panelA.CreateParent(panel);

			panel.SaveLayout();
			System.Collections.Generic.List<string> linkstome = MasterOfLayouts.ReciprocalLinks("mynewpanel");
			Assert.AreEqual(0, linkstome.Count);


			NoteDataXML_LinkNote link = new NoteDataXML_LinkNote();

			panelA.AddNote (link);
			link.CreateParent(panelA.GetPanelsLayout());
			link.SetLink("mynewpanel.thisguid1");
			panel.SaveLayout();
	

			linkstome = MasterOfLayouts.ReciprocalLinks("mynewpanel");
			Assert.AreEqual(1, linkstome.Count);

		}

		[Test]
		public void AddAndThenEditLinks()
		{
			// add a couple links, take count
			// then edit one of the early links a few times
			// get the link into a subpanel and make the counts are as expected
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			//	_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);
			
			NoteDataXML basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();
			
			// in this test we don't use the subpanel but just have it here for fun
			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel ();
			panelA.Caption = "PanelA";
			panel.AddNote(panelA);
			panelA.CreateParent(panel);
			
			panel.SaveLayout();
			System.Collections.Generic.List<string> linkstome = MasterOfLayouts.ReciprocalLinks("mynewpanel");
			Assert.AreEqual(0, linkstome.Count);
			
			
			NoteDataXML_LinkNote link = new NoteDataXML_LinkNote();
			
			panel.AddNote (link);
			link.CreateParent(panel);
			link.SetLink("mynewpanel.thisguid1");
			panel.SaveLayout();
			
			
			linkstome = MasterOfLayouts.ReciprocalLinks("mynewpanel");
			Assert.AreEqual(1, linkstome.Count);

			// now edit the existing link
			link.SetLink("mynewpanel.thisguid2");
			panel.SaveLayout();
			Assert.AreEqual(1, linkstome.Count);
			// the counts should remain the same


		}
		[Test]
		public void IdentifySubpanels ()
		{
		
			// get the link into a subpanel and make the counts are as expected
			_TestSingleTon.Instance._SetupForLayoutPanelTests ();
			//	_SetupForLayoutPanelTests ();
				
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
				
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout ("mynewpanel", true, null);
				
			NoteDataXML basicNote = new NoteDataXML ();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
				
			panel.AddNote (basicNote);
			basicNote.CreateParent (panel);
			panel.SaveLayout ();
				
				
			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel ();
			panelA.Caption = "PanelA";
			panelA.GuidForNote = "panela";
			panel.AddNote (panelA);
			panelA.CreateParent (panel);
				
			panel.SaveLayout ();
			System.Collections.Generic.List<string> linkstome = MasterOfLayouts.ReciprocalLinks ("mynewpanel");
			Assert.AreEqual (0, linkstome.Count);
				
				
			NoteDataXML_LinkNote link = new NoteDataXML_LinkNote ();
				
			panelA.AddNote (link);
			link.CreateParent (panelA.GetPanelsLayout ());
			link.SetLink ("mynewpanel.thisguid1");
			panel.SaveLayout ();
				
				
			linkstome = MasterOfLayouts.ReciprocalLinks ("mynewpanel");

			Assert.AreEqual (true,MasterOfLayouts.ExistsByGUID("panela"),"1");
			Assert.AreEqual (true,MasterOfLayouts.IsSubpanel("panela"),"2");
			Assert.AreEqual (true,MasterOfLayouts.ExistsByGUID("mynewpanel"),"3");
			Assert.AreEqual (false,MasterOfLayouts.IsSubpanel("mynewpanel"),"4");
				

		}
		/// <summary>
		/// Tests to string. Just in case I have messed up the string format
		/// </summary>
		[Test]
		public void TestToString()
		{
			NoteDataXML note = new NoteDataXML();
			note.Caption = "boo";
			Assert.AreNotEqual(0, note.ToString().Length);
		}


		[Test]
		public void AnEmptyPanelStillHasAParent()
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
			panelB.Caption = "PanelBBBBB";
			panelB.GuidForNote = "panelBB";
			panelA.AddNote (panelB);
			panelB.CreateParent (panelA.GetPanelsLayout());
		
			
			
			
			FAKE_NoteDataXML_Text textNote = new FAKE_NoteDataXML_Text ();
			
			panelA.AddNote (textNote);
			textNote.CreateParent (panelA.GetPanelsLayout ());
			textNote.Caption = "My text";
			textNote.GetRichTextBox ().Text = "Hello there." +Environment.NewLine +"I am still here, are you?" + Environment.NewLine+"Yep!";
			panel.SaveLayout ();
			Assert.AreEqual (6, panel.CountNotes ());
			_w.output(panelB.GetPanelsLayout().ParentGUID);

			// get parent GUID directlyf rom database
			FAKE_LayoutPanel SubPanelB = new FAKE_LayoutPanel ("panelBB", true);
			form.Controls.Add (SubPanelB);
			SubPanelB.LoadLayout("panelBB", true, null);

			Assert.AreNotEqual(Constants.BLANK, SubPanelB.GetLayoutDatabase().ParentGuid);
		}

		[Test]
		public void ExportImport ()
		{	lg.Instance.OutputToConstoleToo = true;
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

			// export
			string file = Path.Combine (Environment.CurrentDirectory, "exportest.txt");
			if (File.Exists (file)) {
				File.Delete (file);
			}
			string subfile = file+"__child0.txt";
			if (File.Exists (subfile)) {
				File.Delete (subfile);
			}
			string subfile2 = file+"__child1.txt";
			if (File.Exists (subfile2)) {
				File.Delete (subfile2);
			}
			
			Assert.False (File.Exists (file), file + " does not exist");

			MasterOfLayouts.ExportLayout(ThisLayoutGUID, file);
			// test exportfile existence 	// count 2 files exported
			Assert.True (File.Exists(file),"main file exists");
			Assert.True (File.Exists(subfile),"subfile exists");
			Assert.True (File.Exists(subfile2),"subfile2 exists");
		
		


			// delete original note
			MasterOfLayouts.DeleteLayout(ThisLayoutGUID);


			// test existence
			Assert.False (MasterOfLayouts.ExistsByGUID (ThisLayoutGUID));
			// counting our subpanel we have ZERO
			Assert.AreEqual (0, MasterOfLayouts.Count (true) ,"Nothing should be left");
			// Import as New (but won't be duplicate because old is gone)

			int errorcode = MasterOfLayouts.ImportLayout(file);
			Console.WriteLine(errorcode);
			Assert.True (MasterOfLayouts.ExistsByGUID (ThisLayoutGUID));
			// test existence
			 
			// confirm all notes laoded into layout
			panel = null;
			panel = new FAKE_LayoutPanel(ThisLayoutGUID, false);
			form.Controls.Add (panel);
			panel.LoadLayout (ThisLayoutGUID,false, null);
		//	panel.SaveLayout();

			Assert.AreEqual (7, panel.CountNotes (), "Count2");
			Assert.AreEqual (1, MasterOfLayouts.Count (false));
			Assert.AreEqual (3, MasterOfLayouts.Count (true));
	


			// Import as Overwrite
			if (File.Exists (file)) {
				File.Delete (file);
			}
			subfile = file+"__child0.txt";
			if (File.Exists (subfile)) {
				File.Delete (subfile);
			}
			Assert.True (MasterOfLayouts.ExistsByGUID (ThisLayoutGUID));
			MasterOfLayouts.ExportLayout(ThisLayoutGUID, file);

			panel = null;
			errorcode = MasterOfLayouts.ImportLayout(file);
			Assert.AreEqual (0, errorcode);
			// test existences
			panel = new FAKE_LayoutPanel(ThisLayoutGUID, false);
			form.Controls.Add (panel);
			panel.LoadLayout (ThisLayoutGUID,false, null);
			
			Assert.AreEqual (7, panel.CountNotes ());
			Assert.AreEqual (1, MasterOfLayouts.Count (false));
			Assert.AreEqual (3, MasterOfLayouts.Count (true));


			lg.Instance.OutputToConstoleToo = false;
		}
	}
}

