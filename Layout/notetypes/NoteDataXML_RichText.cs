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
			this.Data1 = richBox.Rtf;
		}

		public override void CreateParent (LayoutPanelBase Layout)
		{
			base.CreateParent (Layout);
			CaptionLabel.Dock = DockStyle.Top;
			richBox = new RichTextBox();
			richBox.Parent = Parent;
			richBox.Dock = DockStyle.Fill;
			richBox.BringToFront();
			richBox.Rtf = this.Data1;
		
		}
	}
}

