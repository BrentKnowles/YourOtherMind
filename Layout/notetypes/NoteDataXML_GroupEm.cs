using System;
using CoreUtilities;
using System.Windows.Forms;
using Storyboards;

namespace Layout
{
	public class NoteDataXML_GroupEm : NoteDataXML
	{
		#region properties xml
	
		bool factmode = false;
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Layout.NoteDataXML_GroupEm"/> is factmode.
		/// 
		/// This is used to hide some gui elements when using this Storyboard for Facts Or Search
		/// </summary>
		/// <value>
		/// <c>true</c> if factmode; otherwise, <c>false</c>.
		/// </value>
		public bool Factmode {
			get {
				return factmode;
			}
			set {
				factmode = value;
			}
		}

		View viewStyle = View.SmallIcon;
		public View ViewStyle {
			get{ return viewStyle;}
			set{viewStyle = value;}
		}

		bool storyboard_ShowPreview=false;

		public bool Storyboard_ShowPreview {
			get {
				return storyboard_ShowPreview;
			}
			set {
				storyboard_ShowPreview = value;
			}
		}

		int storyboard_SplitterSetting=0;

		public int Storyboard_SplitterSetting {
			get {
				return storyboard_SplitterSetting;
			}
			set {
				storyboard_SplitterSetting = value;
			}
		}
		#endregion
		#region gui
		Storyboard StoryBoard = null;
		#endregion
		public NoteDataXML_GroupEm ()
		{
		}
		public NoteDataXML_GroupEm (int _height, int _width): base(_height, _width)
		{
		}

		public ListViewGroupCollection GetGroups ()
		{
			return StoryBoard.Groups;
		}

		protected override void DoBuildChildren (LayoutPanelBase Layout)
		{
			base.DoBuildChildren (Layout);
			CaptionLabel.Dock = DockStyle.Top;

			StoryBoard = new Storyboard();
			StoryBoard.Dock = DockStyle.Fill;


			ParentNotePanel.Controls.Add (StoryBoard);
			StoryBoard.BringToFront();
		

		
			StoryBoard.FactMode(this.Factmode);
//			
//			//moved to end to handle refersh issues
			StoryBoard.Source = this.GuidForNote;//((NotePanelStoryboard)note).groupEms1.Source = note.appearance.GUID;

			// We do not need to call the SetTable function because this is handled when a table isc reated
			StoryBoard.StickyTable = this.Layout.GetLinkTable();
			StoryBoard.NeedSave += HandleNeedSave;

			StoryBoard.ViewStyle = this.ViewStyle;
			if (this.Storyboard_ShowPreview == true)
			{
				StoryBoard.ShowPreview();
			}
			if (this.Storyboard_SplitterSetting > 0)
			{
				StoryBoard.SplitterPosition = this.Storyboard_SplitterSetting;
			}


			StoryBoard.GetNoteForGroupEmPreview += HandleGetNoteForGroupEmPreview;

			StoryBoard.ShowToolbar = true;
			StoryBoard.AddItemFromMenu += HandleAddItemFromMenu;
			StoryBoard.ClickItem += HandleStoryBoardClickItem;
			StoryBoard.InitializeGroupEmFromExternal();

			// will not add these unless I NEED them (I'm assuming some of this is for selecting things?
//			((NotePanelStoryboard)note).groupEms1.DragNote += new GroupEm.groupEms.CustomEventHandler2(groupEms1_DragNote);
//			((NotePanelStoryboard)note).groupEms1.listView.MouseDown += new MouseEventHandler(listView_MouseDown);
//			((NotePanelStoryboard)note).groupEms1.listView.onScroll += new ScrollEventHandler(listView_onScroll);
//     ((NotePanelStoryboard)note).groupEms1.listView.MouseDown += findBar_MouseDown;
//			

		}
		/// <summary>
		/// Handles the story board click item. Goes to the note
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		string HandleStoryBoardClickItem (object sender)
		{
			CoreUtilities.Links.LinkTableRecord record = (CoreUtilities.Links.LinkTableRecord)sender;
			if (record != null) {

				//LayoutDetails.Instance.LoadLayout(Layout.GUID, record.sFileName);
//				NoteDataInterface note = Layout.FindNoteByGuid(record.sFileName);
//				if (note != null)
//				{
//					Layout.GoToNote(note);
//				}
				if (General.IsGraphicFile(record.sFileName))
				{
					//NewMessage.Show ("image");
					Layout.GetNoteOnSameLayout(record.ExtraField, true);
				}
				else
				Layout.GetNoteOnSameLayout(record.sFileName, true);
			}
			return "";
		}

