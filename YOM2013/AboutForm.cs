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
			this.Width = 400;
			this.Height = 400;
			this.StartPosition = FormStartPosition.CenterParent;
			Label mainLabel = new Label();
			mainLabel.Dock = DockStyle.Top;
			mainLabel.Text = Loc.Instance.GetStringFmt ("YourOtherMind Copyright {0}-{1}.",1999,2013);
			Label version = new Label();
			version.Text = Loc.Instance.GetStringFmt("Version {0}", Application.ProductVersion );
			version.Dock = DockStyle.Top;

			Upper.Controls.Add (version);
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

