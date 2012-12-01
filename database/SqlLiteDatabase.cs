using System;
using System.Data.SQLite;
using System.Collections.Generic;
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
				Console.WriteLine(createAppUserTableSQL);
				
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
				
				// Close the database connection
				sqliteCon.Close();
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}

		}

		/// <summary>
		/// Gets the values. Returns a list with an object array. The list is in the form:
		/// 
		///         List[0] = the first row with an array matching in size the array ColumnToReturn that was Passed in
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
		public override List<object[]> GetValues (string tableName, string[] columnToReturn, string columnToTest, string Test)
		{

			if (CoreUtilities.Constants.BLANK == tableName) {
				throw new Exception ("You must provide a table to query");
			}

			if (CoreUtilities.Constants.BLANK == Test || CoreUtilities.Constants.BLANK == columnToTest) {
				throw new Exception ("Must define a Test criteria for retrieving text");
			}

			if (columnToReturn == null || columnToReturn.Length <= 0) {
				throw new Exception ("At least one column is required, to know what you want from the database.");
			}

			string ColumnsToReturnForQuery = base.ColumnArrayToStringForInserting (columnToReturn);

			if (CoreUtilities.Constants.BLANK == ColumnsToReturnForQuery) {
				throw new Exception("Column Array did not create a valid query string");
			}

			SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);
			sqliteCon.Open ();

			List<object[]> ReturnList = new List<object[]>();


			try {

			
				// Execute query on database
				//string selectSQL = "SELECT name, username FROM AppUser";
				string selectSQL = String.Format ("SELECT {0} FROM {1} where {2} = '{3}'", ColumnsToReturnForQuery, tableName, columnToTest, Test);
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
				Console.WriteLine(ex.ToString());
			}
			if (ReturnList == null || 0 == ReturnList.Count)
			{
				Console.WriteLine("GetValues query returned an empty list");
			}
			return ReturnList;
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
		public override bool UpdateSpecificColumnData (string tableName, string[] ColumnToAddTo, string[] ValueToAdd, string WhereColumn, string WhereValue)
		{

			if (ColumnToAddTo.Length != ValueToAdd.Length) {
				throw new Exception("Arrays of Columns and Values for those columns must be the same length");
			}

			bool ReturnValue = false;
			// hacking for rtf http://stackoverflow.com/questions/751172/system-data-sqlite-parameter-issue
			SQLiteParameter param = new SQLiteParameter("@myrtf");
			param.Value = ValueToAdd;
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
					
					SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);
					sqliteCon.Open ();
					using (SQLiteTransaction sqlTransaction = sqliteCon.BeginTransaction()) {
						SQLiteCommand command = new SQLiteCommand (sqlStatement, sqliteCon);

						command.Parameters.Add(param);
					command.Parameters.Add(new SQLiteParameter("@WhereValue", WhereValue ));

					// * build array of other ColumnsToAdd to
						for (int j = 0; j < ValueToAdd.Length; j++)
						{
							command.Parameters.Add(new SQLiteParameter(String.Format ("@{0}VALUE",ColumnToAddTo[j]), ValueToAdd[j] ));
						}

						if (1 == command .ExecuteNonQuery ())
					{
						sqlTransaction.Commit ();
						ReturnValue = true;
					}
					else
					{
						ReturnValue = false;
					}
					


					sqliteCon.Close ();
					}
				}
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString() + sqlStatement);
			}
			return ReturnValue;
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
		public override void InsertData (string tableName, string[] columns, object[] values)
		{

		

			string vals = "";
			string sqlStatement = "";
			// Performs an insert, change contents of sqlStatement to perform
			// update or delete.
			try {
				string cols = ColumnArrayToStringForInserting (columns);
				vals = ValueArrayToStringForInserting (values);
				if ("" != cols) {
					 sqlStatement = String.Format ("INSERT INTO {0}({1}) VALUES({2})", tableName, cols, vals);
			
					SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);
					sqliteCon.Open ();
					using (SQLiteTransaction sqlTransaction = sqliteCon.BeginTransaction()) {
						SQLiteCommand command = new SQLiteCommand (sqlStatement, sqliteCon);
						command .ExecuteNonQuery ();
						sqlTransaction.Commit ();
					}
					sqliteCon.Close ();
				}
			} catch (System.Data.SQLite.SQLiteException) {
				throw new Exception("Trying to add a value that is already present in the unqiue colum");
			}
		}

		/// <summary>
		/// Will export the entire database to the specified file
		/// </summary>
		/// <param name='file'>
		/// File.
		/// </param>
		public override void BackupDatabase (string file)
		{


			/*Proper backup
			 * SELECT name FROM sqlite_master
WHERE type='table'
ORDER BY name;
*/
			try {
				Console.WriteLine ("--------------------------");
				Console.WriteLine ("**Just a hack for testing**");
				SQLiteConnection sqliteCon = new SQLiteConnection (Connection_String);
				sqliteCon.Open ();

				string selectTables = "Select name from sqlite_master where type='table' order by name;";
				SQLiteCommand selectCommand = new SQLiteCommand (selectTables, sqliteCon);
				SQLiteDataReader dataReader = selectCommand.ExecuteReader ();
				Console.WriteLine("TABLES");
				System.Collections.ArrayList ListOfTables = new System.Collections.ArrayList();

				while (dataReader.Read()) {
					Console.WriteLine(dataReader["name"].ToString());
					ListOfTables.Add (dataReader["name"].ToString());
				}
				dataReader.Close ();
			

				foreach (string Table in ListOfTables)
				{
					Console.WriteLine("Writing Table = " + Table);

				// Now write individual tables
					string selectSQL = String.Format ("SELECT * from {0}", Table);

				selectCommand = new SQLiteCommand (selectSQL, sqliteCon);
				dataReader = selectCommand.ExecuteReader ();
			
				// Iterate every record in the AppUser table
				while (dataReader.Read()) {

					for (int i = 0 ; i < dataReader.FieldCount; i++)
					{
						Console.Write (dataReader.GetName(i) + " |" +   dataReader[i].ToString());
						Console.WriteLine ("");
					}
					Console.WriteLine ("*******************");
					/*Console.WriteLine("**Reading a line**");
					string id = dataReader["id"].ToString();
					string guid = dataReader["guid"].ToString();
					string xml = dataReader["xml"].ToString().Substring (0, Math.Min (25, dataReader["xml"].ToString().Length)); //dataReader["xml"].ToString();
					string status = dataReader["status"].ToString();//.Substring (0, Math.Min (25, dataReader["xml"].ToString().Length)); //dataReader["xml"].ToString();
					Console.WriteLine (String.Format ("id={0} | guid={1} |xml= {2}|status = {3}", id, guid, xml,status));
					//Console.WriteLine ("Name: " + dataReader.GetString (0)					                   + " Username: " + dataReader.GetString (1));
					*/
				}
				dataReader.Close ();
				sqliteCon.Close ();
				} // Writing out tables
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}
			Console.WriteLine ("--------------------------");
		}


	}
}

