using System;
using CoreUtilities;
using System.Windows.Forms;
using Storyboards;

namespace Layout
{
	public class NoteDataXML_GroupEm : NoteDataXML
	{
		#region properties xml
	

		View viewStyle = View.SmallIcon;
		View ViewStyle {
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
		public override void CreateParent (LayoutPanelBase _Layout)
		{
			base.CreateParent (_Layout);
			CaptionLabel.Dock = DockStyle.Top;

			StoryBoard = new Storyboard();
			StoryBoard.Dock = DockStyle.Fill;


			ParentNotePanel.Controls.Add (StoryBoard);
			StoryBoard.BringToFront();
		

			// may 16 2012
//			if (appearance != null)
//			{
//				if (appearance.FactMode == true)
//				{
//					((NotePanelStoryboard)note).groupEms1.FactMode(true);
//				}
//			}
//			
			//StoryBoard.linkTable = Links; Try not to use it, don't know if its essential no more
//			//moved to end to handle refersh issues
			StoryBoard.Source = this.GuidForNote;//((NotePanelStoryboard)note).groupEms1.Source = note.appearance.GUID;
//			
//			//Januar 2012
//			if (true == NEW_LINKS_TMP)
//			{
//				if (false == bFormLoad)
//				{
			//Storyboard.StickyTable = ;//((NotePanelStoryboard)note).groupEms1.StickyTable = new GroupEm.StickyLinkTable();
//					
//					((NotePanelStoryboard)note).groupEms1.StickyTable.SetTable(_table);
//				}
//			}
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

			StoryBoard.InitializeGroupEmFromExternal();
//			((NotePanelStoryboard)note).groupEms1.ClickItem += new GroupEm.groupEms.CustomEventHandler(groupEms1_ClickItem);
//			((NotePanelStoryboard)note).groupEms1.AddItemFromMenu += new GroupEm.groupEms.CustomEventHandler(groupEms1_AddItemFromMenu);
//			((NotePanelStoryboard)note).groupEms1.PrintSelected += new GroupEm.groupEms.CustomEventHandler(groupEms1_PrintSelected); REMOVE

//			((NotePanelStoryboard)note).groupEms1.ExportSelected += new GroupEm.groupEms.CustomEventHandler(groupEms1_ExportSelected);
//			((NotePanelStoryboard)note).groupEms1.DragNote += new GroupEm.groupEms.CustomEventHandler2(groupEms1_DragNote);
//			
//			
//			
//			((NotePanelStoryboard)note).groupEms1.listView.MouseDown += new MouseEventHandler(listView_MouseDown);
//			((NotePanelStoryboard)note).groupEms1.listView.onScroll += new ScrollEventHandler(listView_onScroll);
//			
//			//     ((NotePanelStoryboard)note).groupEms1.listView.MouseDown += findBar_MouseDown;
//			
//			// load settings
//			
//		}
//		
//		
//		
//		
//		
//		SetTooltip(note.appearance);
//		
//		// only add to the index if NOT a system panel
//		if (note.Tag.ToString() != IGNORE_ME_TAG && false == bFormLoad)
//		{
//			string sCaption2 = note.appearance.CaptionGenerated;
//			if (sCaption2 == "") sCaption2 = "Note";
//			
//			notes.AddNoteToList(sCaption2, note.appearance.GUID, note.appearance.ShapeType.ToString());
//			UpdateTabs();

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

				NoteDataInterface note = SuperParent.FindNoteByGuid(sGUID);


				if (note != null)
				{
					if (note is NoteDataXML_LinkNote)
					{
						sResult = "setup link stuff";
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
	}
}

