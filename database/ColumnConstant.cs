using System;

namespace database
{
	/// <summary>
	/// Column constant. Represents a COLUMN in a database, used to clean up the constants used for table definitions
	/// 
	/// usage: static public ColumnConstant SUBPANEL = new ColumnConstant("subpanel", 6, "boolean", 3);
	/// </summary>
	public class ColumnConstant
		{
		    // the name of the column in the database
			public string Name;
			// the type in the database
			public string Type; 
		    // Index of column in database
			public int Index;
		    // index in internal storage once loaded into memory (may or may not be needed)
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
}