		string HandleAddItemFromMenu (object sender)
		{

			// Feb 2013 - Difference between this and old version will be that
			// instead of storing the data in different ways for different types
			// we keep it simple.

			// We store only the GUIDs.
			// The Display and Open Note (DoubleClick) functions, will have to handle the logic of figuring
			//  out what to do with the GUID -- i.e., showing a picture instead of text
			// CORRECTION: I stuck with the original way of storing the data to not invalidate old data

			// so simply we need to extract a CAPTION and GUID and we are off
			System.Collections.Generic.List<NoteDataInterface> ListOfNotes = new System.Collections.Generic.List<NoteDataInterface>();

			ListOfNotes.AddRange(Layout.GetAllNotes().ToArray (typeof(NoteDataInterface)) as NoteDataInterface[]);
			ListOfNotes.Sort ();
			LinkPickerForm linkpicker = new LinkPickerForm (LayoutDetails.Instance.MainFormIcon ,ListOfNotes);

								

			DialogResult result = linkpicker.ShowDialog ();
			if (result == DialogResult.OK) {
				
			
				if (linkpicker.GetNote != null)
				{

					string sCaption = Constants.BLANK;
					string sValue = Constants.BLANK;
					string extraField = Constants.BLANK;
					int type = 0;
					linkpicker.GetNote.GetStoryboardData(out sCaption, out sValue, out type, out extraField);
				
					StoryBoard.AddItem (sCaption, sValue, type, extraField);
				StoryBoard.InitializeGroupEmFromExternal();
				}

//				if (tempPanel != null) {
//					// testing if a picture
//					if (tempPanel.appearance.ShapeType == Appearance.shapetype.Picture) {
//						string sFile = tempPanel.appearance.File;
//						(sender as GroupEm.groupEms).AddItem (sText, sFile, 1);
//					} else {
//						// may 2010 -- tring to get the normal name not the fancy caption
//						
//						// add as link
//						(sender as GroupEm.groupEms).AddItem (tempPanel.appearance.Caption, sValue, 0);
//					}
//					
//				}
			}
			return "";
		}

		/// <summary>
		/// Handles the get note for group em preview. (Is how we grab the required text or whatever from the preview
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		string HandleGetNoteForGroupEmPreview (object sender)
		{
			string sResult = "";
			string sGUID = sender.ToString();
			if (sGUID != null && sGUID != "")
			{

				LayoutPanelBase SuperParent = null;
				if (Layout.GetIsChild == true) SuperParent = Layout.GetAbsoluteParent (); else SuperParent = Layout;

				NoteDataInterface note = SuperParent.GetNoteOnSameLayout(sGUID, false);
				//NoteDataInterface note = SuperParent.FindNoteByGuid(sGUID);

				if (note != null)
				{
					if (note is NoteDataXML_LinkNote)
					{
						// because the text is stored this should just work
						sResult =note.GetStoryboardPreview;
//						if (File.Exists(panel.appearance.File) == true)
//						{
//							
//							Worgan2006.classPageMiddle importPage =
//								(Worgan2006.classPageMiddle)CoreUtilities.General.DeSerialize(panel.appearance.File, typeof(Worgan2006.classPageMiddle));
//							
//							
//							
//							
//							if (importPage != null)
//							{
//								if (importPage.PageType == _Header.pagetype.VISUAL)
//								{
//									System.Type t = _Header.GetClassTypeByPageType(importPage.PageType);
//									importPage =
//										(Worgan2006.classPageMiddle)CoreUtilities.General.DeSerialize(panel.appearance.File, t);
//									
//								}
//								
//								sResult = importPage.GetBrainstormText;
//								if (sResult.IndexOf("rtf") == -1) sResult = "";
//								importPage = null;
//							}
//						}
						
					}
					else
					{
						sResult = note.GetStoryboardPreview;
					}
				}
			}
			
			//  Message Box.Show(myAppearance.Caption);
			return sResult;
		}

		string HandleNeedSave (object sender)
		{
			SetSaveRequired(true);
			return "";

		}

		public override void Save ()
		{
			this.ViewStyle = StoryBoard.ViewStyle;
			this.Storyboard_ShowPreview = StoryBoard.IsPreview;
			this.Storyboard_SplitterSetting  = StoryBoard.SplitterPosition;


			base.Save ();

		}
		public override string RegisterType ()
		{
			return Loc.Instance.Cat.GetString("Storyboard");
		}
		protected override void CommonConstructorBehavior ()
		{
			base.CommonConstructorBehavior ();
			Caption = Loc.Instance.Cat.GetString("Storyboard");
		}

		public int CountStoryBoardItems()
		{
			return StoryBoard.Items.Count;
		}
		// so far only being used by test routine
		public void Refresh()
		{
			StoryBoard.InitializeGroupEmFromExternal();
		}

		public void DeleteRecordsForStoryboard ()
		{
			StoryBoard.DeleteRecordsForStoryboard();
		}

		public void SetFactMode ()
		{
			Factmode = true;
		}

		public void RefreshGroupEm ()
		{
			StoryBoard.InitializeGroupEmFromExternal();
		}

		public void AddRecordDirectly (string sTitle,string sLinkUrl, string sGroup )
		{
			StoryBoard.AddItemAndToGroup(sTitle, sLinkUrl, 0, sGroup);
		}
	}
}

