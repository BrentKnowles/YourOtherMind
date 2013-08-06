// TransactionTableTests.cs
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
using Transactions;
using Layout;
using CoreUtilities;
namespace Testing
{
	[TestFixture]
	public class TransactionTableTests
	{
		public TransactionTableTests ()
		{
		}

		private TransactionsTable SetupForEventTests()
		{
			lg.Instance.OutputToConstoleToo = true;
			FAKE_SqlLiteDatabase db = new FAKE_SqlLiteDatabase(_TestSingleTon.TESTDATABASE);
			db.DropTableIfExists("events");
			db.Dispose();


			TransactionsTable eventTable = new TransactionsTable(MasterOfLayouts.GetDatabaseType(_TestSingleTon.TESTDATABASE));
			return eventTable;
		}


		[Test]
		[ExpectedException]
		public void TryingToAddUniqueIDTwice()
		{
			TransactionsTable eventTable = SetupForEventTests();
			Assert.NotNull(eventTable);
			TransactionNewLayout tricky = new TransactionNewLayout(DateTime.Now, "boomer2");
			Add (tricky, eventTable);
			Assert.AreEqual(1, eventTable.Count(),"after one add");
			tricky.UpdateValue(TransactionsTable.ID.Index, "1");
			Add (tricky, eventTable);


		}

		[Test]
		public void AddAndUpdateWorks_All()
		{
			TransactionsTable eventTable = SetupForEventTests();
			Assert.NotNull(eventTable);
			Add (new TransactionNewLayout(DateTime.Now, "boomer2"), eventTable);
			Assert.AreEqual(1, eventTable.Count(),"after one add");
			TransactionBase foundNote = eventTable.GetExisting(new database.ColumnConstant[2] 
			                                                   {TransactionsTable.TYPE, TransactionsTable.DATA1_LAYOUTGUID}, new string[2]{TransactionsTable.T_ADDED.ToString (), "boomer2"});
			Assert.NotNull(foundNote);

			// should still only be 1 beacuse we've just edited the existing
			foundNote.UpdateValue(TransactionsTable.DATA2.Index, "some data2");
			Update (foundNote, eventTable);
			Assert.AreEqual(1, eventTable.Count());
			foundNote.UpdateValue(TransactionsTable.DATA2.Index, "some data3");
			Update (new TransactionNewLayout(DateTime.Now, "boomer2"), eventTable);
			Assert.AreEqual(1, eventTable.Count());

			Add (new TransactionNewLayout(DateTime.Now, "boomer2"), eventTable);
			Assert.AreEqual(2, eventTable.Count());


			Add(new TransactionImportLayout(DateTime.Now, "roar","The Roar Caption"), eventTable);
			Add(new TransactionImportLayout(DateTime.Now, "roar","The Roar Caption"), eventTable);
			Assert.AreEqual(4, eventTable.Count());

			Add(new TransactionDeleteLayout(DateTime.Now, "roar"), eventTable);
			Assert.AreEqual(5, eventTable.Count());

		}

		public void Add(TransactionBase eventRow, TransactionsTable eventTable)
		{

			eventTable.AddEvent(eventRow);

		}
		public void Update (TransactionBase eventRow, TransactionsTable eventTable)
		{
			eventTable.UpdateEvent(eventRow);
		}



		[Test]
		public void DeleteWorks()
		{
			TransactionsTable eventTable = SetupForEventTests();
			Assert.NotNull(eventTable);
			Add (new TransactionNewLayout(DateTime.Now, "boomer2"), eventTable);
			Assert.AreEqual(1, eventTable.Count(),"after one add");
			TransactionBase foundNote = eventTable.GetExisting(new database.ColumnConstant[2] 
			                                                   {TransactionsTable.TYPE, TransactionsTable.DATA1_LAYOUTGUID}, new string[2]{TransactionsTable.T_ADDED.ToString (), "boomer2"});
			Assert.NotNull(foundNote);
			
			// should still only be 1 beacuse we've just edited the existing
			foundNote.UpdateValue(TransactionsTable.DATA2.Index, "some data2");
			Update (foundNote, eventTable);
			Assert.AreEqual(1, eventTable.Count());
			foundNote.UpdateValue(TransactionsTable.DATA2.Index, "some data3");
			Update (new TransactionNewLayout(DateTime.Now, "boomer2"), eventTable);
			Assert.AreEqual(1, eventTable.Count());
			
			Add (new TransactionNewLayout(DateTime.Now, "boomer2"), eventTable);
			Assert.AreEqual(2, eventTable.Count());
			
			
			eventTable.DeleteEvent(TransactionsTable.ID,foundNote.ID);
			Assert.AreEqual(1, eventTable.Count());


			foundNote = eventTable.GetExisting(new database.ColumnConstant[2] 
			                                   {TransactionsTable.TYPE, TransactionsTable.DATA1_LAYOUTGUID}, new string[2]{TransactionsTable.T_ADDED.ToString (), "boomer2"});
			Assert.NotNull(foundNote);
			eventTable.DeleteEvent(TransactionsTable.ID,foundNote.ID);
			Assert.AreEqual(0, eventTable.Count());
		}

