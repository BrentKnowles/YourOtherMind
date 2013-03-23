using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace CoreUtilities
{
	public class CheckBoxForm : Form
	{
		CheckedListBox checkers;
		public CheckBoxForm (List<string>allItems, List<string> itemsChecked, string label, Icon icon, int buttonheight)
		{
			this.Text = label;

			Panel panel = new Panel ();
			panel.Height = buttonheight;
			this.Controls.Add (panel);
			checkers = new CheckedListBox ();
			this.Controls.Add (checkers);
			this.Icon = icon;

			checkers.Dock = DockStyle.Fill;
			panel.Dock = DockStyle.Bottom;

			Button ok = new Button ();
			ok.Text = Loc.Instance.GetString("OK");
			ok.DialogResult = DialogResult.OK;

			Button cancel = new Button ();
			cancel.Text = Loc.Instance.GetString("Cancel");
			cancel.DialogResult = DialogResult.Cancel;

			panel.Controls.Add (ok);
			panel.Controls.Add (cancel);
			ok.Dock = DockStyle.Left;
			cancel.Dock = DockStyle.Right;

			this.AcceptButton = ok;


			foreach (string ss in allItems) {

			 checkers.Items.Add (ss);
				bool check = false;
				if (itemsChecked.Find (s=>s==ss) != null) check = true;
			// grab last added item and  set check state
				checkers.SetItemChecked(checkers.Items.Count-1, check);

			}

			checkers.Sorted = true;
			checkers.BringToFront();

		}

		public List<string> GetItems ()
		{
			List<string> result = new List<string> ();
			foreach (object o in checkers.CheckedItems) {
				result.Add (o.ToString());
			}
			return result;

		}


	}
}

