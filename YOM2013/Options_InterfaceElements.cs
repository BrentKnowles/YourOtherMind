using System;
using CoreUtilities;
using appframe;
using Layout;
using System.Windows.Forms;
using database;

namespace YOM2013
{
	public class Options_InterfaceElements : iConfig, IDisposable
	{

		#region variables_private
		
		
		string DatabaseName;
		Panel configPanel;
#endregion
		
		const string columnID="id";
		const string columnKey="key";
		const string columnValue="value";
		const string TableName = "interfacesettings";

		const string KEY_formsize="formsize";
		const string KEY_markup="markup";
		//formsize blah
		#region variables
		
		#endregion
		#region gui
		ComboBox TextSizeCombo ;
		ComboBox MarkupCombo;
		#endregion
		
		// if true on save we know it is safe to try to save (because interface exists)
		bool PanelWasMade = false;
		public Options_InterfaceElements (string _DatabaseName)
		{

			this.DatabaseName = _DatabaseName;

			
			
			
		}
		public void Dispose ()
		{
		}

		public string ConfigName {
			get { return Loc.Instance.GetString ("Interface");}
		}

		public iMarkupLanguage SelectedMarkup {
			get{ 
				iMarkupLanguage returnvalue = null;
				BaseDatabase db = CreateDatabase ();
				if (db.Exists (TableName, columnKey, KEY_markup)) {
					string value = db.GetValues(TableName, new string[1] {columnValue}, columnKey, KEY_markup)[0][0].ToString();
					if (value != null)
					{
						// we need to create this once but LayoutDetails will store it inot until it changes again so 
						// this won't be a slow operatin, just done on load and if changed

					
						returnvalue = LayoutDetails.Instance.GetMarkupMatch(value);

					
					}
				}
				if (null == returnvalue)
				{
					// we revert to none if plugin has been removed and we store this value too
					returnvalue = new MarkupLanguageNone();
					Store (db, KEY_markup, returnvalue.GetType ().AssemblyQualifiedName.ToString ());
				}
				db.Dispose();

				return returnvalue;}
		}
	
		// public acesssor
		public CoreUtilities.FormUtils.FontSize FontSizeForForm {
			get {
				FormUtils.FontSize value_as = FormUtils.FontSize.Normal;
				BaseDatabase db = CreateDatabase ();
				if (db.Exists (TableName, columnKey, KEY_formsize)) {
					string value = db.GetValues(TableName, new string[1] {columnValue}, columnKey, KEY_formsize)[0][0].ToString();
					if (value != null)
					{
					Enum.TryParse(value, out value_as);
					}
				}
				db.Dispose();
				return value_as;}
		}

