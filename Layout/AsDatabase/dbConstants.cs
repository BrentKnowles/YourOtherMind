using System;

namespace Layout.data
{
	public class ColumnConstant
	{
		public string Name;
		// the type in the database
		public string Type; 
		public int Index;
		public int LayoutIndex;
		public ColumnConstant(string name, int index, string type, int layoutIndex)
		{
			Name = name;
			Index = index;
			Type = type;
			LayoutIndex = layoutIndex;

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
		static public ColumnConstant MAXIMIZETABS = new ColumnConstant("maximizetabs", 7, "boolean", 4);
		static public ColumnConstant STARS = new ColumnConstant("stars", 8, "INTEGER",5);
		static public ColumnConstant HITS = new ColumnConstant("hits", 9, "INTEGER",6);
		static public ColumnConstant DATECREATED = new ColumnConstant("datecreated", 10, "datetime",7);

		public const  int ColumnCount = 11;
		// The number of columns in the array being stoed
		public  const int LayoutCount = 8 ; // 11 - 3

		static public string[] Columns = new string[ColumnCount]{"id",     "guid",        "xml",      "status", "name", "showtabs", "subpanel",
			MAXIMIZETABS.Name, STARS.Name, HITS.Name, DATECREATED.Name};
		static public string[] Types   = new string[ColumnCount]{"INTEGER","TEXT UNIQUE",	"LONGTEXT",	"TEXT",   "VARCHAR(50)", "boolean", "boolean",
			MAXIMIZETABS.Type, STARS.Type, HITS.Type, DATECREATED.Type};





		// COLUMN constants (OLD WAY, will convert over) TO DO
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

