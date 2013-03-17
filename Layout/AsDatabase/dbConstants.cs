using System;
using database;
namespace Layout.data
{

	public static class dbConstants
	{
		static public ColumnConstant NAME = new ColumnConstant("name", 4, "VARCHAR(50)", 1);
		static public ColumnConstant SHOWTABS = new ColumnConstant("showtabs", 5, "boolean", 2);
		// true or false is a subpanel
		static public ColumnConstant SUBPANEL = new ColumnConstant("subpanel", 6, "boolean", 3);
		static public ColumnConstant MAXIMIZETABS = new ColumnConstant("maximizetabs", 7, "boolean", 4);

		// THESE appear in info OR FILTERS
		static public ColumnConstant STARS = new ColumnConstant("stars", 8, "INTEGER",5);
		static public ColumnConstant HITS = new ColumnConstant("hits", 9, "INTEGER",6);
		static public ColumnConstant DATECREATED = new ColumnConstant("datecreated", 10, "datetime",7);
		static public ColumnConstant DATEEDITED = new ColumnConstant("dateedited", 11, "datetime",8);
		static public ColumnConstant NOTEBOOK = new ColumnConstant("notebook", 12, "TEXT",9);
		static public ColumnConstant SECTION = new ColumnConstant("section", 13, "TEXT",10);
		static public ColumnConstant TYPE = new ColumnConstant("type", 14, "TEXT",11);
		// these will be stored in PROPERIES
		static public ColumnConstant SOURCE = new ColumnConstant("source", 15, "TEXT",12);
		static public ColumnConstant WORDS = new ColumnConstant("words", 16, "INTEGER",13);
		static public ColumnConstant KEYWORDS = new ColumnConstant("keywords", 17, "TEXT", 14);
		static public ColumnConstant LINKTABLE = new ColumnConstant("linktable", 18, "LONGTEXT", 15);
		static public ColumnConstant BACKGROUNDCOLOR = new ColumnConstant("backcolor", 19, "INTEGER", 16);
		static public ColumnConstant BLURB = new ColumnConstant("blurb", 20,"TEXT", 17);
		static public ColumnConstant PARENT_GUID = new ColumnConstant("parentguid", 21, "TEXT", 18);


		public const  int ColumnCount = 22;
		// The number of columns in the array being stoed
		public  const int LayoutCount = 19 ; // 11 - 3

		static public string[] Columns = new string[ColumnCount]{"id",     "guid",        "xml",      "status", NAME.Name, SHOWTABS.Name, SUBPANEL.Name,
			MAXIMIZETABS.Name, STARS.Name, HITS.Name, DATECREATED.Name, DATEEDITED.Name, NOTEBOOK.Name, SECTION.Name, TYPE.Name, SOURCE.Name, WORDS.Name, KEYWORDS.Name,
			LINKTABLE.Name, BACKGROUNDCOLOR.Name, BLURB.Name, PARENT_GUID.Name};
		static public string[] Types   = new string[ColumnCount]{"INTEGER","TEXT UNIQUE",	"LONGTEXT",	"TEXT",  NAME.Type, SHOWTABS.Type, SUBPANEL.Type,
			MAXIMIZETABS.Type, STARS.Type, HITS.Type, DATECREATED.Type, DATEEDITED.Type, NOTEBOOK.Type, SECTION.Type, TYPE.Type, SOURCE.Type, WORDS.Type, KEYWORDS.Type,
		    LINKTABLE.Type, BACKGROUNDCOLOR.Type, BLURB.Type, PARENT_GUID.Type};





		// COLUMN constants (OLD WAY, will convert over) TO DO
		static public string ID = Columns[0];//"id";
		static 	public string GUID = Columns[1];
		static public string XML =Columns[2];
		static public string STATUS =Columns[3];
	//	static public string NAME=Columns[4];
		//static public string SHOWTABS=Columns[5];

	
		//having this lower case versus columns makes it easier to see
		static public string table_name = "pages";

	}
}

