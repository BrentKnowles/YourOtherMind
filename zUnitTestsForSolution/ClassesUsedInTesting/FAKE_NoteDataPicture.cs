using System;
using System.Windows.Forms;

namespace Testing
{
	public class FAKE_NoteDataPicture : NoteDataXML_Picture.NoteDataXML_Pictures
	{
		public FAKE_NoteDataPicture ()
		{
		}
		protected override string GetLink ()
		{
			return base.GetLink ();
		}



		public string _GetLink()
		{
			return GetLink();
		}

		public void _SetImage(string file)
		{
			SetImage(file);
		}

		public PictureBox GetPictureBox()
		{
			return this.Pic;
		}

	}
}

