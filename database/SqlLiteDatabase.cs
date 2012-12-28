using System;
using System.Data.SQLite;
using System.Collections.Generic;
using CoreUtilities;

namespace database
{
	/// <summary>
	/// Sql lite database.
	/// </summary>
	public class SqlLiteDatabase : BaseDatabase
	{
		public SqlLiteDatabase (string database):base(database)
		{
			if ("" == database) {
				throw new Exception("invalid database name passed in SqlLiteDatabase");
			}
			this.Database_Name = database;
		}

		/// <summary>
		/// Gets the connection_ string. 
		/// 
		/// Which is the databasename with some coding and versioning
		/// </summary>
		/// <value>
		/// The connection_ string.
		/// </value>
		private string Connection_String {
			get {
				string result =  String.Format (@"Data Source={0};Version=3",Database_Name);
				return result;
			}
		}

		public override void Dispose()
		{
			//this = null;
			this.Database_Name = "";

		}

		/// <summary>
		/// Creates the table if does not exist.
		/// 
		/// A PrimaryKeyField is required
		/// </summary>
		/// <param name='Table'>
		/// Table.
		/// </param>
		/// <param name='Columns'>
		/// Columns.
		/// </param>
		/// <param name='Types'>
		/// Types.
		/// </param>
		/// <param name='PrimaryKeyField'>
		/// Primary key field.
		/// </param>
		public override void CreateTableIfDoesNotExist (string Table, string[] Columns, string[] Types, string PrimaryKeyField)
		{
			if (CoreUtilities.Constants.BLANK == PrimaryKeyField) {
				throw new Exception ("There must be a primary key specified");
			}

			if (Columns.Length != Types.Length) {
				throw new Exception("Each column in the Columns array must have a datatype in the Types array");
			}

			try {
				SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);

				sqliteCon.Open();
				string ColumnsAsText = ColumnArrayToColumnString_ForCreatingATable(Columns, Types);
				// Define the SQL Create table statement
				string createAppUserTableSQL = String.Format ("CREATE TABLE if not exists [{0}] ({1}, PRIMARY KEY ({2}))",Table, ColumnsAsText, PrimaryKeyField);
/*			TODO: remove		"[name] TEXT NULL," +
						"[username] TEXT  NULL" +
						") "*/
				lg.Instance.Line("SqLiteDatabase->CreateTableIfDoesNotExist", ProblemType.MESSAGE, createAppUserTableSQL);
				
				using (SQLiteTransaction sqlTransaction = sqliteCon.BeginTransaction())
				{
					// Create the table
					SQLiteCommand createCommand = new SQLiteCommand(createAppUserTableSQL
					                                                , sqliteCon);
					createCommand.ExecuteNonQuery();
					createCommand.Dispose();
					
					// Commit the changes into the database
					sqlTransaction.Commit();
				} // end using


				//When we create teh table, we can also add additional columns that may
				// not have been present in the original
				AddMissingColumn(Table, Columns, Types);



				// Close the database connection
				sqliteCon.Close();
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}

		}
		public override List<object[]> GetValues (string tableName, string[] columnToReturn, string columnToTest, object Test)
		{
			return GetValues (tableName, columnToReturn, columnToTest, Test, Constants.BLANK);
		}
		/// <summary>
		/// Gets the values. Returns a list with an object array. The list is in the form:
		/// 
		///   *      List[0] = the first row with an array matching in size the array ColumnToReturn that was Passed in
		///   *      To get all values then use columnToTest=any, test="*"
		/// </summary>
		/// <returns>
		/// The values.
		/// </returns>
		/// <param name='tableName'>
		/// Table name.
		/// </param>
		/// <param name='columnToReturn'>
		/// Column to return.
		/// </param>
		/// <param name='columnToTest'>
		/// Column to test.
		/// </param>
		/// <param name='Test'>
		/// Test.
		/// </param>
		public override List<object[]> GetValues (string tableName, string[] columnToReturn, string columnToTest, object Test, string Sorting)
		{

			if (CoreUtilities.Constants.BLANK == tableName) {
				throw new Exception ("You must provide a table to query");
			}

			//if (CoreUtilities.Constants.BLANK == Test.ToString() || CoreUtilities.Constants.BLANK == columnToTest) {
			if (null == Test) {
				throw new Exception ("Must define a Test criteria for retrieving text");
			}

			if (Test.GetType () == typeof(string)) {
				if (Test.ToString() == Constants.BLANK)
				{
					throw new Exception ("Must define a Test criteria for retrieving text");
				}
			}

			if (columnToReturn == null || columnToReturn.Length <= 0) {
				throw new Exception ("At least one column is required, to know what you want from the database.");
			}
			if (columnToReturn.Length > 0 && columnToReturn [0] == "") {
				throw new Exception("Columns must be defined");
			}

			string ColumnsToReturnForQuery = base.ColumnArrayToStringForInserting (columnToReturn);

			if (CoreUtilities.Constants.BLANK == ColumnsToReturnForQuery) {
				throw new Exception("Column Array did not create a valid query string");
			}

			SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);
			sqliteCon.Open ();

