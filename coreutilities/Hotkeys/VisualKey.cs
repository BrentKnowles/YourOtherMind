// VisualKey.cs
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
namespace HotKeys
{
	public class VisualKey : Panel
	{

		// ConfigPanel will test this to see if the user has edited this key
		public bool IsModified = false;
		private KeyData currentKey;
		Icon FormIcon;
		Label modifier ;
		public KeyData keyOut = null;
		Action<string>AfterKeyEdit=null;
		Color oldBackColor;
		void BuildKeyString (KeyData keyToBuild)
		{
			modifier.Text = Loc.Instance.GetStringFmt ("{0} ({1} + {2})", keyToBuild.Label, keyToBuild.ModifyingKey.ToString (), keyToBuild.Key.ToString ());
		}
		public string UniqueCode ()
		{
			return currentKey.Key.ToString () + currentKey.ModifyingKey.ToString ();
		}
		public VisualKey (KeyData keyToBuild, Icon formIcon, Action<string>afterKeyEdit)
		{
			oldBackColor = this.BackColor;
			if (null == keyToBuild) {
				throw new Exception("must pass in a valid key object");
			}
			FormIcon = formIcon;

			modifier = new Label();
			BuildKeyString (keyToBuild);
			modifier.Dock = DockStyle.Bottom;




			Button edit = new Button();
			edit.Text = Loc.Instance.GetString ("Edit");
			edit.Dock = DockStyle.Bottom;

			this.Controls.Add (modifier);
			this.Controls.Add (edit);
		


			                                  

			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.AutoSize = true;
			edit.Click+= HandleKeyChangeClick;
			AfterKeyEdit = afterKeyEdit;

			currentKey = keyToBuild;
		
		}

		public void IsDuplicate (bool b)
		{
			if (true == b) {
				this.BackColor = Color.Red;
			}
			else
				this.BackColor =oldBackColor;
		}

		void HandleKeyChangeClick (object sender, EventArgs e)
		{
			KeyEditForm keyEdit = new KeyEditForm (this.currentKey, FormIcon);
			if (keyEdit.ShowDialog () == DialogResult.OK) {
				//NewMessage.Show (keyEdit.keyOut.Key.ToString ()+ keyEdit.keyOut.ModifyingKey.ToString());
				IsModified = true;
				keyOut = keyEdit.keyOut;

				BuildKeyString (keyOut);

			}
			// do this regardless, we are just updating interface, not data
			if (AfterKeyEdit != null)
			{
				// we just redraw the CURRENT one
				AfterKeyEdit(keyOut.GetGUID ());
			}
		}
	}
}

