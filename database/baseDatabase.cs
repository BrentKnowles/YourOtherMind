// baseDatabase.cs
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
using System.Collections.Generic;
namespace database
{
	/// <summary>
	/// Base database.
	/// </summary>
	abstract public class BaseDatabase : IDisposable
	{public const string GetValues_ANY="any";
		public const string GetValues_WILDCARD="*";
		public BaseDatabase (string database)
		{
		}
		protected string database_name="";

		/// <summary>
		/// The name of the database to use
		/// </summary>
		/// <value>
		/// The connection_ string.
		/// </value>
		protected string Database_Name {
			get { return database_name;}
			set{ database_name = value;}
		}

		public abstract void Dispose();

		public abstract System.Data.DataTable AsDataTable (string tablename);

		public abstract string ExecuteCommand (string select, DateTime start, DateTime end, bool IgnoreDate);



		/// <summary>
		/// Columns the array to column string.
		/// </summary>
		/// <returns>
		/// The array to column string.
		/// </returns>
		/// <param name='Columns'>
		/// Columns.
		/// </param>
		/// <param name='Types'>
		/// Types.
		/// </param>
		protected string ColumnArrayToColumnString_ForCreatingATable (string[] Columns, string[] Types)
		{
			if (Columns.Length != Types.Length) {
				throw new Exception("ColumnArrayToColumnString: Columns and Types must have same number of items");
			}
			string column = "";
			for (int i = 0; i < Columns.Length; i++)
			{
				if ("" != column)
				{
					column = column + ", ";
				}
				column = String.Format("{0} {1} {2}", column, Columns[i], Types[i]);

			}
			return column;
		}
		/// <summary>
		/// Columns the array to string for inserting.
		/// Takes the array and returns something like ItemA,ItemB,ItemC
		/// </summary>
		/// <returns>
		/// The array to string for inserting.
		/// </returns>
		/// <param name='Columns'>
		/// Columns.
		/// </param>
		protected string ColumnArrayToStringForInserting (string[] Columns)
		{
			if (Columns == null) {
				throw new Exception ("need array of columns");
			}
			string result = CoreUtilities.Constants.BLANK;
			foreach (string s in Columns) {
				if (CoreUtilities.Constants.BLANK != result)
				{
					result =  result + ",";
				}
				result = result + s;
			}
			return result;

		}
		/// <summary>
		/// Converts a list of colmns into a string appropriate for an SQL query
		/// Takes the array and returns something like ItemA,ItemB,ItemC
		/// Converting objects to strings
		/// </summary>
		/// <returns>
		/// The array to string for inserting.
		/// </returns>
		/// <param name='Columns'>
		/// Columns.
		/// </param>
		protected string ValueArrayToStringForInserting (object[] Columns)
		{
			if (Columns == null) {
				throw new Exception ("need array of columns");
			}
			string result = "";
			foreach (object s in Columns) {
				if ("" != result)
				{
					result =  result + ",";
				}
				// TODO Make this better. For NULL we do not wrap in quotes
				if (s.ToString()=="NULL")
				{
					result = result + String.Format ("{0}",s.ToString());
				}
				else
				{

				result = result + String.Format ("'{0}'",s.ToString());
				}
			}
			return result;
			
		}
		public bool ColumnExists(System.Data.IDataReader reader, string columnName)
		{
			for (int i = 0; i < reader.FieldCount; i++)
			{
				if (reader.GetName(i) == columnName)
				{
					return true;
				}
			}
			
			return false;
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
		public abstract string[] SearchDatabase (string SearchTerm, string Params);


	//	public abstract void AddColumn(string table, string newColumn, string newColumnType);

		public abstract void DropTableIfExists(string Table);
		public abstract bool Exists(string Table, string Column, string ColumnValue);

		public abstract  List<object[]> GetValues (string tableName, string[] columnToReturn, string columnToTest, object Test ,string Sorting,string ExtraWhere);
		public abstract List<object[]> GetValues (string tableName, string[] columnToReturn, string columnToTest, object Test,string Sorting);
		public abstract List<object[]> GetValues (string tableName, string[] columnToReturn, string columnToTest, object Test);
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
		public abstract void InsertData(string tableName, string[] columns, object[] values);
		public abstract bool UpdateSpecificColumnData (string tableName, string[] ColumnToAddTo, object[] ValueToAdd, string WhereColumn, string WhereValue);
		public abstract bool TableExists (string Table);
		/// <summary>
		/// Will export the entire database to the specified file
		/// </summary>
		/// <param name='file'>
		/// File.
		/// </param>
		public abstract string BackupDatabase();
		

		/// <summary>
		/// Creates the table if does not exist.
		/// 
		/// The implementation should test to see if table exists and then avoid making it
		/// </summary>
		/// <param name='Table'>
		/// Table.
		/// </param>
		/// <param name='Columns'>
		/// Columns.
		/// </param>
		/// <param name='Types'>
		/// A striung deliminted list like TEXT
		/// </param>
		public abstract void CreateTableIfDoesNotExist(string Table, string[] Columns, string[] Types, string PrimaryKeyField);


		public abstract bool IsInMultipleUpdateMode ();
		public abstract void UpdateMultiple_End ();
		public  abstract bool UpdateDataMultiple (string tableName, string[] ColumnToAddTo, object[] ValueToAdd, string WhereColumn, string WhereValue);
		public abstract void UpdateMultiple_Start ();
		public abstract bool Delete (string tableName, string WhereColumn, string WhereValue);
		public abstract int Count(string tablename);
		public abstract  List<string> GetBackupRowAsString(string Table, string ColumnToMatch, string ColumnValueToMatch, bool IncludeTableNameAtTop);
		public abstract int ImportFromString ( string Incoming,  string TestColumn);
		public abstract List<string> GetListOfTables ();
		public abstract List<string> ExecuteCommandMultiple (string select, DateTime start, DateTime end, bool IgnoreDate);
	}
}

