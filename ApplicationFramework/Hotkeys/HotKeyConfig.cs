// HotKeyConfig.cs
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
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using database;
using HotKeys;

namespace appframe
{
	public class HotKeyConfig: iConfig, IDisposable
	{
		#region const
		const string TableName = "hotkeys";
		const string columnGUID ="guid";
		const string columnID = "id";
		const string columnKeyModifier="keymodifier";
		const string columnKey="key";
		const int columnCount= 4;
		#endregion

		#region variables
		// if true on save we know it is safe to try to save (because interface exists)
		bool PanelWasMade = false;
		public string ConfigName {
			get { return Loc.Instance.GetString ("Hotkeys");}
		}
		List<KeyData> HotKeys = null;
	
		string DatabaseName;
		
		Panel configPanel;
		
		#endregion

		#region delegates
		MainFormBase.GetAValidDatabase GetBaseDatabase=null;
		#endregion

		#region interface

		#endregion
		public void Dispose ()
		{
			if (configPanel != null) {
				configPanel.Dispose();
			}
		}

		 BaseDatabase CreateDatabase (string DatabaseName)
		{
			BaseDatabase db = GetBaseDatabase(DatabaseName);
		//	BaseDatabase db = new SqlLiteDatabase (DatabaseName);
			//BaseDatabase db = Layout.MasterOfLayouts.GetDatabaseType(DatabaseName);
			
			
			
			db.CreateTableIfDoesNotExist (TableName, new string[columnCount] 
			                              {columnID, columnGUID, columnKeyModifier, columnKey}, 
			new string[columnCount] {
				"INTEGER",
				"TEXT","TEXT","TEXT"
					
			}, "id"
			);
			return db;
		}

		public  List<KeyData> GetListOfModifiedKeys (string DatabaseName)
		{
			
			BaseDatabase db = CreateDatabase (DatabaseName);
			
			List<object[]> myList = db.GetValues (TableName, new string[3] {columnGUID, columnKeyModifier, columnKey}, "any", "*");
			
			// convert list into something nicer
			List<KeyData> newList = new List<KeyData> ();
			foreach (object[] o in myList) {
				if (o.Length == columnCount-1)
				{
				// we only care about KeyModifier and Key
				KeyData  keysy= new KeyData("", null,(Keys) Enum.Parse (typeof(Keys), o[1].ToString ()),
				                           (Keys) Enum.Parse (typeof(Keys),o[2].ToString ()), "", false, o[0].ToString ());

				
				newList.Add (keysy);
				}
				


			}
			db.Dispose ();
			return newList;
		}

		/// <summary>
		/// Updates the keys. This is similar to RebuildListOfKeys but is called on Application
		/// load to handle modification of the keys.
		/// 
		/// THIS Will override any TEMPORARY modifications made by RebuildListOfKeys (as long as it is called when the form is closed)
		/// </summary>
		public  void UpdateKeys (List<KeyData> HotKeys, string DatabaseName)
		{	List<KeyData> ModifiedKeys = GetListOfModifiedKeys (DatabaseName);
			foreach (KeyData keysy in HotKeys) {
				KeyData keyModified = ModifiedKeys.Find (KeyData => KeyData.GetGUID () == keysy.GetGUID ());
				
				if (keyModified != null) {
					// we have an overriden key
					keysy.Key = keyModified.Key;
					keysy.ModifyingKey = keyModified.ModifyingKey;
				}
			}
		}
		private void RebuildListOfKeys ()
		{
			configPanel.Controls.Clear ();
			HotKeys.Sort ();
			
			List<KeyData> ModifiedKeys = GetListOfModifiedKeys (DatabaseName);
			
			
			//NewMessage.Show (HotKeys.Count.ToString());
			foreach (KeyData keysy in HotKeys) {
				
			
				
				KeyData keyModified = ModifiedKeys.Find (KeyData => KeyData.GetGUID () == keysy.GetGUID ());

				if (keyModified != null) {
					// we have an overriden key
					keysy.Key = keyModified.Key;
					keysy.ModifyingKey = keyModified.ModifyingKey;
				}

				VisualKey keyPanel = new VisualKey (keysy, MainFormBase.MainFormIcon, AfterKeyEdit);
				// apply any overrides from database storage
			
				configPanel.Controls.Add (keyPanel);
				keyPanel.Dock = DockStyle.Top;
				
			}

			Button Reset = new Button();
			Reset.Text = Loc.Instance.GetString ("Reset");
			Reset.Dock = DockStyle.Top;
			Reset.Click+= HandleResetClick;

			configPanel.Controls.Add (Reset);
			Reset.BringToFront();

			CheckForErrors ();
		}

