using System;
using CoreUtilities;
using System.Windows.Forms;
using System.Drawing;

namespace NoteDataXML_Picture
{
	public class NoteDataXML_Pictures  : Layout.NoteDataXML
	{
	
		public NoteDataXML_Pictures () : base()
		{
			Caption = Loc.Instance.GetString("Picture");
		}
		public override void CreateParent (Layout.LayoutPanelBase Layout)
		{
			base.CreateParent (Layout);
			CaptionLabel.Dock = DockStyle.Top;
			CaptionLabel.BackColor = Color.OrangeRed;
			
		}
		

		/// <summary>
		/// Registers the type.
		/// </summary>
		public override string RegisterType()
		{
			return Loc.Instance.GetString("Picture");
		}
	}
}

