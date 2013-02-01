using System;
using NUnit.Framework;
using Layout;

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

			Assert.True (MasterOfLayouts.ExistsByGUID("mynewpanel"));
			MasterOfLayouts.DeleteLayout("mynewpanel");
			Assert.False (MasterOfLayouts.ExistsByGUID("mynewpanel"));

				
				

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
	}
}