		public Panel GetConfigPanel ()
		{
			// if panel made then leave it alone
			if (PanelWasMade == true)
				return configPanel;
			
			PanelWasMade = true;
			
			configPanel = new Panel ();

			
			Label label = new Label ();
			label.AutoSize = true;
			label.Text = "Should maybe inherit some methods from Options.cs. Will have option to Reset System Layout";

			Button buttonResetSystem = new Button ();
			buttonResetSystem.Text = Loc.Instance.GetString ("Reset System Layout");
			buttonResetSystem.Click += HandleResetSysteClick;
			buttonResetSystem.Dock = DockStyle.Top;

			// Text Size

			Panel TextSizePanel = new Panel ();
		
			Label TextSizeLabel = new Label ();
			TextSizeLabel.Dock = DockStyle.Top;

			TextSizeLabel.Text = Loc.Instance.GetString ("Text Size");


			TextSizeCombo = new ComboBox ();

			TextSizeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			//TextSizeCombo.DataSource = Enum.GetValues (typeof(FormUtils.FontSize));
			foreach (FormUtils.FontSize f in Enum.GetValues (typeof(FormUtils.FontSize))) {
				TextSizeCombo.Items.Add (f);
			}


			// load TextSizeFromDatabase
			TextSizeCombo.SelectedItem = FontSizeForForm;



			TextSizePanel.Controls.Add (TextSizeCombo);
			TextSizePanel.Controls.Add (TextSizeLabel);

			TextSizeCombo.Dock = DockStyle.Top;
			TextSizePanel.Dock = DockStyle.Top;

			//
			//
			// MarkupLanguage
			//
			//
			Panel MarkupPanel = new Panel ();

			Label MarkupPanelLabel = new Label ();

			MarkupPanelLabel.Text = Loc.Instance.GetString ("Markup To Use With Text");

			MarkupCombo = new ComboBox ();

			MarkupCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			MarkupCombo.DropDown+= HandleDropDown;
			// we need items to prepopulate this and we refill later too
			LayoutDetails.Instance.BuildMarkupComboBox (MarkupCombo);

			MarkupPanel.Controls.Add (MarkupCombo);
			MarkupPanel.Controls.Add (MarkupPanelLabel);

			MarkupPanel.Dock = DockStyle.Top;
			MarkupPanelLabel.Dock = DockStyle.Top;
			MarkupCombo.Dock = DockStyle.Top;


			// set markup
			//MarkupCombo.SelectedItem = SelectedMarkup;
			if (SelectedMarkup != null) {
				for (int i = 0; i < MarkupCombo.Items.Count; i++) {
					if (MarkupCombo.Items [i].GetType () == SelectedMarkup.GetType ()) {
						MarkupCombo.SelectedIndex = i;
						break;
					}
				}
			}
			configPanel.Controls.Add (label);
			configPanel.Controls.Add (buttonResetSystem);
			configPanel.Controls.Add (TextSizePanel);
			configPanel.Controls.Add (MarkupPanel);


			label.Dock = DockStyle.Top;

			
			
		
			return configPanel;
			
		}

		void HandleDropDown (object sender, EventArgs e)
		{
			MarkupCombo.Items.Clear ();
			LayoutDetails.Instance.BuildMarkupComboBox (MarkupCombo);
		}

		void HandleResetSysteClick (object sender, EventArgs e)
		{
			
			if (NewMessage.Show (Loc.Instance.GetString ("Caution!"), Loc.Instance.GetString ("Reset the system layout to restore default settings. Doing so will lose any customization to the sytem layout AND require a restart of the application. Proceed?"),
			                    MessageBoxButtons.YesNo, null) == DialogResult.Yes) {
				// do reset
				// 1. Delete existing system note
				MasterOfLayouts.DeleteLayout("system");
				MasterOfLayouts.DeleteLayout("system_sidedock");
				MasterOfLayouts.DeleteLayout("tables");

				// JUST DELETE AND REBOOT, it will build on next reload
				// 2. Restore note
				// because we reboot it does not matter that I do not pass a valid contextmenu into here
				//DefaultLayouts.CreateASystemLayout(null,null);
				// 3. Restart
				Application.Exit ();
			}

		}

		private  BaseDatabase CreateDatabase()
		{
			//BaseDatabase db = new SqlLiteDatabase (DatabaseName);
			BaseDatabase db = Layout.MasterOfLayouts.GetDatabaseType(DatabaseName);
			
			
			
			db.CreateTableIfDoesNotExist (TableName, new string[3] 
			                              {columnID, columnKey, columnValue}, 
			new string[3] {
				"INTEGER",
				"TEXT",
				"TEXT"
					
			}, "id"
			);
			return db;
		}

		void Store (BaseDatabase db, string key, string text)
		{
			if (!db.Exists (TableName, columnKey, key)) {
				db.InsertData (TableName, new string[2]{columnKey, columnValue}, new object[2] {key, text});
			} else {
				db.UpdateSpecificColumnData(TableName, new string[2]{columnKey, columnValue}, new object[2] {key, text}, columnKey, key);
			}
		}

		public void SaveRequested ()
		{
			
			if (false == PanelWasMade)
				return;


			if (configPanel == null) {
				throw new Exception ("no config panel defined");
			}
			
			BaseDatabase db = CreateDatabase ();

			Store (db, KEY_formsize, TextSizeCombo.Text);
			if (MarkupCombo.SelectedItem != null) {
				Store (db, KEY_markup, MarkupCombo.SelectedItem.GetType ().AssemblyQualifiedName.ToString ());
			}


			
			db.Dispose();

		}
	}
}

