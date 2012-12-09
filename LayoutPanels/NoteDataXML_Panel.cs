using System;
using Layout;
using System.Windows.Forms;
using System.Drawing;

namespace LayoutPanels
{
	public class NoteDataXML_Panel :NoteDataXML
	{
		// This is where it gets tricky. Need to modify the list of valid data types to store!
		public NoteDataXML_Panel () : base()
		{
		}

		public override void Save()
		{
			base.Save();

		}
		
		public override void CreateParent (LayoutPanelBase Layout)
		{
			base.CreateParent (Layout);
			CaptionLabel.Dock = DockStyle.Top;

			LayoutPanel panelLayout = new LayoutPanel();
			panelLayout.Parent = Parent;
			panelLayout.Visible = true;
			
		}
	}
}

