using System;
using System.Windows.Forms;
using CoreUtilities;
using System.Drawing;
namespace HotKeys
{
	public class KeyEditForm : Form
	{
		// returned when closed
		public KeyData keyOut;
		private ComboBox keyKey ;
		private ComboBox keyModifier;
		public KeyEditForm (KeyData keyIn, Icon formIcon)
		{
			this.Icon = formIcon;
			this.FormClosing += HandleFormClosing;

			Label labelModifier = new Label ();
			labelModifier.Text = Loc.Instance.GetString ("Modifier");
			labelModifier.Dock = DockStyle.Top;

			Label labelKey = new Label ();
			labelKey.Text = Loc.Instance.GetString ("Key");
			labelKey.Dock = DockStyle.Top;

			 keyModifier = new ComboBox ();
			keyModifier.DropDownStyle = ComboBoxStyle.DropDownList;
			keyModifier.Dock = DockStyle.Top;

		





			 keyKey = new ComboBox();


			keyKey.Dock = DockStyle.Top;
		
			keyKey.DropDownStyle = ComboBoxStyle.DropDownList;




			Array arrKeys = Enum.GetValues (typeof(Keys));
			
			foreach (object key in arrKeys) {
				keyModifier.Items.Add (key.ToString ());
				keyKey.Items.Add (key.ToString ());
			}
			keyModifier.Text = keyIn.ModifyingKey.ToString ();
			keyKey.Text = keyIn.Key.ToString ();

			Panel bottom = new Panel();
			bottom.Height = 40;
			bottom.Dock = DockStyle.Bottom;

			Button OK = new Button();
			OK.Text = Loc.Instance.GetString("OK");

			OK.Dock = DockStyle.Right;
			OK.DialogResult = DialogResult.OK;
			
			
			Button Cancel  = new Button();
			Cancel.Text = Loc.Instance.GetString ("Cancel");
			Cancel.Padding = new System.Windows.Forms.Padding(10);
			Cancel.DialogResult = DialogResult.Cancel;
			Cancel.Dock = DockStyle.Right;

			bottom.Controls.Add (OK);
			bottom.Controls.Add (Cancel);


			this.Controls.Add (bottom);


			this.Controls.Add (keyKey);
			this.Controls.Add (labelKey);

			this.Controls.Add (keyModifier);
			this.Controls.Add (labelModifier);


			keyOut = keyIn;
		}

		void HandleFormClosing (object sender, FormClosingEventArgs e)
		{
			keyOut.Key = (Keys)Enum.Parse (typeof(Keys), keyKey.Text);
			keyOut.ModifyingKey = (Keys)Enum.Parse (typeof(Keys), keyModifier.Text);
		}
	}
}

