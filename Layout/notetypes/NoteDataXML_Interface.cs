using System;
using System.Windows.Forms;
using System.Drawing;
using CoreUtilities;
using System.Xml.Serialization;

/* This is to contain the interface related elements (interaction with the GUID) to keep that distinct from the xml definition*/
namespace Layout
{
	public partial class NoteDataXML 
	{
		#region UI
		protected ToolStrip CaptionLabel;
		//protected ContextMenuStrip contextMenu;
		// this holds the PropertyGrid
		protected Panel PropertyPanel; 
		protected PropertyGrid propertyGrid;
		// the properties button on header
		protected ToolStripDropDownButton properties; 
		protected ToolStripLabel captionLabel;
		protected ToolStripButton ReadOnlyButton;
		#endregion

	
		public virtual void Dispose ()
		{


		}

		// http://stackoverflow.com/questions/7367152/c-dynamically-assign-method-method-as-variable
		Func<System.Collections.Generic.List<NoteDataInterface>> GetAvailableFolders;
		Action<string,string> MoveNote;
		private Action<bool> setsaverequired;
		protected Action<NoteDataInterface> DeleteNote;
	

		[XmlIgnore]
		public Action<bool> SetSaveRequired {
			get { return setsaverequired;}
			set { setsaverequired = value;}
		}
		protected delegate void delegate_UpdateListOfNotes();


