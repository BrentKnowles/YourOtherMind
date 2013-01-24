using System;
using System.Windows.Forms;
using System.Drawing;
using CoreUtilities;
namespace appframe
{
	public class VisualKey : Panel
	{

		// ConfigPanel will test this to see if the user has edited this key
		public bool IsModified = false;
		private KeyData currentKey;
		Icon FormIcon;
		Label modifier ;
		public KeyData keyOut = null;
		Action<string>AfterKeyEdit=null;
		Color oldBackColor;
		void BuildKeyString (KeyData keyToBuild)
		{
			modifier.Text = Loc.Instance.GetStringFmt ("{0} ({1} + {2})", keyToBuild.Label, keyToBuild.ModifyingKey.ToString (), keyToBuild.Key.ToString ());
		}
		public string UniqueCode ()
		{
			return currentKey.Key.ToString () + currentKey.ModifyingKey.ToString ();
		}
		public VisualKey (KeyData keyToBuild, Icon formIcon, Action<string>afterKeyEdit)
		{
			oldBackColor = this.BackColor;
			if (null == keyToBuild) {
				throw new Exception("must pass in a valid key object");
			}
			FormIcon = formIcon;

			modifier = new Label();
			BuildKeyString (keyToBuild);
			modifier.Dock = DockStyle.Bottom;




			Button edit = new Button();
			edit.Text = Loc.Instance.GetString ("Edit");
			edit.Dock = DockStyle.Bottom;

			this.Controls.Add (modifier);
			this.Controls.Add (edit);
		


			                                  

			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.AutoSize = true;
			edit.Click+= HandleKeyChangeClick;
			AfterKeyEdit = afterKeyEdit;

			currentKey = keyToBuild;
		
		}

		public void IsDuplicate (bool b)
		{
			if (true == b) {
				this.BackColor = Color.Red;
			}
			else
				this.BackColor =oldBackColor;
		}

		void HandleKeyChangeClick (object sender, EventArgs e)
		{
			KeyEditForm keyEdit = new KeyEditForm (this.currentKey, FormIcon);
			if (keyEdit.ShowDialog () == DialogResult.OK) {
				//NewMessage.Show (keyEdit.keyOut.Key.ToString ()+ keyEdit.keyOut.ModifyingKey.ToString());
				IsModified = true;
				keyOut = keyEdit.keyOut;

				BuildKeyString (keyOut);

			}
			// do this regardless, we are just updating interface, not data
			if (AfterKeyEdit != null)
			{
				// we just redraw the CURRENT one
				AfterKeyEdit(keyOut.GetGUID ());
			}
		}
	}
}

