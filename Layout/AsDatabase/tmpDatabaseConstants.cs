using System;

namespace Layout.data
{
	public static class tmpDatabaseConstants
	{

		// trying to do a clever way of enforcing sizes and stuff
		static public string[] Columns = new string[4]{"id", "guid", "xml", "status"};
		public const  int ColumnCount = 4;



			// old weay
		static public string ID = "id";
		static 	public string GUID = "guid";
		static public string XML = "xml";
		static public string STATUS ="status";

		static public string table_name = "pages";

	}
}

