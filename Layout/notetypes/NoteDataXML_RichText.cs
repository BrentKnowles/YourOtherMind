using System;
using System.Windows.Forms;

namespace Layout
{
	public class NoteDataXML_RichText : NoteDataXML
	{
		#region formcontrols
		RichTextBox richBox ;
		#endregion

		public NoteDataXML_RichText () : base()
		{

		}

		public override void Save()
		{
			base.Save();
			this.RTF = richBox.Rtf;
		}

		public override void CreateParent (LayoutPanel Layout)
		{
			base.CreateParent (Layout);
			richBox = new RichTextBox();
			richBox.Parent = Parent;
			richBox.Dock = DockStyle.Fill;
			richBox.BringToFront();
			richBox.Rtf = this.RTF;
		
		}
	}
}

