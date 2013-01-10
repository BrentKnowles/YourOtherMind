using System;
using Layout;
using NUnit.Framework;

namespace Testing
{
	[TestFixture]
	public class NoteData
	{	protected void _SetupForLayoutPanelTests ()
		{
			LayoutDetails.Instance.YOM_DATABASE = "yom_test_database.s3db";
			LayoutDetails.Instance.AddToList(typeof(FAKE_NoteDataXML_Panel),"testingpanel");
			LayoutDetails.Instance.AddToList(typeof(FAKE_NoteDataXML_Text),"testingtext");
			
			FakeLayoutDatabase layout = new FakeLayoutDatabase("testguid");
			FAKE_SqlLiteDatabase db = new FAKE_SqlLiteDatabase(layout.GetDatabaseName ());
			db.DropTableIfExists(Layout.data.dbConstants.table_name);
			_w.output ("dropping table " + Layout.data.dbConstants.table_name);
		}
		public NoteData ()
		{
		}
		[Test]
		public void TestDefaultHeightAndWidth()
		{
		

			// nest several panels
			_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);
			NoteDataXML test = new NoteDataXML(425,380);
			test.GuidForNote = "thisguid1";
			test.Caption = "note1";
			
			panel.AddNote(test);
			test.CreateParent(panel);
			panel.SaveLayout();
			
			Assert.AreEqual (test.Height, 425);
			Assert.AreEqual (test.Width, 380);
		}
	}
}

