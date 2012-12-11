using System;
using System.Windows.Forms;
using System.Drawing;
using CoreUtilities;

/* This is to contain the interface related elements (interaction with the GUID) to keep that distinct from the xml definition*/
namespace Layout
{
	public partial class NoteDataXML
	{
		#region UI
		protected Label CaptionLabel;
		protected ContextMenu contextMenu;
		#endregion

		public virtual void CreateParent(LayoutPanelBase Layout)
		{
			Parent = new NotePanel(this);
			
			
			
			Parent.Visible = true;
			Parent.Location = Location;
			Parent.Dock = System.Windows.Forms.DockStyle.None;
			Parent.Height = Height;
			Parent.Width = Width;
			
			
			Layout.NoteCanvas.Controls.Add ( Parent);
			
			
			
			CaptionLabel = new Label();
			CaptionLabel.MouseDown+= HandleMouseDown;
			CaptionLabel.MouseUp+= HandleMouseUp;
			CaptionLabel.MouseLeave+= HandleMouseLeave;
			CaptionLabel.MouseMove+= HandleMouseMove;
			CaptionLabel.Parent = Parent;
			CaptionLabel.BackColor = Color.Green;
			CaptionLabel.Dock = DockStyle.Fill;
			
			CaptionLabel.Text = this.Caption;

			contextMenu = new ContextMenu();
			MenuItem menuPanel = new MenuItem( );
			contextMenu.MenuItems.Add (Loc.Instance.Cat.GetString("Folder"));
			CaptionLabel.ContextMenu = contextMenu;

			
			
		}
		
		void HandleMouseMove (object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) {
				this.location.X += e.X - PanelMouseDownLocation.X;
				
				this.location.Y += e.Y - PanelMouseDownLocation.Y;
				UpdateLocation ();
			}
		}
		
		void HandleMouseLeave (object sender, EventArgs e)
		{
			CaptionLabel.BackColor = Color.Green;
		}
		
		void HandleMouseUp (object sender, MouseEventArgs e)
		{
			CaptionLabel.BackColor = Color.Green;
		}
		Point PanelMouseDownLocation ;
		void HandleMouseDown (object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) {
				// start moving
				CaptionLabel.BackColor = Color.Red;
				PanelMouseDownLocation = e.Location;
			}
			
		}

	}
}

