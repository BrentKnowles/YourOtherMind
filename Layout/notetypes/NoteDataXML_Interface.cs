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
		protected ContextMenuStrip contextMenu;
		#endregion


		// http://stackoverflow.com/questions/7367152/c-dynamically-assign-method-method-as-variable
		Func<System.Collections.Generic.List<NoteDataInterface>> GetAvailableFolders;
		Action<string,string> MoveNote;

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

			contextMenu = new ContextMenuStrip();

			ToolStripDropDownButton menuFolder = new ToolStripDropDownButton();
			menuFolder.Text = Loc.Instance.Cat.GetString("Folder");

			contextMenu.Items.Add (menuFolder);
			menuFolder.MouseEnter+= HandleMenuFolderMouseEnter;

			ToolStripButton label  = new ToolStripButton();
			label.Text = Loc.Instance.Cat.GetString("Example2");
			contextMenu.Items.Add (label);

			CaptionLabel.ContextMenuStrip = contextMenu;
			GetAvailableFolders = Layout.GetAvailableFolders;
			MoveNote = Layout.MoveNote;
			
		}
		/// <summary>
		/// Handles the popup menu folder.
		/// Draws in list of all the FOLDERs at the current level 
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleMenuFolderMouseEnter (object sender, EventArgs e)
		{
			(sender as ToolStripDropDownButton).DropDownItems.Clear ();
			foreach (NoteDataInterface note in GetAvailableFolders()) {
				ToolStripMenuItem item = (ToolStripMenuItem)(sender as ToolStripDropDownButton).DropDownItems.Add (note.Caption);
				item.Click+= HandleMenuFolderClick;
				item.Tag = note.GuidForNote;
			}

		}
		/// <summary>
		/// Handles the menu folder click.
		/// 
		/// Assigns to a Folder or to a New Folder
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleMenuFolderClick (object sender, EventArgs e)
		{
			if ((sender as ToolStripMenuItem).Tag == null) {
				lg.Instance.Line("NoteDataXML.HandleMenuFolderCLikc", ProblemType.WARNING, "A GUID was not assigned to this folder menu option");
			}
			else
				MoveNote(this.GuidForNote,  (sender as ToolStripMenuItem).Tag.ToString ());
		}

		void HandlePopupMenuFolder (object sender, EventArgs e)
		{


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

