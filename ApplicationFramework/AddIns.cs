using System.ComponentModel.Composition;
using System;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Collections;
using CoreUtilities;
using System.Windows.Forms;
using System.Drawing;
using database;
using System.Collections.Generic;
using MefAddIns.Extensibility;

namespace appframe
{
	public class AddIns : iConfig, IDisposable
	{
		#region const
		const string columnGUID="guid";
		const string columnID="id";
		const string TableName = "addins_active";
		#endregion
		#region variables
		// if true on save we know it is safe to try to save (because interface exists)
		bool PanelWasMade = false;
		public string ConfigName {
				get { return Loc.Instance.GetString ("AddIns");}
		}
		private List<MefAddIns.Extensibility.mef_IBase>   AddInsList;
		string dataPath = CoreUtilities.Constants.BLANK;
		string DatabaseName;

		Panel configPanel;

		#endregion
		#region interface
		CheckedListBox checkers ;
		#endregion
	
		public AddIns (string path, string storage)
		{
			if (Constants.BLANK == path || Constants.BLANK == storage) {
				throw new Exception("must define a vaild path to search for AddIns and a storage location where their activte status is stored");
			}
			dataPath = path;
			DatabaseName = storage;


			if (Directory.Exists (dataPath) == false) {
				Directory.CreateDirectory (dataPath);
			}

		}

		public int Count
		{
			get { if (AddInsList != null)
				{
					return AddInsList.Count;
				}
				return 0;
				;}
		}

		/// <summary>
		/// Releases all resource used by the <see cref="appframe.AddIns"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the <see cref="appframe.AddIns"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="appframe.AddIns"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="appframe.AddIns"/> so the garbage
		/// collector can reclaim the memory that the <see cref="appframe.AddIns"/> was occupying.
		/// </remarks>
	public void Dispose ()
		{
			PanelWasMade = false;
			if (checkers != null) {
				checkers.Parent.Dispose ();
			}
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
			if (PanelWasMade == true) return configPanel;

	
				PanelWasMade = true;

			 configPanel = new Panel ();
			configPanel.BackColor = Color.Blue;
			checkers = new CheckedListBox ();
			checkers.Parent = configPanel;
			checkers.Dock = DockStyle.Fill;
			checkers.BringToFront ();

		

			BuildListOfAddins ();

			if (null == AddInsList) {
				lg.Instance.Line ("Addins.GetConfigPanel", ProblemType.MESSAGE, "No AddIns discovered.");
			}
			//checkers.DataSource = AddInsList;
			//checkers.DisplayMember = "CalledFrom.MyMenuName";


			//#STEP #1 : Load the previous preferences (go back into GetConfigPanel)
		

			List<string> myList =  GetListOfInstalledPlugs ();
			/*List<string>Guids = new List<string>();


			if (myList != null && myList.Count > 0) {
				
				foreach (object[] o in myList) {
					string GUIDOfAPlugIn = o [0].ToString ();
					Guids.Add (GUIDOfAPlugIn);
				}
			}*/



			foreach (MefAddIns.Extensibility.mef_IBase plug in AddInsList) {

				ListViewItem item = new ListViewItem(plug.CalledFrom.MyMenuName);
				item.Text = String.Format ("{0} ({1}) ", plug.CalledFrom.MyMenuName, plug.Version.ToString());
				if (plug.IsCopy )
				{
					// a copy of this GUID was present
					item.Text = item.Text + Loc.Instance.GetString ( " (COPY) ");
					
				}


		

				item.Tag = plug.CalledFrom;
				bool IsChecked = false;

				foreach (string guid in myList)
				{
					if (guid ==  plug.CalledFrom.GUID)
					{
						IsChecked = true;
					}
				}







				checkers.Items.Add (item, IsChecked);


			}


			checkers.DisplayMember = "Text";
			return configPanel;

		}
		/// <summary>
		/// Gets the list of installed plugs.
		/// 
		/// This would be the GUIDS
		/// </summary>
		/// <returns>
		/// The list of installed plugs.
		/// </returns>
		public List<string> GetListOfInstalledPlugs ()
		{

			BaseDatabase db = CreateAddInDatabase ();
			
			List<object[]> myList = db.GetValues (TableName, new string[1] {columnGUID}, "any", "*");

			// convert list into something nicer
			List<string> newList = new List<string> ();
			foreach (object[] o in myList) {
				newList.Add (o[0].ToString());
			

				db.Dispose ();
				// these are all the plugins that have been set to be active;

			}
			return newList;
		}

