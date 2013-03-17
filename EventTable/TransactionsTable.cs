using System;
using database;
using System.Collections.Generic;
using CoreUtilities;
using System.Data;
using System.Collections;
namespace Transactions

{
	public class TransactionsTable
	{

		// set in LayoutDetails
		public Func<List<Type>> TransactionTypesAddedThroughAddIns=null;


		public const string table_name = "events";
		// an int. Values MEAN:
		public const int T_ADDED = 1;
		public const int T_IMPORTED = 100;



				public const int T_DELETED = 2;
				public const int T_RETIRED = 3;
				public const int T_FINISHED = 4; // definitely at least one in there

				public const int T_NAGSTARTED = 13;
				public const int T_NAGINTERRUPTED = 14;

				public const int T_USER = 5;

		//		public const int T_ADDSUB = 6;
		//		public const int T_REGISTER = 7;
		//		public const int T_WEB = 8;
		//		public const int T_LINK = 9;
		//		public const int T_TENWINDOWS = 10;
		//		public const int T_BRAIN = 11;
		//		public const int T_WORDCOUNT = 12;


		public const int T_SUBMISSION = 1001;
		public const int T_SUBMISSION_DESTINATION= 1005;

		public const int T_GENERIC_STATUS_UPDATE = 1200;

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
		static public ColumnConstant TYPE_OF_OBJECT = new ColumnConstant("typeofobject", 16, "TEXT", 16);
		static public ColumnConstant DATA10 = new ColumnConstant("data10", 17, "TEXT", 17);




//		static public string F_DATE = "Date"; 
//		static public string F_TYPE = "Type"; // int
//		static public string F_DATA = "Data";    // Page Name
//		static public string F_DATA2 = "Data2";  // Page Type
//		static public string F_DATA3 = "Data3";  // 
//		static public string F_DATA4 = "Data4";  // 

		public const int ColumnCount = 18;

