// NoteDataXML_Interface.cs
//
// Copyright (c) 2013 Brent Knowles (http://www.brentknowles.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Review documentation at http://www.yourothermind.com for updated implementation notes, license updates
// or other general information/
// 
// Author information available at http://www.brentknowles.com or http://www.amazon.com/Brent-Knowles/e/B0035WW7OW
// Full source code: https://github.com/BrentKnowles/YourOtherMind
//###
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using CoreUtilities;
using System.Xml.Serialization;

/* This is to contain the interface related elements (interaction with the GUID) to keep that distinct from the xml definition*/
namespace Layout
{
	public partial class NoteDataXML 
	{
		#region UI
		protected ToolStripMenuItem AppearanceSet=null;
		protected ToolStrip CaptionLabel;
		//protected ContextMenuStrip contextMenu;
		// this holds the PropertyGrid
		protected Panel PropertyPanel; 
		protected PropertyGrid propertyGrid;
		// the properties button on header
		protected ToolStripDropDownButton properties; 
		protected ToolStripLabel captionLabel;
		protected ToolStripButton ReadOnlyButton;

		protected ToolStripButton MaximizeButton;
		protected ToolStripButton MinimizeButton;
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


		protected virtual AppearanceClass UpdateAppearance ()
		{

			// This is the slow operation - removing it gives us 3 seconds back

			// takes the current appearance and adjusts it.


			AppearanceClass app = LayoutDetails.Instance.GetAppearanceByName (this.Appearance);
			if (null != app) {
				this.SetSaveRequired(true);

				// I was adding this to all children anyways, so figured I'd try it out in the base
				// REJECTED: This is too ugly
				//ParentNotePanel.BackColor = app.captionBackground;

				//Layout.AppearanceClass app = AppearanceClass.SetAsProgrammer();
				CaptionLabel.SuspendLayout (); //minor, fraction of a second savings
				CaptionLabel.Font = app.captionFont;   //1
				this.CaptionLabel.Height = app.nHeaderHeight; //2
			//	SystemInformation.MouseHoverTime = 1000;
			


				ParentNotePanel.BorderStyle = app.mainPanelBorderStyle; //3

				this.CaptionLabel.BackColor = app.captionBackground; //4
				this.CaptionLabel.ForeColor = app.captionForeground; //5

				foreach (ToolStripItem item in this.CaptionLabel.Items)
				{
					//
					// intending this to COLOR the new NoteDock buttons ONLY 03/11/2014
					//
					if (item.Tag != null && item.Tag.ToString() == "NoteDock")
					{
						item.BackColor = app.captionForeground;
						item.ForeColor = app.captionBackground;
					}
				}

				DoChildAppearance (app);
				CaptionLabel.ResumeLayout ();

			}
			AppearanceSet.Text = Loc.Instance.GetStringFmt ("Appearance: {0}", this.Appearance);

			// we pass this out so inherited methods do not need to make a second load of this resource
			return app;

		}

	
		 
		// this is overrideen by children to updat their own controls colors
		protected virtual void DoChildAppearance (Layout.AppearanceClass app)
		{

		}

