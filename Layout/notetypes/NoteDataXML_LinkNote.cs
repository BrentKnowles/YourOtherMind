// NoteDataXML_LinkNote.cs
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
using CoreUtilities;
using CoreUtilities.Links;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Layout
{
	public class NoteDataXML_LinkNote : NoteDataXML_RichText
	{

		#region variables
		string childGuid = Constants.BLANK;
		// temporary variable, that is restored if Link needs to be reset. Does not need conversion
		public string ChildGuid {
			get { return childGuid;}
			set { childGuid = value;}
		}
		
		string layoutGuid = Constants.BLANK;
		// temporary variable, that is restored if Link needs to be reset. Does not need conversion
		public string LayoutGuid {
			get{ return layoutGuid;}
			set { layoutGuid = value;}
		}
		#endregion
		#region gui
		PictureBox Pic = null;
		#endregion

		public override bool IsLinkable { get { return false; }}
		public NoteDataXML_LinkNote () :base()
		{

		}
		public NoteDataXML_LinkNote (int _height, int _width): base(_height, _width)
		{

		}

		protected override void CommonConstructorBehavior ()
		{
			base.CommonConstructorBehavior ();
			// linknotes are forced to remain readonly
			readOnly = true;
			Caption = Loc.Instance.Cat.GetString("Link Note");

		}

		protected override string GetIcon ()
		{
			return @"%*link.png";
		}
		public override void RebuildLinkAfterMove()
		{
			// requires LayoutGuid and ChildGuid to still be valid, which I'm not sure of
			string originalLink = String.Format (CoreUtilities.Links.LinkTableRecord.PageLinkFormatString, LayoutGuid,ChildGuid);
			SetLink (originalLink);

		}

		protected override void DoBuildChildren (LayoutPanelBase Layout)
		{
			base.DoBuildChildren (Layout);
			ReadOnlyButton.CheckOnClick = false;
			string linkfile = GetLink ();


			ToolStripButton SetupLinkButton = new ToolStripButton();
			SetupLinkButton.Text = Loc.Instance.GetString ("Paste Link");
			SetupLinkButton.Click += HandleSetupLinkClick;

			ToolStripButton RefreshLinkButton = new ToolStripButton();
			RefreshLinkButton.Text = Loc.Instance.GetString("Refresh Link");
			RefreshLinkButton.Click += HandleRefreshLinkClick;


			properties.DropDownItems.Add (SetupLinkButton);
			properties.DropDownItems.Add (RefreshLinkButton);




			if (linkfile != Constants.BLANK) {
				if (this.richBox.Text == Constants.BLANK)
				{
				string[] links = linkfile.Split (new char[1] {'.'});
				if (links.Length == 2)
				{
					// we have a valid link
						LayoutGuid = links[0];
						ChildGuid = links[1];
					if (LayoutGuid != Constants.BLANK && ChildGuid != Constants.BLANK)
					{
						// now retrieve parent
						NoteDataInterface note = MasterOfLayouts.GetNoteFromInsideLayout(LayoutGuid, ChildGuid);

						// TODO: if null then iterate through all CHILDREN PANELS???
							if (null == note)
							{
							System.Collections.Generic.List<string> children = MasterOfLayouts.GetListOfChildren(LayoutGuid);
								foreach (string child in children)
								{
									note = MasterOfLayouts.GetNoteFromInsideLayout(child, ChildGuid);
									if (note != null)
									{
										break;
									}
								}
							}


						if (note != null)
						{
							if (true == note.IsLinkable)
							{
									SetData (note);
								
							}
						}
						else
						{
							string name = MasterOfLayouts.GetNameFromGuid(LayoutGuid);
							if (Constants.BLANK != name)
							{
							this.richBox.Text = name;
							}
							else
							{
								this.richBox.Text = Loc.Instance.GetStringFmt("There is a link but the target does not appear to be imported yet. Guid is {0}", linkfile);
							}
						}



						// now grab child text
					}
					

				}
				
				else
				{
					this.richBox.Text = Loc.Instance.GetStringFmt("There is a link but the target does not appear to be imported yet. Guid is {0}", linkfile);

				}
				}
			}
			else
			{
				this.richBox.Text = Loc.Instance.GetString ("Start a link by selecting the note you want to link to and select the Link option. Then return here to set the link up");
			}

			Button GoToLayout = new Button();
			GoToLayout.Dock = DockStyle.Top;
			GoToLayout.Text = Loc.Instance.GetString("Go To Link");
			GoToLayout.Enabled = false;
			GoToLayout.Click+= HandleGoToLayoutClick;
			if (LayoutGuid != Constants.BLANK) GoToLayout.Enabled = true;

			ParentNotePanel.Controls.Add (GoToLayout);
			GoToLayout.BringToFront();
			richBox.BringToFront();
		}
		public override void GetStoryboardData (out string sCaption, out string sValue, out int type, out string extraField)
		{
			base.GetStoryboardData (out sCaption, out sValue, out type, out extraField);
			
			// the base is great for text notes BUT we have to modify this if we have an image happening
			if (Pic != null) {
				if (Pic.Image != null && Pic.Image.Tag != null)
				{
					sValue = Pic.Image.Tag.ToString ();
					type = 1;
				}
			}
		}
		/// <summary>
		/// Sets the data. Called from CreateParent
		/// </summary>
		/// <param name='note'>
		/// Note.
		/// </param>
		private void SetData(NoteDataInterface note)
		{
			string link = note.GetLinkData();
			if (General.IsGraphicFile(link))
			{
				// A linknote will not benefit from the smart file finding that a NotePicture might do in tracking down a missing file
				if (File.Exists (link) == true)
				{
				
				if (null == Pic)
				{
					Pic = new PictureBox();
						ParentNotePanel.Controls.Add (Pic);
						Pic.SizeMode = PictureBoxSizeMode.StretchImage;
					
				}
					Pic.Dock = DockStyle.Fill;
					Pic.Visible = true;
					richBox.Visible = false;

					Pic.Image = Image.FromFile (link);
					// we store this for use when a storyboard wants to link to a linked iamge
					Pic.Image.Tag = link;
				}
				else
				{
					this.richBox.Text= Loc.Instance.GetStringFmt("The file {0} does not exist. If the original note displays correctly it might be that the file is not actually in the location set. Doublecheck.", link);
				}
			}
			else
			{
				if (Pic != null)
				{
					Pic.Visible = false;
					Pic.Image.Tag = null;
					Pic.Image.Dispose();
					richBox.Visible = true;
				}
				this.richBox.Rtf = link;
			}
		}

		void HandleGoToLayoutClick (object sender, EventArgs e)
		{
			if (LayoutGuid == Constants.BLANK) {
				NewMessage.Show (Loc.Instance.GetString ("No link defined for this note."));
				return;
			}
			LayoutDetails.Instance.LoadLayout(LayoutGuid, ChildGuid);
		}

		void HandleRefreshLinkClick (object sender, EventArgs e)
		{
			this.richBox.Text = "";
			Layout.SaveLayout();
			Update(Layout);
			BringToFrontAndShow();

		}

		void HandleSetupLinkClick (object sender, EventArgs e)
		{
			string link = LayoutDetails.Instance.PopLink ();
			if (link == Constants.BLANK) {
				NewMessage.Show ("To link Layouts, first go to the note you want to link to on the other Layout. Select the 'Create a Link' option. Now go to the Layout where you want the link to be, create a Link Note, such as this, and then select this option again.");
			} else {

				SetLink(link);
			}
		}



		// sFile comes in the format of parent.child
		public virtual void SetLink (string file)
		{


		//the sExtra needs to be in Parent.Child form for the REciprocal links to work later

			LinkTableRecord result = Array.Find (Layout.GetLinkTable ().GetRecords (), LinkTableRecord => LinkTableRecord.sExtra == String.Format (CoreUtilities.Links.LinkTableRecord.PageLinkFormatString, Layout.GUID,this.GuidForNote));
			
			//string file = String.Format (CoreUtilities.Links.LinkTableRecord.PageLinkFormatString, LayoutGuid, ChildGuid);


			// August 2013 -- moved this out of the results==null routine BEACAUSE we need this to be updated when
			//editing a link too! Not just on the initial creation
			string newCaption = Constants.BLANK;
			// may 2013 - if we have a * it means we have a caption encoded at end of string and this neesd to be removed.
			if (file.IndexOf("*") > -1)
			{
				string[] details = file.Split(new char[1]{'*'});
				if (details != null && details.Length == 2)
				{
					file = details[0];
					newCaption = details[1];
					this.Caption = newCaption;
				}
			}

			if (result == null) {
				// add new
				result = new LinkTableRecord();


				result.sFileName = file;


				string SourceContainerGUIDToUse = Layout.GUID;
			



				result.sExtra = String.Format (CoreUtilities.Links.LinkTableRecord.PageLinkFormatString, SourceContainerGUIDToUse, this.GuidForNote);
				result.sText = this.Caption;
				
				Layout.GetLinkTable ().Add (result);
			} else {
				// edit existing
				result.sFileName = file;
				Layout.GetLinkTable ().Edit(result);
			}
			this.richBox.Text  = Constants.BLANK;
			// force a save else table might not be made correctly
			Layout.SaveLayout();
			Update(Layout);
			BringToFrontAndShow();
		}

	
		/// <summary>
		/// Gets the link. Returns it in the format 
		/// ParentLayout.ChildNote
		/// </summary>
		/// <returns>
		/// The link.
		/// </returns>
		protected virtual string GetLink ()
		{
			string stringResult = Constants.BLANK;
			
			// The link Table will be null if we are using the FindNote functions, in which case we ignore this though log it in case its an error
			if (Layout.GetLinkTable () != null) {
				LinkTableRecord result = Array.Find (Layout.GetLinkTable ().GetRecords (), 
				                                     LinkTableRecord => LinkTableRecord.sExtra == String.Format (CoreUtilities.Links.LinkTableRecord.PageLinkFormatString,
				                                                            Layout.GUID, this.GuidForNote));
				stringResult = Constants.BLANK;
				
				if (result != null) {
					lg.Instance.Line ("NoteDataXML_LinkNote->GetLink", ProblemType.MESSAGE, result.sFileName);
					stringResult = result.sFileName;
				} else
					lg.Instance.Line ("NoteDataXML_LinkNote->GetLink", ProblemType.WARNING, "No  file defined");        
			} else {
				lg.Instance.Line("NoteDataXML_LinkNote->GetLInk", ProblemType.WARNING, "Possible error in GetLink, the LinkTable was null though that is OKAY if called during a FindNote search");
			}
			return stringResult;//@"C:\Users\Public\Pictures\PhotoStage\001.jpg";
		}

		public override void Save ()
		{
			base.Save ();
		}

		public override string RegisterType()
		{
			return Loc.Instance.Cat.GetString("Link");
		}
		protected override void HandleReadOnlyClick (object sender, EventArgs e)
		{
			// Setting ReadOnly does nothing with a LinkNote
			//base.HandleReadOnlyClick (sender, e);
		}
		public override bool ReadOnly {
			get {
				return base.ReadOnly;
			}
			set {
				// intentionally you cannot change the readonly setting
			}
		}
		public NoteDataXML_LinkNote(NoteDataInterface Note) : base(Note)
		{
			
		}
		public override void CopyNote (NoteDataInterface Note)
		{
			base.CopyNote (Note);
		}
	}
}