		void HandleResetClick (object sender, EventArgs e)
		{
			if (NewMessage.Show (Loc.Instance.GetString ("Reset Hotkeys?"), Loc.Instance.GetStringFmt ("If you do this you will lose any custom hotkey assignments."),
			                 MessageBoxButtons.YesNo, null) == DialogResult.Yes) {
				BaseDatabase db = CreateDatabase (DatabaseName);

				db.DropTableIfExists (TableName);

				db.Dispose ();

				// okay to call Rebuild because we have wiped modification data
				RebuildListOfKeys ();
			}
		}
		public int Duplicates=0;

		void CheckForErrors()
		{
			Duplicates =0;
			// update for duplicates?
			System.Collections.Hashtable hash = new System.Collections.Hashtable();
			foreach (Control control in configPanel.Controls) {
				if (control is VisualKey)
				{
					((VisualKey)control).IsDuplicate(false);
					if (hash[((VisualKey)control).UniqueCode()] != null)
					{
						Duplicates++;
						// this combination already exist.
						((VisualKey)control).IsDuplicate(true);
					}
					else
						// how to know if we have a duplicate? A simple hash, if key already present?
						hash.Add(((VisualKey)control).UniqueCode(),"present");
					
				}
			}
		}

		void AfterKeyEdit (string GUID)
		{
			CheckForErrors ();



			// this does not make sense now that we are not storing until a SAVE... we need to use the GUI
//			KeyData keyDuplicate = ModifiedKeys.Find ( (KeyData => (KeyData.ModifyingKey == keysy.ModifyingKey && KeyData.Key == KeyData.Key)) );
//			if (keyDuplicate!= null)
//			{
//				keyPanel.IsDuplicate(true);
//			}

			// do we need to actually do anything?
//			for (int i = 0; i < configPanel.Controls.Count; i++)
//			{
//				if (configPanel.Controls[i] is VisualKey)
//				{
//					if ( ((VisualKey)configPanel.Controls[i]).IsModified == true && ((VisualKey)configPanel.Controls[i]).keyOut != null)
//					{
//						if (((VisualKey)configPanel.Controls[i]).keyOut.GetGUID() == GUID)
//						{
//								// we have f
//						}
//					}
//				}
//			}
		}

		/// <summary>
		/// Gets the config panel for AddIns
		/// </summary>
		/// <returns>
		/// The config panel.
		/// </returns>
		public Panel GetConfigPanel ()
		{
			// if panel made then leave it alone
//			if (PanelWasMade == true)
//				return configPanel;

			// unlike other option forms we need to rebuild this each time
			
			PanelWasMade = true;
			
			configPanel = new Panel ();
			configPanel.AutoScroll = true;
			RebuildListOfKeys();

			return configPanel;
		}
		public HotKeyConfig (string storage,  ref List<KeyData> _HotKeys, MainFormBase.GetAValidDatabase _GetBaseDatabase)
		{
			HotKeys = _HotKeys;


			if (Constants.BLANK == storage) {
				throw new Exception("must specify a nonBlank database name to store the HotKeyModifications inside of.");
			}
			GetBaseDatabase = _GetBaseDatabase;
			DatabaseName = storage;
			
	

		}

		public void SaveRequested ()
		{
			BaseDatabase db = CreateDatabase (DatabaseName);


			// if we did not create the panel then do not waste time trying to save
			if (false == PanelWasMade)
				return;
			for (int i = 0; i < configPanel.Controls.Count; i++) {
				if (configPanel.Controls[i] is VisualKey)
				{
				VisualKey key = (VisualKey)configPanel.Controls [i];
				if (true == key.IsModified && key.keyOut != null) {
					// we have a modified key and it should be stored into the database
					if (db.Exists (TableName, columnGUID, key.keyOut.GetGUID ())) {
						// modified existing
						db.UpdateSpecificColumnData (TableName, new string[columnCount - 1] {
							columnGUID,
							columnKeyModifier,
							columnKey
						}, new object[columnCount - 1] {
							key.keyOut.GetGUID (),
							key.keyOut.ModifyingKey.ToString (),
							key.keyOut.Key.ToString ()
						}, columnGUID, key.keyOut.GetGUID ());
					}
					else {
						// add new
						db.InsertData (TableName, new string[columnCount - 1] {
							columnGUID,
							columnKeyModifier,
							columnKey
						}, new object[columnCount - 1] {
							key.keyOut.GetGUID (),
							key.keyOut.ModifyingKey.ToString (),
							key.keyOut.Key.ToString ()
						});
					}
				}
				}
			}

			db.Dispose();
		}
	}
}

