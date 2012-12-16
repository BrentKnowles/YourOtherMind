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
		protected ToolStrip CaptionLabel;
		protected ContextMenuStrip contextMenu;
		// this holds the PropertyGrid
		protected Panel PropertyPanel; 
		protected PropertyGrid propertyGrid;
		// the properties button on header
		protected ToolStripDropDownButton properties; 
		protected ToolStripLabel captionLabel;

		#endregion


		// http://stackoverflow.com/questions/7367152/c-dynamically-assign-method-method-as-variable
		Func<System.Collections.Generic.List<NoteDataInterface>> GetAvailableFolders;
		Action<string,string> MoveNote;
		protected Action<bool> SetSaveRequired;
		protected delegate void delegate_UpdateListOfNotes();


		//delegate_UpdateListOfNotes UpdateListOfNotes;
		public virtual void CreateParent (LayoutPanelBase _Layout)
		{
			Parent = new NotePanel (this);
			

			
			Parent.Visible = true;
			Parent.Location = Location;
			Parent.Dock = System.Windows.Forms.DockStyle.None;
			Parent.Height = Height;
			Parent.Width = Width;
			
			try {
				_Layout.NoteCanvas.Controls.Add (Parent);
			} catch (Exception ex) {
				lg.Instance.Line("CreateParent", ProblemType.EXCEPTION, "Unable to create note after changing properties" + ex.ToString ());
				throw new Exception("Failed to add control");
			}
			
			
			CaptionLabel = new ToolStrip();
			CaptionLabel.MouseDown+= HandleMouseDown;
			CaptionLabel.MouseUp+= HandleMouseUp;
			CaptionLabel.MouseLeave+= HandleMouseLeave;
			CaptionLabel.MouseMove+= HandleMouseMove;
			CaptionLabel.Parent = Parent;
			CaptionLabel.BackColor = Color.Green;
			CaptionLabel.Dock = DockStyle.Fill;
			CaptionLabel.GripStyle = ToolStripGripStyle.Hidden;
			//CaptionLabel.Text = this.Caption;
			 captionLabel = new ToolStripLabel(this.Caption);

			captionLabel.MouseDown+= HandleMouseDown;
			captionLabel.MouseUp+= HandleMouseUp;
			captionLabel.MouseLeave+= HandleMouseLeave;
			captionLabel.MouseMove+= HandleMouseMove;
			CaptionLabel.Items.Add (captionLabel);
			if (Caption == "") NewMessage.Show ("Caption is blank");


			properties = new ToolStripDropDownButton(Loc.Instance.GetString("*"));
			ToolStripTextBox captionEditor = new ToolStripTextBox();
			captionEditor.Text = Caption;
			captionEditor.TextChanged+= HandleCaptionTextChanged;
			captionEditor.KeyDown += HandleCaptionEditorKeyDown;
			properties.DropDownItems.Add (captionEditor);

			CaptionLabel.Items.Add (properties);
			ToolStripSeparator sep =  new ToolStripSeparator();
			sep.Width = 0;
			sep.BackColor = CaptionLabel.BackColor;
			sep.ForeColor = CaptionLabel.ForeColor;
			//HACK for some reason labels inside of panels don't draw LAST toolstripitem on LOAD (on only on load!) 
			CaptionLabel.Items.Add (sep);

			contextMenu = new ContextMenuStrip();

		


			ToolStripDropDownButton menuFolder = new ToolStripDropDownButton();
			menuFolder.Text = Loc.Instance.Cat.GetString("Folder");
			contextMenu.Items.Add (menuFolder);
			menuFolder.MouseEnter+= HandleMenuFolderMouseEnter;


			ToolStripButton menuProperties = new ToolStripButton();
			menuProperties.Text = Loc.Instance.Cat.GetString ("Properties");
			contextMenu.Items.Add (menuProperties);
			menuProperties.Click+= HandlePropertiesClick;


			ToolStripButton label  = new ToolStripButton();
			label.Text = Loc.Instance.Cat.GetString("Example2");
			contextMenu.Items.Add (label);

			CaptionLabel.ContextMenuStrip = contextMenu;

			PropertyPanel = new Panel();
			PropertyPanel.Visible = false;
			PropertyPanel.Parent = Parent;
			PropertyPanel.Height = 300;
			PropertyPanel.Dock = DockStyle.Top;
			PropertyPanel.AutoScroll = true;

			 propertyGrid = new PropertyGrid();
			propertyGrid.SelectedObject = this;

			propertyGrid.PropertyValueChanged+= HandlePropertyValueChanged;
			propertyGrid.Parent = PropertyPanel;

			propertyGrid.Dock = DockStyle.Fill;

			Button CommitChanges = new Button();
			CommitChanges.Text = Loc.Instance.Cat.GetString("Update Note");
			CommitChanges.Parent = PropertyPanel;
			CommitChanges.Dock = DockStyle.Bottom;
			CommitChanges.Click += HandleCommitChangesClick;
			CommitChanges.BringToFront();


			// Set up delegates

			GetAvailableFolders = _Layout.GetAvailableFolders;
			MoveNote = _Layout.MoveNote;
			SetSaveRequired = _Layout.SetSaveRequired;

			Layout = _Layout;
		}

		void HandleCaptionEditorKeyDown (object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) {
				// commit the change to the caption
				Caption = (sender as ToolStripTextBox).Text;
				SetSaveRequired (true);
				// so we don't need to call update for such a simple change
				captionLabel.Text = Caption; 
			}
		}

		void HandleCaptionTextChanged (object sender, EventArgs e)
		{
			
		}


		void HandleCommitChangesClick (object sender, EventArgs e)
		{
			Parent.AutoScroll = false;
			((NoteDataXML)propertyGrid.SelectedObject).Update(Layout);
		}

		void HandlePropertyValueChanged (object s, PropertyValueChangedEventArgs e)
		{
	// TODO: Finish reomving this if I don't need it

	
		}

		void HandlePropertiesClick (object sender, EventArgs e)
		{

			PropertyPanel.Visible = !PropertyPanel.Visible;
			if (PropertyPanel.Visible == true) {
				Parent.AutoScroll = true;
				// TODO just a hack until sizing is in
				Height = 200;
				Width = 200;
				PropertyPanel.AutoScroll = true;
				//PropertyPanel.SendToBack();
				//CaptionLabel.BringToFront();
				CaptionLabel.SendToBack ();
				UpdateLocation ();
			} else {
				Parent.AutoScroll = false;
			}
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
			// Draw Potential Folders
			(sender as ToolStripDropDownButton).DropDownItems.Clear ();
			ToolStripMenuItem up = (ToolStripMenuItem)(sender as ToolStripDropDownButton).DropDownItems.Add (Loc.Instance.Cat.GetString ("Out of Folder"));
			up.Tag = "up";
			up.Click += HandleMenuFolderClick;

			foreach (NoteDataInterface note in GetAvailableFolders()) {
				// we do not add ourselves
				if (note.GuidForNote != this.GuidForNote) {
					ToolStripMenuItem item = (ToolStripMenuItem)(sender as ToolStripDropDownButton).DropDownItems.Add (note.Caption);
					item.Click += HandleMenuFolderClick;
					item.Tag = note.GuidForNote;
				}
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
				lg.Instance.Line ("NoteDataXML.HandleMenuFolderCLikc", ProblemType.WARNING, "A GUID was not assigned to this folder menu option");
			} else {
				MoveNote (this.GuidForNote, (sender as ToolStripMenuItem).Tag.ToString ());
				SetSaveRequired(true);
			}
		}

		void HandlePopupMenuFolder (object sender, EventArgs e)
		{


		}
		
		void HandleMouseMove (object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) {


				int X = this.location.X + e.X - PanelMouseDownLocation.X;
				int Y = this.location.Y += e.Y - PanelMouseDownLocation.Y;
				// do not allow to drag off screen into non-scrollable terrain
				if (X < 0 )
				{
					X = 0;

				}
				if (Y < 0)
				{
					Y = 0;

				}
				this.Location = new Point(X,Y);
				//this.location.X += e.X - PanelMouseDownLocation.X;
				
				//this.location.Y += e.Y - PanelMouseDownLocation.Y;
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

