// Options_InterfaceElements.cs
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
using CoreUtilities;
using appframe;
using Layout;
using System.Windows.Forms;
using database;
using System.Xml.Serialization;
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
		const string columnType="type"; // 0 = normal, 1 = appearance
		const string TableName = "interfacesettings";

		const string KEY_formsize="formsize";
		const string KEY_markup="markup";
		//formsize blah
		#region variables
		
		#endregion
		#region gui
		ComboBox TextSizeCombo ;
		ComboBox MarkupCombo;
		GroupBox AppearanceGroup ;
		ListBox Appearances;
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
					Store (db, KEY_markup, returnvalue.GetType ().AssemblyQualifiedName.ToString (),0);
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
		private void TestExistence (BaseDatabase db, string key, Func<Layout.AppearanceClass> Setup)
		{
			if (!db.Exists (TableName, columnKey, key)) {
				Layout.AppearanceClass default1 = Setup();
				//Setup();
				SaveAppearance (default1);
			}
		}
		/// <summary>
		/// Builds the default note appearance if needed.
		/// </summary>
		public void BuildDefaultNoteAppearanceIfNeeded ()
		{
			BaseDatabase db = CreateDatabase ();

			TestExistence (db, "classic", Layout.AppearanceClass.SetAsClassic);
			TestExistence (db, "fantasy", Layout.AppearanceClass.SetAsFantasy);
			TestExistence (db, "scifi", Layout.AppearanceClass.SetAsSciFI);
			TestExistence (db, "research", Layout.AppearanceClass.SetAsResearch);

			TestExistence (db, "blue", Layout.AppearanceClass.SetAsBlue);
			TestExistence (db, "modern", Layout.AppearanceClass.SetAsModern);
			TestExistence (db, "note", Layout.AppearanceClass.SetAsNote);
			TestExistence (db, "programmer", Layout.AppearanceClass.SetAsProgrammer);

//			// if appearance 1  does not exist, create and call SaveAppearance
//			if (!db.Exists (TableName, columnKey, "classic")) {
//				Layout.Appearance default1 = new Layout.Appearance ();
//				default1.SetAsClassic ();
//				SaveAppearance (default1);
//			}
			
			// if appearance 2 does not exist, create and call SaveAppearance
//			if (!db.Exists (TableName, columnKey, "fantasy")) {
//				Layout.Appearance default1 = new Layout.Appearance ();
//				default1.SetAsFantasy ();
//				SaveAppearance (default1);
//			}
			
			db.Dispose();
		}
		/// <summary>
		/// Gets the appearance by key. Called from the main form in respond to a callback from LayoutDetails. Will also be used internall
		/// </summary>
		/// <returns>
		/// The appearance by key.
		/// </returns>
		/// <param name='Key'>
		/// Key.
		/// </param>
		public Layout.AppearanceClass GetAppearanceByKey(string Key)
		{
			Layout.AppearanceClass app = null;
			BaseDatabase db = CreateDatabase();
			if (db.Exists (TableName, columnKey, Key))
			{
				// put from database
				System.Collections.Generic.List<object[]> values = db.GetValues (TableName, new string[1] {columnValue}, columnKey,Key);
				if (values != null && values.Count > 0) {
					if (values[0] != null && values[0].Length > 0)
					{
						string xml = values[0][0].ToString ();

						// now deserialize the object

						XmlSerializer serializer = new XmlSerializer (typeof(Layout.AppearanceClass));
						
						System.IO.StringReader reader = new System.IO.StringReader(xml);
						app = (Layout.AppearanceClass)serializer.Deserialize (reader);



					}
				}

			}
			
			return app;
		}
		/// <summary>
		/// Saves the appearance. Triggered from callback in AppearancePanel itself, passing itself back to be stored in the database.
		/// </summary>
		/// <param name='obj'>
		/// Object.
		/// </param>
		void SaveAppearance (Layout.AppearanceClass obj)
		{
			if (null == obj)
				throw new Exception ("A null appearance was passed into save routine.");
			if (obj.Name == Constants.BLANK)
				throw new Exception ("A name must be assigned to any new Appearance that is created!");
			BaseDatabase db = CreateDatabase ();
			
			Store (db, obj.Name, obj.GetAppearanceXML (), 1);
			db.Dispose ();

			// the moment we save an appearance I think we need to PURGE the Cahce in LayoutDetails
			// so that the NEXT time a page is loaded, it benefits from the new appearnces (likewise if a new note is created).
			LayoutDetails.Instance.PurgeAppearanceCache ();
			if (null != Appearances) {
				// deselect on the list
				Appearances.SelectedIndex = -1;

				// we rebuild the list in case we added a new one
				BuildAppearanceListBox (Appearances);
			}

		}
		void BuildAppearanceListBox (ListBox appearances)
		{
			appearances.Items.Clear ();
			BaseDatabase db = CreateDatabase ();
			//System.Collections.Generic.List<object[]> values = db.GetValues (TableName, new string[1] {columnKey}, BaseDatabase.GetValues_ANY, BaseDatabase.GetValues_WILDCARD);
			System.Collections.Generic.List<object[]> values = db.GetValues (TableName, new string[1] {columnKey}, columnType, 1);
			if (values != null) {
				foreach (object[] o in values) {
					if (o != null && o.Length > 0)
					if (o [0].ToString () != Constants.BLANK) {
						appearances.Items.Add (o [0].ToString ());
					}
				}
			}
			db.Dispose ();
		}

		public System.Collections.Generic.List<string> GetListOfAppearances ()
		{
			System.Collections.Generic.List<string>  list = new System.Collections.Generic.List<string>();
			BaseDatabase db = CreateDatabase ();
			//System.Collections.Generic.List<object[]> values = db.GetValues (TableName, new string[1] {columnKey}, BaseDatabase.GetValues_ANY, BaseDatabase.GetValues_WILDCARD);
			System.Collections.Generic.List<object[]> values = db.GetValues (TableName, new string[1] {columnKey}, columnType, 1);
			if (values != null) {
				foreach (object[] o in values) {
					if (o != null && o.Length > 0)
					if (o [0].ToString () != Constants.BLANK) {
						list.Add (o [0].ToString ());
					}
				}
			}
			db.Dispose ();
			return list;
		}

		public Panel GetConfigPanel ()
		{
			// if panel made then leave it alone
			if (PanelWasMade == true)
				return configPanel;
			
			PanelWasMade = true;
			
			configPanel = new Panel ();

			
		

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

			//
			//
			// Appearance Group
			//
			//

			AppearanceGroup = new GroupBox();
			AppearanceGroup.Height = 150;
			AppearanceGroup.Text = Loc.Instance.GetString ("Note Appearances");
			AppearanceGroup.Dock = DockStyle.Top;
			 Appearances = new ListBox();
			Appearances.Dock = DockStyle.Left;


			// get values from list and put into db
			BuildAppearanceListBox(Appearances);
			Appearances.SelectedIndexChanged+= HandleAppearanceSelectedIndexChanged;
			


			AppearanceGroup.Controls.Add (Appearances);




			configPanel.Controls.Add (buttonResetSystem);
			configPanel.Controls.Add (TextSizePanel);
			configPanel.Controls.Add (MarkupPanel);
			configPanel.Controls.Add (AppearanceGroup);



			
			
		
			return configPanel;
			
		}
		AppearancePanel lastAppPanel = null;
		void HandleAppearanceSelectedIndexChanged (object sender, EventArgs e)
		{
			// we always get rid of the control, when deselecting the list item too.

			if (null != lastAppPanel) {
				AppearanceGroup.Controls.Remove (lastAppPanel);
			}

			if ((sender as ListBox).SelectedItem != null) {
				//GetRidOfMe should be picked from the list we fill instead
				Layout.AppearanceClass App = GetAppearanceByKey ((sender as ListBox).SelectedItem.ToString ());
				if (App != null) {
				
					AppearancePanel appPanel = new AppearancePanel (true, App, SaveAppearance, null, false);

					appPanel.Dock = DockStyle.Fill;
					AppearanceGroup.Controls.Add (appPanel);
					appPanel.BringToFront ();

					// store this so we can delete it next time
					lastAppPanel = appPanel;
				}
			}
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
			
			

			// stores generic stuff. Appearances will also be stored here, each as a row with columnKey = appearanceName and columnValue = the XML
			db.CreateTableIfDoesNotExist (TableName, new string[4] 
			                              {columnID, columnKey, columnValue,columnType}, 
			new string[4] {
				"INTEGER",
				"TEXT",
				"TEXT",
				"INTEGER"
					
			}, "id"
			);
			return db;
		}

		void Store (BaseDatabase db, string key, string text, int type)
		{
		

			if (!db.Exists (TableName, columnKey, key)) {
				db.InsertData (TableName, new string[3]{columnKey, columnValue, columnType}, new object[3] {key, text, type});
			} else {
				// we do not obther updating TYPE
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

			Store (db, KEY_formsize, TextSizeCombo.Text, 0);
			if (MarkupCombo.SelectedItem != null) {
				Store (db, KEY_markup, MarkupCombo.SelectedItem.GetType ().AssemblyQualifiedName.ToString (),0);
			}


			
			db.Dispose();

		}
	}
}