		//delegate_UpdateListOfNotes UpdateListOfNotes;
		public virtual void CreateParent (LayoutPanelBase _Layout)
		{



			ParentNotePanel = new NotePanel (this);
			ParentNotePanel.SuspendLayout ();

			
			ParentNotePanel.Visible = true;
			ParentNotePanel.Location = Location;
			ParentNotePanel.Dock = System.Windows.Forms.DockStyle.None;
			ParentNotePanel.Height = Height;
			ParentNotePanel.Width = Width;
			ParentNotePanel.Dock = this.Dock;
			
			try {
				_Layout.NoteCanvas.Controls.Add (ParentNotePanel);
			} catch (Exception ex) {
				lg.Instance.Line ("CreateParent", ProblemType.EXCEPTION, "Unable to create note after changing properties" + ex.ToString ());
				throw new Exception ("Failed to add control");
			}
			
			
			CaptionLabel = new ToolStrip ();
			CaptionLabel.SuspendLayout ();
			CaptionLabel.DoubleClick += HandleCaptionLabelDoubleClick;
			CaptionLabel.MouseDown += HandleMouseDown;
			CaptionLabel.MouseUp += HandleMouseUp;
			CaptionLabel.MouseLeave += HandleMouseLeave;
			CaptionLabel.MouseMove += HandleMouseMove;
			CaptionLabel.Parent = ParentNotePanel;
			CaptionLabel.BackColor = Color.Green;
			CaptionLabel.Dock = DockStyle.Fill;
			CaptionLabel.GripStyle = ToolStripGripStyle.Hidden;





			captionLabel = new ToolStripLabel (this.Caption);
			captionLabel.ToolTipText = "TIP: Doubleclick this to set the note to its regular size";

			//captionLabel.MouseDown += HandleMouseDown;
			//captionLabel.MouseUp += HandleMouseUp;
			////captionLabel.MouseLeave += HandleMouseLeave;
			//captionLabel.MouseMove += HandleMouseMove;



			CaptionLabel.Items.Add (captionLabel);
			//if (Caption == "")				NewMessage.Show ("Caption is blank");


			properties = new ToolStripDropDownButton ("");
			properties.Image = CoreUtilities.FileUtils.GetImage_ForDLL ("application_form_edit.png");
			CaptionLabel.Items.Add (properties);


			ToolStripButton MinimizeButton = new ToolStripButton ();
			//MinimizeButton.Text = "--";
			//TODO: Optimization: Should these images just be stored somewhere and retrieved?
			MinimizeButton.Image = CoreUtilities.FileUtils.GetImage_ForDLL ("application_put.png");
			MinimizeButton.ToolTipText = "Hides the note. Bring it back by using the List or a Tab";
			MinimizeButton.Click += HandleMinimizeButtonClick;



			ToolStripButton MaximizeButton = new ToolStripButton ();
			//MaximizeButton.Text = "[  ]";
			MaximizeButton.Image = CoreUtilities.FileUtils.GetImage_ForDLL ("application_xp.png");
			MaximizeButton.ToolTipText = Loc.Instance.GetString ("Fills available screen");
			MaximizeButton.Click += HandleMaximizeButtonClick;



			if (true == IsSystemNote) {
				// not really a delete, more of a close
				ToolStripButton closeButton = new ToolStripButton ();
				//closeButton.Text = " X ";
				closeButton.Image = CoreUtilities.FileUtils.GetImage_ForDLL ("delete_x.png");
				closeButton.Click += HandleCloseClick;
				;
				CaptionLabel.Items.Add (closeButton);
				//closeButton.Anchor = AnchorStyles.Right;
				closeButton.Alignment = ToolStripItemAlignment.Right;
				//	ToolStripItem item = new ToolStripItem();
				//item.ToolStripItemAlignment = ToolStripItemAlignment.Right;
				//closeButton.Dock = DockStyle.Right;
			}

			if (false == IsSystemNote) {
				ToolStripTextBox captionEditor = new ToolStripTextBox ();
				captionEditor.Text = Caption;
				captionEditor.TextChanged += HandleCaptionTextChanged;
				captionEditor.KeyDown += HandleCaptionEditorKeyDown;
				properties.DropDownItems.Add (captionEditor);


			}

			CaptionLabel.Items.Add (MaximizeButton);
			MaximizeButton.Alignment = ToolStripItemAlignment.Right;
			CaptionLabel.Items.Add (MinimizeButton);
			MinimizeButton.Alignment = ToolStripItemAlignment.Right;


			//contextMenu = new ContextMenuStrip();

			ToolStripButton BringToFront = new ToolStripButton ();
			BringToFront.Text = Loc.Instance.Cat.GetString ("Bring to Front");
			BringToFront.Click += HandleBringToFrontClick;
			properties.DropDownItems.Add (BringToFront);

			properties.DropDownItems.Add (new ToolStripSeparator ());

			if (false == IsSystemNote) {
				ReadOnlyButton = new ToolStripButton ();
				ReadOnlyButton.CheckOnClick = true;
				ReadOnlyButton.Checked = this.ReadOnly;
				ReadOnlyButton.Text = Loc.Instance.Cat.GetString ("Read Only");
				ReadOnlyButton.Click += HandleReadOnlyClick;
				properties.DropDownItems.Add (ReadOnlyButton);
			}

			if (false == IsSystemNote) {
				ToolStripMenuItem menuFolder = new ToolStripMenuItem ();
				menuFolder.Text = Loc.Instance.Cat.GetString ("Folder");
				properties.DropDownItems.Add (menuFolder);
				// just here to make sure that there's a dropdown singal before populating
				menuFolder.DropDownItems.Add ("");
				menuFolder.DropDownOpening += HandleFolderDropDownOpening;
				//	menuFolder.MouseEnter += HandleMenuFolderMouseEnter;
			}

			if (false == IsSystemNote) {
				ToolStripButton deleteNote = new ToolStripButton ();
				deleteNote.Text = Loc.Instance.GetString ("Delete This Note");
				properties.DropDownItems.Add (deleteNote);
				deleteNote.Click += HandleDeleteNoteClick;
			} else
				if (true == IsSystemNote) {
				// add a way to DELETE a Layout?
				ToolStripButton deleteNote = new ToolStripButton ();
				deleteNote.Text = Loc.Instance.GetString ("Delete This Layout");
				properties.DropDownItems.Add (deleteNote);
				deleteNote.Click += HandleDeleteLayoutClick;

			}
		
			if (true == IsLinkable) {
				ToolStripButton linkNote = new ToolStripButton();
				linkNote.Text  = Loc.Instance.GetString("Create a Link To This Note");
				properties.DropDownItems.Add (linkNote);
				linkNote.Click+= HandleLinkNoteClick;

			}

			properties.DropDownItems.Add (new ToolStripSeparator());
			ToolStripButton menuProperties = new ToolStripButton();
			menuProperties.Text = Loc.Instance.Cat.GetString ("Properties");
			properties.DropDownItems.Add (menuProperties);
			menuProperties.Click+= HandlePropertiesClick;



			// Should be at the end
//			ToolStripSeparator sep = new ToolStripSeparator ();
//			sep.Width = 0;
//			sep.BackColor = CaptionLabel.BackColor;
//			sep.ForeColor = CaptionLabel.ForeColor;
			//HACK for some reason labels inside of panels don't draw LAST toolstripitem on LOAD (on only on load!) 
			// NOTE JAN 2013 this problem MAY have gone away
			//CaptionLabel.Items.Add (sep);

			//CaptionLabel.ContextMenuStrip = contextMenu;

			PropertyPanel = new Panel();
			PropertyPanel.Visible = false;
			PropertyPanel.Parent = ParentNotePanel;
			PropertyPanel.Height = 300;
			PropertyPanel.Dock = DockStyle.Top;
			PropertyPanel.AutoScroll = true;

//			

			Button CommitChanges = new Button();
			CommitChanges.Text = Loc.Instance.Cat.GetString("Update Note");
			CommitChanges.Parent = PropertyPanel;
			CommitChanges.Dock = DockStyle.Bottom;
			CommitChanges.Click += HandleCommitChangesClick;
			CommitChanges.BringToFront();



			ParentNotePanel.Visible = this.Visible;
			CaptionLabel.ResumeLayout();
			ParentNotePanel.ResumeLayout();


			// Set up delegates

			GetAvailableFolders = _Layout.GetAvailableFolders;
			MoveNote = _Layout.MoveNote;
			SetSaveRequired = _Layout.SetSaveRequired;
			DeleteNote = _Layout.DeleteNote;
			Layout = _Layout;

		}

