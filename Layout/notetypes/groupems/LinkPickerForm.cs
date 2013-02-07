using System;
using System.Windows.Forms;
using System.Drawing;
using CoreUtilities;

namespace Layout
{
	public class LinkPickerForm : Form
	{
		ComboBox ListOfNotes = null;

		public NoteDataInterface GetNote {
			get {
				if (ListOfNotes.SelectedItem != null) {
					return (NoteDataInterface)ListOfNotes.SelectedItem;
				}
				return null;
			}
		}
		public string GetSelectedCaption {
			get {
				if (ListOfNotes.SelectedItem != null) {
					return ListOfNotes.Text;
				}
				return null;
				;
			}
		}
	 public string GetSelectedGUID {
			get {
				if (ListOfNotes.SelectedItem != null) {
					return ListOfNotes.SelectedValue.ToString ();
				}
				return null;
				;
			}
								        
		}
		public LinkPickerForm (Icon icon, System.Collections.Generic.List<NoteDataInterface> listOfNotes)
		{

			this.Icon = icon;
			this.Text = Loc.Instance.GetString ("Add Note to Storyboard");

			Button bOk = new Button();
			bOk.DialogResult = DialogResult.OK;
			bOk.Text = Loc.Instance.GetString ("OK");
			bOk.Dock = DockStyle.Left;

			Button bCancel = new Button();
			bCancel.DialogResult = DialogResult.Cancel;
			bCancel.Text = Loc.Instance.GetString ("Cancel");
			bCancel.Dock = DockStyle.Right;

			 ListOfNotes = new ComboBox();
			ListOfNotes.DropDownStyle = ComboBoxStyle.DropDownList;
			ListOfNotes.Dock = DockStyle.Top;
			ListOfNotes.DataSource = listOfNotes;



			Label LabelListOfNotes = new Label();
			LabelListOfNotes.Text = Loc.Instance.GetString("Select a Note");
			LabelListOfNotes.Dock = DockStyle.Top;

			Panel bottom = new Panel();
			bottom.Height = 40;
			bottom.Dock = DockStyle.Bottom;
			bottom.Controls.Add (bOk);
			bottom.Controls.Add (bCancel);


			this.Controls.Add (bottom);
			this.Controls.Add (ListOfNotes);
			this.Controls.Add (LabelListOfNotes);


			ListOfNotes.ValueMember = "GuidForNote";
			ListOfNotes.DisplayMember = "Caption";
		}
	}
}

