// Options.cs
//
// Copyright (c) 2013 Brent Knowles (http://www.brentknowles.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Review documentation at http://www.yourothermind.com for updated implementation notes, license updates
// or other general information/
// 
// Author information available at http://www.brentknowles.com or http://www.amazon.com/Brent-Knowles/e/B0035WW7OW
// Full source code: https://github.com/BrentKnowles/YourOtherMind
//###
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

		Label WordSystem;
		string DatabaseName;
		Panel configPanel;
		#endregion

		const string columnID="id";
		const string columnKey="key";
		const string columnValue="value";
		const string TableName = "generalsettings";

		#region variables

	//	public bool dualScreens_TallestHeight=false; 
	//	public bool autoSave=false;


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
		checkBoxOptions[] booleanValues = new checkBoxOptions[4] {
			new checkBoxOptions(Loc.Instance.GetString("Autosave?"), "autosave", "tt", true),
			new checkBoxOptions(Loc.Instance.GetString("Multiple Screens - Set Height to Highest?"), "multiscreenhigh", "tt", false),
			new checkBoxOptions(Loc.Instance.GetString ("Beta Updates (allows updating to beta versions)"), "betaversions","tt", false),
			new checkBoxOptions(Loc.Instance.GetString ("Increase Memory Usage (more layouts open at one time)"), "memory_high","tt", false)
		};

		string dataPath = CoreUtilities.Constants.BLANK;
		#endregion

		public Options (string _DatabaseName)
		{

			this.DatabaseName = _DatabaseName;



		}

		public void Dispose ()
		{
		}
		public bool Betaupdates {
			get {
				checkBoxOptions defaultValue = Array.Find (booleanValues, checkBoxOptions => checkBoxOptions.columnKey == "betaversions");
				return GetOption ("betaversions", defaultValue.defaultValue);
			}
		}

		public bool Autosave {
			get {
				checkBoxOptions defaultValue = Array.Find (booleanValues,checkBoxOptions => checkBoxOptions.columnKey == "autosave");
				return GetOption ("autosave", defaultValue.defaultValue);
			}
		}

		public bool MultipleScreenHigh {
			get {
				checkBoxOptions defaultValue = Array.Find (booleanValues,checkBoxOptions => checkBoxOptions.columnKey == "multiscreenhigh");
				return GetOption ("multiscreenhigh", defaultValue.defaultValue);
			}
		}
		public bool HighMemory {
			get {
				checkBoxOptions defaultValue = Array.Find (booleanValues,checkBoxOptions => checkBoxOptions.columnKey == "memory_high");
				return GetOption ("memory_high", defaultValue.defaultValue);
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

			// we need to revise this if plugins have been adding
			if (null != WordSystem) {
				UpdateCurrentWordSystem(WordSystem);
			}

			// if panel made then leave it alone
			if (PanelWasMade == true)
				return configPanel;
		
			PanelWasMade = true;
			
			configPanel = new Panel ();




			foreach (checkBoxOptions option in booleanValues) {
				lg.Instance.Line ("Options.GetConfigPane", ProblemType.TEMPORARY, option.labelName);
				CheckBox autoSave = new CheckBox ();
				autoSave.Name = option.columnKey;
				autoSave.Text = option.labelName;


			

				bool result = GetOption (option.columnKey, option.defaultValue);


				autoSave.Checked = result;

				configPanel.Controls.Add (autoSave);
				autoSave.Dock = DockStyle.Top;
			}

			GroupBox Info = new GroupBox();
			Info.Text = Loc.Instance.GetString ("Info");
			//Info.Font = new System.Drawing.Font(Info.Font.FontFamily, Info.Font.Size, System.Drawing.FontStyle.Bold);
			Info.Dock = DockStyle.Bottom;
			WordSystem = new Label ();
			WordSystem.AutoSize = false;
			WordSystem.Height = 200;

			UpdateCurrentWordSystem (WordSystem);
			Info.Controls.Add (WordSystem);

			WordSystem.Dock = DockStyle.Top;
			configPanel.Controls.Add (Info);

			return configPanel;

		}

		static void UpdateCurrentWordSystem (Label WordSystem)
		{
			if (Layout.LayoutDetails.Instance.WordSystemInUse == null) {
				// when a plugin unloads it needs to set the layoutsystem to the default
				WordSystem.Text = Loc.Instance.GetString ("No Word System Was Assigned. This is an error. Contact developer.");
			}
			else {
				WordSystem.Text = Loc.Instance.GetStringFmt ("Using: {0}. (NOTE: AddIns can deploy new word systems).", Layout.LayoutDetails.Instance.WordSystemInUse.ToString ());
			}
		}

		private BaseDatabase CreateDatabase()
		{
			//BaseDatabase db = new SqlLiteDatabase (DatabaseName);
			BaseDatabase db = Layout.MasterOfLayouts.GetDatabaseType(DatabaseName);
			
			

			db.CreateTableIfDoesNotExist (TableName, new string[3] 
			                              {columnID, columnKey, columnValue}, 
			new string[3] {
				"INTEGER",
				"TEXT",
				"TEXT"
					
			}, columnID
			);
			return db;
		}
		public void SaveRequested ()
		{
			if (false == PanelWasMade)
				return;

			if (configPanel == null) {
				throw new Exception ("no config panel defined");
			}

		

			BaseDatabase db = CreateDatabase ();
//			string result = (db as SqlLiteDatabase).BackupTable("",TableName ,null); 
//			NewMessage.Show (result);
			//return;

			foreach (checkBoxOptions option in booleanValues) {

				CheckBox checker = (CheckBox)configPanel.Controls.Find (option.columnKey, true)[0];

				if (checker != null) {


					// foreach GUIelement
			
					string key = option.columnKey;
					bool booleanvalue = checker.Checked;
					try
					{
					if (!db.Exists (TableName, columnKey, key)) {

							// if we were updating multiple AND we have to add a new key, we need to terminate the multiple update
							if (db.IsInMultipleUpdateMode() == true) db.UpdateMultiple_End();

						db.InsertData (TableName, new string[2]{columnKey, columnValue}, new object[2] {key, booleanvalue});
					}
					else
					{
						if (db.IsInMultipleUpdateMode() == false) db.UpdateMultiple_Start ();
						db.UpdateDataMultiple (TableName, new string[1] {columnValue}, new object[1] {booleanvalue}, columnKey, key);
					}
					}
					catch (Exception ex)
					{
						NewMessage.Show ("Are we in the middle of updating one value while inserting a new value? " + ex.ToString());
					}


				}

			}
			if (db.IsInMultipleUpdateMode() == true) db.UpdateMultiple_End();

			db.Dispose();



		}
	}
}

