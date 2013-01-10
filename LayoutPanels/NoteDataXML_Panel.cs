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
		protected LayoutPanel panelLayout;
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
		}
		public override void Save()
		{
			base.Save();

			panelLayout.SaveLayout ();
		}
		
		public override void CreateParent (LayoutPanelBase Layout)
		{
			base.CreateParent (Layout);
			ParentNotePanel.BorderStyle = BorderStyle.Fixed3D;
		CaptionLabel.Dock = DockStyle.Top;

			panelLayout = new LayoutPanel(Layout.GUID, false);
			panelLayout.SetSubNoteSaveRequired = Layout.SetSaveRequired;

			// load the layout based on the note
			panelLayout.LoadLayout(this.GuidForNote, true, Layout.GetLayoutTextEditContextStrip());

			panelLayout.Parent = ParentNotePanel;
			panelLayout.Visible = true;
			panelLayout.Dock = DockStyle.Fill;
			panelLayout.BringToFront();
			panelLayout.SetParentLayoutCurrentNote = Layout.SetCurrentTextNote;


			ToolStripButton ShowTabs = new ToolStripButton(Loc.Instance.GetString("Show Tabs?"));
			ShowTabs.CheckOnClick = true;
			ShowTabs.Checked = panelLayout.ShowTabs;
			ShowTabs.CheckedChanged+= HandleCheckedChanged;
			
			properties.DropDownItems.Add (ShowTabs);
			
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
	}
}

