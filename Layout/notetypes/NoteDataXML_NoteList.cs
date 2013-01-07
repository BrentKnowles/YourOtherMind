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
		public enum Modes  {NOTES, LAYOUTS};

		public override int defaultHeight { get { return 600; } }
		public override int defaultWidth { get { return 200; } }

		#endregion
		#region variables

		#endregion

		#region variables_saved_in_xml
		Modes mode;
		/// <summary>
		/// Gets or sets the mode. (Showing the list of notes or the list of layouts)
		/// </summary>
		/// <value>
		/// The mode.
		/// </value>
		public Modes Mode {
			get { return mode;}
			set { mode = value;}
		}
		#endregion

		#region formcontrols
		ListBox list;
		Label count;
		
		#endregion

		public NoteDataXML_NoteList () : base()
		{
			Caption = Loc.Instance.Cat.GetString("Note List");
			mode = Modes.NOTES;
		}
		public NoteDataXML_NoteList(int height, int width) : base(height, width)
		{
			Caption = Loc.Instance.Cat.GetString("Note List");
			mode = Modes.NOTES;
		}

		public override void CreateParent (LayoutPanelBase Layout)
		{
			base.CreateParent (Layout);
			CaptionLabel.Dock = DockStyle.Top;

			ComboBox mode = new ComboBox ();
			mode.Parent = ParentNotePanel;
			mode.DropDownStyle = ComboBoxStyle.DropDownList;
			mode.Dock = DockStyle.Top;
			mode.BringToFront ();

			mode.Items.Add (">ThisLayout");
			mode.Items.Add (">AllLayouts");


			mode.SelectedIndexChanged += HandleDropDownSelectedIndexChanged;
		

			list = new ListBox();
			//list.SelectedIndexChanged += HandleDropDownSelectedIndexChanged;
			list.Parent = ParentNotePanel;
			list.Width = 125;
			list.Dock = DockStyle.Fill;
			list.BringToFront();
			list.BindingContextChanged+= HandleBindingContextChanged;
			list.DoubleClick += HandleListBoxDoubleClick;


			Button refresh = new Button();
			refresh.Text = Loc.Instance.GetString("Refresh");
			refresh.Dock = DockStyle.Bottom;
			refresh.Parent = ParentNotePanel;
			refresh.Click += HandleRefreshClick;

			 count = new Label();
			count.Parent = ParentNotePanel;
			count.Dock = DockStyle.Top;




			// All other elements need to be made before this code
			switch (Mode) {
			case Modes.NOTES: mode.SelectedIndex = 0; break;
			case Modes.LAYOUTS: mode.SelectedIndex = 1; break;
			}

		}

		void HandleBindingContextChanged (object sender, EventArgs e)
		{
			count.Text = Loc.Instance.GetStringFmt("Count = {0}", this.list.Items.Count);
		}

	


		public void Refresh()
		{
			if (Modes.NOTES == mode) {
				UpdateListOfNotesOnLayout ();
			} else
			if (Modes.LAYOUTS == mode) {
				UpdateListOfLayouts();
				
			}
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
			Refresh ();
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


					NoteDataInterface note =  (NoteDataInterface)this.list.SelectedItem;

				if (note != null)
				{

					Layout.GoToNote(note);
				}
				else
				{
					lg.Instance.Line ("LayoutPanel->HandleTabButtonClick", ProblemType.WARNING, String.Format ("Note with guid = {0} not found",note.GuidForNote));
				}

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
			this.list.DataSource = master.GetListOfLayouts ("filternotdone");
			this.list.DisplayMember = "Caption";
			this.list.ValueMember = "Guid";
			master.Dispose ();


		}

		void HandleDropDownSelectedIndexChanged (object sender, EventArgs e)
		{
			if ((sender as ComboBox).SelectedIndex == 0) {
				// just the notes in this layout
				mode = Modes.NOTES;
				UpdateListOfNotesOnLayout ();
			} else
			if ((sender as ComboBox).SelectedIndex == 1) {
				// all notes
				mode = Modes.LAYOUTS;
				//TODO: The RIGHT way to do this woudl be subclasses.
				// this is just a HACK to get up and running
				UpdateListOfLayouts ();



			}

		}

		public void UpdateListOfNotesOnLayout ()
		{
			// if find one then we call its update function
			if (mode == Modes.NOTES) {
				this.list.DataSource = null;
				this.list.Items.Clear ();
				this.list.Sorted = true;
				this.list.DataSource = Layout.GetAllNotes();
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

