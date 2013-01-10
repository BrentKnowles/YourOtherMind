using System;
using NUnit.Framework;
using Layout;
using System.Collections.Generic;

namespace Testing
{
	[TestFixture]
	public class NoteDataTableTests
	{
		public NoteDataTableTests ()
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

	
		/// <summary>
		/// Initializes a new instance of the <see cref="Testing.NoteDataTableTests"/> class.
		/// </summary>
		[Test]
		public void GetListOfStringsInColumn()
		{// nest several panels
			_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);
			NoteDataXML_Table test = new NoteDataXML_Table(425,380);
			test.GuidForNote = "thisguid1";
			test.Caption = "note1";

			test.Columns = new appframe.ColumnDetails[3] {new appframe.ColumnDetails("snakes", 109), new appframe.ColumnDetails("fish", 90), new appframe.ColumnDetails("goobers", 11111)};

			panel.AddNote(test);
			test.CreateParent(panel);
			panel.SaveLayout();

// not sure what we are testin here
			// testing notedataxml_table 
			Assert.True (false);
		}
		[Test]
		public void GetValuesForTable ()
		{
			_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout ("mynewpanel", true, null);
			NoteDataXML_Table test = new NoteDataXML_Table (425, 380);
			test.GuidForNote = "thisguid1";
			test.Caption = "note1";
			
			test.Columns = new appframe.ColumnDetails[3] {
				new appframe.ColumnDetails ("snakes", 109),
				new appframe.ColumnDetails ("fish", 90),
				new appframe.ColumnDetails ("goobers", 11111)
			};
			test.AddRow(new object[3] {"1", "value1", "test"});
			
			panel.AddNote (test);
			test.CreateParent (panel);
			panel.SaveLayout ();
			List<string> cols = test.GetValuesForColumn (1,"*");
			for (int i = 0; i <3; i++) {
				switch (i)
				{
				case 0: Assert.AreEqual(cols[i], "value1"); break;
				case 1:Assert.AreEqual(cols[i], "value2"); break;
				case 2: Assert.AreEqual(cols[i], "value3"); break;
				}
			}

			Assert.True (false);
		}
	}
}

