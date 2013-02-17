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

			//TODO:  set this to FALSE in ADD mode but assume valid in Edit Mode
			ok.Enabled = true;



			ok.Text = Loc.Instance.GetString ("OK");
			ok.DialogResult = DialogResult.OK;
			ok.Dock = DockStyle.Left;


			Button Cancel = new Button();
			Cancel.Text = Loc.Instance.GetString ("Cancel");
			Cancel.Dock = DockStyle.Right;
			Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

			bottom.Controls.Add (ok);
			bottom.Controls.Add(Cancel);


			appearancePanel = new AppearancePanel(false, app, null, ValidData);

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

