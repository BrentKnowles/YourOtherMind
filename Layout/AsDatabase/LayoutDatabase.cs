using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using database;
using Layout.data;
using CoreUtilities;
namespace Layout
{
	/// <summary>
	/// Effectively the FILE in memory for a particular Layout
	/// </summary>
	public class LayoutDatabase : LayoutInterface
	{
		#region variables

		// we keep this as a protected string so that unit testing can override it 
		protected virtual string YOM_DATABASE {
			get { return "yomdata.s3db";}
		}

		// TODO: this variable is stored on LOAD and a simple message is written out on Save if it is less than it started out being
		protected int debug_ObjectCount = 0;

		//This is the list of notes
		private List<NoteDataInterface> dataForThisLayout= null;
		//These are the OTHER variables (like status) associated with this note
		private const  int data_size = 2; // size of data array
		private object[] data= null;
		public string Status {
			get { return data [0].ToString();}
			set { data[0] = value;}
		}
		/// <summary>
		/// Gets or sets the name for this Layout.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name {
			get { return data [1].ToString ();}
			set { data [1] = value;}
		}


		// This is the GUID for the page. It comes from
		//  
		//  (a) When a New Layout is Created or  a Layout Loaded (in both cases constructor is called
		private string LayoutGUID = CoreUtilities.Constants.BLANK;

		#endregion
		public LayoutDatabase (string GUID)
		{
			dataForThisLayout = new List<NoteDataInterface> ();
			LayoutGUID = GUID;


			// Create the database
			// will always try to create the database if it is not present. The means creating the pages table too.
			CreateDatabase();
			data = new object[data_size];

			// defaults
			Status = " default status";
			Name = "default name";

		}
		/// <summary>
		/// Gets the note by GUI.
		/// </summary>
		/// <returns>
		/// The note by GUI. 
		/// Null if not found.
		/// </returns>
		/// <param name='GUID'>
		/// GUI.
		/// </param>
		public NoteDataInterface GetNoteByGUID (string GUID)
		{
			foreach (NoteDataInterface note in GetNotes ()) {
				if (note.GuidForNote == GUID)
				{
					return note;
				}
			}
			return null;
		}

		/// <summary>
		/// Backup the entire layout database, NOT just this layout (though I could tweak it to do so)
		/// </summary>
		public string Backup ()
		{
			SqlLiteDatabase db = new SqlLiteDatabase (YOM_DATABASE);
			return db.BackupDatabase();
		}
		/// <summary>
		/// Wrapper function for creating the database
		/// This is only place in this code where we reference the TYPE of database being used
		/// </summary>
		/// <returns>
		/// 
		/// </returns>
		private BaseDatabase CreateDatabase()
		{


			SqlLiteDatabase db = new SqlLiteDatabase (YOM_DATABASE);
			db.CreateTableIfDoesNotExist (tmpDatabaseConstants.table_name, 
			    tmpDatabaseConstants.Columns, 
			tmpDatabaseConstants.Types, String.Format ("{0}", tmpDatabaseConstants.ID)
			);
			return db;
		}
		/// <summary>
		/// Adds the children. If a panel notetype
		/// </summary>
		/// <returns>
		/// The children.
		/// </returns>
		/// <param name='data'>
		/// Data.
		/// </param>
		/// <param name='result'>
		/// Result.
		/// </param>
		private void AddChildrenToGetList (NoteDataInterface data,  System.Collections.ArrayList result)
		{
			lg.Instance.Line("AddChildrenToList", ProblemType.TEMPORARY, "checking...");
			if ( data.IsPanel == true)
			{
				lg.Instance.Line("AddChildrenToList", ProblemType.TEMPORARY, "is a Panel...");
				foreach (NoteDataInterface data2 in data.GetChildNotes())
				{
					lg.Instance.Line("AddChildrenToList", ProblemType.TEMPORARY, "scanning children...");
					result.Add (data2);
					AddChildrenToGetList(data2,  result);
				}
			}

		}
		/// <summary>
		/// Gets all notes.
		/// </summary>
		/// <returns>
		/// The all notes.
		/// </returns>
		public System.Collections.ArrayList GetAllNotes ()
		{
			System.Collections.ArrayList result = new System.Collections.ArrayList ();
			foreach (NoteDataInterface data in dataForThisLayout) {
				result.Add (data);
				 AddChildrenToGetList(data,  result);

			}
			return result;

		}
		/// <summary>
		/// Gets the notes. Gets the notes. (Read-only only so that ONLY this class is able to modify the contents of the list)
		/// ONLY: Gets the notes on this layout. For children notes too then use GetAllNotes()
		/// USAGE: foreach (NoteDataXML notes in layout.GetNotes())
		///  or
		/// As a Datasource
	    /// </summary>
		/// <returns>
		/// The notes. READ ONLY
		/// </returns>
		public System.Collections.ObjectModel.ReadOnlyCollection<NoteDataInterface> GetNotes()
		{

			return dataForThisLayout.AsReadOnly();
		}

