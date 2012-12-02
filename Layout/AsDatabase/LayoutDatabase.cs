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
		#region members

		//This is the list of notes
		private List<NoteDataInterface> dataForThisLayout= null;
		//These are the OTHER variables (like status) associated with this note
		private object[] data= null;
		public string Status {
			get { return data [0].ToString();}
			set { data[0] = value;}
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
			CreateTestDatabase();
			data = new object[1];

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
		
		public void LoadFrom ()
		{
			SqlLiteDatabase MyDatabase = CreateTestDatabase ();
			
			if (MyDatabase == null) {
				throw new Exception ("Unable to create database in SaveTo");
			}



			// we only care about FIRST ROW. THere should never be two matches on a GUID
			object[] result = MyDatabase.GetValues (tmpDatabaseConstants.table_name, new string[2] {
				tmpDatabaseConstants.STATUS,
				tmpDatabaseConstants.XML
			}
			, tmpDatabaseConstants.GUID, LayoutGUID) [0];


			// deserialize from database and load into memor
			if (result != null) {

				Status = result [0].ToString ();
				//dataForThisLayout
				System.IO.StringReader reader = new System.IO.StringReader (result [1].ToString ());
				System.Xml.Serialization.XmlSerializer test = new System.Xml.Serialization.XmlSerializer(typeof(System.Collections.Generic.List<NoteDataXML>));

				// have to load it in an as array of target type and then convert
				List<NoteDataXML> ListAsDataObjectsOfType = (System.Collections.Generic.List<NoteDataXML>)test.Deserialize (reader);

				dataForThisLayout = new List<NoteDataInterface>();
				for (int i = 0; i < ListAsDataObjectsOfType.Count; i++)
				{
				dataForThisLayout.Add (ListAsDataObjectsOfType[i]);
				}

				//ListAsDataObjectsOfType.CopyTo(dataForThisLayout);

				// = new NoteDataXML[dataForThisLayout.Count];
				//dataForThisLayout.CopyTo (ListAsDataObjectsOfType);

			} else {
				throw new Exception("LoadFrom failed to load the object from the database");
			}




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

				if (MyDatabase.Exists(tmpDatabaseConstants.table_name, tmpDatabaseConstants.GUID, LayoutGUID) == false)
				    {
					Console.WriteLine("We have new data. Adding it.");
				// IF NOT, Insert
				MyDatabase.InsertData(tmpDatabaseConstants.table_name, 
				                      tmpDatabaseConstants.Columns,
				                      new object[tmpDatabaseConstants.ColumnCount]
				                      {"NULL",LayoutGUID, XMLAsString, "this status update"});

				}
				else
				{
					//TODO: Still need to save all the object properties out. And existing data.
					Console.WriteLine("We are UPDATING existing Row." + LayoutGUID);

				MyDatabase.UpdateSpecificColumnData(tmpDatabaseConstants.table_name, 
					                                    new string[3]{"guid", "xml", "status"},
				                                    new object[tmpDatabaseConstants.ColumnCount-1]
				                                    {
						LayoutGUID as string, 
						XMLAsString as string, 
						"this status NOW UPDATED!" as string},
				tmpDatabaseConstants.GUID, LayoutGUID);
				}



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