		static public string[] Columns = new string[ColumnCount] {ID, DATE, TYPE, DATA1_LAYOUTGUID, DATA2, DATA3, DATA4
		,MONEY1,MONEY2, DATE2, NOTES, DATA5, DATA6, DATA7, DATA8, DATA9, TYPE_OF_OBJECT, DATA10};
		static public string[] Types   = new string[ColumnCount]{ID.Type, DATE.Type, TYPE.Type, DATA1_LAYOUTGUID.Type, DATA2.Type, DATA3.Type, DATA4.Type,
			MONEY1.Type, MONEY2.Type, DATE2.Type, NOTES.Type, DATA5.Type, DATA6.Type, DATA7.Type, DATA8.Type, DATA9.Type, TYPE_OF_OBJECT.Type, DATA10.Type};

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
		public TransactionBase GetExisting (ColumnConstant[] _Columns, string[] _Values)
		{
			TransactionBase returnValue = null;
			// will grab everything that matches the first Column=Value pairing
			// then we parse results
			List<object[]> results = ThisDatabase.GetValues (table_name, Columns, _Columns [0], _Values [0]);
			if (results != null) {
				foreach (object[] objArray in results) {
					// now we look for the fine match
					if (objArray [_Columns [1].Index].ToString() == _Values [1]) {
						// we found the complete match
						// now build a new row object
						//returnValue = new TransactionReturnNote(objArray);
						///returnValue = (TransactionBase)Activator.CreateInstance ("TransactionSystem",objArray[TYPE_OF_OBJECT.Index].ToString());
						/// 
					
						Type returnValueType = Type.GetType (objArray[TYPE_OF_OBJECT.Index].ToString());
						returnValue = (TransactionBase)Activator.CreateInstance(returnValueType, objArray.Clone());

						//Type TypeTest = Type.GetType (((Type)(sender as ToolStripButton).Tag).AssemblyQualifiedName);
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
		public List<TransactionBase> GetEventsForLayoutGuid (string Guid)
		{
			return GetEventsForLayoutGuid (Guid, Constants.BLANK);
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
		public List<TransactionBase> GetEventsForLayoutGuid (string Guid, string ExtraWhere)
		{
			List<TransactionBase> returnValue = new List<TransactionBase> ();
			string ColumnToTest = DATA1_LAYOUTGUID;
			string ColumnValueToTest = Guid;

			if (Guid == "*") {
				ColumnToTest = "any";
				ColumnValueToTest = "*";
			}


			List<object[]> results = ThisDatabase.GetValues (table_name, Columns, ColumnToTest, ColumnValueToTest,Constants.BLANK, ExtraWhere);
			if (results != null) {
				foreach(object[] objArray in results)
				{
					Type returnValueType = Type.GetType (objArray[TYPE_OF_OBJECT.Index].ToString());
					if (null == returnValueType)
					{
						lg.Instance.Line ("TransactionsTable->GetEventsForLayoutGuid", ProblemType.WARNING, "Did not find Type : " + objArray[TYPE_OF_OBJECT.Index].ToString());
						// as a last ditch effort (because I was having troubles getting the types to appear before functonality in the addin was used
						// we scroll through the type list we made and try to find a match
						List<Type> TranTypes = TransactionTypesAddedThroughAddIns();
						foreach (Type t in TranTypes)
						{
							if (t.AssemblyQualifiedName.ToString () == objArray[TYPE_OF_OBJECT.Index].ToString())
							{
								returnValueType = t;
							}
						}
						if (returnValueType == null)
						{
							lg.Instance.Line ("TransactionsTable->GetEventsForLayoutGuid", ProblemType.WARNING, "STILL FAILING Did not find Type : " + objArray[TYPE_OF_OBJECT.Index].ToString());
						returnValueType = new TransactionBase().GetType();
						}
					}
					TransactionBase transaction = (TransactionBase)Activator.CreateInstance(returnValueType, objArray.Clone());

					// (TransactionBase)Activator.CreateInstance ("TransactionSystem",objArray[TYPE_OF_OBJECT.Index].ToString());
					returnValue.Add (transaction);
				}
			}
			
			return returnValue;
		}

		//
		// QUERIES
		//




		/// <summary>
		/// January 8 2010
		/// Breaking apart QueryMonthInYear so the date look up is a bit moduldar
		/// </summary>
		/// <param name="sSumColumn"></param>
		/// <param name="sExtraFilter"></param>
		/// <param name="newDateStart"></param>
		/// <param name="newDateEnd"></param>
		/// <param name="nTypeOfQuery">0 - Normal, 1 - Max</param>
		/// <param name="IgnoteDate">If true searches 'all time'</para>
		/// <returns></returns>
		public string QueryValueForTimePeriod (string sSumColumn, string sExtraFilter, DateTime newDateStart, DateTime newDateEnd, int nTypeOfQuery, bool IgnoreDates)
		{

			/* HUGE ISSUE
              * ew DateTime(2008,10,12) works because it is encoding it as YEAR/DAY/MONTH which is what the database wants
              * I need a way to force the database to work with the same date system
              * as the core*/


			string sValue = "error";

			string sCalcString1 = "";
			/// if not an integer column do a count instead
			if (1 == nTypeOfQuery) {
				//sCalcString1 = String.Format ("MAX(convert(int,{0}))", sSumColumn); DID NOT FIx problems (returns error)
				sCalcString1 = String.Format ("MAX( Cast({0} as int))", sSumColumn);
			} else
				if (sSumColumn == DATA1_LAYOUTGUID) {
				// changed this to do direct count of ID, because it won't be blank (and I'm not sure if that was messing with 
				//sCalcString1 = String.Format ("COUNT({0})", sSumColumn);
				sCalcString1 = String.Format ("COUNT(id)", sSumColumn);
			} else {
				sCalcString1 = String.Format ("SUM({0})", sSumColumn);
			}
			//sCalcString1 = String.Format("SUM(Convert('{0}', 'System.Int32'))", sSumColumn);
			//sCalcString1 = String.Format("SUM(Convert({0}, 'INTEGER'))", sSumColumn);
			


			/*/PROPER:*/
			string sCalcString2 = "";//String.Format("({0} >= #{1}#) AND ({0} <#{2}#)", DATE, newDateStart.ToString("u"), newDateEnd.ToString("u"));// String.Format("({0} >= #{1}#) AND ({0} < #{2}#)", F_DATE, newDateStart, newDateEnd);


			string DateComparer = String.Format ("({0} >= @DateStart) AND ({0} < @DateEnd)", DATE);

			if (true == IgnoreDates) {
				DateComparer = "";
			}

			//string sCalcString2 = String.Format("({0} >= #{1}#) AND ({0} < #{2}#)", DATE, newDateStart.ToString (), newDateEnd.ToString ());
			if (sExtraFilter != "")
			{
				// add an extra query to query
				sCalcString2 = sCalcString2 + String.Format(" ({0}) ", sExtraFilter);

				if (DateComparer != "")
				{
					sCalcString2 = sCalcString2 + " AND ";
				}
			}

			sValue =  ThisDatabase.ExecuteCommand(String.Format ("Select {0} from {1} where {2} {3}", sCalcString1, table_name, sCalcString2,
			                                                     			                                                     DateComparer), newDateStart, newDateEnd, IgnoreDates);
			lg.Instance.Line("TransactionsTable->QueryValueForTimePeriod", ProblemType.MESSAGE, sValue);
			return sValue;
		}

		public DataTable AsDataTable ()
		{
			return ThisDatabase.AsDataTable(table_name);
		}

		/// <summary>
		/// Takes current date.
		/// Finds most recent monday.
		/// Then tabulates up until this date.
		/// 
		/// So, basically ifyou did this on a Tuesday you wuld get the Monday and the Tuesday result.
		/// </summary>
		/// <param name="sSumColumn"></param>
		/// <param name="?"></param>
		/// <param name="sExtraFilter"></param>
		/// <returns></returns>
		public string QueryLastWeek(DateTime daytouse, string sSumColumn, string sExtraFilter, bool IgnoreDates)
		{
			string sValue = Constants.BLANK;
			try
			{


				
				
				
				
				DateTime newDateEnd = daytouse.Date; // was DateTime.Today.Date
				int nDays = 0; // how many days to compare
				
				
				
				DayOfWeek dayofweek = newDateEnd.DayOfWeek;
				switch (dayofweek)
				{
				case DayOfWeek.Monday: nDays = 0; break;
				case DayOfWeek.Tuesday: nDays = 1; break;
				case DayOfWeek.Wednesday: nDays = 2; break;
				case DayOfWeek.Thursday: nDays = 3; break;
				case DayOfWeek.Friday: nDays = 4; break;
				case DayOfWeek.Saturday: nDays = 5; break;
				case DayOfWeek.Sunday : nDays = 6; break;
				}
				
				// we have to add 1 to dateend
				newDateEnd = newDateEnd.AddDays(1).Date;
				DateTime newDateStart = daytouse.AddDays(-1 * nDays).Date;
				
				//newDate
				

				// Goign to attempt to write my own for YOM 2013

				 sValue = QueryValueForTimePeriod( sSumColumn, sExtraFilter, newDateStart, newDateEnd, 0,IgnoreDates);
//				sValue =  ThisDatabase.ExecuteCommand(String.Format ("Select Sum({0}) from {1} where type='5' AND ({2} >= {3}) AND ({2} < {4})", sSumColumn, table_name,
//				                                                     DATE, newDateStart.), newDateEnd.ToUniversalTime()));


				// This works but now I try to generalize
//				sValue =  ThisDatabase.ExecuteCommand(String.Format ("Select Sum({0}) from {1} where type='5' AND ({2} >= @DateStart) AND ({2} < @DateEnd)", sSumColumn, table_name,
//				                                                     DATE), newDateStart, newDateEnd);
			}
			catch (Exception ex)
			{
				lg.Instance.Line ("TransactionsTable->QueryLastWeek", ProblemType.EXCEPTION, ex.ToString ());
			//	NewMessage.Show(ex.ToString());
			}
			return sValue;
		}
		/// <summary>
		///  get week states for current week
		/// </summary>
		/// <param name="daytouse">Will build the week based off of this date</param>
		/// <returns></returns>
		public  string GetWeekStats(DateTime daytouse)
		{
			//DateTime todaysDate = DateTime.Today();
			
			string nMinutes = QueryLastWeek(daytouse, DATA3, String.Format("{0}='{1}'", TYPE, T_USER),false);
			
			int minutes = 0;
			int hours = 0;
			GetHoursAndMinutes(nMinutes, out minutes, out hours);
			nMinutes = Loc.Instance.GetStringFmt("{0} (~{1} hours)", minutes.ToString(), hours.ToString());
			
			string Words = QueryLastWeek(daytouse, DATA4, String.Format("{0}='{1}'", TYPE, T_USER),false);
			if (Words.IndexOf("null") > -1) Words = "0"; // sometimes they show null compute
			
			string sResult = Loc.Instance.GetStringFmt("WORKLOG - ALL LAYOUTS (THIS WEEK){1}Minutes Worked: {0} {1}Words Written: {2}", nMinutes, Environment.NewLine, Words);
			return sResult;
			
		}
		private void GetHoursAndMinutes(string Minutes, out int minutes, out int hours)
		{
			Int32.TryParse(Minutes, out minutes);
			hours = (int)(minutes / 60);
		}
		// These are specific to a GUID, but still time-limited
		public  string GetWorkStats_SpecificLayout(DateTime daytouse, string GUID)
		{
			//DateTime todaysDate = DateTime.Today();


			string ExtraFilter =  String.Format("{0}='{1}' and {2}='{3}'", TYPE, T_USER, DATA1_LAYOUTGUID, GUID);

			string nMinutes = QueryLastWeek(daytouse, DATA3, ExtraFilter,false);
			
			int minutes = 0;
			int hours = 0;
			GetHoursAndMinutes(nMinutes, out minutes, out hours);
			nMinutes = Loc.Instance.GetStringFmt("{0} (~{1} hours)", minutes.ToString(), hours.ToString());
			
			string Words = QueryLastWeek(daytouse, DATA4, ExtraFilter,false);
			if (Words.IndexOf("null") > -1) Words = "0"; // sometimes they show null compute




			string sResult = Loc.Instance.GetStringFmt("WORKLOG - THIS LAYOUT (THIS WEEK){1}Minutes Worked: {0} {1}Words Written: {2}", nMinutes, Environment.NewLine, Words);


			// Now Get 'All Time Stats' for this layout
			nMinutes = QueryLastWeek(daytouse, DATA3, ExtraFilter, true);
			GetHoursAndMinutes(nMinutes, out minutes, out hours);
			nMinutes = Loc.Instance.GetStringFmt("{0} (~{1} hours)", minutes.ToString(), hours.ToString());
			Words = QueryLastWeek(daytouse, DATA4, ExtraFilter,true);
			if (Words.IndexOf("null") > -1) Words = "0"; // sometimes they show null compute

			sResult = String.Format ("{0}{1}THIS LAYOUT (ALL TIME){1}Minutes Worked: {2} {1}Words Written: {3}", sResult, Environment.NewLine, nMinutes, Words);


			return sResult;
			
		}


		/// </summary>
		/// <param name="sSumColumn"></param>
		/// <param name="nYear"></param>
		/// <param name="sExtraFilter"></param>
		/// <param name="nMonth"></param>
		/// <returns></returns>
		 public string QueryMonthInYear(string sSumColumn, int nYear, string sExtraFilter, int nMonth, int nTypeOfQuery)
		{
			string sValue = "error";
			try
			{

				
				
				int nNumberOfDaysInMonth = DateTime.DaysInMonth(nYear, nMonth);
				
				
				DateTime newDateStart = new DateTime(nYear, nMonth, 1).Date;
				DateTime newDateEnd = new DateTime(nYear, nMonth, nNumberOfDaysInMonth).Date;
				
				//newDate
				
				
				sValue = QueryValueForTimePeriod(sSumColumn, sExtraFilter, newDateStart, newDateEnd, nTypeOfQuery,false);
				
				
			}
			catch (Exception ex)
			{
				lg.Instance.Line("TransactionTable->QueryMonthInYear",ProblemType.EXCEPTION, ex.ToString ());
				//NewMessage.Show(ex.ToString());
			}
			return sValue;
			
		}
		
		/// <summary>
		/// returns a month by month breakdown of progress
		/// 
		/// will include both Hours, Words, FInished and retired, formatted like
		/// 
		///   January 2008
		///     Minutes: 203  (F_DATA3)
		///     Words: 1200 (F_DATA4)
		///     Finished: 0
		///     Retired: 1
		///  
		/// Example usage
		/// </summary>
		/// <param name="nYear"></param>
		/// <returns></returns>
		 public string QueryMonthsInYearReport(int nYear, int nMonth)
		{
			//
			string nMinutes = QueryMonthInYear(DATA3, nYear, String.Format("{0}='{1}'", TYPE, T_USER), nMonth,0);
			string Words = "0";
			string Added = "0";
			string AddedSub = "0";
			string Finished = "0";
			string Retired = "0";
			string Nag = "0";
			string MaxWords = "0";
			int minutes = 0;
			int hours = 0;
			
			try
			{
				minutes = Int32.Parse(nMinutes);
				hours = (int)(minutes / 60);
				nMinutes = String.Format("{0} (~{1} hours)", nMinutes, hours.ToString());
				
				
				
				Words = QueryMonthInYear(DATA4, nYear, String.Format("{0}='{1}'", TYPE, T_USER), nMonth,0);
				Added = QueryMonthInYear(DATA1_LAYOUTGUID, nYear, String.Format("{0}={1}", TYPE, T_ADDED), nMonth,0);
				
				AddedSub = QueryMonthInYear(DATA1_LAYOUTGUID, nYear, String.Format("{0}={1}", TYPE, T_SUBMISSION), nMonth,0);
				
				Finished = QueryMonthInYear(DATA1_LAYOUTGUID, nYear, String.Format("{0}={1}", TYPE, T_FINISHED), nMonth,0);
				
				Retired = QueryMonthInYear(DATA1_LAYOUTGUID, nYear, String.Format("{0}={1}", TYPE, T_RETIRED), nMonth,0);
				Nag = QueryMonthInYear(DATA1_LAYOUTGUID, nYear, String.Format("{0}={1}", TYPE, T_NAGINTERRUPTED), nMonth,0);
				MaxWords = QueryMonthInYear(DATA4, nYear, String.Format("{0}={1}", TYPE, T_USER), nMonth, 1);
			}
			catch (Exception)
			{
				minutes = 0;
				hours = 0;
				nMinutes = "0";
			}
			
			
			DateTime date = new DateTime(nYear, nMonth, 1);
			
			
			string sValue =
				String.Format("{0}\r\nMinutes: {1} \r\nWords: {2}\r\nFinished: {3}\r\nRetired: {4}\r\nAdded: {5} \r\nSubmissions: {6} \r\nMax Words in One Day: {7} \r\nDistracted: {8}",
				              date.ToString("MMMM"), nMinutes, Words, Finished, Retired, Added, AddedSub, MaxWords, Nag);
			return sValue;
			
		}
		
		/// <summary>
		/// returns an array of the year in which subs happened
		/// </summary>
		/// <returns></returns>
		public int[] GetWorkHistoryYears()
		{

			ArrayList newList = new ArrayList();
			List<object[]> results = ThisDatabase.GetValues(table_name,new string[1]{DATE},"any","*"); 

			foreach (object[] r in results)
			{
			//	System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
				DateTime date = DateTime.Parse(r[0].ToString ());//(DateTime.ParseExact(r[0].ToString(),"MM/DD/YYYY",provider));
				
				// 1950 was used as a code year for Brainstorm counting
				if (date.Year != 1950 && date.Year != 1)
				{
					if (newList.IndexOf(date.Year) == -1)
					{
						// year does not exist, add it
						newList.Add(date.Year);
					}
				}

			}
			newList.Sort();
			
			int[] list = new int[newList.Count];
			newList.CopyTo(list);
			return list;
			
			
		}
		/// <summary>
		/// retrieves a  SUM value based on criteria
		///  
		/// HOURS is 3
		/// 
		/// Example usage
		/// </summary>
		/// <param name="nYear"></param>
		/// <returns></returns>
		public string Query(string sSumColumn, int nYear, string sExtraFilter)
		{
			string sValue = "error";
			try
			{

				
				string sCalcString1 = String.Format("SUM({0})", sSumColumn);
				DateTime newDateStart = new DateTime(nYear, 1, 1).Date;
				DateTime newDateEnd = new DateTime(nYear + 1, 1, 1).Date;



				//newDate
//				string sCalcString2 = String.Format("({0} >= #{1}#) AND ({0} < #{2}#)", DATE, newDateStart, newDateEnd);
//				if (sExtraFilter != "")
//				{
//					// add an extra query to query
//					sCalcString2 = sCalcString2 + String.Format(" AND ({0})", sExtraFilter);
//				}
				//  Messa geBox.Show(String.Format("{0} -- {1}", sCalcString1, sCalcString2));
				sValue =QueryValueForTimePeriod(sSumColumn, sExtraFilter, newDateStart, newDateEnd, 0,false);

				//sValue = ((Int64)(table.Compute(sCalcString1, sCalcString2))).ToString();
			}
			catch (Exception ex)
			{
				lg.Instance.Line("TransactionsTable->Query", ProblemType.EXCEPTION, ex.ToString());
			//	NewMessage.Show(ex.ToString());
			}
			return sValue;
			
		}
		/// <summary>
		/// counts the number of occurences of a type
		/// </summary>
		/// <param name="nYear"></param>
		/// <param name="sExtraFilter"></param>
		/// <returns>-1 if no event table found</returns>
		 public string QueryCount(int nYear, string sExtraFilter, bool bAllYears)
		{
			string sValue = "error";
			try
			{

//				DataSet ds = _LoadTable();
//				if (ds == null)
//				{
//					return "-1";
//				}
//				DataTable table = ds.Tables[0];
//				
//				string sCalcString1 = String.Format("Count({0})", F_DATA);
//				
//				//newDate
//				string sCalcString2 = "";
				DateTime newDateStart=DateTime.Now;
				DateTime newDateEnd=DateTime.Now;
				if (bAllYears == false)
				{
					 newDateStart = new DateTime(nYear, 1, 1).Date;
					 newDateEnd = new DateTime(nYear + 1, 1, 1).Date;
					
				//	sCalcString2 = String.Format("({0} >= #{1}#) AND ({0} < #{2}#)", F_DATE, newDateStart, newDateEnd);
				}
				sValue = QueryValueForTimePeriod(DATA1_LAYOUTGUID, sExtraFilter, newDateStart, newDateEnd, 0,bAllYears);
//				else if (bAllYears == true)
//				{
//					sCalcString2 = sExtraFilter;
//					sExtraFilter = "";
//				}
//				
//				if (sExtraFilter != "")
//				{
//					// add an extra query to query
//					sCalcString2 = sCalcString2 + String.Format(" AND ({0})", sExtraFilter);
//				} 
//				//  Messa geBox.Show(String.Format("{0} -- {1}", sCalcString1, sCalcString2));
//				//Messa geBox.Show(table.Compute(sCalcString1, sCalcString2).GetType().ToString());
//				sValue = ((int)(table.Compute(sCalcString1, sCalcString2))).ToString();
			}
			catch (Exception ex)
			{
				lg.Instance.Line("TransactionsTable->QueryCount", ProblemType.EXCEPTION, ex.ToString());
			}
			return sValue;
			
		}
		/// <summary>
		/// counts the number of occurences of a type
		/// </summary>
		/// <param name="nYear"></param>
		/// <param name="sExtraFilter"></param>
		/// <returns>-1 if no event table found</returns>
		public string QueryMax(int nYear, string sExtraFilter, bool bAllYears, string maxcol)
		{
			string sValue = "error";
			try
			{

				
				//string sCalcString1 = String.Format("Max({0})", maxcol);
				DateTime newDateStart=DateTime.Now;
				DateTime newDateEnd=DateTime.Now;
				//newDate
//				string sCalcString2 = "";
				if (bAllYears == false)
				{
					 newDateStart = new DateTime(nYear, 1, 1).Date;
					 newDateEnd = new DateTime(nYear + 1, 1, 1).Date;
					
					//sCalcString2 = String.Format("({0} >= #{1}#) AND ({0} < #{2}#)", DATE, newDateStart, newDateEnd);
				}
//				else if (bAllYears == true)
//				{
//					sCalcString2 = sExtraFilter;
//					sExtraFilter = "";
//				}
				
//				if (sExtraFilter != "")
//				{
//					// add an extra query to query
//					sCalcString2 = sCalcString2 + String.Format(" AND ({0})", sExtraFilter);
//				}
				//  Messa geBox.Show(String.Format("{0} -- {1}", sCalcString1, sCalcString2));
				//Messa geBox.Show(table.Compute(sCalcString1, sCalcString2).GetType().ToString());
				//sValue = ((int)(table.Compute(sCalcString1, sCalcString2))).ToString();
				sValue = QueryValueForTimePeriod(maxcol, sExtraFilter, newDateStart, newDateEnd, 1,bAllYears);
			}
			catch (Exception ex)
			{
				lg.Instance.Line("TransactionsTable->QueryMax", ProblemType.EXCEPTION, ex.ToString());
			}
			return sValue;
			
		}

		


		public int CountQuery (string str)
		{
			int output = 0;
			string result = ThisDatabase.ExecuteCommand(str, DateTime.Now, DateTime.Now, true);
			Int32.TryParse(result, out output);
			return output;
		}
	} // class


}
