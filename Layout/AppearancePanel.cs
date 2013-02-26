using System;
using System.Windows.Forms;
using CoreUtilities;

namespace Layout
{
	public class AppearancePanel : Panel
	{
		ToolStrip CaptionBar;
		TextBox NameField ;

		// calls into options panel to store to actual database
		Action<AppearanceClass> SaveAppearance;
		// used when editing text field to alert the AppearancePanelForm whether to enable/disable the OK button (invalid text stops saving)
		Action<bool> ValidDataToSave;

		AppearanceClass App = null;
	

		// This panel is used in read-only mode to show what a particular appearance looks like
		// It can also be used in edit mode
		public AppearancePanel (bool ShowEditButton, AppearanceClass app, Action<AppearanceClass> _SaveAppearance, Action<bool>validDataToSave, bool AllowNameEdit)
		{
			if (app == null)
				throw new Exception ("An invalid appearance was passed into AppearancePanel");
			ValidDataToSave = validDataToSave;

			App = app;
			SaveAppearance = _SaveAppearance;

			CaptionBar = new ToolStrip ();
			CaptionBar.AutoSize = false;
			ToolStripLabel CaptionLabel = new ToolStripLabel ();
			CaptionLabel.Text = Loc.Instance.GetString ("Example");
			CaptionBar.GripStyle = ToolStripGripStyle.Hidden;

			//(readonlyunlessvalidtext
			NameField = new TextBox ();
		
			NameField.Text = app.Name;
			NameField.TextChanged += HandleTextChanged;
			NameField.Dock = DockStyle.Top;
			NameField.ReadOnly = !AllowNameEdit;

			this.Controls.Add (NameField);
			this.Controls.Add (CaptionBar);


			CaptionBar.Items.Add (CaptionLabel);



			//
			// THE SETTINGS
			//

			CaptionBar.Font = app.captionFont; //[x]
			CaptionBar.Height = app.nHeaderHeight; //[x]
			//this.CaptionLabel.Bord = app.HeaderBorderStyle;
			this.BorderStyle = app.mainPanelBorderStyle; //[x]
			
			CaptionBar.BackColor = app.captionBackground; //[x]
			CaptionBar.ForeColor = app.captionForeground; //[x]
			NameField.ForeColor = app.captionForeground;
			try {
				NameField.BackColor = app.mainBackground; //[x]

			} catch (Exception) {


				NewMessage.Show ("The Main Background color is invalid. Try choosing another.");
				NameField.BackColor = this.BackColor; //[x]
			}


			if (true == ShowEditButton) {
				Button EditBut = new Button ();
				EditBut.Text = Loc.Instance.GetString ("Edit");
				EditBut.Click += HandleClick;
				this.Controls.Add (EditBut);
				EditBut.Dock = DockStyle.Bottom;

				Button AddBut = new Button();
				AddBut.Text= Loc.Instance.GetString ("New");
				AddBut.Click += HandleAddAppearanceClick1;
				AddBut.Dock = DockStyle.Bottom;
				this.Controls.Add (AddBut);


			} else {

				// when we show the EditButton that means we are in READ ONLY MODE
				// when do do not show the EditButton that means we are in Edit Mode
				
				// setup the clicks
				ToolStripButton CaptionForeColor = new ToolStripButton();
				CaptionForeColor.Text = Loc.Instance.GetString ("Forecolor");
				CaptionForeColor.Click+= HandleForeColorClick;


				ToolStripButton CaptionBackColor = new ToolStripButton();
				CaptionBackColor.Text = Loc.Instance.GetString ("Backcolor");
				CaptionBackColor.Click+= HandleBackColorClick1;

				ToolStripButton CaptionFont = new ToolStripButton();
				CaptionFont.Text= Loc.Instance.GetString ("Font");
				CaptionFont.Click+= HandleFontClick1;

				ToolStripButton OtherBackColor = new ToolStripButton();
				OtherBackColor.Text= Loc.Instance.GetString ("Secondary Back Color");
				OtherBackColor.Click+= HandleSecondaryBackColorClick1;;

				ToolStripComboBox BorderStyleSet = new ToolStripComboBox();
				BorderStyleSet.DropDownStyle = ComboBoxStyle.DropDownList;
				int count = 0;
				int match = 0;
				foreach (string s in Enum.GetNames (typeof(BorderStyle)))
				{
					BorderStyleSet.Items.Add (s);
					string name = Enum.GetName (typeof(BorderStyle),app.mainPanelBorderStyle);
					if (s == name)
					{
						match = count;
					}
					count++;
				}
				BorderStyleSet.SelectedIndex = match;
				BorderStyleSet.SelectedIndexChanged+= HandleMainBorderSelectedIndexChanged;

				NumericUpDown numbers = new NumericUpDown ();
				
				
				numbers.Value = app.nHeaderHeight;
				numbers.ValueChanged += HandleIconsPerColumnValueChanged;
				numbers.Minimum = 1;
				numbers.Maximum = 50;
				ToolStripControlHost numbersHost = new ToolStripControlHost (numbers);
				numbersHost.ToolTipText = Loc.Instance.GetString ("Adjust this number to change caption height");

				CaptionBar.Items.Add (CaptionForeColor);
				CaptionBar.Items.Add (CaptionBackColor);
				CaptionBar.Items.Add (CaptionFont);
				CaptionBar.Items.Add (OtherBackColor);
				CaptionBar.Items.Add (BorderStyleSet);
				CaptionBar.Items.Add (numbersHost);


			}
		}