		//delegate_UpdateListOfNotes UpdateListOfNotes;
		public void CreateParent (LayoutPanelBase _Layout)
		{



			ParentNotePanel = new NotePanel (this);
			//ParentNotePanel.Visible = false;
			//	ParentNotePanel.SuspendLayout ();

			
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
			// must be false for height to matter
			CaptionLabel.AutoSize = false;
			//		CaptionLabel.SuspendLayout ();
			CaptionLabel.Click += (object sender, EventArgs e) => BringToFrontAndShow ();
		//	CaptionLabel.DoubleClick += HandleCaptionLabelDoubleClick;
			CaptionLabel.MouseDown += HandleMouseDown;
			CaptionLabel.MouseUp += HandleMouseUp;
			CaptionLabel.MouseLeave += HandleMouseLeave;
			CaptionLabel.MouseMove += HandleMouseMove;
			CaptionLabel.Parent = ParentNotePanel;
			CaptionLabel.BackColor = Color.Green;
			CaptionLabel.Dock = DockStyle.Fill;
			CaptionLabel.GripStyle = ToolStripGripStyle.Hidden;





			captionLabel = new ToolStripLabel (this.Caption);
			captionLabel.ToolTipText = Loc.Instance.GetString ("TIP: Doubleclick this to set the note to its regular size");

			captionLabel.MouseDown += HandleMouseDown;
			captionLabel.MouseUp += HandleMouseUp;
			//captionLabel.MouseLeave += HandleMouseLeave;
			captionLabel.MouseMove += HandleMouseMove;
			captionLabel.DoubleClickEnabled = true;
			captionLabel.DoubleClick+= HandleCaptionLabelDoubleClick;

			CaptionLabel.Items.Add (captionLabel);
			//if (Caption == "")				NewMessage.Show ("Caption is blank");


			properties = new ToolStripDropDownButton ("");
			properties.Image = CoreUtilities.FileUtils.GetImage_ForDLL ("application_form_edit.png");
			CaptionLabel.Items.Add (properties);


			MinimizeButton = new ToolStripButton ();
			//MinimizeButton.Text = "--";

			MinimizeButton.Image = CoreUtilities.FileUtils.GetImage_ForDLL ("application_put.png");
			MinimizeButton.ToolTipText = "Hides the note. Bring it back by using the List or a Tab";
			MinimizeButton.Click += HandleMinimizeButtonClick;



			MaximizeButton = new ToolStripButton ();
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

			//
			//
			// APPEARANCE
			//
			//
			AppearanceSet = new ToolStripMenuItem ();

		
						
			
			ContextMenuStrip AppearanceMenu = new ContextMenuStrip ();

			ToolStripLabel empty = new ToolStripLabel ("BB");
			AppearanceMenu.Items.Add (empty);

			AppearanceSet.DropDown = AppearanceMenu;
			AppearanceMenu.Opening += HandleAppearanceMenuOpening;
			properties.DropDownItems.Add (AppearanceSet);
			//
			//
			// DOCK STYLE
			//
			//

			ToolStripComboBox DockPicker = new ToolStripComboBox ();
			DockPicker.ToolTipText = Loc.Instance.GetString ("Set Docking Behavior For Note");
			DockPicker.DropDownStyle = ComboBoxStyle.DropDownList;
			//DockPicker.DropDown+= HandleDockDropDown;




		

			(DockPicker).Items.Clear ();
			int found = -1;
			int count = -1;

			// this loop does not affect performance
			foreach (string s in Enum.GetNames(typeof(DockStyle))) {
				count++;
				(DockPicker).Items.Add (s);
				if (s == this.Dock.ToString ()) {
					found = count;
				}
			}
			if (found > -1)
				DockPicker.SelectedIndex = found;
			DockPicker.SelectedIndexChanged += HandleDockStyleSelectedIndexChanged;
			properties.DropDownItems.Add (DockPicker);


			//
			//
			// LOCK
			//
			//
			ToolStripButton LockState = new ToolStripButton ();
			LockState.Text = Loc.Instance.GetString ("Lock");
			LockState.Checked = this.LockState;
			LockState.CheckOnClick = true;
			LockState.Click += HandleLockStateClick;
			properties.DropDownItems.Add (LockState);


			//
			//
			// FOLDER
			//
			//
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
				ToolStripButton linkNote = new ToolStripButton ();
				linkNote.Text = Loc.Instance.GetString ("Create a Link To This Note");
				properties.DropDownItems.Add (linkNote);
				linkNote.Click += HandleLinkNoteClick;

			}

			ToolStripButton copyNote = new ToolStripButton ();
			copyNote.Text = Loc.Instance.GetString ("Copy Note");
			properties.DropDownItems.Add (copyNote);
			copyNote.Click += (object sender, EventArgs e) => Layout.CopyNote (this);

			properties.DropDownItems.Add (new ToolStripSeparator ());

