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
		#region constants

		#endregion
		#region formcontrols
		ListBox list;
		enum Modes  {NOTES, LAYOUTS};
		Modes mode;
		#endregion

		public NoteDataXML_NoteList () : base()
		{
			Caption = Loc.Instance.Cat.GetString("Note List");
			mode = Modes.NOTES;
		}

		public override void CreateParent (LayoutPanelBase Layout)
		{
			base.CreateParent (Layout);
			CaptionLabel.Dock = DockStyle.Top;

			ComboBox mode = new ComboBox();
			mode.Parent = Parent;
			mode.Dock = DockStyle.Top;
			mode.BringToFront();

			mode.Items.Add (">ThisLayout");
			mode.Items.Add (">AllLayouts");
			mode.SelectedIndexChanged += HandleDropDownSelectedIndexChanged;


			list = new ListBox();
			//list.SelectedIndexChanged += HandleDropDownSelectedIndexChanged;
			list.Parent = Parent;
			list.Width = 125;
			list.Dock = DockStyle.Fill;
			list.BringToFront();





		}
		void HandleDropDownSelectedIndexChanged (object sender, EventArgs e)
		{
			if ((sender as ComboBox).SelectedIndex == 0) {
				// just the notes in this layout
				mode = Modes.NOTES;
			} else
			if ((sender as ComboBox).SelectedIndex == 1) {
				// all notes
				mode = Modes.LAYOUTS;
				//TODO: The RIGHT way to do this woudl be subclasses.
				// this is just a HACK to get up and running
				this.list.DataSource = null;
				this.list.Items.Clear ();

				MasterOfLayouts master = new MasterOfLayouts();


				this.list.DataSource = master.GetListOfLayouts("filter");
				this.list.DisplayMember="Caption";
				this.list.ValueMember="Guid";
				master.Dispose();
			}

		}

		public void UpdateList (System.Collections.ObjectModel.ReadOnlyCollection<NoteDataInterface> notes)
		{
			// if find one then we call its update function
			if (mode == Modes.NOTES) {
				this.list.DataSource = null;
				this.list.Items.Clear ();
				this.list.DataSource = notes;
				this.list.DisplayMember = "Caption";
				this.list.ValueMember = "GuidForNote";
			} else {

			}
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