		void HandleMainBorderSelectedIndexChanged (object sender, EventArgs e)
		{
			this.BorderStyle = (BorderStyle)Enum.Parse (typeof(BorderStyle), (sender as ToolStripComboBox).Text);
		}

		void HandleIconsPerColumnValueChanged (object sender, EventArgs e)
		{
			CaptionBar.Height = (int)(sender as NumericUpDown).Value;
		}

		void HandleSecondaryBackColorClick1 (object sender, EventArgs e)
		{
			ColorDialog colorPick = new ColorDialog();
			colorPick.Color = NameField.BackColor;
			if (colorPick.ShowDialog() == DialogResult.OK)
			{
				NameField.BackColor = colorPick.Color;;
			}
		}

		void HandleFontClick1 (object sender, EventArgs e)
		{
			FontDialog fontPick = new FontDialog ();
			fontPick.Font =  (CaptionBar.Font);
			if (fontPick.ShowDialog () == DialogResult.OK) {
				CaptionBar.Font= fontPick.Font;
			}
		}

		void HandleBackColorClick1 (object sender, EventArgs e)
		{
			ColorDialog colorPick = new ColorDialog();
			colorPick.Color = CaptionBar.BackColor;
			if (colorPick.ShowDialog() == DialogResult.OK)
			{
				CaptionBar.BackColor = colorPick.Color;
			}
		}

		void HandleForeColorClick (object sender, EventArgs e)
		{
			ColorDialog colorPick = new ColorDialog();
			colorPick.Color = CaptionBar.ForeColor;
			if (colorPick.ShowDialog() == DialogResult.OK)
			{
				CaptionBar.ForeColor = colorPick.Color;
				NameField.ForeColor =  colorPick.Color;
			}
		}
		void HandleTextChanged (object sender, EventArgs e)
		{
			if ((sender as TextBox).Text != Constants.BLANK) {
				ValidDataToSave(true);
			}
			else
				ValidDataToSave(false);
		}

		public AppearanceClass GetAppearanceSelected ()
		{
			// This is caused only from the FORM version of this control, see below, the HandleClick routine
			AppearanceClass ThisAppearance = new  AppearanceClass();

			// grab name from TextField (which will remain in readonly mode WHEN editing existing but is rightable
			ThisAppearance.Name = NameField.Text;


			ThisAppearance.captionFont=CaptionBar.Font;
			ThisAppearance.nHeaderHeight=CaptionBar.Height;
			ThisAppearance.mainPanelBorderStyle=this.BorderStyle;
			ThisAppearance.captionBackground=CaptionBar.BackColor ;
			ThisAppearance.captionForeground=CaptionBar.ForeColor;

			ThisAppearance.mainBackground = NameField.BackColor;
				//this.CaptionLabel.Bord = app.HeaderBorderStyle;  //6
				


			return ThisAppearance;
		}


		void HandleClick (object sender, EventArgs e)
		{
			if (null == SaveAppearance) {
				throw new Exception ("No proper save method for appearances has been initalized");
			}
			AppearancePanelForm form = new AppearancePanelForm (App);
			if (DialogResult.OK ==form.ShowDialog() ) {
				AppearanceClass ThisAppearance = form.GetAppearance();


				SaveAppearance (ThisAppearance);
			}
		}
		void HandleAddAppearanceClick1 (object sender, EventArgs e)
		{
			if (null == SaveAppearance) {
				throw new Exception ("No proper save method for appearances has been initalized");
			}

			// Add options
			// We build off the selected Appearance Type but will rename it [No. We do not. Don't want to mess up an existing object]
		
			AppearancePanelForm form = new AppearancePanelForm (null);
			if (DialogResult.OK ==form.ShowDialog() ) {
				AppearanceClass ThisAppearance = form.GetAppearance();
				
				
				SaveAppearance (ThisAppearance);
			}
		}
	}
}

