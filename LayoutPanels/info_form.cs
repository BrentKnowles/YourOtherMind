// info_form.cs
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
using System.Collections.Generic;
using CoreUtilities;
using System.ComponentModel;

namespace Layout
{
	public class info_form : Form
	{

		string GUID= Constants.BLANK;

		#region gui
		TableLayoutPanel layPanel;
		ListBox ListOfEvents = null;
	
		Button refresh = null;
		#endregion

		public info_form (string infoText, string _GUID)
		{

			this.Width = 700;
			this.Height = 500;
			this.Icon = LayoutDetails.Instance.MainFormIcon;
			FormUtils.SizeFormsForAccessibility(this, LayoutDetails.Instance.MainFormFontSize);

			//this.Font = new Font(this.Font.FontFamily, 18.0f);

			if (Constants.BLANK == _GUID) {
				throw new Exception("GUID cannot be blank in info_form");
			}
			GUID = _GUID;
			RichTextBox infohead = new RichTextBox();
			infohead.Text = infoText;
			infohead.ReadOnly = true;
			infohead.Dock = DockStyle.Top;

			refresh = new Button();
			refresh.Text = Loc.Instance.GetString("Update Reciprocal Links");
			refresh.Dock = DockStyle.Top;
			refresh.Click+= HandleRefreshClick;

			layPanel = new TableLayoutPanel();
			layPanel.Dock = DockStyle.Fill;


			ListOfEvents = new ListBox();
			ListOfEvents.Height = 300;
			ListOfEvents.Dock = DockStyle.Bottom;
			ListOfEvents.DoubleClick+= HandleDoubleClick;


			this.Controls.Add (layPanel);
			this.Controls.Add(refresh);
			this.Controls.Add(infohead);
			this.Controls.Add (ListOfEvents);



			// Draw in Event Details

			List<Transactions.TransactionBase> LayoutEvents = LayoutDetails.Instance.TransactionsList.GetEventsForLayoutGuid (GUID);
			LayoutEvents.Sort ();
			LayoutEvents.Reverse();
			if (LayoutEvents != null) {
				ListOfEvents.DataSource = LayoutEvents;
				ListOfEvents.DisplayMember = "Display";
			}
			BuildReciprocaInBackground();
		}

		void HandleDoubleClick (object sender, EventArgs e)
		{
			if (ListOfEvents.SelectedItem != null) {
				GenericTextForm TextForm = new GenericTextForm();
				TextForm.GetRichTextBox().Text = ((Transactions.TransactionBase)ListOfEvents.SelectedItem).ExpandedDescription();
				TextForm.ShowDialog();
			}

		}
		void BuildReciprocaInBackground()
		{
			List<string> ListOfItems = null;
			//from:  http://stackoverflow.com/questions/363377/c-sharp-how-do-i-run-a-simple-bit-of-code-in-a-new-thread
			BackgroundWorker bw = new BackgroundWorker();
			
			// this allows our worker to report progress during work
			bw.WorkerReportsProgress = false;
			
			// what to do in the background thread
			bw.DoWork += new DoWorkEventHandler(
				delegate(object o, DoWorkEventArgs args)
				{refresh.Enabled = false;
				BackgroundWorker b = o as BackgroundWorker;
			//	LinkProgressLabel = new Label();
				refresh.Text = Loc.Instance.GetString("Loading Reciprocal Links...");
//				this.Controls.Add (LinkProgressLabel);
//				LinkProgressLabel.Left = this.Width / 2;
//				LinkProgressLabel.Top = this.Height / 2;
				ListOfItems = BuildLinkList();

				// do some simple processing for 10 seconds
//				for (int i = 1; i <= 10; i++)
//				{
//					// report the progress in percent
//					b.ReportProgress(i * 10);
//					Thread.Sleep(1000);
//				}
				
			});
			
			// what to do when progress changed (update the progress bar for example)
//			bw.ProgressChanged += new ProgressChangedEventHandler(
//				delegate(object o, ProgressChangedEventArgs args)
//				{
//				label1.Text = string.Format("{0}% Completed", args.ProgressPercentage);
//			});
			
			// what to do when worker completes its task (notify the user)
			bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
				delegate(object o, RunWorkerCompletedEventArgs args)
				{
				//label1.Text = "Finished!";
				//this.Controls.Remove (LinkProgressLabel);
				if (null != ListOfItems)
				{
				RefreshReciprocalLinks(ListOfItems);
				}
				refresh.Text = Loc.Instance.GetString("Update Reciprocal Links");
				refresh.Enabled = true;
			});
			
			bw.RunWorkerAsync();
		}

		List<string> BuildLinkList()
		{
			return MasterOfLayouts.ReciprocalLinks (GUID);
		}
		void RefreshReciprocalLinks (List<string> connections )
		{
			// splitting this so that the Threaded version can still perform the heavy listing while we update the UI at the end
			// moving connections out
			layPanel.ColumnCount = 2;
			layPanel.Controls.Clear ();
			foreach (string s in connections) {
				string[] data = s.Split (new char[1] {
					'.'
				});
				if (data != null && data.Length == 2) {
					LinkLabel link = new LinkLabel ();
					link.Tag = data [1];
					link.Text = data [0];
					link.LinkBehavior = LinkBehavior.AlwaysUnderline;
					link.LinkClicked += HandleLinkClicked;
					layPanel.Controls.Add (link);
				}
			}
		}

		/// <summary>
		/// Handles the refresh click for REDRAWING RECIPROCAL LINKS
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleRefreshClick (object sender, EventArgs e)
		{

			this.Cursor = Cursors.WaitCursor;
			RefreshReciprocalLinks (BuildLinkList());





		
			this.Cursor = Cursors.Default;
		}

		void HandleLinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			if ((sender as LinkLabel).Tag != null && (sender as LinkLabel).Tag is String) {
				string guidtogoto = (sender as LinkLabel).Tag.ToString ();

				LayoutDetails.Instance.LoadLayout(guidtogoto);
				this.Close ();
			}
		}
	}
}

