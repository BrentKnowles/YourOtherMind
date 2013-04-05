// KeyEditForm.cs
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
using CoreUtilities;
using System.Drawing;
namespace HotKeys
{
	public class KeyEditForm : Form
	{
		// returned when closed
		public KeyData keyOut;
		private ComboBox keyKey ;
		private ComboBox keyModifier;
		public KeyEditForm (KeyData keyIn, Icon formIcon)
		{
			this.Icon = formIcon;
			this.FormClosing += HandleFormClosing;

			Label labelModifier = new Label ();
			labelModifier.Text = Loc.Instance.GetString ("Modifier");
			labelModifier.Dock = DockStyle.Top;

			Label labelKey = new Label ();
			labelKey.Text = Loc.Instance.GetString ("Key");
			labelKey.Dock = DockStyle.Top;

			 keyModifier = new ComboBox ();
			keyModifier.DropDownStyle = ComboBoxStyle.DropDownList;
			keyModifier.Dock = DockStyle.Top;

		





			 keyKey = new ComboBox();


			keyKey.Dock = DockStyle.Top;
		
			keyKey.DropDownStyle = ComboBoxStyle.DropDownList;




			Array arrKeys = Enum.GetValues (typeof(Keys));
			
			foreach (object key in arrKeys) {
				keyModifier.Items.Add (key.ToString ());
				keyKey.Items.Add (key.ToString ());
			}
			keyModifier.Text = keyIn.ModifyingKey.ToString ();
			keyKey.Text = keyIn.Key.ToString ();

			Panel bottom = new Panel();
			bottom.Height = 40;
			bottom.Dock = DockStyle.Bottom;

			Button OK = new Button();
			OK.Text = Loc.Instance.GetString("OK");

			OK.Dock = DockStyle.Right;
			OK.DialogResult = DialogResult.OK;
			
			
			Button Cancel  = new Button();
			Cancel.Text = Loc.Instance.GetString ("Cancel");
			Cancel.Padding = new System.Windows.Forms.Padding(10);
			Cancel.DialogResult = DialogResult.Cancel;
			Cancel.Dock = DockStyle.Right;

			bottom.Controls.Add (OK);
			bottom.Controls.Add (Cancel);


			this.Controls.Add (bottom);


			this.Controls.Add (keyKey);
			this.Controls.Add (labelKey);

			this.Controls.Add (keyModifier);
			this.Controls.Add (labelModifier);


			keyOut = keyIn;
		}

		void HandleFormClosing (object sender, FormClosingEventArgs e)
		{
			keyOut.Key = (Keys)Enum.Parse (typeof(Keys), keyKey.Text);
			keyOut.ModifyingKey = (Keys)Enum.Parse (typeof(Keys), keyModifier.Text);
		}
	}
}