		[Test]
		public void ExistWorks()
		{
			TransactionsTable eventTable = SetupForEventTests();
			Assert.NotNull(eventTable);
			Add (new TransactionNewLayout(DateTime.Now, "boomer2"), eventTable);
			Assert.AreEqual(1, eventTable.Count(),"after one add");
			TransactionBase foundNote = eventTable.GetExisting(new database.ColumnConstant[2] 
			                                                   {TransactionsTable.TYPE, TransactionsTable.DATA1_LAYOUTGUID}, new string[2]{TransactionsTable.T_ADDED.ToString (), "boomer2"});
			Assert.NotNull(foundNote);
			
		
		}

		[Test]
		public void QueryTests()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests ();
			TransactionsTable eventTable = SetupForEventTests();
			LayoutDetails.Instance.TransactionsList = eventTable;
			System.Windows.Forms.Form form = new System.Windows.Forms.Form();
			form.Show ();
			LayoutDetails.Instance.GetAppearanceFromStorage = _TestSingleTon.Instance.GetAppearanceFromStorage;
			YOM2013.DefaultLayouts.CreateASystemLayout(form,null);
			LayoutDetails.Instance.SystemLayout = new Layout.LayoutPanel (CoreUtilities.Constants.BLANK, true);
			LayoutDetails.Instance.SystemLayout.LoadLayout (LayoutPanel.SYSTEM_LAYOUT, false, null); 
			form.Controls.Add (LayoutDetails.Instance.SystemLayout);

			Worklog.JournalPanel Journal = new Worklog.JournalPanel("A", null);

			Assert.NotNull(eventTable);

			// Add a Really Old Task (20 days ago)
			Add (new TransactionWorkLog(DateTime.Now.AddDays(-20),"BOOM", "", 1000, 2000, "Writing"), eventTable);


			Add (new TransactionWorkLog(DateTime.Now,"BOOM", "", 100, 200, "Writing"), eventTable);
			string value = Journal.GetWeekStats(DateTime.Now);
			Assert.True (value.IndexOf("100") > -1, "Found Words");
			Assert.True (value.IndexOf ("200")> -1, "Found Minutes");

			// add a task for another entity

			Add (new TransactionWorkLog(DateTime.Now,"BOOM2", "", 50, 50, "Writing"), eventTable);
			value = Journal.GetWeekStats(DateTime.Now);
			Assert.True (value.IndexOf("150") > -1, "Found Words2");
			Assert.True (value.IndexOf ("250")> -1, "Found Minutes2");
			// now look 'a week ago' should find none
			value = Journal.GetWeekStats(DateTime.Now.AddDays(-10));
			Assert.False (value.IndexOf("100") > -1, "Found Words3");
			Assert.False (value.IndexOf ("200")> -1, "Found Minutes3");
			Assert.False (value.IndexOf("150") > -1, "Found Words3");
			Assert.False (value.IndexOf ("250")> -1, "Found Minutes3");


			// Now Layout Specific Tests

			value = Journal.GetWorkStats_SpecificLayout(DateTime.Now, "BOOM2");
			Assert.True (value.IndexOf("50") > -1, "Found Words2");
			Assert.False (value.IndexOf ("250")> -1, "Found Minutes2");

			value = Journal.GetWorkStats_SpecificLayout(DateTime.Now, "BOOM");
			Assert.True (value.IndexOf("100") > -1, "Found WordsBOOMSPECIFIC");
			Assert.True (value.IndexOf ("200")> -1, "Found MinutesBOOMSPECIFIC");
			Assert.False (value.IndexOf ("250")> -1, "Found Minutes2");

			_w.output(value);
			// Now do Test 'ALL TIME' bylooking at the value we already grabbed because SpecificLayout also includes a Global Count
			Assert.True (value.IndexOf("1100") > -1, "Found WordsBOOMSPECIFIC _ALLTIME");
			Assert.True (value.IndexOf("2200") > -1, "Found MinutesBOOMSPECIFIC _ALLTIME");

		}

	}
}

