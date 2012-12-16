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
			list.DoubleClick += HandleListBoxDoubleClick;


			Button refresh = new Button();
			refresh.Text = Loc.Instance.GetString("Refresh");
			refresh.Dock = DockStyle.Bottom;
			refresh.Parent = Parent;
			refresh.Click += HandleRefreshClick;


		}
		/// <summary>
		/// Redraws the lists
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleRefreshClick (object sender, EventArgs e)
		{
			if (Modes.NOTES == mode) {
				NewMessage.Show ("Finish me please");
			} else
			if (Modes.LAYOUTS == mode) {
				UpdateListOfLayouts();
				
			}
		}
		/// <summary>
		/// Handles the list box double click.
		/// 
		/// Will go to the note (if in OnLayout mode) or open a new note
		/// 
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleListBoxDoubleClick (object sender, EventArgs e)
		{
			if (Modes.NOTES == mode) {
				NewMessage.Show ("Finish me please");
			} else
				if (Modes.LAYOUTS == mode) {
				if (this.list.SelectedItem != null)
				{
				MasterOfLayouts.NameAndGuid record = (MasterOfLayouts.NameAndGuid)this.list.SelectedItem;
				LayoutDetails.Instance.LoadLayout(record.Guid);
				}

			}
		}

		void UpdateListOfLayouts ()
		{
			this.list.DataSource = null;
			this.list.Items.Clear ();
			MasterOfLayouts master = new MasterOfLayouts ();
			this.list.DataSource = master.GetListOfLayouts ("filter");
			this.list.DisplayMember = "Caption";
			this.list.ValueMember = "Guid";
			master.Dispose ();
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
				UpdateListOfLayouts ();



			}

		}

		public void UpdateListOfNotesOnLayout (System.Collections.ObjectModel.ReadOnlyCollection<NoteDataInterface> notes)
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

