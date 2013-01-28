using System;

namespace Testing
{
	public class FAKE_NoteDataXML_Panel : LayoutPanels.NoteDataXML_Panel
	{
		public FAKE_NoteDataXML_Panel ()
		{
		}
		public FAKE_NoteDataXML_Panel (int _height, int _width):base (_height, _width)
		{
		}
		public Layout.LayoutPanel myLayoutPanel()
		{
			return (Layout.LayoutPanel)this.panelLayout;
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
		public string GetParent_Section()
		{
			return this.panelLayout.Section;
		}
		public string GetParent_Keywords()
		{
			return this.panelLayout.Keywords;
		}
		public string GetParent_Notebook()
		{
			return this.panelLayout.Notebook;
		}
		public string GetParent_Subtype()
		{
			return this.panelLayout.Subtype;
		}
	}
}

