// MasterOfLayouts.cs
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
using database;
using System.Collections.Generic;
using Layout.data;
using CoreUtilities;
using System.Data;
using System.IO;

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

			// This is purely a hack because the way I structured automatically generated cover letters. Both the project obect and the market object
			// had fields named Caption. (see ViewProjectSUbmissions.cs, Generate_CoverLetter()
			public string ProjectName {
				get {return Caption;}
			}

			private string _guid;
			private string _caption;
			public string Guid {get {return _guid;}  set {_guid = value;}}
			public string Caption {get {return _caption;}  set {_caption = value;}}

			private int words;

			public int Words {
				get {
					return words;
				}
				set {
					words = value;
				}
			}
		}
		#endregion
		/// <summary>
		/// Creates the database. (MOVED from LayoutDatabase());
		/// </summary>
		public static BaseDatabase CreateDatabase ()
		{
			BaseDatabase db = GetDatabaseType(LayoutDetails.Instance.YOM_DATABASE);//new SqlLiteDatabase (LayoutDetails.Instance.YOM_DATABASE);
			db.CreateTableIfDoesNotExist (dbConstants.table_name, 
		                              dbConstants.Columns, 
		                              dbConstants.Types, String.Format ("{0}", dbConstants.ID)
			);
			return db;
		}

		public static void DeleteTransactionTable ()
		{
		//	NewMessage.Show ("Not done. Just temp for import/export testing");

			BaseDatabase db = CreateDatabase();
			db.DropTableIfExists(Transactions.TransactionsTable.table_name);
			db.Dispose ();
		}

		public static BaseDatabase GetDatabaseType(string DatabaseName)
		{
			return new SqlLiteDatabase(DatabaseName);
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
		/// Gets the subpanels parent. Assumes is a subpanel.
		/// </summary>
		/// <returns>
		/// The subpanels parent.
		/// </returns>
		/// <param name='guidtoload'>
		/// Guidtoload.
		/// </param>
		public static string GetSubpanelsParent (string guidtoload)
		{
			BaseDatabase MyDatabase = CreateDatabase ();
			string sresult = Constants.BLANK;
			
			List<object[]> result = MyDatabase.GetValues(dbConstants.table_name, new string[1] {dbConstants.PARENT_GUID}, dbConstants.GUID, guidtoload);
			if (result != null && result.Count > 0)
			{
				if (result[0][0] != null)
				{
					
					sresult = ((result[0][0]).ToString());
				}
			}
			MyDatabase.Dispose();
			return sresult;
		}

		public static bool IsSubpanel (string guidtoload)
		{
		

			BaseDatabase MyDatabase = CreateDatabase ();
			bool IsSub = false;

			List<object[]> result = MyDatabase.GetValues(dbConstants.table_name, new string[1] {dbConstants.SUBPANEL}, dbConstants.GUID, guidtoload);
			if (result != null && result.Count > 0)
			{
				if (result[0][0] != null)
				{

					IsSub = bool.Parse ((result[0][0]).ToString());
				}
			}
			MyDatabase.Dispose();
			return IsSub;
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
		public static int GetWordsFromGuid (string guid)
		{
			BaseDatabase MyDatabase = CreateDatabase ();
			string name=Constants.BLANK;
			List<object[]> result = MyDatabase.GetValues(dbConstants.table_name, new string[1] {dbConstants.WORDS}, dbConstants.GUID, guid);
			if (result != null && result.Count > 0)
			{
				if (result[0][0] != null)
				{
					name = (result[0][0]).ToString();
				}
			}

			int words = 0;
			Int32.TryParse(name, out words);
			MyDatabase.Dispose();
			return words;
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
			BaseDatabase db = CreateDatabase();//new SlLiteDatabase (LayoutDetails.Instance.YOM_DATABASE);
			string result = db.BackupDatabase();
			db.Dispose();
			return result;
		}
		public static string LookupFilter (string filter)
		{
			return LookupFilter (filter, null);
		}
		public static string LookupFilter (string filter, LayoutPanelBase OverridePanelToUse)
		{
			LayoutPanelBase LayoutToUse = LayoutDetails.Instance.TableLayout;
			if (null != OverridePanelToUse) {
				LayoutToUse = OverridePanelToUse;
			}
			if (filter != Constants.BLANK) {
				List<string> results = LayoutToUse.GetListOfStringsFromSystemTable (LayoutDetails.SYSTEM_QUERIES, 2, String.Format ("1|{0}", filter));
				if (results != null && results.Count > 0) {
					return results [0];
				}
			}
			return Constants.BLANK;
		}
		public static List<NameAndGuid> GetListOfLayouts (string filter)
		{
			return GetListOfLayouts(filter, Constants.BLANK, false, null);
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
		public static List<NameAndGuid> GetListOfLayouts (string filter, string likename, bool FullTextSearch, LayoutPanelBase OverrideLayoutToUseToFindTable)
		{
			string test = Constants.BLANK;
			if (filter.IndexOf ("CODE_KEYWORD_AUTO") > -1) {
				// a keyword was clicked on the interface
				// we now extract it from
				//CODE_KEYWORD_AUTO, KEYWORD
				string[] items = filter.Split (new char[1] {','});
				if (items != null && items.Length == 2) {
					string value = items [1];

					test = String.Format ("{0} LIKE '%{1}%'",dbConstants.KEYWORDS, value);
					//NewMessage.Show (test);
				}
			}


			// cleanup on likename (can't have apostophes
			if (likename.IndexOf ("\"") > 0) {
				likename = likename.Replace ("\"", "");
			}
			if (likename.IndexOf ("'") > 0) {
				likename = likename.Replace ("'", "");
			}
			
			BaseDatabase MyDatabase = CreateDatabase ();
			List<NameAndGuid> result = new List<NameAndGuid> ();
			if (MyDatabase == null) {
				throw new Exception ("Unable to create database in LoadFrom");
			}



			// it may be blank if we have done a Keyword click filter, from above
			if (Constants.BLANK == test) {
				test = LookupFilter (filter, OverrideLayoutToUseToFindTable); //"and notebook='Writing' ";
			}
			if (test != Constants.BLANK) {
				// initial implementation influenced by needs of SUbmission list
				// always has an existing Where clause (no subpanels) so we need to 
				// predicate with an AND
				test = String.Format ("and {0}", test);
			}
			if (Constants.BLANK == filter)
				test = "";


			// Add a name filter
			if (false == FullTextSearch && Constants.BLANK != likename) {
				test = String.Format ("and name like '%{0}%'", likename);
			} else if (true == FullTextSearch) {
				// modify the query to handle full text searching
				test = String.Format ("and xml like '%{0}%'", likename);
			}


		

			List<object[]> myList = MyDatabase.GetValues (dbConstants.table_name, new string[4] {dbConstants.GUID, dbConstants.NAME, dbConstants.BLURB
			,dbConstants.WORDS},
			dbConstants.SUBPANEL , 0,String.Format(" order by {0} COLLATE NOCASE", dbConstants.NAME),test);
			

			if (myList != null && myList.Count > 0) {
				
				foreach (object[] o in myList)
				{
					NameAndGuid record = new NameAndGuid();
					record.Guid = o[0].ToString();
					record.Caption = o [1].ToString();
					record.Blurb = o [2].ToString();
					int Words = 0;
					Int32.TryParse(o[3].ToString(), out Words);
					record.Words = Words;
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
					LayoutDetails.Instance.TransactionsList.AddEvent (new Transactions.TransactionDeleteLayout(DateTime.Now, guid));

					//now DELETE  any page with the GUId as their ParentGuid

					// we have to grab the actual list of children and delete each individually
					List<object[]> Deletable = MyDatabase.GetValues(dbConstants.table_name, new string[1] {dbConstants.GUID}, dbConstants.PARENT_GUID, guid);
					if (Deletable != null)
					{
						foreach (object[] oArray  in Deletable)
						{
							if (oArray.Length >0 )
							{
								DeleteLayout (oArray[0].ToString());
							}
						}
					}

					//MyDatabase.Delete(dbConstants.table_name, dbConstants.PARENT_GUID, guid);


//					List<object> children = MyDatabase.GetValues(dbConstants.table_name, dbConstants.PARENT_GUID, guid);
//
//					if (children != null && children.Count > 0) {
//						for (int i = 0; i < children.Count; i++)
//						{
//							WriteFile (String.Format ("{1}__child{0}.txt", i.ToString (),FileName),children[i]);
//						}
//					}

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
			//int record = 0;
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
		/// <summary>
		/// Count the number of Rows in the Layout table.
		/// 
		/// This WILL include subpanels
		/// </summary>
		public static int Count (bool CountSubpanels)
		{
			BaseDatabase MyDatabase = CreateDatabase ();
			int count = 0;

			string testColumn = BaseDatabase.GetValues_ANY;
			string testValue = BaseDatabase.GetValues_WILDCARD;

			if (false == CountSubpanels) {
				testColumn = dbConstants.SUBPANEL;
				testValue = "0";
			}
		

			List<object[]> results = MyDatabase.GetValues (dbConstants.table_name, new string[1] {dbConstants.NAME}, testColumn, testValue);
			if (results != null) {
				count = results.Count;
			}
			MyDatabase.Dispose();
			return count;
		}

		private static void WriteFile(string FileName, string value)
		{
			lg.Instance.Line("MasterOfLayouts->WriteFile", ProblemType.MESSAGE, "Writing: " + FileName);
			StreamWriter writer = new StreamWriter (FileName, false);
			writer.Write (value);
			writer.Close ();
			writer.Dispose ();

		}
		public static int ImportLayout (string file)
		{
			int errorCode = 0;
			if (File.Exists (file) == true) {
				BaseDatabase MyDatabase = CreateDatabase ();
				string line = Constants.BLANK;
				using (StreamReader sr = new StreamReader(file)) {
					line = sr.ReadToEnd ();
					//Console.WriteLine (line);
				}

				errorCode = MyDatabase.ImportFromString (line, dbConstants.GUID);




				if (-1 == errorCode) {
					NewMessage.Show (Loc.Instance.GetStringFmt ("We found {0} columns but found {1} values. They do not match. Failing to import"));
					return errorCode;
				}
				if (-2 == errorCode) {
					NewMessage.Show (Loc.Instance.GetString ("An invalid tablename was found in the file to be imported"));
					return errorCode;
				}
				if (-3 == errorCode) {
					NewMessage.Show (Loc.Instance.GetString ("No data was found to be imported (no columns), everything skipped"));
					return errorCode;
				}
				if (-4 == errorCode)
				{
					NewMessage.Show (Loc.Instance.GetString ("Did not find TestColumn in the list of data and hence have no value to compare for uniqueness"));
					return errorCode;
				}


				//Test for existence in a loop??!? Recursion
				if (errorCode == 0 && file.IndexOf("_child") == -1) {
					// do not bother importing if Parent failed
					int counter = 0;
					bool done = false;
					while (false == done) {
						string ChildFile = String.Format ("{0}__child{1}.txt",file, counter.ToString ());
						if (File.Exists (ChildFile)) {
							lg.Instance.Line ("MasterOfLayouts->ImportLayout", ProblemType.MESSAGE,"A child was found");
							errorCode = ImportLayout(ChildFile);
							if (errorCode < 0)
							{
								done = true;
							}
						}
						else
						{
							lg.Instance.Line ("MasterOfLayouts->ImportLayout", ProblemType.MESSAGE,ChildFile + " did not exist ************************");
							done = true;
						}
						counter++;


					}

				}
				//Console.WriteLine (errorCode);
				MyDatabase.Dispose ();
			}

			
			return errorCode;
		}
		public static void ExportLayout (string GUID, string FileName)
		{
		
			BaseDatabase MyDatabase = CreateDatabase ();
			string value = MyDatabase.GetBackupRowAsString (dbConstants.table_name, dbConstants.GUID, GUID, true) [0];
			if (value != Constants.BLANK) {
			//	NewMessage.Show("Writing " + FileName);
				WriteFile (FileName, value);
			} else {
				//NewMessage.Show("Skipped " + FileName);
			}

//			List<object[]> Deletable = MyDatabase.GetValues(dbConstants.table_name, new string[1] {dbConstants.GUID}, dbConstants.PARENT_GUID, GUID);
//			if (Deletable != null)
//			{
//				foreach (object[] oArray  in Deletable)
//				{
//					if (oArray.Length >0 )
//					{
//						ExportLayout (oArray[0].ToString(),String.Format ("{1}__child{0}.txt",FileName) );
//					}
//				}
//			}



			// Attempt 1
			//now export any page with the GUId as their ParentGuid
			ExportChildLayout (GUID, FileName, MyDatabase);


			MyDatabase.Dispose ();

		}
		static void ExportChildLayout (string GUID, string FileName, BaseDatabase MyDatabase)
		{
			List<string> children = MyDatabase.GetBackupRowAsString (dbConstants.table_name, dbConstants.PARENT_GUID, GUID, true);
			if (children != null && children.Count > 0) {
				for (int i = 0; i < children.Count; i++) {
					WriteFile (String.Format ("{1}__child{0}.txt", i.ToString (), FileName), children [i]);

				}
			}
			// Nope: Was actually working *but* again the new notes are messed up. All Child Panels are suppose to have the PARENTGUID of the the Master Layout (the absolute parent)
//			// now we need the GUIDS for each of these children layouts
//			List<object[]> results = MyDatabase.GetValues(dbConstants.table_name, new string[1]{ dbConstants.GUID}, dbConstants.PARENT_GUID, GUID);
//			if (results != null)
//			{
//				foreach (object[] result in results)
//				{
//					if (result.Length > 0)
//					{
//						string childGUID = result[0].ToString();
//						//ExportChildLayout(childGUID
//					}
//				}
//			}

		}

		static List<string> GetListOfLayoutsChangedRecently (int hours)
		{
			BaseDatabase MyDatabase = CreateDatabase ();
			DateTime ToCheck = DateTime.Now.AddHours (-hours);
			DateTime AboutNow = DateTime.Now;
			string DateComparer = String.Format ("({0} >= @DateStart) AND ({0} < @DateEnd)", dbConstants.DATEEDITED);
			List<string> Results = MyDatabase.ExecuteCommandMultiple (String.Format ("Select {0} from {1} where {2}", dbConstants.GUID, dbConstants.table_name, 
			                                                   DateComparer), ToCheck, AboutNow, false);
			List<string> ResultList = new  List<string>();
			if (Results != null) {
				foreach (string s in Results)
				{

						ResultList.Add (s);

				}

			}
			MyDatabase.Dispose();
			return ResultList;
		}

	
		public static void ExportRecent ()
		{
			BaseDatabase MyDatabase = CreateDatabase ();


			// get list of GUIDS changed in the last 12 hours
			List<string> GuidsChanged = GetListOfLayoutsChangedRecently (12);

			// Create Export/Timed, if necessary
			string exportDirectory = Path.Combine (LayoutDetails.Instance.Path, "export");
			exportDirectory = Path.Combine (exportDirectory, "timed");
			if (!Directory.Exists (exportDirectory)) {
				Directory.CreateDirectory (exportDirectory);
			} else {
				// Delete all files in Export/Timed

				string[] files = Directory.GetFiles (exportDirectory);
				foreach (string s in files) {
					File.Delete (s);
				}

			}
			int count = 0;
			//string temp = "";
			foreach (string guid in GuidsChanged) {
			//	temp = temp + Environment.NewLine + guid;
				count++;
				string THISGUID = guid;
				THISGUID = guid.Replace ("{","");
				THISGUID = THISGUID.Replace ("}","");
				string name = MasterOfLayouts.GetNameFromGuid(THISGUID);
				string filename = String.Format ("{0}_{1}{2}",name, THISGUID, ".txt");
				filename = FileUtils.MakeValidFilename(filename);
				ExportLayout(guid, Path.Combine (exportDirectory,filename));

			}
			NewMessage.Show (Loc.Instance.GetStringFmt("Exported {0} files", count));

			//NewMessage.Show (temp);



			MyDatabase.Dispose ();
		}

		/// <summary>
		/// Searchs for. Searches the entire database for any layout with a note mentoining ths search phrase
		/// </summary>
		/// <param name='fish'>
		/// Fish.
		/// </param>
		public static void SearchFor (string search_phrase)
		{

			// I think this is good enough, I don't need full text search

			BaseDatabase MyDatabase = CreateDatabase ();
			List<object[]> results = MyDatabase.GetValues (dbConstants.table_name, dbConstants.Columns, "any", "*", "", String.Format ("and xml like '%{0}%'", search_phrase));
			string resultnames = Constants.BLANK;
			if (results != null) {
				if (results.Count > 0)
				{
					foreach (object[] objectAray in results)
					{
						resultnames = resultnames +  " " + objectAray[dbConstants.NAME.Index].ToString ();
					}
				}
			}

			NewMessage.Show (resultnames);
			MyDatabase.Dispose();
		}

	}
}