			ToolStripButton menuProperties = new ToolStripButton ();
			menuProperties.Text = Loc.Instance.Cat.GetString ("Properties");
			properties.DropDownItems.Add (menuProperties);
			menuProperties.Click += HandlePropertiesClick;


			// 04/11/2014
			// TODO REMOVE
			// If I ever build a cleaner system for setting up notedocks this can be removed
			// but for now we display the GUID as a clickable button
			// that puts in on the clipboard
			ToolStripButton menuGUIDButton = new ToolStripButton();
			menuGUIDButton.Text = this.GuidForNote;
			menuGUIDButton.Click+= (object sender, EventArgs e) => {
				Clipboard.SetText(this.GuidForNote);
			};
			properties.DropDownItems.Add (menuGUIDButton);


			PropertyPanel = new Panel ();
			PropertyPanel.Visible = false;
			PropertyPanel.Parent = ParentNotePanel;
			PropertyPanel.Height = 300;
			PropertyPanel.Dock = DockStyle.Top;
			PropertyPanel.AutoScroll = true;


			Button CommitChanges = new Button ();
			CommitChanges.Text = Loc.Instance.Cat.GetString ("Update Note");
			CommitChanges.Parent = PropertyPanel;
			CommitChanges.Dock = DockStyle.Bottom;
			CommitChanges.Click += HandleCommitChangesClick;
			CommitChanges.BringToFront ();



			ParentNotePanel.Visible = this.Visible;
//			CaptionLabel.ResumeLayout(); this did not seem to help
//			ParentNotePanel.ResumeLayout();


			// Set up delegates

			GetAvailableFolders = _Layout.GetAvailableFolders;
			MoveNote = _Layout.MoveNote;
			SetSaveRequired = _Layout.SetSaveRequired;
			DeleteNote = _Layout.DeleteNote;
			Layout = _Layout;


			ToolStripMenuItem TokenItem = 
				LayoutDetails.BuildMenuPropertyEdit (Loc.Instance.GetString("Note Dock String: {0}"), 
				                                     this.Notedocks,
				                                     Loc.Instance.GetString ("example: note1*44159e01-b2c6-4b1f-9b68-8d3c85755f14*[[chapter1]]."),
				                                     HandleTokenChange );

			properties.DropDownItems.Add (TokenItem);
			BuildNoteDocks();



			// February 17 2013 - needed to ensure that children build their controls before Updateappearance is called
			DoBuildChildren(_Layout);

			UpdateAppearance ();

		}

