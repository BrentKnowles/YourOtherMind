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
		public OptionForm (List<iConfig> optionPanels)
		{
			this.Icon = MainFormBase.MainFormIcon;
			this.Width = 400;
			this.Height = 400;

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
			Cancel.Padding = new System.Windows.Forms.Padding(10);
			Cancel.Dock = DockStyle.Right;
			Cancel.DialogResult = DialogResult.Cancel;


			 container = new SplitContainer ();
			container.Parent = this;
			container.Dock = DockStyle.Fill;
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
			// TODO: Call save here?
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

