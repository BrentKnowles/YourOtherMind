using System;
using database;
using System.Collections.Generic;
using CoreUtilities;
namespace Transactions
{
	public class TransactionsTable
	{

		const string table_name = "events";
		// an int. Values MEAN:
		public const int T_ADDED = 1;
		public const int T_IMPORTED = 100;
		//		public const int T_DELETED = 2;
		//		public const int T_RETIRED = 3;
		//		public const int T_FINISHED = 4;
		//		public const int T_USER = 5;
		//		public const int T_ADDSUB = 6;
		//		public const int T_REGISTER = 7;
		//		public const int T_WEB = 8;
		//		public const int T_LINK = 9;
		//		public const int T_TENWINDOWS = 10;
		//		public const int T_BRAIN = 11;
		//		public const int T_WORDCOUNT = 12;
		//		public const int T_NAGSTARTED = 13;
		//		public const int T_NAGINTERRUPTED = 14;

		static public ColumnConstant ID = new ColumnConstant("id", 0, "INTEGER", 0);
		static public ColumnConstant DATE = new ColumnConstant("date",1,"datetime",1);
	
		static public ColumnConstant TYPE = new ColumnConstant("type", 2, "TEXT", 2);

		static public ColumnConstant DATA1_LAYOUTGUID = new ColumnConstant("data1", 3, "TEXT", 3);
		static public ColumnConstant DATA2 = new ColumnConstant("data2", 4, "TEXT", 4);
		static public ColumnConstant DATA3 = new ColumnConstant("data3", 5, "INTEGER", 5); //minutes
		static public ColumnConstant DATA4 = new ColumnConstant("data4", 6, "INTEGER", 6); //words


		static public ColumnConstant MONEY1 = new ColumnConstant("money1", 7, "float", 7); 
		static public ColumnConstant MONEY2 = new ColumnConstant("money2", 8, "float", 8); 

		static public ColumnConstant DATE2 = new ColumnConstant("date2",9,"datetime",9);
		static public ColumnConstant NOTES = new ColumnConstant("notes", 10, "TEXT", 10);
		static public ColumnConstant DATA5 = new ColumnConstant("data5", 11, "TEXT", 11);
		static public ColumnConstant DATA6 = new ColumnConstant("data6", 12, "TEXT", 12);
		static public ColumnConstant DATA7 = new ColumnConstant("data7", 13, "TEXT", 13);
		static public ColumnConstant DATA8 = new ColumnConstant("data8", 14, "TEXT", 14);
		static public ColumnConstant DATA9 = new ColumnConstant("data9", 15, "TEXT", 15);

		/* Mapping Musing - Submissions
		 * 
		 * date = date sent
		 * 
		 * data1 - Guid of Layout
		 * data2 - Guid of Market
		 * data3 - Priority
		 * data4 - WILL NOT BE USED
		 * Expenses     - Money1
		 * Earned       - Money2
		 * DateReplied  - Date2
		 * Notes - Notes
		 * 		 * Rights  - Data5
		 * DraftVersionUsed - Data6
		 * 
		 * ReplyType - Data7
		 * ReplyFeedback - Data8
		 * 
		 * 
		 * SubmissionType - Data9
		 */


//		static public string F_DATE = "Date"; 
//		static public string F_TYPE = "Type"; // int
//		static public string F_DATA = "Data";    // Page Name
//		static public string F_DATA2 = "Data2";  // Page Type
//		static public string F_DATA3 = "Data3";  // 
//		static public string F_DATA4 = "Data4";  // 

		public const int ColumnCount = 16;

		static public string[] Columns = new string[ColumnCount] {ID, DATE, TYPE, DATA1_LAYOUTGUID, DATA2, DATA3, DATA4
		,MONEY1,MONEY2, DATE2, NOTES, DATA5, DATA6, DATA7, DATA8, DATA9};
		static public string[] Types   = new string[ColumnCount]{ID.Type, DATE.Type, TYPE.Type, DATA1_LAYOUTGUID.Type, DATA2.Type, DATA3.Type, DATA4.Type,
			MONEY1.Type, MONEY2.Type, DATE2.Type, NOTES.Type, DATA5.Type, DATA6.Type, DATA7.Type, DATA8.Type, DATA9.Type};

		BaseDatabase ThisDatabase=null;

		/// <summary>
		/// Initializes a new instance of the <see cref="EventTable.EventTable"/> class.
		/// </summary>
		/// <param name='_Database'>
		/// _ database. We pass in the result of a class to MasterOfLayouts.GetDatabaseType(databaseName)
		/// </param>
		public TransactionsTable (BaseDatabase _Database)
		{
			if (null == _Database) throw new Exception("Database must be initialized for EventTable");

			ThisDatabase = _Database;
			CreateDatabase();
		}

