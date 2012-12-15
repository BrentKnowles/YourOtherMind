using System;

namespace Layout.data
{
	public static class tmpDatabaseConstants
	{


		public const  int ColumnCount = 6;
		static public string[] Columns = new string[ColumnCount]{"id",     "guid",        "xml",      "status", "name","showtabs"};
		static public string[] Types   = new string[ColumnCount]{"INTEGER","TEXT UNIQUE",	"LONGTEXT",	"TEXT",   "VARCHAR(50)", "boolean"};


		// COLUMN constants
		static public string ID = Columns[0];//"id";
		static 	public string GUID = Columns[1];
		static public string XML =Columns[2];
		static public string STATUS =Columns[3];
		static public string NAME=Columns[4];
		static public string SHOWTABS=Columns[5];
		//having this lower case versus columns makes it easier to see
		static public string table_name = "pages";

	}
}