		/// <summary>
		/// Builds the notedocks.
		/// </summary>
		void BuildNoteDocks ()
		{
			try {

				// step 1: delete any existing notedocks

				if (CaptionLabel != null && CaptionLabel.Items != null && CaptionLabel.Items.Count > 0)
					for (int i = CaptionLabel.Items.Count-1; i >=0; i--) {
						ToolStripItem control = (ToolStripItem)CaptionLabel.Items [i];
						if (control.Tag != null) {
							if (control.Tag.ToString () == "NoteDock") {
								CaptionLabel.Items.Remove ((ToolStripItem)control);
							}
						}
					}

				//
				// 03/11/2014
				//
				// Add custom button links (to give user ability to pair development notes with chapters)
				//
				//string customNoteLinks = "note1*44159e01-b2c6-4b1f-9b68-8d3c85755f14*chapter1;[[wordcount]]*44159e01-b2c6-4b1f-9b68-8d3c85755f14*chapter1";
			
				// quick test just fake it
				//foreach (string link in customNoteLinks)
				{
					// breakdown strings
					string[] links = this.Notedocks.Split (new char[1]{';'}, StringSplitOptions.RemoveEmptyEntries);
					if (links != null && links.Length > 0) {
						foreach (string link_unit in links) {
							string[] units = link_unit.Split (new char[1]{'*'}, StringSplitOptions.RemoveEmptyEntries);
						
							// anchor optional
							if (units != null && units.Length >= 2) {
								ToolStripButton noteLink = new ToolStripButton ();
							
								noteLink.Text = units [0];
								//							// tag contains GUID plus anchor reference.
								//							noteLink.Tag =  units[1];
								//
								//							// final anchor is optional
								//							if (units.Length == 3)
								//							{
								//								noteLink.Tag = units[1] + "*" + units[2];
								//							}
							
								noteLink.Tag = "NoteDock";

								if (units [0] == "[[wordcount]]") {
									noteLink.Text = "<press for word count>";
									noteLink.Click += (object sender, EventArgs e) => 
									{
										if (this.IsPanel) {
											int maxwords = 0;

											int maxwords2=0; // able to show three numbers. Document 0/2309 (next stage) / 99999 max

											Int32.TryParse (units [1], out maxwords);
											if (units.Length == 3)
											{
												Int32.TryParse (units [2], out maxwords2);
											}
											int words = 0;
											//NewMessage.Show ("boo");
											// iterate through all notes
											foreach (NoteDataInterface note_ in this.ListOfSubnotesAsNotes()) {
												//NewMessage.Show (note_.Caption);
												if (note_ is NoteDataXML_RichText) {
													// fetch word count
													//((NoteDataXML_RichText)note_).GetRichTextBox().

													if (LayoutDetails.Instance.WordSystemInUse != null)
													{
														try{
															// we fetch data1 because the actual rich text box is NOT loaded
															// but I found the word count quite excessive so we load it instead
															RichTextBox faker = new RichTextBox();
															faker.Rtf = ((NoteDataXML_RichText)note_).Data1;
															string ftext = faker.Text;
															faker.Dispose ();
															words = words + LayoutDetails.Instance.WordSystemInUse.CountWords(ftext);
														}
														catch(System.Exception)
														{
															NewMessage.Show (Loc.GetStr("ERROR: Internal Word System Word Counting Error"));
														}
													}
													else
													{
														NewMessage.Show (Loc.GetStr("Is there a Word system installed for counting words?"));
													}
												}
											}
											if (maxwords2 > 0) {
												noteLink.Text = String.Format ("{0}/{1}/{2}", words, maxwords, maxwords2);
											}
											else
											if (maxwords > 0) {
												noteLink.Text = String.Format ("{0}/{1}", words, maxwords);
											} else {
												noteLink.Text = String.Format ("{0}", words);
											}
										} else {
											NewMessage.Show (Loc.Instance.GetString ("Word Count only works if attached to a Layout Note"));
										}
									
									
									};
								} else
									noteLink.Click += (object sender, EventArgs e) => {
								
										string theNoteGuid = units [1];
								
										string linkanchor = "";
										if (units.Length == 3) {
											linkanchor = units [2];
										}
								
										LayoutPanelBase SuperParent = null;
										if (this.Layout.GetIsChild == true)
											SuperParent = this.Layout.GetAbsoluteParent ();
										else
											SuperParent = this.Layout;
								
										NoteDataInterface theNote = this.Layout.GetNoteOnSameLayout (theNoteGuid, false);
								
										//NewMessage.Show ("hi " + theNoteGuid);

										// GetNonteOnSameLayout always returns a valid note.
										if (theNote != null && theNote.Caption != "findme") {
											//
											// Toggle Visibility State
											//
									
											if (theNote.Visible == true) {
												//NewMessage.Show ("Hide");
												theNote.Visible = false;
												theNote.UpdateLocation ();
											} else {
												//NewMessage.Show ("Show");
												int theNewX = this.Location.X;
												int theNewY = this.Location.Y;
										
										
										
										
												if (this.Layout.GetIsChild) {
													//NewMessage.Show ("child");
													//ok this seems convulted but I need to get the Note containing this layout
													// and there does not seem to be a short cut for it.
													string GuidOfLayoutNote = this.Layout.GUID;
											
													NoteDataInterface parentNote = SuperParent.GetNoteOnSameLayout (GuidOfLayoutNote, false);
													if (parentNote != null) {
														//NewMessage.Show (GuidOfLayoutNote);
														// if we are a child then use the coordinates of the layoutpanel itself
														theNewX = parentNote.Location.X;
														theNewY = parentNote.Location.Y;
														theNote.Height = Math.Max (0, parentNote.Height / 2);
														theNote.Width = Math.Max (0, parentNote.Width - 4);
													} else {
														NewMessage.Show ("Parent note with GUID was blank " + GuidOfLayoutNote);
													}
												} else {
													// get width and height from non-layout enclosed note
													theNote.Height = Math.Max (0, this.Height / 2);
													theNote.Width = Math.Max (0, this.Width - 4);
												}
										
										
												// now offset a bit
												theNewX = theNewX + 25;
												theNewY = theNewY + 100;
										
												theNote.Location = new Point (theNewX, theNewY);
										
												theNote.Visible = true;
										
												theNote.UpdateLocation (); // so size changes stick

												//theNote.BringToFrontAndShow ();

											theNote = this.Layout.GetNoteOnSameLayout (theNoteGuid, true, linkanchor);

											}
									
									
											//	this.Layout.GoToNote(theNote);
										} else {
											NewMessage.Show (Loc.Instance.GetStringFmt ("The guid {0} was null", theNoteGuid));
										}
									}; // The click
							
								CaptionLabel.Items.Add (noteLink);
								// do this after adding to INHERIT the proper styles from TOOLBAR
								// reverse color scheme
								// coloring should happen in UPDATE apeparance
								//							Color foreC = CaptionLabel.BackColor;
								//							Color backC = CaptionLabel.ForeColor;
								noteLink.Font = new Font (noteLink.Font.FontFamily, noteLink.Font.Size - 1);
								//							noteLink.BackColor = app.captionBackground;;
								//							noteLink.ForeColor = backC;
							}
						}
					}
				}

			} catch (System.Exception ex) {
				NewMessage.Show (ex.ToString ());
			}
		}

