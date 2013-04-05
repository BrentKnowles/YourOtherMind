// OptionForm.cs
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
using CoreUtilities;

namespace appframe
{
	public class OptionForm :Form
	{
		#region interface
		SplitContainer container;
		#endregion
		public OptionForm (List<iConfig> optionPanels, MEF_Interfaces.iAccess access)
		{
			FormUtils.SizeFormsForAccessibility(this, access.FontSizeForForm);
			this.KeyPreview = true;
			this.Icon = MainFormBase.MainFormIcon;
			this.Width = 700;
			this.Height = 600;
			this.Name = "optionform";

			Panel bottom = new Panel();

			bottom.Parent = this;
			bottom.Dock = DockStyle.Bottom;
			bottom.Height = 40;

			Button OK = new Button();
			OK.Text = Loc.Instance.GetString("OK");

			OK.Dock = DockStyle.Right;
			OK.Click+= HandleOkayClick;


			Button Cancel  = new Button();
			Cancel.Text = Loc.Instance.GetString ("Cancel");
			Cancel.Padding = new System.Windows.Forms.Padding(5);
			Cancel.Dock = DockStyle.Right;
			Cancel.DialogResult = DialogResult.Cancel;


			 container = new SplitContainer ();


			container.BorderStyle = BorderStyle.FixedSingle;
			container.Parent = this;
			container.Dock = DockStyle.Fill;
			container.Panel1.BorderStyle = BorderStyle.FixedSingle;
			container.Panel2.BorderStyle = BorderStyle.FixedSingle;
			container.Panel1.Padding = new Padding(5);
			container.Panel2.Padding = new Padding(5);
			container.BringToFront();

			OK.Parent = bottom;
			Cancel.Parent = bottom;

			foreach (iConfig panel in optionPanels) {
				Button button = new Button();
				button.Text = panel.ConfigName;
				button.Tag = panel;
				button.Dock = DockStyle.Top;
				button.Parent = container.Panel1;
				button.Click += HandleOptionButtonClick;

			}

		}
		void HandleOkayClick (object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}
		void HandleOptionButtonClick (object sender, EventArgs e)
		{
			if (container == null) {
				throw new Exception("container object not defined in constructor!");
			}

			container.Panel2.Controls.Clear ();
			if ((sender as Button).Tag != null) {
				iConfig panelInterface = (iConfig)(sender as Button).Tag;
				Panel configPanel = panelInterface.GetConfigPanel();

				container.Panel2.Controls.Add (configPanel);
				configPanel.Dock = DockStyle.Fill;
				configPanel.BringToFront ();
				//container.Panel2.BringToFront();

			}
		}
	}
}

