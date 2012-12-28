using System;
using appframe;
using CoreUtilities;
using System.Windows.Forms;
using database;
using System.Collections.Generic;

namespace YOM2013
{
	public class Options : iConfig, IDisposable
	{



		#region variables_private


		string DatabaseName;
		Panel configPanel;
		#endregion

		const string columnID="id";
		const string columnKey="key";
		const string columnValue="value";
		const string TableName = "generalsettings";

		#region variables

		public bool dualScreens_TallestHeight=false; 
		public bool autoSave=false;


		// if true on save we know it is safe to try to save (because interface exists)
		bool PanelWasMade = false;
		public string ConfigName {
			get { return Loc.Instance.GetString ("General");}
		}

		// a feeder array to setup the buttons. one array for each 'type' of config data
struct checkBoxOptions 
{
			public string labelName;
			// the identifier for this field in the database under the "key" column
			public string columnKey;
			public bool defaultValue;
			public string toolTip;
			public checkBoxOptions(string labelname, string columnkey, string tooltip, bool defaultvalue)
			{
				labelName = labelname;
				columnKey = columnkey;
				toolTip = tooltip;
				defaultValue =defaultvalue;
			}
}
		checkBoxOptions[] booleanValues = new checkBoxOptions[2] {
			new checkBoxOptions(Loc.Instance.GetString("Autosave?"), "autosave", "tt", false),
			new checkBoxOptions(Loc.Instance.GetString("Multiple Screens - Set Height to Highest?"), "multiscreenhigh", "tt", false)};

		string dataPath = CoreUtilities.Constants.BLANK;
		#endregion

		public Options (string _DatabaseName)
		{

			this.DatabaseName = _DatabaseName;



		}

		public void Dispose ()
		{
		}

		public bool MultipleScreenHigh {
			get {
				checkBoxOptions defaultValue = Array.Find (booleanValues,checkBoxOptions => checkBoxOptions.columnKey == "multiscreenhigh");
				return GetOption ("multiscreenhigh", defaultValue.defaultValue);
			}
		}

		private bool GetOption (string option, bool defaultValue)
		{
			BaseDatabase db = CreateDatabase ();
			bool result = defaultValue;
			if (db.Exists (TableName, columnKey, option))
			{
				object o = db.GetValues(TableName, new string[1] {columnValue}, columnKey, option)[0][0];
				if (o.ToString() == "1") result = true; else result = false;
				lg.Instance.Line("Options->GetConfigPanel", ProblemType.TEMPORARY, o.ToString());
				
			}
			db.Dispose();
			return result;
			// where option matches 

			// where option matches the columnKey of the array (for default values)
		}

		public Panel GetConfigPanel ()
		{
			// if panel made then leave it alone
			if (PanelWasMade == true) return configPanel;
		
			PanelWasMade = true;
			
			configPanel = new Panel ();




			foreach (checkBoxOptions option in booleanValues) {
				lg.Instance.Line("Options.GetConfigPane", ProblemType.TEMPORARY, option.labelName);
				CheckBox autoSave = new CheckBox ();
				autoSave.Name = option.columnKey;
				autoSave.Text = option.labelName;


			

				bool result = GetOption (option.columnKey,option.defaultValue);


				autoSave.Checked = result;

				configPanel.Controls.Add (autoSave);
				autoSave.Dock = DockStyle.Top;
			}







			return configPanel;

		}

		private BaseDatabase CreateDatabase()
		{
			BaseDatabase db = new SqlLiteDatabase (DatabaseName);
			
			
			

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
		public void SaveRequested ()
		{


			if (configPanel == null) {
				throw new Exception ("no config panel defined");
			}

			if (false == PanelWasMade)
				return;

			BaseDatabase db = CreateDatabase ();


			foreach (checkBoxOptions option in booleanValues) {

				CheckBox checker = (CheckBox)configPanel.Controls.Find (option.columnKey, true)[0];

				if (checker != null) {


					// foreach GUIelement
			
					string key = option.columnKey;
					bool booleanvalue = checker.Checked;
					if (!db.Exists (TableName, columnKey, key)) {
						db.InsertData (TableName, new string[2]{columnKey, columnValue}, new object[2] {key, booleanvalue});
					}
					else
					{
						if (db.IsInMultipleUpdateMode() == false) db.UpdateMultiple_Start ();
						db.UpdateDataMultiple (TableName, new string[1] {columnValue}, new object[1] {booleanvalue}, columnKey, key);
					}


				}

			}
			if (db.IsInMultipleUpdateMode() == true) db.UpdateMultiple_End();

			db.Dispose();



		}
	}
}
