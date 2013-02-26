using System;
using Layout;
namespace Testing
{
	public class FAKE_FindBar : FindBarStatusStrip
	{
		public FAKE_FindBar ()
		{
		}
		public int zPosition()
		{
			return this.Position;
		}
		public int PositionsFOUND()
		{
			return PositionsFound.Count;
		}
		public void SetLastRichText(RichTextExtended box)
		{
			LastRichText = box;
		}
		public void GoToNext()
		{
			FindNext ();
		}
		public void DoFindBuildLIstTesty(string Text)
		{
			DoFind_BuildListOfFoundPositions("dog", false, Text,0);
		}
		public void Replace_Text(string oldText, string newText)
		{
			this.ReplaceTextNow(oldText, newText);
		}
	}
}

