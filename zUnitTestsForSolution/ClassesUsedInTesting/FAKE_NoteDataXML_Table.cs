using System;

namespace Testing
{
	public class FAKE_NoteDataXML_Table : Layout.NoteDataXML_Table
	{
		public FAKE_NoteDataXML_Table (int height, int width):base (height, width)
		{

		}
		public FAKE_NoteDataXML_Table ():base()
		{
		}
		public void Copy()
		{
			Table.Copy();
		}

		public appframe.TablePanel GetTablePanel()
		{
			return Table;
		}

	}
}

