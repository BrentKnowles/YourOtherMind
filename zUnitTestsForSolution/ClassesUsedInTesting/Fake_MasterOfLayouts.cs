using System;

namespace Testing
{
	public class Fake_MasterOfLayouts : Layout.MasterOfLayouts
	{
		public Fake_MasterOfLayouts ()
		{
		}

		public static int GetRowCountOfCombinedLinkTableAfterCallToReciprocalLinks()
		{
			Layout.NoteDataXML_Table link = null;
			link = BuildLinMasterLinkTable();
			return link.RowCount();
		}
	}
}

