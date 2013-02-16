using System;
using System.Collections;
using System.Collections.Generic;
using CoreUtilities;
using System.Windows.Forms;
using System.Drawing;
using Transactions;

namespace Layout
{
	/// <summary>
	/// Wiill hold info about NoteTypes, About the active layoutpanel
	/// </summary>
	public class GenericTextForm : Form
	{
		RichTextBox Box =null;
		public GenericTextForm()
		{
			Icon = LayoutDetails.Instance.MainFormIcon;
			FormUtils.SizeFormsForAccessibility(this, LayoutDetails.Instance.MainFormFontSize);
			Box = new RichTextBox();
			Box.ReadOnly = true;
			Box.Dock = DockStyle.Fill;
			this.Controls.Add (Box);
			this.WindowState = FormWindowState.Maximized;;
		}

		public RichTextBox GetRichTextBox(){
			return Box;
		}
	}

}