			List<object[]> ReturnList = new List<object[]>();


			try {
				if (columnToTest == "any")
				{
					// i.e., query will become where 1=1
					columnToTest="1";
					Test = "1";
				}
				else
				{
					if (Test.GetType() == typeof(string))
						Test = String.Format ("'{0}'", Test);
				}
				if (Sorting == Constants.BLANK)
				{
					//Sorting = ";";
				}
			
				// Execute query on database
				//string selectSQL = "SELECT name, username FROM AppUser";
				string selectSQL = String.Format ("SELECT {0} FROM {1} where {2} = {3} {4}", 
				                                  ColumnsToReturnForQuery, tableName, columnToTest, Test, Sorting);
				lg.Instance.Line("SqlLiteDatabase.GetValues", ProblemType.WARNING, selectSQL, Loud.CTRIVIAL);
				//string selectSQL = String.Format ("SELECT {0} FROM {1} LIMIT 1", columnToReturn, tableName, columnToTest, Test);
				SQLiteCommand selectCommand = new SQLiteCommand (selectSQL, sqliteCon);
				SQLiteDataReader dataReader = selectCommand.ExecuteReader ();

				// Iterate every record in the AppUser table
				while (dataReader.Read()) {
					object[] tempRow = new object[columnToReturn.Length];
					for (int i = 0; i < columnToReturn.Length; i++ )
					{
						tempRow[i] = dataReader[i];
					}
					if (null != tempRow)
					{
						ReturnList.Add ( tempRow);
					}
					//Console.WriteLine ("Name: " + dataReader.GetString (0)					                   + " Username: " + dataReader.GetString (1));
				}
				dataReader.Close ();
				sqliteCon.Close ();
			} catch (Exception ex) {
				throw new Exception(ex.ToString());
			}
			if (ReturnList == null || 0 == ReturnList.Count)
			{
			 lg.Instance.Line("SqlLiteDatabase.GetValues", ProblemType.MESSAGE,"GetValues query returned an empty list");
			}
			return ReturnList;
		}


		/// <summary>
		/// If we are adding data from a column, then add it, if not present
		/// </summary>
		/// <param name='columns'>
		/// Columns.
		/// </param>
		protected bool AddMissingColumn (string table, string[] columns, string[] types)
		{
			bool result = false;
			if (columns.Length != types.Length) {
				throw new Exception("column size must match type size");
			}

			SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);
			sqliteCon.Open ();

			string selectSQL = String.Format ("SELECT * FROM {0} LIMIT 1", table); 
			//string selectSQL = String.Format ("SELECT {0} FROM {1} LIMIT 1", columnToReturn, tableName, columnToTest, Test);
			SQLiteCommand selectCommand = new SQLiteCommand (selectSQL, sqliteCon);
			SQLiteDataReader dataReader = selectCommand.ExecuteReader ();
			for (int i = 0; i < columns.Length; i++)
			{
				string column = columns[i];
				if (ColumnExists(dataReader, column) == false)
				{
					// column does not exist, add it
					lg.Instance.Line("AddMissingColumn", ProblemType.MESSAGE, "This column does not exist " + column);
					//AddColumn(table, column, types[i]);
					result = true;

					// Now add the column
					if (CoreUtilities.Constants.BLANK == column)
					{
						throw new Exception("Table cannot be blank.");
					}
					if (CoreUtilities.Constants.BLANK == column || CoreUtilities.Constants.BLANK == types[i]) {
						throw new Exception("column not defined");
					}

					selectSQL = String.Format ("ALTER TABLE {0} ADD COLUMN {1} {2};", table, column, types[i]);
					lg.Instance.Line("SqlLiteDatabase.AddMissingColumn", ProblemType.TEMPORARY, selectSQL);									
					using (SQLiteTransaction sqlTransaction = sqliteCon.BeginTransaction()) {
						SQLiteCommand command = new SQLiteCommand (selectSQL, sqliteCon);
						command .ExecuteNonQuery ();
						sqlTransaction.Commit ();
					}


				}
			}
			dataReader.Close ();
			sqliteCon.Close ();
			return result;
		}

		private bool UpdateDataCore(SQLiteTransaction sqlTransaction, string tableName, string[] ColumnToAddTo, object[] ValueToAdd, string WhereColumn, string WhereValue)
		{
			if (ColumnToAddTo.Length != ValueToAdd.Length) {
				throw new Exception ("Arrays of Columns and Values for those columns must be the same length");
			}
			
			if (ColumnToAddTo == null || ValueToAdd == null) {
				throw new Exception ("Must have a valid Column and Value array");
			}
			
			if (WhereColumn == "") {
				throw new Exception ("Must supply a WhereColumn");
			}
			if (WhereValue == "") {
				throw new Exception ("must supply a Where Value");
			}
			
			
			
			bool ReturnValue = false;
			// hacking for rtf http://stackoverflow.com/questions/751172/system-data-sqlite-parameter-issue
			//	SQLiteParameter param = new SQLiteParameter("@myrtf");
			//	param.Value = ValueToAdd;
			string sqlStatement = "";
			
			Console.WriteLine("WHERE: " + WhereValue);
			try {
				
				string[] ColumnsWithValues = new string[ColumnToAddTo.Length];
				for (int i = 0 ; i < ColumnToAddTo.Length ;i++)
				{
					ColumnsWithValues[i] = String.Format ("{0} = @{1}VALUE", ColumnToAddTo[i], ColumnToAddTo[i]);
				}
				string ColumnAndValueString = base.ColumnArrayToStringForInserting(ColumnsWithValues);
				
				if (CoreUtilities.Constants.BLANK != ColumnAndValueString)
				{
					//ColumnToAddTo
					// {1}=@myrtf
					sqlStatement = String.Format ("UPDATE {0} set {1} where {2}=@WhereValue", tableName, ColumnAndValueString, WhereColumn);
					lg.Instance.Line("SqlLiteDatabase.UpdateSpecific", ProblemType.MESSAGE, sqlStatement);

					
					

					SQLiteCommand command = new SQLiteCommand (sqlStatement, sqlTransaction.Connection);
						
						//command.Parameters.Add(param);
						command.Parameters.Add(new SQLiteParameter("@WhereValue", WhereValue ));
						
						// * build array of other ColumnsToAdd to
						for (int j = 0; j < ValueToAdd.Length; j++)
						{
							command.Parameters.Add(new SQLiteParameter(String.Format ("@{0}VALUE",ColumnToAddTo[j]), ValueToAdd[j] ));
						}
						
						if (1 == command .ExecuteNonQuery ())
						{
							
							ReturnValue = true;
						}
						else
						{
							ReturnValue = false;
						}
						
						/// What I learned 
						/// I thought the commit shouldn't happen when a RETURNVALUE=FALSE occurred because there was no point
						/// But this actually hung the database. You need your commits
						

				}
			} catch (Exception ex) {
				//Console.WriteLine(ex.ToString() + sqlStatement);
				throw new Exception(ex.ToString());
			}
			return ReturnValue;
		}


		/// <summary>
		/// Trying to more easily wrap mulitple transactions as an experiment.
		/// 
		/// Decided upon building a Start/Stop mechanism because it works better with loops elsewhere.
		/// I also needed toa void exposing the SQLiteTransacton, outside of the class, to keep
		/// the actual data mechanism unexposed, which is why I'm playing with it as a variablehere.
		/// My worry is using it for the wrong thing later and causing problems...
		/// </summary>
		SQLiteTransaction updateMultiple_sqlTransaction;
		public override void UpdateMultiple_Start ()
		{
			SQLiteConnection sqliteCon = new SQLiteConnection ( Connection_String);
			sqliteCon.Open ();
			updateMultiple_sqlTransaction = sqliteCon.BeginTransaction ();
		}

		public override bool UpdateDataMultiple (string tableName, string[] ColumnToAddTo, object[] ValueToAdd, string WhereColumn, string WhereValue)
		{
			if (null == updateMultiple_sqlTransaction) {
				throw new Exception("UpdateMultiple Cannot be called until UpdateMultiple_Start is called");
			}

			bool result = UpdateDataCore (updateMultiple_sqlTransaction, tableName, ColumnToAddTo, ValueToAdd, WhereColumn, WhereValue);

			               
			               
		
			return result;
		}
		public  override bool IsInMultipleUpdateMode ()
		{
			if (null != updateMultiple_sqlTransaction)
			{
				return true;
			}
			return false;
		}

		public override void UpdateMultiple_End ()
		{

			try {
				SQLiteConnection sqliteCon =updateMultiple_sqlTransaction.Connection;
				updateMultiple_sqlTransaction.Commit ();
				sqliteCon.Close ();
			} catch (Exception ex) {
				NewMessage.Show (ex.ToString());
			}
		}


		/// <summary>
		/// Updates specific data.
		/// 
		/// The WhereColumn should be a unique column.
		/// 
		/// </summary>
		/// <param name='tableName'>
		/// Table name.
		/// </param>
		/// <param name='ColumnToAddTo'>
		/// Column to add to.
		/// </param>
		/// <param name='ValueToAdd'>
		/// Value to add.
		/// </param>
		/// <param name='WhereColumn'>
		/// Where column.
		/// </param>
		/// <param name='WhereValue'>
		/// Where value.
		/// </param>
		/// <returns>False if unable to update row because the GUID did not match any GUID</returns>
		public override bool UpdateSpecificColumnData (string tableName, string[] ColumnToAddTo, object[] ValueToAdd, string WhereColumn, string WhereValue)
		{
			SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);
			sqliteCon.Open ();
			bool result = false;
			using (SQLiteTransaction sqlTransaction = sqliteCon.BeginTransaction ()) {
				 result = UpdateDataCore (sqlTransaction, tableName, ColumnToAddTo, ValueToAdd, WhereColumn, WhereValue);
				sqlTransaction.Commit ();
			}
			
			sqliteCon.Close ();
			return result;

			/*if (ColumnToAddTo.Length != ValueToAdd.Length) {
				throw new Exception ("Arrays of Columns and Values for those columns must be the same length");
			}

			if (ColumnToAddTo == null || ValueToAdd == null) {
				throw new Exception ("Must have a valid Column and Value array");
			}

			if (WhereColumn == "") {
				throw new Exception ("Must supply a WhereColumn");
			}
			if (WhereValue == "") {
				throw new Exception ("must supply a Where Value");
			}



			bool ReturnValue = false;
			// hacking for rtf http://stackoverflow.com/questions/751172/system-data-sqlite-parameter-issue
		//	SQLiteParameter param = new SQLiteParameter("@myrtf");
		//	param.Value = ValueToAdd;
			string sqlStatement = "";
		
			Console.WriteLine("WHERE: " + WhereValue);
			try {
			
				string[] ColumnsWithValues = new string[ColumnToAddTo.Length];
				for (int i = 0 ; i < ColumnToAddTo.Length ;i++)
				{
					ColumnsWithValues[i] = String.Format ("{0} = @{1}VALUE", ColumnToAddTo[i], ColumnToAddTo[i]);
				}
				string ColumnAndValueString = base.ColumnArrayToStringForInserting(ColumnsWithValues);

				if (CoreUtilities.Constants.BLANK != ColumnAndValueString)
				{
				//ColumnToAddTo
				// {1}=@myrtf
					sqlStatement = String.Format ("UPDATE {0} set {1} where {2}=@WhereValue", tableName, ColumnAndValueString, WhereColumn);
					lg.Instance.Line("SqlLiteDatabase.UpdateSpecific", ProblemType.MESSAGE, sqlStatement);
					SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);
					sqliteCon.Open ();
					

					using (SQLiteTransaction sqlTransaction = sqliteCon.BeginTransaction()) {
						SQLiteCommand command = new SQLiteCommand (sqlStatement, sqliteCon);

						//command.Parameters.Add(param);
					command.Parameters.Add(new SQLiteParameter("@WhereValue", WhereValue ));

					// * build array of other ColumnsToAdd to
						for (int j = 0; j < ValueToAdd.Length; j++)
						{
							command.Parameters.Add(new SQLiteParameter(String.Format ("@{0}VALUE",ColumnToAddTo[j]), ValueToAdd[j] ));
						}

						if (1 == command .ExecuteNonQuery ())
					{
						
						ReturnValue = true;
					}
					else
					{
						ReturnValue = false;
					}

						/// What I learned 
						/// I thought the commit shouldn't happen when a RETURNVALUE=FALSE occurred because there was no point
						/// But this actually hung the database. You need your commits
						sqlTransaction.Commit ();


					sqliteCon.Close ();
					}
				}
			} catch (Exception ex) {
				//Console.WriteLine(ex.ToString() + sqlStatement);
				throw new Exception(ex.ToString());
			}
			return ReturnValue;

*/
		}


		/// <summary>
		/// Searchs the database. TO DO: Will use the full-text functionality, though I'll need 
		/// plan how this will work
		/// </summary>
		/// <returns>
		/// The database.
		/// </returns>
		/// <param name='SearchTerm'>
		/// Search term.
		/// </param>
		/// <param name='Params'>
		/// Parameters.
		/// </param>
		public override string[] SearchDatabase (string SearchTerm, string Params)
		{
			throw new Exception("Not implemented");
		}

		/// <summary>
		/// Exists the specified Column and ColumnValue.
		/// </summary>
		/// <param name='Column'>
		/// Column.
		/// </param>
		/// <param name='ColumnValue'>
		/// Column value.
		/// </param>
		/// <returns>T</returns>true if there is a row with this column and this value
		public override bool Exists(string Table, string Column, string ColumnValue)
		{
			bool found = false;
			SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);
			sqliteCon.Open ();
			try {
				
				
				// Execute query on database
				//string selectSQL = "SELECT name, username FROM AppUser";
				string selectSQL = String.Format ("SELECT * FROM {0} where {1} = '{2}'", Table, Column, ColumnValue);
				//string selectSQL = String.Format ("SELECT {0} FROM {1} LIMIT 1", columnToReturn, tableName, columnToTest, Test);
				SQLiteCommand selectCommand = new SQLiteCommand (selectSQL, sqliteCon);
				SQLiteDataReader dataReader = selectCommand.ExecuteReader ();



				// Iterate every record in the AppUser table
				while (dataReader.Read()) {
					// if we find ONE row, it means success, no?
					found = true;
					break;

				}
				dataReader.Close ();
				sqliteCon.Close ();
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}
			return found;
		}

		public  void InsertDataMultiple(string tableName, string[] columns, object[] values)
		{
			// ToDO: multiple insert trasnactions
			SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);
			sqliteCon.Open ();
			SQLiteTransaction sqlTransaction = sqliteCon.BeginTransaction();
			InsertDataCore(sqlTransaction, tableName, columns, values);

			sqlTransaction.Commit ();
			
			sqliteCon.Close ();
		}

		private void InsertDataCore(SQLiteTransaction sqlTransaction, string tableName, string[] columns, object[] values)
		{
			// current
			if (columns.Length != values.Length) {
				throw new Exception("Must be same number of columns and column values");
			}
			
			
			string vals = "";
			string sqlStatement = "";
			// Performs an insert, change contents of sqlStatement to perform
			// update or delete.
			try {
				string cols = ColumnArrayToStringForInserting (columns);
				//vals = ValueArrayToStringForInserting (values);
				if ("" != cols) {
					
					for (int i = 0 ; i < columns.Length; i++)
					{
						if (vals != "")
						{
							vals = vals + ",";
						}
						// build string full of params
						vals = vals + String.Format ("@{0}VALUE", columns[i]);
					}
					
					sqlStatement = String.Format ("INSERT INTO {0}({1}) VALUES({2})", tableName, cols, vals);
					lg.Instance.Line("SqlLiteDatabase.InsertData", ProblemType.MESSAGE, sqlStatement);


					SQLiteCommand command = new SQLiteCommand (sqlStatement, sqlTransaction.Connection);
						// fill in parameters
						
						for (int j = 0; j < columns.Length; j++)
						{
							string param = String.Format ("@{0}VALUE",columns[j]);
							lg.Instance.Line("SqlLiteDatabase.InsertData", ProblemType.TEMPORARY, param);
							lg.Instance.Line("SqlLiteDatabase.InsertData", ProblemType.TEMPORARY, values[j].ToString());
							command.Parameters.AddWithValue(param, values[j]);
							//command.Parameters.Add(new SQLiteParameter(param, values[j] ));
						}
						
						
						command .ExecuteNonQuery ();
				
				}
			} catch (System.Data.SQLite.SQLiteException) {
				throw new Exception("Trying to add a value that is already present in the unqiue colum");
			}
		}


		/// <summary>
		/// Inserts the data.
		/// </summary>
		/// <param name='tableName'>
		/// Table name.
		/// </param>
		/// <param name='columns'>
		/// Columns.
		/// </param>
		/// <param name='values'>
		/// Values.
		/// </param>
		public override void InsertData (string tableName, string[] columns, object[] values)
		{

			SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);
			sqliteCon.Open ();
			using (SQLiteTransaction sqlTransaction = sqliteCon.BeginTransaction()) {
				InsertDataCore (sqlTransaction, tableName, columns, values);
			
				sqlTransaction.Commit ();
			}
			sqliteCon.Close ();
			return;
			/*if (columns.Length != values.Length) {
				throw new Exception("Must be same number of columns and column values");
			}
		

			string vals = "";
			string sqlStatement = "";
			// Performs an insert, change contents of sqlStatement to perform
			// update or delete.
			try {
				string cols = ColumnArrayToStringForInserting (columns);
				//vals = ValueArrayToStringForInserting (values);
				if ("" != cols) {

					for (int i = 0 ; i < columns.Length; i++)
					{
						if (vals != "")
						{
							vals = vals + ",";
						}
						// build string full of params
						vals = vals + String.Format ("@{0}VALUE", columns[i]);
					}

					 sqlStatement = String.Format ("INSERT INTO {0}({1}) VALUES({2})", tableName, cols, vals);
					lg.Instance.Line("SqlLiteDatabase.InsertData", ProblemType.MESSAGE, sqlStatement);

					SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);
					sqliteCon.Open ();




					
					using (SQLiteTransaction sqlTransaction = sqliteCon.BeginTransaction()) {
						SQLiteCommand command = new SQLiteCommand (sqlStatement, sqliteCon);
						// fill in parameters
						
						for (int j = 0; j < columns.Length; j++)
						{
							string param = String.Format ("@{0}VALUE",columns[j]);
							lg.Instance.Line("SqlLiteDatabase.InsertData", ProblemType.TEMPORARY, param);
							lg.Instance.Line("SqlLiteDatabase.InsertData", ProblemType.TEMPORARY, values[j].ToString());
							command.Parameters.AddWithValue(param, values[j]);
							//command.Parameters.Add(new SQLiteParameter(param, values[j] ));
						}


						command .ExecuteNonQuery ();
						sqlTransaction.Commit ();
					}
					sqliteCon.Close ();
				}
			} catch (System.Data.SQLite.SQLiteException) {
				throw new Exception("Trying to add a value that is already present in the unqiue colum");
			}

*/
		}
		/// <summary>
		/// Tables the exists.
		/// </summary>
		/// <returns>
		/// <c>true</c>, if exists was tabled, <c>false</c> otherwise.
		/// </returns>
		/// <param name='Table'>
		/// Table.
		/// </param>
		public override bool TableExists (string Table)
		{
			bool result = false;
			SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);
			sqliteCon.Open ();
			string selectTables = "Select name from sqlite_master where type='table' order by name;";
			SQLiteCommand selectCommand = new SQLiteCommand (selectTables, sqliteCon);
			SQLiteDataReader dataReader = selectCommand.ExecuteReader ();
			while (dataReader.Read()) {
				if (dataReader["name"].ToString() == Table){result = true; break;}

			}
			dataReader.Close ();
			lg.Instance.Line("BackupDatabase", ProblemType.MESSAGE, "TABLES");
			sqliteCon.Close ();
			return result;
		}

		/// <summary>
		/// Will export the entire database to a string and you can do what you want with it.
		/// </summary>
		/// <param name='file'>
		/// File.
		/// </param>
		public override string BackupDatabase ()
		{


			string result = "";
			/*Proper backup
			 * SELECT name FROM sqlite_master
WHERE type='table'
ORDER BY name;
*/
			try {
				result = ("--------------------------") + Environment.NewLine;
				result = result + ("**Just a hack for testing**")+ Environment.NewLine;
				SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);
				sqliteCon.Open ();

				string selectTables = "Select name from sqlite_master where type='table' order by name;";
				SQLiteCommand selectCommand = new SQLiteCommand (selectTables, sqliteCon);
				SQLiteDataReader dataReader = selectCommand.ExecuteReader ();
				lg.Instance.Line("BackupDatabase", ProblemType.MESSAGE, "TABLES");
				System.Collections.ArrayList ListOfTables = new System.Collections.ArrayList();

				while (dataReader.Read()) {
					result = result + (dataReader["name"].ToString())+ Environment.NewLine;
					ListOfTables.Add (dataReader["name"].ToString());
				}
				dataReader.Close ();
			

				foreach (string Table in ListOfTables)
				{
					result = result + ("Writing Table = " + Table)+ Environment.NewLine;;

				// Now write individual tables
					string selectSQL = String.Format ("SELECT * from {0}", Table);

				selectCommand = new SQLiteCommand (selectSQL, sqliteCon);
				dataReader = selectCommand.ExecuteReader ();
			
				// Iterate every record in the AppUser table
				while (dataReader.Read()) {

					for (int i = 0 ; i < dataReader.FieldCount; i++)
					{
							result = result +  (dataReader.GetName(i) + " |" +   dataReader[i].ToString())+ Environment.NewLine;
					//	Console.WriteLine ("");
					}
					result = result + ("*******************") + Environment.NewLine;
					/*Console.WriteLine("**Reading a line**");
					string id = dataReader["id"].ToString();
					string guid = dataReader["guid"].ToString();
					string xml = dataReader["xml"].ToString().Substring (0, Math.Min (25, dataReader["xml"].ToString().Length)); //dataReader["xml"].ToString();
					string status = dataReader["status"].ToString();//.Substring (0, Math.Min (25, dataReader["xml"].ToString().Length)); //dataReader["xml"].ToString();
					Console.WriteLine (String.Format ("id={0} | guid={1} |xml= {2}|status = {3}", id, guid, xml,status));
					//Console.WriteLine ("Name: " + dataReader.GetString (0)					                   + " Username: " + dataReader.GetString (1));
					*/
				}
				
				} // Writing out tables
				dataReader.Close ();
				sqliteCon.Close ();
			} catch (Exception ex) {
				lg.Instance.Line("BackupDatabase", ProblemType.EXCEPTION,ex.ToString());
			}
			result = result +  ("--------------------------")+ Environment.NewLine;
			return result;
		}
		public override void DropTableIfExists (string Table)
		{
			SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);
			sqliteCon.Open ();
			string sql = String.Format ("drop table if exists {0}", Table);
			try {
				SQLiteCommand selectCommand = new SQLiteCommand (sql, sqliteCon);
				selectCommand.ExecuteNonQuery ();
			} catch (Exception ex) {
				lg.Instance.Line("SqlLiteDatabase.DropTableIfExists",ProblemType.EXCEPTION, String.Format ("{0}, {1}", Table, ex.ToString()));
			}
			sqliteCon.Close ();
		}

		public void SearchFullSearchDatabase ()
		{

			//TODO: just testing
			SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);
			
			sqliteCon.Open();
			string selectSQL = String.Format ("SELECT * FROM {0} where {1} MATCH '{2}'", "fulltextsearch", "texttosearch", "fish");
			
			lg.Instance.Line("SqlLiteDatabase.GetValues", ProblemType.WARNING, selectSQL, Loud.CTRIVIAL);
			//string selectSQL = String.Format ("SELECT {0} FROM {1} LIMIT 1", columnToReturn, tableName, columnToTest, Test);
			SQLiteCommand selectCommand = new SQLiteCommand (selectSQL, sqliteCon);
			SQLiteDataReader dataReader = selectCommand.ExecuteReader ();
			int ColumnToReturn = 1;

			// Iterate every record in the AppUser table
			while (dataReader.Read()) {
				object[] tempRow = new object[ColumnToReturn]; // only have one search column
				for (int i = 0; i < ColumnToReturn; i++ )
				{
					Console.WriteLine (dataReader[i].ToString ());
				}

				//Console.WriteLine ("Name: " + dataReader.GetString (0)					                   + " Username: " + dataReader.GetString (1));
			}
			dataReader.Close ();
			sqliteCon.Close();
		}
		public void CreateFullSearchDatabase()
		{
			//TODO: http://answers.oreilly.com/topic/1955-how-to-use-full-text-search-in-sqlite/
			SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);
			
			sqliteCon.Open();
			string createAppUserTableSQL = String.Format ("CREATE VIRTUAL TABLE {0} USING FTS3 (texttosearch)","fulltextsearch");
			// status: just testing

			using (SQLiteTransaction sqlTransaction = sqliteCon.BeginTransaction())
			{
				// Create the table
				SQLiteCommand createCommand = new SQLiteCommand(createAppUserTableSQL
				                                                , sqliteCon);
				createCommand.ExecuteNonQuery();
				createCommand.Dispose();
				
				// Commit the changes into the database
				sqlTransaction.Commit();
			} // end using

			sqliteCon.Close();


		}
	}
}

