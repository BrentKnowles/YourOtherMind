using System;
using System.Windows.Forms;
using CoreUtilities;

namespace Layout
{
	public class NoteDataXML_RichText : NoteDataXML
	{
		#region formcontrols
		RichTextBox richBox ;
		#endregion

		public NoteDataXML_RichText () : base()
		{
			Caption = Loc.Instance.Cat.GetString("Text Note");
		}

		public override void Save()
		{
			base.Save();
			this.Data1 = richBox.Rtf;
		}
		/// <summary>
		/// returns the text as text
		/// </summary>
		/// <returns>
		/// The as text.
		/// </returns>
		public string GetAsText()
		{
			return richBox.Text;
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

