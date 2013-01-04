using System;

namespace Layout.data
{
	public class ColumnConstant
	{
		public string Name;
		// the type in the database
		public string Type; 
		public int Index;
		public ColumnConstant(string name, int index, string type)
		{
			Name = name;
			Index = index;
			Type = type;
		}
		public static implicit operator string(ColumnConstant x) {
			return x.ToString();}
		public override string ToString ()
		{
			return Name;
		}

	}
	public static class dbConstants
	{
		static public ColumnConstant MAXIMIZETABS = new ColumnConstant("maximizetabs", 7, "boolean");

		public const  int ColumnCount = 8;
		static public string[] Columns = new string[ColumnCount]{"id",     "guid",        "xml",      "status", "name", "showtabs", "subpanel",
			MAXIMIZETABS.Name};
		static public string[] Types   = new string[ColumnCount]{"INTEGER","TEXT UNIQUE",	"LONGTEXT",	"TEXT",   "VARCHAR(50)", "boolean", "boolean",
			MAXIMIZETABS.Type};





		// COLUMN constants
		static public string ID = Columns[0];//"id";
		static 	public string GUID = Columns[1];
		static public string XML =Columns[2];
		static public string STATUS =Columns[3];
		static public string NAME=Columns[4];
		static public string SHOWTABS=Columns[5];
		static public string SUBPANEL=Columns[6];
	
		//having this lower case versus columns makes it easier to see
		static public string table_name = "pages";

	}
}

