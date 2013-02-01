using System;
using Layout;

namespace Testing
{
	public class FAKE_NoteDataXML_Text : NoteDataXML_RichText
	{
		public FAKE_NoteDataXML_Text ()
		{
		}
		public FAKE_NoteDataXML_Text(int height, int width):base(height, width)
		{
		}
		public void SetActive()
		{
			SetThisTextNoteAsActive();
		}

	}
}