		/// <summary>
		/// Handles the token change.
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleTokenChange (object sender, KeyEventArgs e)
		{
			string tablecaption = Notedocks;
			LayoutDetails.HandleMenuLabelEdit (sender, e, ref tablecaption, SetSaveRequired);
			Notedocks = tablecaption;

			// whenever this changes we rebuild the notedocks
			BuildNoteDocks();
		}

		protected virtual void DoBuildChildren(LayoutPanelBase _Layout)
		{
			// children ned to modify this (basically it is their version of CreatePraent)
		}
		void HandleAppearanceMenuOpening (object sender, System.ComponentModel.CancelEventArgs e)
		{

			(sender as ContextMenuStrip).Items.Clear ();
			// build list from list of appearances in database
			System.Collections.Generic.List<string> list = LayoutDetails.Instance.GetListOfAppearances ();
			foreach (string s in list) {
				ToolStripButton button = new ToolStripButton();
				button.CheckOnClick=true;
				button.Text = s;
				if (s == this.Appearance)
					button.Checked= true;
				button.Click+= HandleChooseNewAppearanceClick;
				(sender as ContextMenuStrip).Items.Add (button);
			}


		}

		void HandleChooseNewAppearanceClick (object sender, EventArgs e)
		{
			this.Appearance = (sender as ToolStripButton).Text;
			UpdateAppearance();
		}


		void HandleLockStateClick (object sender, EventArgs e)
		{
			this.LockState = (sender as ToolStripButton).Checked;
			SetSaveRequired(true);
		}

		void HandleDockStyleSelectedIndexChanged (object sender, EventArgs e)
		{
			string value = (sender as ToolStripComboBox).SelectedItem.ToString ();
			this.Dock = (DockStyle)Enum.Parse (typeof(DockStyle), value);
			ParentNotePanel.Dock = this.Dock;
			SetSaveRequired(true);
		}

//		void HandleDockDropDown (object sender, EventArgs e)
//		{
//			// fill the Dock Style Drop Down
//			(sender as ToolStripComboBox).Items.Clear();
//			foreach (string s in Enum.GetNames(typeof(DockStyle))){
//				(sender as ToolStripComboBox).Items.Add (s);
//			}
//
//		}

