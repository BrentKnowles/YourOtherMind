// TabNavigation.cs
//
// Copyright (c) 2014 Brent Knowles (http://www.brentknowles.com)
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
using CoreUtilities;
using System.Windows.Forms;
using System.Collections.Generic;
using Layout;

namespace Layout
{
	public class TabNavigation : TabControl
	{
		RichTextExtended textBox = null;
		public TabNavigation (RichTextExtended box)
		{
			if (box == null) {
				throw new Exception(Loc.Instance.GetString ("Must pass a valid RichTextBox into TabNavigation"));
			}
			textBox = box;
			this.Height = 25;
			this.Selected+= (object sender, TabControlEventArgs e) => 
			{
				if (this.SelectedIndex > -1)
				{
				//	NewMessage.Show (this.SelectedIndex
					string textToFind = this.TabPages[this.SelectedIndex].Text;
					//NewMessage.Show (this.TabPages[this.SelectedIndex].Text);
					if (textToFind != Constants.BLANK)
					{
						textBox.Find ("="+textToFind+"=",0, RichTextBoxFinds.None);
						textBox.SelectionLength = 0; // get rid of highlight
					}
				}
			};

			this.VisibleChanged+= (object sender, EventArgs e) => {
				UpdateView ();
			};
			Parse();

		}
		public void UpdateView()
		{
			Parse ();
		}
		/// <summary>
		/// Builds the tab.
		/// </summary>
		/// <param name='tabName'>
		/// Tab name.
		/// </param>
		private void BuildTab (string tabName)
		{
			tabName = tabName.Replace ("=", "");

			this.TabPages.Add (tabName);
		}
		/// <summary>
		/// Takes the associated textBox and parses it for heading information and builds tabs from that.
		/// </summary>
		private void Parse ()
		{

			if (Disposing || IsDisposed) return;
			// don't waste time redrawing if not visible.08/07/2014
			if (this.Visible == false) return;

			this.TabPages.Clear ();
			// - look for all = = matches (Steal YOMMarkup Code)
			List<string> tabs = LayoutDetails.Instance.GetCurrentMarkup ().GetMainHeadings (textBox);
			if (tabs != null) {
				// - Build Tabs based on those.
				foreach (string tab in tabs)
				{
					BuildTab(tab);
				}
			}
		}
	}
}

