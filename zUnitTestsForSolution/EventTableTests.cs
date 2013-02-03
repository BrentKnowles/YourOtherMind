using System;
using NUnit.Framework;
using Transactions;
using Layout;
using CoreUtilities;
namespace Testing
{
	[TestFixture]
	public class EventTableTests
	{
		public EventTableTests ()
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

	}
}

