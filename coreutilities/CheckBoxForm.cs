// CheckBoxForm.cs
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