		private BaseDatabase CreateAddInDatabase()
		{
			BaseDatabase db = new SqlLiteDatabase (DatabaseName);




			db.CreateTableIfDoesNotExist (TableName, new string[2] 
			                              {columnID, columnGUID}, 
			new string[2] {
				"INTEGER",
				"TEXT"

			}, "id"
			);
			return db;
		}

		public void SaveRequested ()
		{
		
			// if we did not create the panel then do not waste time trying to save
			if (false == PanelWasMade) return;


			// STEP #2 next step: save and load preferences (get all that saving/loading logic of OptinPanel worked out
			// each iConfig decides for itself how to save/load
			BaseDatabase db = CreateAddInDatabase ();
			// We always erasae the TABLE because we will build a new a list
			try {

				db.DropTableIfExists(TableName);
				db = CreateAddInDatabase ();
			} catch (Exception ex) {
				lg.Instance.Line("AddIns.SaveRequested",ProblemType.EXCEPTION, String.Format ("Unable to drop table {0}", ex.ToString()));
			}

			// foreach Active plugin Add them to the list
			for (int i =0; i < checkers.Items.Count; i++) {
				if (checkers.GetItemChecked (i) == true) {
					MefAddIns.Extensibility.PlugInAction plug =   (MefAddIns.Extensibility.PlugInAction) ((ListViewItem) checkers.Items[i]).Tag;
					if (plug.GUID != Constants.BLANK)
					{
						string guid = plug.GUID;
					

						db.InsertData(TableName, new string[1]{ columnGUID}, new object[1] {guid});
					}
				}
			}
			// in this case will create a new table but using exist database Name (somehow -- pass in as parameter?)


		

			db.Dispose();
		}



		public List<MefAddIns.Extensibility.mef_IBase> BuildListOfAddins ()
		{
			if (CoreUtilities.Constants.BLANK == dataPath) {
				throw new Exception ("No path defined for AddIns");
			}
			lg.Instance.Line("AddIns.BuildListOfAddIns", ProblemType.MESSAGE, String.Format ("Scanning {0} for addins", dataPath));
			
			var bootStrapper = new MefAddIns.Terminal.Bootstrapper ();
			//An aggregate catalog that combines multiple catalogs
			var catalog = new AggregateCatalog ();
			//Adds all the parts found in same directory where the application is running!
			//var currentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(MainForm)).Location);
			catalog.Catalogs.Add (new DirectoryCatalog (dataPath));
			
			//Create the CompositionContainer with the parts in the catalog
			var _container = new CompositionContainer (catalog);

			//Fill the imports of this object
			try {
				_container.ComposeParts (bootStrapper);
			} catch (CompositionException compositionException) {
				CoreUtilities.lg.Instance.Line ("AddIns->BuildListOfAddIns", ProblemType.EXCEPTION, compositionException.ToString ());
			}
			catch (System.Reflection.ReflectionTypeLoadException)
			{
				CoreUtilities.lg.Instance.Line ("AddIns->BuildListOfAddIns", ProblemType.EXCEPTION, "At least one AddIn does not implement the entirety of the needed Interface Contract");

			}
			
			//Prints all the languages that were found into the application directory
			var i = 0;

			AddInsList = new List<MefAddIns.Extensibility.mef_IBase>();



			foreach (var language in bootStrapper.Base) {
			
				// We look to see if this is a copy of another addin loaded already
				mef_IBase AddInAlreadyIn = AddInsList.Find (mef_IBase => mef_IBase.CalledFrom.GUID == language.CalledFrom.GUID);
				if (null != AddInAlreadyIn)
				{
					lg.Instance.Line("AddIns.BuildListOfAddIns", ProblemType.MESSAGE,"This AddIn is already found. Adding as a copy");
					language.SetGuid(language.CalledFrom.GUID +  Loc.Instance.GetString(" (COPY) "));
					language.IsCopy = true;

				}
				lg.Instance.Line("AddIns.BuildListOfAddIns", ProblemType.MESSAGE,
				                 String.Format ("[{0}] {1} by {2}.\n\t{3}\n", language.Version, language.Name, language.Author, language.Description));

				i++;
				language.SetStorage(DatabaseName);
				AddInsList.Add (language);



			}

//			// added this back in, but as part of the "Consdensceing process", will remove again TO DO
//			foreach (var language in bootStrapper.Notes) {
//				
//				// We look to see if this is a copy of another addin loaded already
//				mef_IBase AddInAlreadyIn = AddInsList.Find (mef_IBase => mef_IBase.CalledFrom.GUID == language.CalledFrom.GUID);
//				if (null != AddInAlreadyIn)
//				{
//					lg.Instance.Line("AddIns.BuildListOfAddIns", ProblemType.MESSAGE,"This AddIn is already found. Adding as a copy");
//					language.SetGuid(language.CalledFrom.GUID +  Loc.Instance.GetString(" (COPY) "));
//					language.IsCopy = true;
//					
//				}
//				lg.Instance.Line("AddIns.BuildListOfAddIns", ProblemType.MESSAGE,
//				                 String.Format ("[{0}] {1} by {2}.\n\t{3}\n", language.Version, language.Name, language.Author, language.Description));
//				
//				i++;
//				AddInsList.Add (language);
//				
//				
//				
//			}

//			foreach (var form in bootStrapper.FormBasic) {
//
//				// We look to see if this is a copy of another addin loaded already
//				mef_IBase AddInAlreadyIn = AddInsList.Find (mef_IBase => mef_IBase.CalledFrom.GUID == form.CalledFrom.GUID);
//				if (null != AddInAlreadyIn)
//				{
//					lg.Instance.Line("AddIns.BuildListOfAddIns", ProblemType.MESSAGE,"This AddIn is already found. Adding as a copy");
//					form.SetGuid(form.CalledFrom.GUID +  Loc.Instance.GetString(" (COPY) "));
//					form.IsCopy = true;
//					
//				}
//				lg.Instance.Line("AddIns.BuildListOfAddIns", ProblemType.MESSAGE,
//				                 String.Format ("[{0}] {1} by {2}.\n\t{3} {4}\n", form.Version, form.Name, form.Author, form.Description, form.IsCopy));
//				
//				i++;
//				AddInsList.Add (form);
//
////				form.ShowWindow();
//			}
			/*
			foreach (var note in bootStrapper.Notes) {
				Console.WriteLine ("[{0}] {1} by {2}.\n\t{3}\n", note.Version, note.Name, note.Author, note.Description);
				AddInsList.Add (note);
				//language.Boom ();
				//string result = language.Tester ("this is the string I passed in");
				//Console.WriteLine ("RESULT = " + result);
				
//				note.RegisterType();
			}
*/

			lg.Instance.Line("AddIns.BuildListOfAddIns", ProblemType.MESSAGE, String.Format ("AddIns Found: {0}.", i));

			return AddInsList;

			// 

		}


