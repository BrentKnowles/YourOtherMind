using System;

namespace appframe
{
	/// <summary>
	/// Column details.
	/// This is stored in the NoteDataXML_Table class. We use it here to build tables when columsn reorganized
	/// </summary>
	public class ColumnDetails
	{
		private string columnName;
		private int columnWidth;
		public string ColumnName {
			get { return columnName; }
			set { columnName = value;}
		}
		public int ColumnWidth {
			get { return columnWidth;}
			set { columnWidth = value;}
		}
		public ColumnDetails(string name, int width)
		{
			columnName = name;
			columnWidth = width;
		}
		// need this for serializing
		public ColumnDetails()
		{
		}
	}
}

