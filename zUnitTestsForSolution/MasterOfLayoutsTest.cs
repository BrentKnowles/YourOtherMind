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
		protected void _SetupForLayoutPanelTests ()
		{
			LayoutDetails.Instance.YOM_DATABASE = "yom_test_database.s3db";
			LayoutDetails.Instance.AddToList(typeof(FAKE_NoteDataXML_Panel),"testingpanel");
			LayoutDetails.Instance.AddToList(typeof(FAKE_NoteDataXML_Text),"testingtext");
			
			FakeLayoutDatabase layout = new FakeLayoutDatabase("testguid");
			FAKE_SqlLiteDatabase db = new FAKE_SqlLiteDatabase(layout.GetDatabaseName ());
			db.DropTableIfExists(Layout.data.dbConstants.table_name);
			_w.output ("dropping table " + Layout.data.dbConstants.table_name);
		}
		[Test]
		public void TestExistsByName()
		{
			_SetupForLayoutPanelTests ();
			
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

				_SetupForLayoutPanelTests ();
			
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
				_SetupForLayoutPanelTests ();
				
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
	}
}

