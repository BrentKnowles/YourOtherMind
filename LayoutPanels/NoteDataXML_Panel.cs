using System;
using Layout;
using System.Windows.Forms;
using System.Drawing;
using CoreUtilities;

namespace LayoutPanels
{
	public class NoteDataXML_Panel :NoteDataXML
	{
		#region constants
		public override int defaultHeight { get { return 400; } }
		public override int defaultWidth { get { return 500; } }
		#endregion
		#region gui
		protected LayoutPanelBase panelLayout;
		#endregion
		#region variable
		public override bool IsPanel {
			get { return true;}
		}
		#endregion
		#region testingstub
		public void Add10TestNotes ()
		{
			for (int i=0; i <10; i++) {
				NoteDataXML note = new NoteDataXML();
				note.Caption = "hello there " + i.ToString();
				panelLayout.AddNote (note);
			}
		}
		#endregion
		// This is where it gets tricky. Need to modify the list of valid data types to store!
		public NoteDataXML_Panel () : base()
		{
			Caption = Loc.Instance.Cat.GetString("Panel");




		}
		public override void Dispose ()
		{
			lg.Instance.Line("NoteDataXML_Panel->Dispose", ProblemType.MESSAGE, String.Format ("Dispose called for Note Panel Caption/Guid {0}/{1}", Caption, this.GuidForNote),Loud.ACRITICAL);
			base.Dispose ();
		}

		/// <summary>
		/// Gets the panels layout. This is used when autobuilding a panel inside a panel inside DefaultLayouts.cs
		/// </summary>
		public LayoutPanelBase GetPanelsLayout ()
		{
			return panelLayout;
		}
		public NoteDataXML_Panel(int height, int width) : base(height, width)
		{
			Caption = Loc.Instance.Cat.GetString("Panel");

		}
		/// <summary>
		/// Gets the child notes.
		/// </summary>
		public  override System.Collections.ArrayList  GetChildNotes()
		{
			LayoutDatabase layout = new LayoutDatabase(this.GuidForNote);
			layout.LoadFrom(null);
			return layout.GetAllNotes();

		}
		/// <summary>
		/// Adds the note. (during a move operation. Called from LayoutPanel)
		/// </summary>
		/// <param name='note'>
		/// Note.
		/// </param>
		public void AddNote (NoteDataInterface note)
		{
			panelLayout.AddNote (note);
			// jan 20 2013 - added this because i was doing an Update in LayoutPanel but that was causing
			// issues with destroying/disposing the original object
			//note.CreateParent(panelLayout);
		}
		public override void Save()
		{
			base.Save();

			// need some kind of copy constructor to grab things like Notebook and Section from the parent to give to the child panels
			panelLayout.SetParentFields(Layout.Section, Layout.Keywords, Layout.Subtype, Layout.Notebook);
			panelLayout.SaveLayout ();
		}
		
		protected override void DoBuildChildren (LayoutPanelBase Layout)
		{
			base.DoBuildChildren (Layout);
			ParentNotePanel.BorderStyle = BorderStyle.Fixed3D;
		CaptionLabel.Dock = DockStyle.Top;

			panelLayout = new LayoutPanel(Layout.GUID, false);
			panelLayout.SetSubNoteSaveRequired = Layout.SetSaveRequired;

			// must set the Parent before loading
			panelLayout.Parent = ParentNotePanel;

			// load the layout based on the note
			panelLayout.LoadLayout(this.GuidForNote, true, Layout.GetLayoutTextEditContextStrip());

		
			panelLayout.Visible = true;
			panelLayout.Dock = DockStyle.Fill;
			panelLayout.BringToFront();
			panelLayout.SetParentLayoutCurrentNote = Layout.SetCurrentTextNote;

			/* I do not know why I duplicated this!
			ToolStripButton ShowTabs = new ToolStripButton(Loc.Instance.GetString("Show Tabs?"));
			ShowTabs.CheckOnClick = true;
			ShowTabs.Checked = panelLayout.ShowTabs;
			ShowTabs.CheckedChanged+= HandleCheckedChanged;
			
			properties.DropDownItems.Add (ShowTabs);
			*/
			
		}
		/// <summary>
		/// Registers the type.
		/// </summary>
		public override string RegisterType()
		{
			return Loc.Instance.Cat.GetString("Panel");
		}

		void HandleCheckedChanged (object sender, EventArgs e)
		{
			panelLayout.ShowTabs = !panelLayout.ShowTabs;
			panelLayout.RefreshTabs();
			SetSaveRequired(true);
		}
		public NoteDataInterface FindSubpanelNote(NoteDataInterface note)
		{
			lg.Instance.Line("NoteDataXML_Panel->FindSubpanelNote", ProblemType.MESSAGE, String.Format ("Searching {0} for subnotes", this.Caption),Loud.CTRIVIAL); 
			return panelLayout.FindSubpanelNote(note);
		}

		public void ClearDrag()
		{
			panelLayout.ClearDrag();
		}
	}
}

