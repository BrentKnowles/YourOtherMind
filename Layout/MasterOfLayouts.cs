using System;
using database;
using System.Collections.Generic;
using Layout.data;
using CoreUtilities;
using System.Data;

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
		public static string GetNameFromGuid (string guid)
		{
			BaseDatabase MyDatabase = CreateDatabase ();
			string name=Constants.BLANK;
			List<object[]> result = MyDatabase.GetValues(dbConstants.table_name, new string[1] {dbConstants.NAME}, dbConstants.GUID, guid);
			if (result != null && result.Count > 0)
			{
				if (result[0][0] != null)
				{
					name = (result[0][0]).ToString();
				}
			}
			MyDatabase.Dispose();
			return name;
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

		/// <summary>
		/// Gets the note from inside layout. Called from the LinkNote note type
		/// </summary>
		/// <returns>
		/// The note from inside layout.
		/// </returns>
		/// <param name='ParentGuid'>
		/// Parent GUID.
		/// </param>
		/// <param name='NoteGuid'>
		/// Note GUID.
		/// </param>
		public static NoteDataInterface GetNoteFromInsideLayout (string ParentGuid, string NoteGuid)
		{
			if (ParentGuid == Constants.BLANK || NoteGuid == Constants.BLANK) {
				throw new Exception("Must define valid GUIDs for the Layout and the Note");
			}

			
			BaseDatabase MyDatabase = CreateDatabase ();
		
			

			{
				// we load it
				LayoutInterface layoutdata = LayoutDetails.DATA_Layout(ParentGuid);
				//	LayoutPanelBase panel = new Layout.LayoutPanel();
				layoutdata.LoadFrom (null);
				System.Collections.ObjectModel.ReadOnlyCollection<NoteDataInterface> listofnotes = layoutdata.GetNotes();
				if (listofnotes != null && listofnotes.Count > 0)
				{
					foreach (NoteDataInterface note in listofnotes)
					{
						if (NoteGuid == note.GuidForNote)
						{
							// we found the note we search for
							return note;
						}
					}
//					int pickme = LayoutDetails.Instance.RandomNumbers.Next (1, listofnotes.Count + 1);
//					NoteDataInterface randomNote = (NoteDataInterface)listofnotes[pickme-1];
					//temp = String.Format ("Layout: {0} Note Caption: {1}", temp, randomNote.Caption);
				}
			}

			return null;
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
			MyDatabase.Dispose();
				// additional hack is just to return the CAPTION, not the GUID, else I won't know if it worked!
			return temp;
		}


		private static NoteDataXML_Table LoadStringAsTable (string data)
		{
			// Loading existing table
			System.IO.StringReader LinkTableReader = new System.IO.StringReader (data);
			System.Xml.Serialization.XmlSerializer LinkTableXML = new System.Xml.Serialization.XmlSerializer (typeof(NoteDataXML_Table));
			
			NoteDataXML_Table table = new NoteDataXML_Table ();
			
			table = (NoteDataXML_Table)LinkTableXML.Deserialize (LinkTableReader);
			//						if (table != null)
			//						{
			//							MyLinkTable.SetTable (table.dataSource);
			//						}
			//NewMessage.Show("Loading a link table with GUID = " + table.GuidForNote);
			LinkTableXML = null;
			LinkTableReader.Close ();
			LinkTableReader.Dispose ();
			return table;
		}
		protected  static NoteDataXML_Table BuildLinMasterLinkTable()
		{
			NoteDataXML_Table table = null;
			BaseDatabase MyDatabase = CreateDatabase ();
			
			
			// 0 - GetValues (all linktable columns)  easy
			List<object[]> results = MyDatabase.GetValues (dbConstants.table_name, new string[1]{dbConstants.LINKTABLE}, "any", "*");
			string combinedResult = Constants.BLANK;
			int record = 0;
			//NoteDataXML_Table table = null;
			
			if (results != null && results.Count > 0) {
				foreach (object[] row in results) {
					
					if (row.Length > 0) {
						
						if (row [0].ToString () != Constants.BLANK) {
							if (table != null) {
								// now load secondtable and copy rows over
								NoteDataXML_Table secondTable = LoadStringAsTable (row [0].ToString ());
								foreach (DataRow drow in secondTable.GetRows()) {
									object[] values = drow.ItemArray;
									values [0] = null;//DBNull.Value;//DBNull.Value;
									
									table.AddRow (values);
								}
							} else
								table = LoadStringAsTable (row [0].ToString ());
							
							//							if (row[0].ToString ().IndexOf("<DocumentElement>") > -1)
							//							{
							//							record++;
							//							if (1 == record)
							//							{
							//
							//								combinedResult = row[0].ToString ();
							//								// the first time we add it in a special way
							//
							//								int index_of_end = combinedResult.IndexOf("</DocumentElement>");
							//								if (index_of_end > -1)
							//								{
							//									// chop string
							//									combinedResult = combinedResult.Substring (0, index_of_end);
							//								}
							//								else
							//								{
							//									throw new Exception("Invalid table passed in");
							//								}
							//								// cut the bottom part off
							//
							//						
							//						//NewMessage.Show (row[0].ToString());
							//							
							//							}
							//							}//DocumentElement exists
							//							else
							//							{
							//								// now we add each row
							//							}
						}
					}
				}
				
				// we rebuild the text to be a proper table
				//				string END_TEXT = " </DocumentElement></diffgr:diffgram>  </dataSource> </NoteDataXML_Table>";
				//				combinedResult = combinedResult + END_TEXT;
			}
			// 1 - Consolidage all LinkTables  hard --> requires: 
			MyDatabase.Dispose();
			return table;
		}
		public static System.Collections.Generic.List<string> ReciprocalLinks (string myLayoutGuid)
		{
			NoteDataXML_Table table =  BuildLinMasterLinkTable();

			// returns a list of (Page Names.Guids) which will be links to Layouts
			// that are linked to me or notes inside me


//			if (combinedResult != Constants.BLANK) {
//				// Loading existing table
//				System.IO.StringReader LinkTableReader = new System.IO.StringReader (combinedResult);
//				System.Xml.Serialization.XmlSerializer LinkTableXML = new System.Xml.Serialization.XmlSerializer (typeof(NoteDataXML_Table));
//					
//					
//					
//				table = (NoteDataXML_Table)LinkTableXML.Deserialize (LinkTableReader);
//				//						if (table != null)
//				//						{
//				//							MyLinkTable.SetTable (table.dataSource);
//				//						}
//				//NewMessage.Show("Loading a link table with GUID = " + table.GuidForNote);
//				LinkTableXML = null;
//				LinkTableReader.Close ();
//				LinkTableReader.Dispose ();
//
//			}
//			if (table != null) {
//
//			}


			//NewMessage.Show("Combined row count " + table.RowCount().ToString());

			// 2 - search the parent.child column where child=myGuid
			//     and retrieve the (group) column this list is a list of guids in form parent.child (the LinkNote)


			List<string> result = new List<string>();
			if (table != null) {
				DataView dataView = ((DataTable)table.dataSource).DefaultView;
				//dataView.Tables.Add();
			//	((DataTable)table.dataSource).RowFilter="";

				// The GUID we have is the LayoutGuid, so the Parent.* of the eqution
				dataView.RowFilter=String.Format ("linkid Like '{0}.%'",myLayoutGuid );
				foreach(DataRowView row in dataView)
				{
					// if linkid is equal


					// add group AFTER translating
					string doubleguid = row[CoreUtilities.Links.LinkTable.GROUP].ToString();
					// this will be in parent.child form
					string[] guids = doubleguid.Split (new char[1]{'.'});
					if (guids != null && guids.Length == 2)
					{
						string guid = guids[0];
					
					string name = GetNameFromGuid(guid);
					result.Add (String.Format("{0}.{1}", name, guid));
					}
				}
			}
		

			// 3 - go through and parse the parent. part of the parent.child guids and return a list of
			//    Name.parentguid

		
			return result;
		}

	}
}

