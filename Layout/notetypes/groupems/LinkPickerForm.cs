// LinkPickerForm.cs
//
// Copyright (c) 2013 Brent Knowles (http://www.brentknowles.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Review documentation at http://www.yourothermind.com for updated implementation notes, license updates
// or other general information/
// 
// Author information available at http://www.brentknowles.com or http://www.amazon.com/Brent-Knowles/e/B0035WW7OW
// Full source code: https://github.com/BrentKnowles/YourOtherMind
//###
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
			FormUtils.SizeFormsForAccessibility(this, LayoutDetails.Instance.MainFormFontSize);
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