		/// <summary>
		/// Creates the name of the file.
		/// </summary>
		/// <returns>
		/// The file name.
		/// </returns>
		private string CreateFileName()
		{
			return LayoutGUID;
		}



		/// <summary>
		/// Loads from the appropriate Data Location. If pass Layout as null this is a REMOTE NOTE load, meaning we are grabbing this information without
		///    the layout being loaded into a LayoutPanel.
		/// </summary>
		/// <param name='GUID'>
		/// The unique identifier representing this Layout
		/// </param>
		/// <returns>
		/// <c>true</c>, if from was loaded, <c>false</c> otherwise.
		/// </returns>
		/// <param name='Layout'>
		/// 
		/// </param>
		public bool LoadFrom (LayoutPanelBase LayoutPanelToLoadNoteOnto)
		{
			BaseDatabase MyDatabase = CreateDatabase ();
			
			if (MyDatabase == null) {
				throw new Exception ("Unable to create database in LoadFrom");
			}
			bool Success = false;


			// we only care about FIRST ROW. THere should never be two matches on a GUID

			List<object[]> myList =  MyDatabase.GetValues (tmpDatabaseConstants.table_name, tmpDatabaseConstants.Columns
			                      , tmpDatabaseConstants.GUID, LayoutGUID);
			if (myList != null && myList.Count > 0)
			{

			object[] result = myList[0];


			// deserialize from database and load into memor
			if (result != null && result.Length > 0) {


				// Fill in LAYOUT specific details
				Status = result [3].ToString ();
				Name = result [4].ToString ();
				// Fill in XML details

				//dataForThisLayout
				System.IO.StringReader reader = new System.IO.StringReader (result [2].ToString ());
				System.Xml.Serialization.XmlSerializer test = new System.Xml.Serialization.XmlSerializer(typeof(System.Collections.Generic.List<NoteDataXML>),
					                                                                                         LayoutDetails.Instance.ListOfTypesToStoreInXML());

				// have to load it in an as array of target type and then convert
				List<NoteDataXML> ListAsDataObjectsOfType = (System.Collections.Generic.List<NoteDataXML>)test.Deserialize (reader);

				dataForThisLayout = new List<NoteDataInterface>();
				for (int i = 0; i < ListAsDataObjectsOfType.Count; i++)
				{
				dataForThisLayout.Add (ListAsDataObjectsOfType[i]);
				if (null != LayoutPanelToLoadNoteOnto)
				{
					ListAsDataObjectsOfType[i].CreateParent(LayoutPanelToLoadNoteOnto);
				}
				}
				Success = true;
					debug_ObjectCount = ListAsDataObjectsOfType.Count;
				}
			} 


			return Success;

		}
		/// <summary>
		/// Saves to the XML file.
		/// 
		/// Notice the GUID is NOT allowed to be passed in. It is stored in the document when Loaded or Created
		/// </summary>
		/// <returns>
		/// The to.
		/// </returns>
		public void SaveTo ()
		{
			if (LayoutGUID == CoreUtilities.Constants.BLANK) {
				throw new Exception ("GUID need to be set before calling SaveTo");
			}
			string XMLAsString = CoreUtilities.Constants.BLANK;

			BaseDatabase MyDatabase = CreateDatabase ();

			if (MyDatabase == null) {
				throw new Exception("Unable to create database in SaveTo");
			}


			// we are storing a LIST of NoteDataXML objects
			//	CoreUtilities.General.Serialize(dataForThisLayout, CreateFileName());
			try {
				
				NoteDataXML[] ListAsDataObjectsOfType = new NoteDataXML[dataForThisLayout.Count];
				dataForThisLayout.CopyTo (ListAsDataObjectsOfType);
				
				// doing some data tracking to try to detect save failures later
				if (ListAsDataObjectsOfType.Length < debug_ObjectCount)
				{
					lg.Instance.Line("LayoutDatabase.SaveTo", ProblemType.WARNING, String.Format ("Less objects being saved than loaded. Loaded {0}. Saved {0}.",
					                                                                              ListAsDataObjectsOfType.Length , debug_ObjectCount));
				}

				foreach (NoteDataInterface note in ListAsDataObjectsOfType)
				{
					// saves the actual UI elements
					note.Save();

				}
				debug_ObjectCount = ListAsDataObjectsOfType.Length;
			
			  System.Xml.Serialization.XmlSerializer x3 = 
					new System.Xml.Serialization.XmlSerializer (typeof(NoteDataXML[]), 
					                                            LayoutDetails.Instance.ListOfTypesToStoreInXML());

				/*This worked but would need to iterate and get a list of all potential types present, not just the first, else it will fail withmixed types
				System.Xml.Serialization.XmlSerializer x4 = 
					new System.Xml.Serialization.XmlSerializer (ListAsDataObjectsOfType.GetType(), 
					                                            new Type[1] {ListAsDataObjectsOfType[0].GetType()});
*/
				System.IO.StringWriter sw = new System.IO.StringWriter();
				//sw.Encoding = "";
				x3.Serialize (sw, ListAsDataObjectsOfType);
				//x3.Serialize (sw, ListAsDataObjectsOfType,ns, "utf-8");
				XMLAsString =sw.ToString();
				sw.Close ();


				// here is where we need to test whether the Data exists

				if (MyDatabase.Exists(tmpDatabaseConstants.table_name, tmpDatabaseConstants.GUID, LayoutGUID) == false)
				    {
					lg.Instance.Line("LayoutDatabase.SaveTo", ProblemType.MESSAGE, "We have new data. Adding it.");
				// IF NOT, Insert
				MyDatabase.InsertData(tmpDatabaseConstants.table_name, 
				                      tmpDatabaseConstants.Columns,
				                      new object[tmpDatabaseConstants.ColumnCount]
				                      {"NULL",LayoutGUID, XMLAsString, Status,Name});

				}
				else
				{
					//TODO: Still need to save all the object properties out. And existing data.

					lg.Instance.Line("LayoutDatabase.SaveTo", ProblemType.MESSAGE, "We are UPDATING existing Row." + LayoutGUID);
				MyDatabase.UpdateSpecificColumnData(tmpDatabaseConstants.table_name, 
					                                    new string[tmpDatabaseConstants.ColumnCount-1]{tmpDatabaseConstants.GUID, tmpDatabaseConstants.XML, tmpDatabaseConstants.STATUS,
						tmpDatabaseConstants.NAME},
				                                    new object[tmpDatabaseConstants.ColumnCount-1]
				                                    {
						LayoutGUID as string, 
						XMLAsString as string, 
						Status as string
					,Name},
				tmpDatabaseConstants.GUID, LayoutGUID);
				}



			} catch (Exception ex) {

				throw new Exception(String.Format ("Must call CreateParent before calling Save or Update!! Exception: {0}", ex.ToString()));
				//lg.Instance.Line("LayoutDatabase.SaveTo", ProblemType.EXCEPTION, ex.ToString());
			}
		
		}
		/// <summary>
		/// Does the specified GUID exist?
		/// </summary>
		/// <param name='GUID'>
		/// GUI.
		/// </param>
		public bool Exists (string GUID)
		{
			BaseDatabase MyDatabase = CreateDatabase ();
			return MyDatabase.Exists (tmpDatabaseConstants.table_name, tmpDatabaseConstants.GUID, GUID);
		}
		/// <summary>
		/// Gets the available folders. (notes in this collection that are of the folder type
		/// </summary>
		/// <returns>
		/// The available folders.
		/// </returns>
		public System.Collections.Generic.List<NoteDataInterface> GetAvailableFolders ()
		{
			System.Collections.Generic.List<NoteDataInterface> result = new System.Collections.Generic.List<NoteDataInterface> ();
			foreach (NoteDataInterface note in GetNotes ()) {
				if (true == note.IsPanel)
					result.Add (note);
			}
			return result;
		}
		/// <summary>
		/// Moves the note from one Layout to another.
		/// 
		/// THe layout that is OWNING this currently is the one that called the MOVE
		/// </summary>
		/// <param name='GUIDOfNoteToMove'>
		/// GUID of note to move.
		/// </param>
		/// <param name='GUIDOfLayoutToMoveItTo'>
		/// GUID of layout to move it to.
		/// </param>
		public void MoveNote (string GUIDOfNoteToMove, string GUIDOfLayoutToMoveItTo)
		{
			string GUIDOfLayoutThatOwnsIt = this.LayoutGUID;
			Console.WriteLine(String.Format ("{0} {1} {2}", GUIDOfNoteToMove, GUIDOfLayoutThatOwnsIt, GUIDOfLayoutToMoveItTo));
		}


		/*Not needed?
		private List<NoteDataInterface> DataForThisLayout {
			get { return dataForThisLayout;}
			set { dataForThisLayout = value;}
		}
		*/
		/// <summary>
		/// Add the specified note to the DataForThisLayout list
		/// </summary>
		/// <param name='note'>
		/// Note.
		/// </param>
		public void Add (NoteDataInterface note)
		{
			if (null != note) {
				dataForThisLayout.Add ((NoteDataXML)note);
			}
		}
		/// <summary>
		/// Searches for a NoteList note and calls its update function [does it call them all? Yeah, probably]
		/// </summary>
		public void UpdateListOfNotes ()
		{
			// search for a NoteDataXLM_NOteList
			foreach (NoteDataInterface note in GetNotes ()) {
				if (note is NoteDataXML_NoteList)
				{
					(note as NoteDataXML_NoteList).UpdateList(GetNotes ());
				}
			}


		}
	}
}

