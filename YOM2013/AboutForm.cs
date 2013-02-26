using System;
using Layout;
using System.Windows.Forms;
using CoreUtilities;

namespace YOM2013
{
	public class AboutForm : Form
	{
		public AboutForm () 
		{
			this.Icon = LayoutDetails.Instance.MainFormIcon;
			FormUtils.SizeFormsForAccessibility(this, LayoutDetails.Instance.MainFormFontSize);
			Panel Upper = new Panel();
			Upper.Dock = DockStyle.Fill;

			Label mainLabel = new Label();
			mainLabel.Dock = DockStyle.Top;
			mainLabel.Text = Loc.Instance.GetString ("YourOtherMind Copyright 1999-2013");
			Upper.Controls.Add (mainLabel);


			Button ok = new Button();
			ok.DialogResult = DialogResult.OK;
			ok.Text = Loc.Instance.GetString ("OK");
			ok.Dock = DockStyle.Bottom;


			this.Controls.Add(ok);
			this.Controls.Add(Upper);

		}
	}
}

