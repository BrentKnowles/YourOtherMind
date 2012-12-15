using System;
using Layout;
using System.Windows.Forms;
using System.Drawing;
using CoreUtilities;

namespace LayoutPanels
{
	public class NoteDataXML_Panel :NoteDataXML
	{
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
			Caption = Loc.Instance.Cat.GetString("PANEL");




		}

		/// <summary>
		/// Gets the child notes.
		/// </summary>
		public  override System.Collections.ObjectModel.ReadOnlyCollection<NoteDataInterface>  GetChildNotes()
		{
			LayoutDatabase layout = new LayoutDatabase(this.GuidForNote);
			layout.LoadFrom(null);
			return layout.GetNotes();

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
			Parent.BorderStyle = BorderStyle.Fixed3D;
		CaptionLabel.Dock = DockStyle.Top;

			panelLayout = new LayoutPanel(Layout.GUID);
			panelLayout.SetSubNoteSaveRequired = Layout.SetSaveRequired;

			// load the layout based on the note
			panelLayout.LoadLayout(this.GuidForNote);

			panelLayout.Parent = Parent;
			panelLayout.Visible = true;
			panelLayout.Dock = DockStyle.Fill;
			panelLayout.BringToFront();
			
		}
		/// <summary>
		/// Registers the type.
		/// </summary>
		public override string RegisterType()
		{
			return Loc.Instance.Cat.GetString("Panel");
		}
	}
}

