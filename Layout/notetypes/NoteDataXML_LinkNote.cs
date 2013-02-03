using System;
using CoreUtilities;
using CoreUtilities.Links;
using System.Windows.Forms;

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


		public override void CreateParent (LayoutPanelBase Layout)
		{
			base.CreateParent (Layout);
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
						if (note != null)
						{
							if (true == note.IsLinkable)
							{
								this.richBox.Rtf = note.ToString();
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
			
			if (result == null) {
				// add new
				result = new LinkTableRecord();
				result.sFileName = file;
				result.sExtra = String.Format (CoreUtilities.Links.LinkTableRecord.PageLinkFormatString, Layout.GUID, this.GuidForNote);
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
	}
}

