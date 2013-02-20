using System;
using System.Windows.Forms;
using CoreUtilities;

namespace Layout
{
	public class AppearancePanelForm : Form
	{
		AppearancePanel appearancePanel = null;
		Button ok = null;
		public AppearancePanelForm (Appearance app)
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
				app = Appearance.SetAsClassic();

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
		public Appearance GetAppearance ()
		{
			return appearancePanel.GetAppearanceSelected();
		}
	}
}