		void HandleLinkNoteClick (object sender, EventArgs e)
		{
			LayoutDetails.Instance.PushLink(String.Format (CoreUtilities.Links.LinkTableRecord.PageLinkFormatString, Layout.GUID, GuidForNote));
		}



		/// <summary>
		/// Will be overridden in children (i.e., a RichEdit set to readonly and whatnot
		/// </summary> 
		protected virtual void RespondToReadOnlyChange()
		{
		}

		protected virtual void  HandleReadOnlyClick (object sender, EventArgs e)
		{

			this.ReadOnly = !this.ReadOnly;
			RespondToReadOnlyChange();
		}
		/// <summary>
		/// Handles the delete layout click for a SYSTEM NOTE
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleDeleteLayoutClick (object sender, EventArgs e)
		{
			if (NewMessage.Show (Loc.Instance.GetString ("Delete?"), Loc.Instance.GetStringFmt ("Do you REALLY want to delete Layout '{0}'?", ((NoteDataXML_SystemOnly) this).GetLayoutName()),
			                    MessageBoxButtons.YesNo, null) == DialogResult.Yes) {
				// delete
				// then dispose
				MasterOfLayouts.DeleteLayout(((NoteDataXML_SystemOnly) this).GetLayoutGUID());
				// close this one final time wihtout saving since we are now gone
				((NoteDataXML_SystemOnly) this).DoCloseNote(false);
			}
		}

		void HandleCaptionLabelDoubleClick (object sender, EventArgs e)
		{

			Maximize(false);
		}

	

		/// <summary>
		/// Handles the delete click. SHOULD DO NOTHING IN ALL CLASSES EXCEPT THE SYSTEM NOTE
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		protected virtual void HandleCloseClick (object sender, EventArgs e)
		{
			// actual code is in the SystemOnly noet
		}

		void HandleMinimizeButtonClick (object sender, EventArgs e)
		{
			// if just click then we UnMaximize
			// doubleclick does a mnimize

			Minimize ();
				//Maximize (false);

		}

