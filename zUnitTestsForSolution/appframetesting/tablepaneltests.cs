// tablepaneltests.cs
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
using appframe;
using System.Collections.Generic;
using CoreUtilities.Tables;
using System.Windows.Forms;
using Layout;
using System.Data;
using System.Xml;
using System.Threading;

namespace Testing
{
	[TestFixture]
	public class tablepaneltests
	{
		public tablepaneltests ()
		{
		}


		[Test]
		public void ImportListTest_MatchingColumns()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();


		
			Form form = new Form();
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			form.Controls.Add (panel);
			
			// needed else DataGrid does not initialize

			form.Show ();
			form.Visible = false;
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout ("mynewpanel", true, null);
			NoteDataXML_Table test = new NoteDataXML_Table (425, 380);
			test.GuidForNote = "thisguid1";
			test.Caption = "note1";
			panel.SaveLayout ();

			string[] newData = new string[3]{"1,col2,col3,col4","2,col2,col3,col4","3,col2,col3,col4"};

			TableWrapper.ImportList(newData, (DataTable)test.dataSource);
			panel.SaveLayout();
			List<string> values =  test.GetValuesForColumn(1, "*");
			Assert.AreEqual (3, values.Count);
			Assert.AreEqual ("col2", values[0]);



//			Number of  1,2,3,4 must match number of columns
//				
//				1,fish,2
//					2,snake,3
//					3,lizard,4

			form.Dispose ();

		}
		[Test]
		public void ImportListTest_UnequalColumns()
		{

			
			
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			Form form = new Form();
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			form.Controls.Add (panel);
			
			// needed else DataGrid does not initialize
			
			form.Show ();
			form.Visible = false;
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout ("mynewpanel", true, null);
			NoteDataXML_Table test = new NoteDataXML_Table (425, 380);
			test.GuidForNote = "thisguid1";
			test.Caption = "note1";
			panel.SaveLayout ();
			
			string[] newData = new string[3]{"1,col2,col3","2,col2,col3","3,col2,col3"};
			
			TableWrapper.ImportList(newData, (DataTable)test.dataSource);
			panel.SaveLayout();
			List<string> values =  test.GetValuesForColumn(1, "*");
			Assert.AreEqual (3, values.Count);
			Assert.AreEqual ("1,col2,col3", values[0]);
			form.Dispose ();

		}
		[Test]
		[RequiresSTA]
		public void CopyToClipboard()
		{
//			Table
//				Roll	Result	NextTable	Modifier	ff	seer	eee
//					z. This is a test. How to do al inefeed,,,length,,,
//					beer.This is the story that never was,e,,,bb,,

			
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			Form form = new Form();
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			form.Controls.Add (panel);
			
			// needed else DataGrid does not initialize
			
			form.Show ();
			form.Visible = false;
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout ("mynewpanel", true, null);
			FAKE_NoteDataXML_Table test = new FAKE_NoteDataXML_Table (33,0009);
			test.GuidForNote = "thisguid1";
			test.Caption = "note1";
			test.CreateParent(panel);
			
			test.AddRow(new object[3] {"1", "value1", "testA"});
			test.AddRow(new object[3] {"2", "value2", "testB"});
			test.AddRow(new object[3] {"3", "value3", "testC"});
			
			panel.SaveLayout ();
			test.Copy ();

		

			string result = Clipboard.GetText();
			Assert.AreEqual(91, result.Length);
			

			form.Dispose ();

		}

		[Test]
		public void InsertRowTest()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			Form form = new Form();
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			form.Controls.Add (panel);
			
			// needed else DataGrid does not initialize
			
			form.Show ();
			form.Visible = false;
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout ("mynewpanel", true, null);
			FAKE_NoteDataXML_Table test = new FAKE_NoteDataXML_Table (33,0009);
			test.GuidForNote = "thisguid1";
			test.Caption = "note1";
			test.CreateParent(panel);
			
			test.AddRow(new object[3] {"1", "value1", "testA"});
			test.AddRow(new object[3] {"2", "value2", "testB"});
			test.AddRow(new object[3] {"3", "value3", "testC"});
			panel.SaveLayout ();
			Assert.AreEqual (3, test.RowCount());
			test.GetTablePanel().InsertRow();
			Assert.AreEqual (4, test.RowCount());
			test.GetTablePanel().InsertRow();
			test.GetTablePanel().InsertRow();
			Assert.AreEqual (6, test.RowCount());
			form.Dispose ();
		}


	}
}