		/*
		public void OldTestMethod()
		{
			if (CoreUtilities.Constants.BLANK == dataPath) {
				throw new Exception ("No path defined for AddIns");
			}
			
			var bootStrapper = new MefAddIns.Terminal.Bootstrapper ();
			//An aggregate catalog that combines multiple catalogs
			var catalog = new AggregateCatalog ();
			//Adds all the parts found in same directory where the application is running!
			//var currentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(MainForm)).Location);
			catalog.Catalogs.Add (new DirectoryCatalog (dataPath));
			
			//Create the CompositionContainer with the parts in the catalog
			var _container = new CompositionContainer (catalog);
			
			//Fill the imports of this object
			try {
				_container.ComposeParts (bootStrapper);
			} catch (CompositionException compositionException) {
				Console.WriteLine (compositionException.ToString ());
			}
			
			//Prints all the languages that were found into the application directory
			var i = 0;
			foreach (var language in bootStrapper.Base) {
				Console.WriteLine ("[{0}] {1} by {2}.\n\t{3}\n", language.Version, language.Name, language.Author, language.Description);
				//language.Boom ();
				
				
				//string result = language.Tester ("this is the string I passed in");
				//Console.WriteLine ("RESULT = " + result);
				
				i++;
			}
			Console.WriteLine("It has been found {0} supported languages",i);
			
			
			foreach (var form in bootStrapper.FormBasic) {
				Console.WriteLine ("[{0}] {1} by {2}.\n\t{3}\n", form.Version, form.Name, form.Author, form.Description);
				System.Diagnostics.Process proc = System.Diagnostics.Process.GetCurrentProcess ();
				long size = proc.PrivateMemorySize64;
				Console.WriteLine ("Memory " + size);
				form.ShowWindow();
			}
			
			foreach (var note in bootStrapper.Notes) {
				Console.WriteLine ("[{0}] {1} by {2}.\n\t{3}\n", note.Version, note.Name, note.Author, note.Description);
				//language.Boom ();
				
				
				//string result = language.Tester ("this is the string I passed in");
				//Console.WriteLine ("RESULT = " + result);
				
				note.RegisterType();
			}
		}*/


	}
}