		void HandleLinkNoteClick (object sender, EventArgs e)
		{
			string SourceContainerGUIDToUse = Layout.GUID;

			// - Shouldn't the notes store the GUID of the Master Note (not the panel? they are in?) May 2013
			//cannot use Layout.ParentGUIDFromNote -- it was blank
			// OK: I was testing on a non-panel. So the issue becomes, can I use
			// one of these with a panel and fall back when not?
			if (Layout.ParentGuidFromNotes != Constants.BLANK)
			{

				SourceContainerGUIDToUse = Layout.ParentGuidFromNotes;
			}
			//NewMessage.Show(Layout.ParentGUID + " - " + Layout.GUID);

			// May 2013 - adding the caption to the string encoding. This is pulled off of with SetLink
			LayoutDetails.Instance.PushLink(String.Format (CoreUtilities.Links.LinkTableRecord.PageLinkFormatString, SourceContainerGUIDToUse, GuidForNote)+"*"+this.Caption);
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
			if (sender != null ) {
				Maximize (!IsMaximized);
			}
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
/// <summary>
/// Brings to front and show.
/// </summary>
		public void BringToFrontAndShow ()
		{
			if (ParentNotePanel == null) {
				NewMessage.Show (Loc.Instance.GetStringFmt ("The note {0} with guid={1} does not have a valid parent.", this.Caption, this.GuidForNote));
			} else {

				// 20/04/2014 -- we enable note if disabled DID NOT WORK.
//				if (this.Layout != null)
//				{
//					// disable false means ACTIVE
//					this.Layout.DisableLayout(false);
//				}



				// we always make the note visible if showing
				this.Visible = true;
				ParentNotePanel.Visible = true;
				ParentNotePanel.BringToFront ();



				if (this.Layout != null)
				{
					//
					// MAJOR
					// 05/11/2014
					// This might have side-effects but the CurrentTextNote as only being set when OnEnter fired on the NoteDamaXML_RichText.cs
					// THE PROBLEM?
					// - this did not fire under most circumstances! Clicking a link to go to a note, for example. The cursor changed to the new text note
					//   but things like Word Count/Text Operations DID NOT.
					//  
					if (this is NoteDataXML_RichText)
					{
						((NoteDataXML_RichText)this).GetRichTextBox().Focus ();
						Layout.CurrentTextNote = (NoteDataXML_RichText)this;

					}
					if (this.Layout.GetIsChild == true)
					{
						// get its Panel note somehow?
						if (this.Layout.Parent is NotePanel)
						{
							((NotePanel)Layout.Parent).BringToFrontChild();
							//this.Layout.Parent.BringToFront();
						}

					}
				}
				//MOVE THIS TO BRINGTOFRONT override... basically whenever you want to bring a note to front, if system, we will amke the active note
				RespondToNoteSelection();



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
					if (Layout != null)
					{
						// getting tabs to update when the label is changed
						Layout.RefreshTabs();
						SetSaveRequired(true);
					}
				}
			}
		
		}
		/// <summary>
		/// Override this method to have child classes respond to a change of Caption
		/// </summary>
		/// <param name='cap'>
		/// Cap.
		/// </param>
		protected virtual void CaptionChanged(string cap)
		{
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
					CaptionChanged(Caption);
					SetSaveRequired (true);
					// so we don't need to call update for such a simple change
					captionLabel.Text = Caption; 
					if (Layout != null)
					{
						// getting tabs to update when the label is changed
						Layout.RefreshTabs();
					}
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
			// close the Property View after pressing this button (July 2013)
			TogglePropertyView ();
		}

