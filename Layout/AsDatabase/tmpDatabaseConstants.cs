using System;

namespace Layout.data
{
	public static class tmpDatabaseConstants
	{

		// trying to do a clever way of enforcing sizes and stuff
		static public string[] Columns = new string[4]{"id", "guid", "xml", "status"};
		public const  int ColumnCount = 4;
		// COLUMN constants
		static public string ID = Columns[0];//"id";
		static 	public string GUID = Columns[1];
		static public string XML =Columns[2];
		static public string STATUS =Columns[3];
		//having this lower case versus columns makes it easier to see
		static public string table_name = "pages";

	}
}

