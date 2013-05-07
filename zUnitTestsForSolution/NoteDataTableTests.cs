// NoteDataTableTests.cs
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
using System.Collections.Generic;
using CoreUtilities;
using System.Windows.Forms;

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

			// just to make sure we are calling any other essential setters we call the Singleton, even though we are doign some custom stuff here to
			_TestSingleTon.Instance._SetupForLayoutPanelTests();


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
		public void GetListOfStringsInColumn ()
		{// nest several panels
			_SetupForLayoutPanelTests ();
			Form form = new Form();
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			form.Controls.Add (panel);

			// needed else DataGrid does not initialize
			form.Visible = false;
			form.Show ();

			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout ("mynewpanel", true, null);
			NoteDataXML_Table test = new NoteDataXML_Table (425, 380);
			test.GuidForNote = "thisguid1";
			test.Caption = "note1";
			panel.SaveLayout ();
			//	test.Columns = new appframe.ColumnDetails[3] {new appframe.ColumnDetails("snakes", 109), new appframe.ColumnDetails("fish", 90), new appframe.ColumnDetails("goobers", 11111)};

			panel.AddNote (test);
			test.CreateParent (panel);
			_w.output ("COLUMN "+test.Columns.Length.ToString());
			Assert.AreEqual (4, test.Columns.Length,"before save");

			// saving is deleting the columns?!
			panel.SaveLayout ();
			Assert.AreEqual (4, test.Columns.Length, "after save");
			for (int i = 0; i < 4; i ++)
			{
				switch (i)
				{
				case 0: Assert.AreEqual("Roll", test.Columns[i].ColumnName); break;
				case 1:  Assert.AreEqual("Result", test.Columns[i].ColumnName); break;
				case 2: Assert.AreEqual("Next Table", test.Columns[i].ColumnName); break;
				case 3:  Assert.AreEqual("Modifier", test.Columns[i].ColumnName); break;



				}
			}


// not sure what we are testin here [The default Columns?]
			// testing notedataxml_table 
		
		}
		[Test]
		public void GetValuesForTable ()
		{
			_SetupForLayoutPanelTests ();
			Form form = new Form();
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			form.Controls.Add (panel);
			
			// needed else DataGrid does not initialize
			form.Visible = false;
			form.Show ();
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout ("mynewpanel", true, null);
			NoteDataXML_Table test = new NoteDataXML_Table (425, 380);
			test.GuidForNote = "thisguid1";
			test.Caption = "note1";
			
			test.Columns = new ColumnDetails[3] {
				new ColumnDetails ("snakes", 109),
				new ColumnDetails ("fish", 90),
				new ColumnDetails ("goobers", 11111)
			};
			test.AddRow(new object[3] {"1", "value1", "testA"});
			test.AddRow(new object[3] {"2", "value2", "testB"});
			test.AddRow(new object[3] {"3", "value3", "testC"});
			
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
			cols = test.GetValuesForColumn (2,"*");
			for (int i = 0; i <3; i++) {
				switch (i)
				{
				case 0: Assert.AreEqual(cols[i], "testA"); break;
				case 1:Assert.AreEqual(cols[i], "testB"); break;
				case 2: Assert.AreEqual(cols[i], "testC"); break;
				}
			}


		}
	}
}

