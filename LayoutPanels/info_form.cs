using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using CoreUtilities;

namespace Layout
{
	public class info_form : Form
	{
		TableLayoutPanel layPanel;
		string GUID= Constants.BLANK;

		ListBox ListOfEvents = null;

		public info_form (string infoText, string _GUID)
		{

			//this.Font = new Font(this.Font.FontFamily, 18.0f);

			if (Constants.BLANK == _GUID) {
				throw new Exception("GUID cannot be blank in info_form");
			}
			GUID = _GUID;
			RichTextBox infohead = new RichTextBox();
			infohead.Text = infoText;
			infohead.ReadOnly = true;
			infohead.Dock = DockStyle.Top;

			Button refresh = new Button();
			refresh.Text = Loc.Instance.GetString("Update Reciprocal Links");
			refresh.Dock = DockStyle.Top;
			refresh.Click+= HandleRefreshClick;

			layPanel = new TableLayoutPanel();
			layPanel.Dock = DockStyle.Fill;


			ListOfEvents = new ListBox();
			ListOfEvents.Dock = DockStyle.Bottom;


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
		}

		void HandleRefreshClick (object sender, EventArgs e)
		{

			this.Cursor = Cursors.WaitCursor;
			List<string> connections = MasterOfLayouts.ReciprocalLinks (GUID);

			layPanel.ColumnCount = 2;
			layPanel.Controls.Clear ();

			foreach (string s in connections) {
				string[] data = s.Split (new char[1]{'.'});
				if (data != null && data.Length == 2) {
					LinkLabel link = new LinkLabel ();
					link.Tag = data [1];
					link.Text = data [0];
				
					link.LinkBehavior = LinkBehavior.AlwaysUnderline;
					link.LinkClicked += HandleLinkClicked;
					layPanel.Controls.Add (link);
				}
			}



		
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

