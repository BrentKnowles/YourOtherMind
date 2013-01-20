using System;
using database;
using System.Collections.Generic;
using Layout.data;
using CoreUtilities;

namespace Layout
{
	/// <summary>
	/// List of layouts try1.  (This is effectively ALL THE PAGES
	/// </summary>
	public class MasterOfLayouts : IDisposable
	{
		public MasterOfLayouts ()
		{
		}
		#region structs
		public struct NameAndGuid
		{
			private string _blurb;

			public string Blurb {
				get {
					return _blurb;
				}
				set {
					_blurb = value;
				}
			}

			private string _guid;
			private string _caption;
			public string Guid {get {return _guid;}  set {_guid = value;}}
			public string Caption {get {return _caption;}  set {_caption = value;}}
		}
		#endregion
		/// <summary>
		/// Creates the database. (MOVED from LayoutDatabase());
		/// </summary>
		public static BaseDatabase CreateDatabase ()
		{
			SqlLiteDatabase db = new SqlLiteDatabase (LayoutDetails.Instance.YOM_DATABASE);
			db.CreateTableIfDoesNotExist (dbConstants.table_name, 
		                              dbConstants.Columns, 
		                              dbConstants.Types, String.Format ("{0}", dbConstants.ID)
			);
			return db;
		}



		public void Dispose()
		{
		}
		/// <summary>
		/// Exists the specified LayoutName.
		/// </summary>
		/// <param name='LayoutName'>
		/// Layout name.
		/// </param>
		public static bool ExistsByName(string LayoutName)
		{

			BaseDatabase MyDatabase = CreateDatabase ();
			bool result =  MyDatabase.Exists (dbConstants.table_name, dbConstants.NAME, LayoutName);
			MyDatabase.Dispose();
			return result;
		}

		/// <summary>
		/// Gets the name of the GUID from.
		/// </summary>
		/// <returns>
		/// The GUID from name.
		/// </returns>
		/// <param name='name'>
		/// Name.
		/// </param>
		public static string GetGuidFromName (string name)
		{
			BaseDatabase MyDatabase = CreateDatabase ();
			string guid=Constants.BLANK;
			List<object[]> result = MyDatabase.GetValues(dbConstants.table_name, new string[1] {dbConstants.GUID}, dbConstants.NAME, name);
			if (result != null && result.Count > 0)
			{
				if (result[0][0] != null)
				{
					guid = (result[0][0]).ToString();
				}
			}
			MyDatabase.Dispose();
			return guid;
		}
		public static bool ExistsByGUID(string GUID)
		{
			
			BaseDatabase MyDatabase = CreateDatabase ();
			bool result =  MyDatabase.Exists (dbConstants.table_name, dbConstants.GUID, GUID);
			MyDatabase.Dispose();
			return result;
		}
		public string Backup ()
		{
			SqlLiteDatabase db = new SqlLiteDatabase (LayoutDetails.Instance.YOM_DATABASE);
			string result = db.BackupDatabase();
			db.Dispose();
			return result;
		}

		/// <summary>
		/// Gets the list of layouts.
		/// </summary>
		/// <returns>
		/// The list of layouts.
		/// </returns>
		/// <param name='filter'>
		/// Filter.
		/// </param>
			/// 
		public List<NameAndGuid> GetListOfLayouts (string filter)
		{
			
			BaseDatabase MyDatabase = CreateDatabase ();
			List<NameAndGuid>result = new List<NameAndGuid>();
			if (MyDatabase == null) {
				throw new Exception ("Unable to create database in LoadFrom");
			}
			
			List<object[]> myList = MyDatabase.GetValues (dbConstants.table_name, new string[3] {dbConstants.GUID, dbConstants.NAME, dbConstants.BLURB},
			dbConstants.SUBPANEL , 0,String.Format(" order by {0} COLLATE NOCASE", dbConstants.NAME));
			
			
			if (myList != null && myList.Count > 0) {
				
				foreach (object[] o in myList)
				{
					NameAndGuid record = new NameAndGuid();
					record.Guid = o[0].ToString();
					record.Caption = o [1].ToString();
					record.Blurb = o [2].ToString();
					lg.Instance.Line("MasterOfLayouts->GetListOfLayouts", ProblemType.MESSAGE, "adding to ListOfLayouts " + record.Caption);
					result.Add (record);
				}
			}
			MyDatabase.Dispose();
			return result;
		}
		/// <summary>
		/// Deletes the layout.
		/// </summary>
		/// <param name='guid'>
		/// GUID.
		/// </param>
		public static void DeleteLayout (string guid)
		{

			BaseDatabase MyDatabase = CreateDatabase ();
			if (MyDatabase.Exists (dbConstants.table_name, dbConstants.GUID, guid) == true) {
				if (MyDatabase.Delete(dbConstants.table_name, dbConstants.GUID, guid) == true)
				{
				}
				else
				{
					NewMessage.Show (Loc.Instance.GetStringFmt("Unable to delete layout {0}", guid));
				}
			}


			//if (MyDatabase.Exists (dbConstants.table_name, dbConstants.GUID, guid) == true) NewMessage.Show ("Did not delete"); else NewMessage.Show ("Was deleted!");

			MyDatabase.Dispose();
		}
		//TODO: This is just a hack to prove a deeper system. Will need to be done properly
		//right now the context is GetLayoutBy("section", "writing")
		//returns a random NOTE  matching context INCLUDING notes on subpanels (once subpanels able to inherit Details of their Parents)
		public static string GetRandomNoteBy (string typeofsearch, string param)
		{

			typeofsearch = dbConstants.NOTEBOOK;
			
			BaseDatabase MyDatabase = CreateDatabase ();
			List<object[]> results = MyDatabase.GetValues (dbConstants.table_name, new string[2] {
				dbConstants.NAME, dbConstants.GUID

			}, typeofsearch, param);
			// Do a query on the database
			// then process, grabbing first ONE Layout out of the mix
			//

			string temp = "";
			string guid = Constants.BLANK;
			if (results != null && results.Count > 0) {
				int pickme = LayoutDetails.Instance.RandomNumbers.Next (1, results.Count + 1);
				temp = results [pickme - 1] [0].ToString ();
				guid = results [pickme - 1] [1].ToString ();
			}


			// TODO: Next will be loading the XML and grabbing a note.

			if (Constants.BLANK != guid)
			{
			// we load it
				LayoutInterface layoutdata = LayoutDetails.DATA_Layout(guid);
			//	LayoutPanelBase panel = new Layout.LayoutPanel();
				layoutdata.LoadFrom (null);
				System.Collections.ObjectModel.ReadOnlyCollection<NoteDataInterface> listofnotes = layoutdata.GetNotes();
				if (listofnotes != null && listofnotes.Count > 0)
				{
				int pickme = LayoutDetails.Instance.RandomNumbers.Next (1, listofnotes.Count + 1);
				NoteDataInterface randomNote = (NoteDataInterface)listofnotes[pickme-1];
				temp = String.Format ("Layout: {0} Note Caption: {1}", temp, randomNote.Caption);
				}
			}
				// additional hack is just to return the CAPTION, not the GUID, else I won't know if it worked!
			return temp;
		}

	}
}

