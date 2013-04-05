// AppearancePanelForm.cs
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

namespace Layout
{
	public class AppearancePanelForm : Form
	{
		AppearancePanel appearancePanel = null;
		Button ok = null;
		public AppearancePanelForm (AppearanceClass app)
		{
		

			this.Icon = LayoutDetails.Instance.MainFormIcon;
			FormUtils.SizeFormsForAccessibility(this, LayoutDetails.Instance.MainFormFontSize);

			Panel bottom = new Panel();
			bottom.Dock = DockStyle.Bottom;
			bottom.Height = LayoutDetails.ButtonHeight;
			 ok = new Button();

		



			ok.Text = Loc.Instance.GetString ("OK");
			ok.DialogResult = DialogResult.OK;
			ok.Dock = DockStyle.Left;


			Button Cancel = new Button();
			Cancel.Text = Loc.Instance.GetString ("Cancel");
			Cancel.Dock = DockStyle.Right;
			Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

			bottom.Controls.Add (ok);
			bottom.Controls.Add(Cancel);


			if (app == null) {
				// we are in build mode. So we create a classic note
				app = AppearanceClass.SetAsClassic();

				app.Name = "Choose A Unique Name For Your New Appearance";
				ok.Enabled = false;

			}



			appearancePanel = new AppearancePanel(false, app, null, ValidData, true);

			appearancePanel.Dock = DockStyle.Fill		;
				this.Controls.Add(bottom);
			this.Controls.Add (appearancePanel);
			appearancePanel.BringToFront();
		}

		public void ValidData (bool valid)
		{
			this.ok.Enabled = valid;
		}

		/// <summary>
		/// Gets the appearance. Called from the caller and returns the current object set in the form.
		/// </summary>
		/// <returns>
		/// The appearance.
		/// </returns>
		public AppearanceClass GetAppearance ()
		{
			return appearancePanel.GetAppearanceSelected();
		}
	}
}

