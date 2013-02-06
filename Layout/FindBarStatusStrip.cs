using System;
using System.Windows.Forms;
using CoreUtilities;
using System.Collections.Generic;

namespace Layout
{

	/// <summary>
	/// The new FindBar, for placement on Layouts
	/// </summary>
	public class FindBarStatusStrip : StatusStrip
	{

		#region GUID
		ToolStripLabel CurrentNote=null;
		ToolStripTextBox Searchbox=null;
		#endregion

		public FindBarStatusStrip()
		{



			CurrentNote = new ToolStripLabel();
			CurrentNote.BackColor = this.BackColor;
			CurrentNote.Text = "Current selected Note";

			Searchbox = new ToolStripTextBox();

			this.Items.Add (CurrentNote);
			this.Items.Add (Searchbox);
		}


		public void FocusOnSearchEdit ()
		{
			if (Searchbox == null) {
				throw new Exception("No searchbox defined in Findbar");
			}

			Searchbox.Focus();
		}
	}

}

