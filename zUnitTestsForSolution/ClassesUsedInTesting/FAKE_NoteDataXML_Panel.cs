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
		public int CountNotes ()
		{
			int count = 0;
			System.Collections.ArrayList list = this.panelLayout.GetAllNotes();
			foreach (Layout.NoteDataInterface note in list) {
				count++;
				_w.output(String.Format ("**{0}** GUID = {1}", count, note.GuidForNote));
			}
			return count;
			//return this.panelLayout.CountNotes();
		}
	}
}

