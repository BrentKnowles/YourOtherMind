using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using database;
using Layout.data;
namespace Layout
{
	/// <summary>
	/// Effectively the FILE in memory for a particular Layout
	/// </summary>
	public class LayoutDatabase : LayoutInterface
	{
		//variables
		private List<NoteDataInterface> dataForThisLayout= null;

		// This is the GUID for the page. It comes from
		//  (a) Either set during the Load Method
		//  (b) When a New Layout is Created
		private string LayoutGUID = CoreUtilities.Constants.BLANK;
		public LayoutDatabase (string GUID)
		{
			dataForThisLayout = new List<NoteDataInterface> ();
			LayoutGUID = GUID;


			// Create the database
			// will always try to create the database if it is not present. The means creating the pages table too.
			CreateTestDatabase();

		}

		/// <summary>
		/// just a testing wrapper
		/// </summary>
		/// <returns>
		/// The test database.
		/// </returns>
		private SqlLiteDatabase CreateTestDatabase()
		{
			SqlLiteDatabase db = new SqlLiteDatabase ("yomdata.s3db");
			db.CreateTableIfDoesNotExist (tmpDatabaseConstants.table_name, new string[4] {tmpDatabaseConstants.ID, tmpDatabaseConstants.GUID, tmpDatabaseConstants.XML,
				tmpDatabaseConstants.STATUS}, 
			new string[4] {
				"INTEGER",
				"TEXT UNIQUE",
				"LONGTEXT",
				"TEXT"
			}, String.Format ("{0}", tmpDatabaseConstants.ID)
			);
			return db;
		}

		/// <summary>
		/// Gets the notes. Gets the notes. (Read-only only so that ONLY this class is able to modify the contents of the list)
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
		
		public void LoadFrom (string GUID)
		{
			LayoutGUID = GUID;
			
			
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

			SqlLiteDatabase MyDatabase = CreateTestDatabase ();

			if (MyDatabase == null) {
				throw new Exception("Unable to create database in SaveTo");
			}


			// we are storing a LIST of NoteDataXML objects
			//	CoreUtilities.General.Serialize(dataForThisLayout, CreateFileName());
			try {
				
				NoteDataXML[] ListAsDataObjectsOfType = new NoteDataXML[dataForThisLayout.Count];
				dataForThisLayout.CopyTo (ListAsDataObjectsOfType);
				
			
			
			  System.Xml.Serialization.XmlSerializer x3 = 
					new System.Xml.Serialization.XmlSerializer (typeof(NoteDataXML[]), 
					                                            new Type[1] {typeof(NoteDataXML)});
				
				

				System.IO.StringWriter sw = new System.IO.StringWriter();
				//sw.Encoding = "";
				x3.Serialize (sw, ListAsDataObjectsOfType);
				//x3.Serialize (sw, ListAsDataObjectsOfType,ns, "utf-8");
				XMLAsString =sw.ToString();
				sw.Close ();


				// here is where we need to test whether the Data exists

				// IF NOT, Insert
				MyDatabase.InsertData(tmpDatabaseConstants.table_name, 
				                      tmpDatabaseConstants.Columns,
				                      new object[tmpDatabaseConstants.ColumnCount]
				                      {"NULL",LayoutGUID, XMLAsString, "this status update"});

				//TODO: Test that "" will still allow an automincrement on ID


				// TODO IF Data does exist then we need to do an update





			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}
		
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
		public void Add(NoteDataInterface note)
		{
			dataForThisLayout.Add((NoteDataXML)note);
		}
		//would this list actually be a NoteDataList BARTER type of class that stores the List
		//this class is actually what ANOTHER page might call (for Random Tables)
	}
}