		void HandleMaximizeButtonClick (object sender, EventArgs e)
		{
			Maximize(true);
		}

		public void BringToFrontAndShow ()
		{
			if (ParentNotePanel == null) {
				NewMessage.Show (Loc.Instance.GetStringFmt ("The note {0} with guid={1} does not have a valid parent.", this.Caption, this.GuidForNote));
			} else {
				// we always make the note visible if showing
				this.Visible = true;
				ParentNotePanel.Visible = true;
				ParentNotePanel.BringToFront ();
			}
		}

		/// <summary>
		/// Handles the bring to front click.
		/// 
		/// Will use this sort of thing more sparingly than in previous version because it caused more confusion than it helped, I think
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleBringToFrontClick (object sender, EventArgs e)
		{
			BringToFrontAndShow();
		}

		/// <summary>
		/// Handles the delete note click. This is the true deleting of the note
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleDeleteNoteClick (object sender, EventArgs e)
		{
			if (Caption == CoreUtilities.Links.LinkTable.STICKY_TABLE) {
				NewMessage.Show (Loc.Instance.GetStringFmt ("{0} is a protected note. It cannot be renamed or deleted.", Caption));
				return;
			}

			if (NewMessage.Show (Loc.Instance.GetString ("Delete?"), Loc.Instance.GetStringFmt ("Do you REALLY want to delete Note {0}?", this.Caption),
			                     MessageBoxButtons.YesNo, null) == DialogResult.Yes) {
				if (DeleteNote != null) {
					DeleteNote(this);
				}
			}
		
		}

		void HandleCaptionEditorKeyDown (object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) {
				if (Caption == CoreUtilities.Links.LinkTable.STICKY_TABLE)
				{
					NewMessage.Show (Loc.Instance.GetStringFmt("{0} is a protected note. It cannot be renamed or deleted.", Caption));
				}
				else
				{
				// commit the change to the caption
				Caption = (sender as ToolStripTextBox).Text;
				SetSaveRequired (true);
				// so we don't need to call update for such a simple change
				captionLabel.Text = Caption; 
				}
				// supress key beep from pressing enter
				e.SuppressKeyPress = true;
			}
		}

		void HandleCaptionTextChanged (object sender, EventArgs e)
		{
			
		}


		void HandleCommitChangesClick (object sender, EventArgs e)
		{
			ParentNotePanel.AutoScroll = false;
		//	((NoteDataXML)propertyGrid.SelectedObject).Update(Layout);
			((NoteDataXML)propertyGrid.SelectedObject).UpdateLocation();
			SetSaveRequired(true);
		}

		void HandlePropertyValueChanged (object s, PropertyValueChangedEventArgs e)
		{
	// TODO: Finish reomving this if I don't need it

	
		}

