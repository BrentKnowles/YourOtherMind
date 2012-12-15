using System;

namespace Testing
{
	public class FAKE_NoteDataXML_Panel : LayoutPanels.NoteDataXML_Panel
	{
		public FAKE_NoteDataXML_Panel ()
		{
		}
		public Layout.LayoutPanel myLayoutPanel()
		{
			return this.panelLayout;
		}
		public int CountNotes()
		{
			return this.panelLayout.CountNotes();
		}
	}
}

