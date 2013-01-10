using System;
using Layout;

namespace Testing
{
	public class FAKE_NoteDataXML_Text : NoteDataXML_RichText
	{
		public FAKE_NoteDataXML_Text ()
		{
		}
		public void SetActive()
		{
			SetThisTextNoteAsActive();
		}

	}
}

