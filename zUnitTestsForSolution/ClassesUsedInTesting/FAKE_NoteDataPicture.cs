using System;
using System.Windows.Forms;

namespace Testing
{
	public class FAKE_NoteDataPicture : NoteDataXML_Picture.NoteDataXML_Pictures
	{
		public FAKE_NoteDataPicture ()
		{
		}
		public FAKE_NoteDataPicture(int _height, int _width	): base(_height, _width)
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

