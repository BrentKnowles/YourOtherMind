using System;
using CoreUtilities;
using System.Xml.Serialization;
using System.Windows.Forms;

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
		public NoteDataXML_SystemOnly () : base()
		{
			// do not want system layouts to have captions
			Caption = Constants.BLANK;
		}
		// this delegate is setup when the window is open and fired if user closes the note
		public Action<LayoutPanelBase> AlertWhenClosed;
		public NoteDataXML_SystemOnly(int _height, int _width) : base(_height, _width)
		{
			// do not want system layouts to have captions
			Caption = Constants.BLANK;
		}
		[XmlIgnore]
		public override bool IsSystemNote{
			get {return true;}
		}
		public override void CreateParent (LayoutPanelBase _Layout)
		{
			//Width = 500;
			base.CreateParent (_Layout);

			CaptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
			//CaptionLabel.Visible = false;

		//	Maximize(true);
			//Parent.Enter+= HandleEnter;
			Parent.MouseEnter+= HandleMouseEnter;
			Parent.BringToFront();
			//Parent.Dock = System.Windows.Forms.DockStyle.Left;
			Dock = DockStyle.Fill;
			UpdateLocation();
		//	Parent.BringToFront();
		}
		/// <summary>
		/// Gets the child layout.
		/// 
		/// called when needing to remove the Layout from the Window List (i.e., X clicked to close it)
		/// </summary>
		/// <returns>
		/// The child layout.
		/// </returns>
		public LayoutPanelBase GetChildLayout()
		{
			foreach (Control control in Parent.Controls) {
				if (control.GetType ().BaseType == typeof(LayoutPanelBase))
				{
					//NewMessage.Show ("FOUND : " + ((LayoutPanelBase)control).GUID);
					return ((LayoutPanelBase)control);
					//LayoutDetails.Instance.CurrentLayout =((LayoutPanelBase)control);

				}
			}
			return null;
		}

		/// <summary>
		/// Handles the mouse enter.
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleMouseEnter (object sender, EventArgs e)
		{
			// do not worry about lack of return value here or anything because we are setting the layout in the loop
			GetChildLayout(); 
		}

		void HandleEnter (object sender, EventArgs e)
		{

		}
	

		/// <summary>
		/// Registers the type.
		/// </summary>
		public override string RegisterType()
		{
			return Loc.Instance.Cat.GetString("System");
		}
		protected override void HandleDeleteClick (object sender, EventArgs e)
		{

			if (AlertWhenClosed != null)
			{
			
				AlertWhenClosed(GetChildLayout());
			}
			DeleteNote(this);
		}
	}
}