		void CreateDatabase()
		{
			if (null == ThisDatabase) throw new Exception("Database for EventTable has been set to null.");

			// This part already done during Constructor BaseDatabase db = GetDatabaseType(LayoutDetails.Instance.YOM_DATABASE);//new SqlLiteDatabase (LayoutDetails.Instance.YOM_DATABASE);
			ThisDatabase.CreateTableIfDoesNotExist (table_name, 
			                              Columns, 
			                              Types, String.Format ("{0}", ID)
			                              );

			lg.Instance.Line ("EventTable->CreateDatabase", ProblemType.MESSAGE, "Creating/verifying event table");
		}

		public int Count()
		{
			return ThisDatabase.Count (table_name);
		}
		/// <summary>
		/// Adds the event. We have to Manually decide whether to add/edit at a higher level
		///  because whether an event is unqiue depends on the context
		/// </summary>
		/// <param name='EventRow'>
		/// Event row.
		/// </param>
		public void AddEvent (TransactionBase EventRow)
		{
		
			if (null == ThisDatabase)
				throw new Exception ("Database for EventTable has been set to null.");

				// add new
				if (Columns.Length != EventRow.GetRowData ().Length) throw new Exception ("Column length does not match row length");
				ThisDatabase.InsertData(table_name, Columns, EventRow.GetRowData());

		}
		/// <summary>
		/// Updates the event. Must make sure there is a valid ID in EventRow
		/// </summary>
		/// <param name='EventRow'>
		/// Event row.
		/// </param>
		public void UpdateEvent (TransactionBase EventRow)
		{
			if (Columns.Length != EventRow.GetRowData ().Length)
				throw new Exception ("Column length does not match row length");
			if (ExistsEvent (ID, EventRow.ID )) {
				// we update
				ThisDatabase.UpdateSpecificColumnData (table_name, Columns, EventRow.GetRowData (), ID,  EventRow.ID );
			} else {
				// this row was not present
			}
		}

		public bool ExistsEvent(string Column, object value)
		{

			return ThisDatabase.Exists (table_name, Column, value.ToString());

		}
		/// <summary>
		/// Testing existence is more complicated given that each EventType must decide for itself
		/// 
		/// Expecting ONLY 2 Columsn and 2 Values!!
		/// </summary>
		/// <returns>
		/// The existing.
		/// </returns>
		/// <param name='Columns'>
		/// Columns.
		/// </param>
		/// <param name='Values'>
		/// Values.
		/// </param>
		public TransactionReturnNote GetExisting (ColumnConstant[] _Columns, string[] _Values)
		{
			TransactionReturnNote returnValue = null;
			// will grab everything that matches the first Column=Value pairing
			// then we parse results
			List<object[]> results = ThisDatabase.GetValues (table_name, Columns, _Columns [0], _Values [0]);
			if (results != null) {
				foreach (object[] objArray in results) {
					// now we look for the fine match
					if (objArray [_Columns [1].Index].ToString() == _Values [1]) {
						// we found the complete match
						// now build a new row object
						returnValue = new TransactionReturnNote(objArray);
					}

				}
			}
			return returnValue;
		}
		public bool DeleteEvent (string Column, object value)
		{
			bool returnvalue = false;
			if (ExistsEvent (Column, value) == true) {
				returnvalue= ThisDatabase.Delete(table_name, Column, value.ToString());
			}
			return returnvalue;
		}
		/// <summary>
		/// Gets the events for layout GUID.
		/// </summary>
		/// <returns>
		/// The events for layout GUID.
		/// </returns>
		/// <param name='Guid'>
		/// GUID.
		/// </param>
		public List<TransactionReturnNote> GetEventsForLayoutGuid (string Guid)
		{
			List<TransactionReturnNote> returnValue = new List<TransactionReturnNote> ();
			List<object[]> results = ThisDatabase.GetValues (table_name, Columns, DATA1_LAYOUTGUID, Guid);
			if (results != null) {
				foreach(object[] objArray in results)
				{
					returnValue.Add (new TransactionReturnNote(objArray));
				}
			}
			
			return returnValue;
		}

	}

	/// <summary>
	/// Event row_ return note. This is just for returnign from the GetExisting Function in the EventTable (to avoid confusion)
	/// </summary>

//	public class EventRowSubmission : EventRowBase
//	{
//		// obviouslly addedin ADDIN
//	}
}
