using System;
using CoreUtilities;

namespace Layout
{
	/// <summary>
	/// Note data XM l_ system only.
	/// 
	/// This is a special note type that is only allowed on the System note. It 'holds' other notes and is the target of the NoteList (when in Layout List mode)
	/// 
	/// </summary>
	public class NoteDataXML_SystemOnly: NoteDataXML
	{
		public NoteDataXML_SystemOnly ()
		{
		}
		public override void CreateParent (LayoutPanelBase _Layout)
		{
			base.CreateParent (_Layout);

			//CaptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
			CaptionLabel.Visible = false;
			Parent.Dock = System.Windows.Forms.DockStyle.Left;
		}
		/// <summary>
		/// Registers the type.
		/// </summary>
		public override string RegisterType()
		{
			return Loc.Instance.Cat.GetString("System");
		}
	}
}
