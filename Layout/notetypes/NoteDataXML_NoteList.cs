using System;
using System.Windows.Forms;
using CoreUtilities;
namespace Layout
{
	/// <summary>
	/// Note data XM l_ note list.
	/// 
	/// This will be both:
	/// 
	/// 1. The list of Notes on a Layout
	/// 2. The list of Layouts
	/// 
	/// Shared filtering between them.
	/// </summary>
	public class NoteDataXML_NoteList : NoteDataXML
	{
		#region formcontrols
		ListBox list;
		#endregion

		public NoteDataXML_NoteList () : base()
		{
			Caption = Loc.Instance.Cat.GetString("Note List");
		}

		public override void CreateParent (LayoutPanelBase Layout)
		{
			base.CreateParent (Layout);
			CaptionLabel.Dock = DockStyle.Top;
			list = new ListBox();
			list.SelectedIndexChanged += HandleSelectedIndexChanged;
			list.Parent = Parent;
			list.Width = 125;
			list.Dock = DockStyle.Fill;
			list.BringToFront();
		}
		public void UpdateList(System.Collections.ObjectModel.ReadOnlyCollection<NoteDataInterface> notes)
		{
			// if find one then we call its update function
			
			this.list.DataSource = null;
			this.list.Items.Clear ();
			this.list.DataSource = notes;
			this.list.DisplayMember = "Caption";
			this.list.ValueMember = "GuidForNote";
		}
		void HandleSelectedIndexChanged (object sender, EventArgs e)
		{
			//grid.SelectedObject = (NoteDataXML)list.SelectedItem;
		}
		/// <summary>
		/// Registers the type.
		/// </summary>
		public override string RegisterType()
		{
			return Loc.Instance.Cat.GetString("List");
		}
		
	
	}

}

