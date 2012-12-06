using System;
using database;


namespace Testing
{
	/// <summary>
	/// This is a Fake testing class to expose Protected methods in nuni
	/// as : http://www.codeproject.com/Articles/9715/How-to-Test-Private-and-Protected-methods-in-NET
	/// </summary>
	public class UnitTest_Class_Database : SqlLiteDatabase
	{
		public UnitTest_Class_Database (string database):base(database)
		{
		}
		public string TestColumnArrayToStringForInserting(string[] columns)
		{
			return ColumnArrayToStringForInserting(columns);
		}

		public bool TestAddMissingColumn (string table, string[] columns, string[] types)
		{
			return AddMissingColumn(table, columns, types);
		}
	}
}