		void TogglePropertyView ()
		{
			PropertyPanel.Visible = !PropertyPanel.Visible;
			if (PropertyPanel.Visible == true) {
				if (propertyGrid == null) {
					propertyGrid = new PropertyGrid ();
					propertyGrid.SelectedObject = this;
					//	propertyGrid.PropertyValueChanged+= HandlePropertyValueChanged;
					propertyGrid.Parent = PropertyPanel;
					propertyGrid.Dock = DockStyle.Fill;
				}
				ParentNotePanel.AutoScroll = true;
				PropertyPanel.AutoScroll = true;
				//PropertyPanel.SendToBack();
				//CaptionLabel.BringToFront();
				CaptionLabel.SendToBack ();
			}
			else {
				ParentNotePanel.AutoScroll = false;
			}
		}



		void HandlePropertiesClick (object sender, EventArgs e)
		{

			TogglePropertyView ();
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

		// set to true if I am the note being dragged
		bool IAmDragging = false;
		Point PanelMouseDownLocation ;
		Color lastColor;
		void HandleMouseMove (object sender, MouseEventArgs e)
		{
			CaptionLabel.Cursor = Cursors.Hand;
			if (this.Dock == DockStyle.None) {
				if (e.Button == MouseButtons.Left) {

					if (false == Layout.IsDraggingANote ) {
						BringToFrontAndShow ();
						if (false == this.LockState)
						{
						// start drag mode
						Layout.IsDraggingANote = true;
						IAmDragging = true;
						

						lastColor = CaptionLabel.BackColor;
						CaptionLabel.BackColor = Color.Red;

						PanelMouseDownLocation = e.Location;
					
						}
					}
					if (true == Layout.IsDraggingANote && true == IAmDragging) {
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
			} else {
				EndDrag();
			}
		}

		public void EndDrag ()
		{

			if (null != Layout) {
				if (Layout.IsDraggingANote && IAmDragging) {
					CaptionLabel.BackColor = lastColor;

					Layout.IsDraggingANote = false;
					IAmDragging = false;
				}
			} else {


				//UPDATE: We can't actually clear this because we do not have a reference to it!

				// Not a big deal. When we clear the drag state we don't actually load the 
				// child notes (not fully).
				// Therefore they do not have layouts
				// so in this case we just clear backcolor and person dragging
				//NewMessage.Show (this.Caption + " has a null Layout? How?");
				//if (IAmDragging)
				//{
				// force the removal since IamDragging is not a stored flag
				// we do not actually know
				//	CaptionLabel.BackColor = lastColor;
				//	IAmDragging = false;
				//}
			}
		}

		void HandleMouseLeave (object sender, EventArgs e)
		{
			CaptionLabel.Cursor = Cursors.Default;
			// suppressing this so that you only stop dragging by lifting mouse up
//			if (this.Dock == DockStyle.None) {
//				EndDrag();
//			}
		}
		
		void HandleMouseUp (object sender, MouseEventArgs e)
		{

			if (this.Dock == DockStyle.None) {
			
				EndDrag();
			}

		}
		public virtual void RespondToNoteSelection()
		{
			// signaleld from NOTEPANEL.cs
			// intended only for the system note to override


		}
		void HandleMouseDown (object sender, MouseEventArgs e)
		{
			// going to try moving Dragging from here, so there's
			// no 'jerk' when clicking on a note
			// you have to be dragging for a bit to kick into dragmode

		
//			if (this.Dock == DockStyle.None) {
//				if (e.Button == MouseButtons.Left ) {
//					// start moving
//					Layout.IsDraggingANote = true;
//					lastColor = CaptionLabel.BackColor;
//					CaptionLabel.BackColor = Color.Red;
//					
//					PanelMouseDownLocation = e.Location;
//					CaptionLabel.Cursor = Cursors.Hand;
//				}
//			}
			
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
		public virtual void UpdateAfterLoad()
		{
			// some notes will override this.
			// They also need to add themselves to LayoutDetails.Instance.UpdateAfterLoadList
		}
		// for use with the F6 command in mainform (hiding the system panel)
		public void ToggleTemporaryVisibility ()
		{
			// this is NOT the same as the Visiblity propery
			// it is a temporary (life of application) visiblity instead
			if (ParentNotePanel != null) {
				ParentNotePanel.Visible = !ParentNotePanel.Visible;
			}
		}

	}
}