		void HandlePropertiesClick (object sender, EventArgs e)
		{

			PropertyPanel.Visible = !PropertyPanel.Visible;
			if (PropertyPanel.Visible == true) {

				if (propertyGrid == null)
				{
				propertyGrid = new PropertyGrid();
				propertyGrid.SelectedObject = this;
				
				propertyGrid.PropertyValueChanged+= HandlePropertyValueChanged;
				propertyGrid.Parent = PropertyPanel;
				
				propertyGrid.Dock = DockStyle.Fill;
				}
				ParentNotePanel.AutoScroll = true;

				PropertyPanel.AutoScroll = true;
				//PropertyPanel.SendToBack();
				//CaptionLabel.BringToFront();
				CaptionLabel.SendToBack ();

			} else {
				ParentNotePanel.AutoScroll = false;

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
//		void HandleMenuFolderMouseEnter (object sender, EventArgs e)
//		{
//			// Draw Potential Folders
//			(sender as ToolStripMenuItem).DropDownItems.Clear ();
//			ToolStripMenuItem up = (ToolStripMenuItem)(sender as ToolStripMenuItem).DropDownItems.Add (Loc.Instance.Cat.GetString ("Out of Folder"));
//			up.Tag = "up";
//			up.Click += HandleMenuFolderClick;
//
//			foreach (NoteDataInterface note in GetAvailableFolders()) {
//				// we do not add ourselves
//				if (note.GuidForNote != this.GuidForNote) {
//					ToolStripMenuItem item = (ToolStripMenuItem)(sender as ToolStripMenuItem).DropDownItems.Add (note.Caption);
//					item.Click += HandleMenuFolderClick;
//					item.Tag = note.GuidForNote;
//				}
//			}
//
//		}

		void HandleFolderDropDownOpening (object sender, EventArgs e)
		{
			// Draw Potential Folders
			(sender as ToolStripMenuItem).DropDownItems.Clear ();
			ToolStripMenuItem up = (ToolStripMenuItem)(sender as ToolStripMenuItem).DropDownItems.Add (Loc.Instance.Cat.GetString ("Out of Folder"));
			up.Tag = "up";
			up.Click += HandleMenuFolderClick;
			
			foreach (NoteDataInterface note in GetAvailableFolders()) {
				// we do not add ourselves
				if (note.GuidForNote != this.GuidForNote) {
					ToolStripMenuItem item = (ToolStripMenuItem)(sender as ToolStripMenuItem).DropDownItems.Add (note.Caption);
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
			if (this.Dock == DockStyle.None) {
				if (e.Button == MouseButtons.Left) {


					int X = this.location.X + e.X - PanelMouseDownLocation.X;
					int Y = this.location.Y += e.Y - PanelMouseDownLocation.Y;
					// do not allow to drag off screen into non-scrollable terrain
					if (X < 0) {
						X = 0;

					}
					if (Y < 0) {
						Y = 0;

					}
					this.Location = new Point (X, Y);
					//this.location.X += e.X - PanelMouseDownLocation.X;
				
					//this.location.Y += e.Y - PanelMouseDownLocation.Y;
					UpdateLocation ();
				}
			}
		}
		
		void HandleMouseLeave (object sender, EventArgs e)
		{
			if (this.Dock == DockStyle.None) {
				//TODO: Needs to be the default apeparance color
				CaptionLabel.BackColor = Color.Green;
			}
		}
		
		void HandleMouseUp (object sender, MouseEventArgs e)
		{

			if (this.Dock == DockStyle.None) {
				//TODO: Needs to be the default apeparance color
				CaptionLabel.BackColor = Color.Green;
			}
		}
		Point PanelMouseDownLocation ;
		void HandleMouseDown (object sender, MouseEventArgs e)
		{
			BringToFrontAndShow();
			if (this.Dock == DockStyle.None) {
				if (e.Button == MouseButtons.Left) {
					// start moving
					CaptionLabel.BackColor = Color.Red;
					PanelMouseDownLocation = e.Location;
				}
			}
			
		}

		Color currentColor ;
		Timer myTimer;
		public void Flash ()
		{
			if (ParentNotePanel != null) {
				currentColor = CaptionLabel.BackColor;
				CaptionLabel.BackColor = Color.Blue;
				ParentNotePanel.Invalidate ();

				myTimer = new Timer ();
				myTimer.Tick += new EventHandler (myTimer_Tick);
				myTimer.Interval = 200;
				myTimer.Start ();
				lg.Instance.Line ("myTimer_Tick done", ProblemType.TEMPORARY, "timer asked to START");
			}
		}

	
		/// <summary>
		/// run by timer
		/// 
		/// sets color back after GUIFlash
		/// and then it destroys the timer
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void myTimer_Tick(object sender, EventArgs e)
		{lg.Instance.Line("myTimer_Tick done", ProblemType.TEMPORARY, "timer tick");
			if (myTimer != null)
			{
				lg.Instance.Line("myTimer_Tick done", ProblemType.TEMPORARY, "timer asked to dispose");
				myTimer.Stop();
				myTimer.Dispose();
				myTimer = null;

				CaptionLabel.BackColor  = currentColor;


			}
			
			
		}

	}
}

