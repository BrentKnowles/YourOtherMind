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



		// just a debug counter to see if anything weird is happening on save and load
		protected int debug_ObjectCount = 0;
		// keeps track of whether we are tryign to save. Only one at a time allowed
		private bool AmSaving = false;
		//This is the list of notes
		private List<NoteDataInterface> dataForThisLayout= null;
		//These are the OTHER variables (like status) associated with this note
		private const  int data_size = 4; // size of data array
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

		public bool ShowTabs {
			get { return (bool)data [2];}
			set { data [2] = value;}
		}
		/// <summary>
		/// Gets or sets a value indicating whether this instance is sub panel.
		/// 
		/// If true this means this is a subpanel that is owned by another note.
		/// Should never be able to be opened directly
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is sub panel; otherwise, <c>false</c>.
		/// </value>
		public bool IsSubPanel {
			get { return (bool)data [3];}
			set { data [3] = value;}
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
			ShowTabs = true;
			IsSubPanel = false;

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
		/// Wrapper function for creating the database
		/// This is only place in this code where we reference the TYPE of database being used
		/// </summary>
		/// <returns>
		/// 
		/// </returns>
		private BaseDatabase CreateDatabase()
		{
			return MasterOfLayouts.CreateDatabase();




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
		/// TEMP HACK
		/// 
		/// For loading notes from the original YOM
		/// 
		/// Loads from the appropriate Data Location.
		/// </summary>
		/// <param name='GUID'>
		/// The unique identifier representing this Layout
		/// </param>
		/// <returns>
		/// <c>true</c>, if from was loaded, <c>false</c> otherwise.
		/// </returns>
		/// <param name='sFile'>
		/// S file.
		/// </param>
		public bool LoadFromOld (string sFile)
		{
			Name = sFile;
			Status = "Imported";
			ShowTabs = false;

			XmlSerializer serializer = new XmlSerializer (typeof(NoteDataXML[]), 
			                                              new Type[1] {typeof(NoteDataXML_RichText)});
			
			System.IO.StreamReader reader = new System.IO.StreamReader (sFile);
			NoteDataXML[] cars = (NoteDataXML[])serializer.Deserialize (reader);

			if (null != cars) {
				dataForThisLayout = new List<NoteDataInterface> ();
				for (int i = 0; i < cars.Length; i++) {
					dataForThisLayout.Add (cars [i]);

				}
			
			
			}
		

			reader.Close ();
			return true;
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

			List<object[]> myList = MyDatabase.GetValues (tmpDatabaseConstants.table_name, tmpDatabaseConstants.Columns
			                      , tmpDatabaseConstants.GUID, LayoutGUID);
			if (myList != null && myList.Count > 0) {

				object[] result = myList [0];


				// deserialize from database and load into memor
				if (result != null && result.Length > 0) {


					// Fill in LAYOUT specific details
					Status = result [3].ToString ();
					Name = result [4].ToString ();
					if (result [5].ToString () != Constants.BLANK) {
						ShowTabs = (bool)result [5];
					} else {
						//ToDo: This does not seem growable easily
						ShowTabs = true;
					}
					if (result [6].ToString () != Constants.BLANK) {
						IsSubPanel = (bool)result[6];
					}else {
						IsSubPanel = false;
					}

					// Fill in XML details

					//dataForThisLayout
					System.IO.StringReader reader = new System.IO.StringReader (result [2].ToString ());
					System.Xml.Serialization.XmlSerializer test = new System.Xml.Serialization.XmlSerializer (typeof(System.Collections.Generic.List<NoteDataXML>),
					                                                                                         LayoutDetails.Instance.ListOfTypesToStoreInXML ());
					List<NoteDataXML> ListAsDataObjectsOfType = null;
					try {
						// have to load it in an as array of target type and then convert
						ListAsDataObjectsOfType = (System.Collections.Generic.List<NoteDataXML>)test.Deserialize (reader);
					} catch (System.InvalidOperationException e) {
						string exception = e.ToString ();
						int pos = exception.IndexOf ("name");
						int pos2 = exception.IndexOf ("namespace");
						if (pos2 < pos)
							pos2 = pos + 1;

						string notetype = exception.Substring (pos, pos2 - pos - 2);
						string message = 
			Loc.Instance.Cat.GetStringFmt ("The notetype {0} was present in this Layout but not loaded in memory! This means that the AddIn used to add this note to this Layout has been removed. Loading of this layout is now aborted. Please exit this note and close it to prevent data loss and reinstall the disabled AddIn", notetype);
						NewMessage.Show (message);
					} catch (Exception ex) {
						throw new Exception (ex.ToString ());
					}
					if (null != ListAsDataObjectsOfType) {
						dataForThisLayout = new List<NoteDataInterface> ();
						for (int i = 0; i < ListAsDataObjectsOfType.Count; i++) {
							dataForThisLayout.Add (ListAsDataObjectsOfType [i]);
							if (null != LayoutPanelToLoadNoteOnto) {
								ListAsDataObjectsOfType [i].CreateParent (LayoutPanelToLoadNoteOnto);
							}
						}
						Success = true;
						debug_ObjectCount = ListAsDataObjectsOfType.Count;
					}
				}
			}

			long workingSet = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;
			lg.Instance.Line("LoadFrom", ProblemType.TEMPORARY, workingSet.ToString());
			return Success;

		}
		/// <summary>
		/// Counts the system notes.
		/// Needed to prevent system notes from being saved out
		/// </summary>
		/// <returns>
		/// The system notes.
		/// </returns>
		private int CountSystemNotes ()
		{
			int count = 0;
			foreach (NoteDataInterface note in dataForThisLayout) {
				if (note.GetType () == typeof(NoteDataXML_SystemOnly)) {
					count++;
				}
			}
			return count;
		}


		/// <summary>
		/// Saves to the XML file.
		/// 
		/// Notice the GUID is NOT allowed to be passed in. It is stored in the document when Loaded or Created
		/// </summary>
		/// <returns>
		/// The to.
		/// </returns>
		public bool SaveTo ()
		{
			bool saveworked= false;
			if (false == AmSaving) {
				AmSaving = true;

				if (LayoutGUID == CoreUtilities.Constants.BLANK) {
					throw new Exception ("GUID need to be set before calling SaveTo");
				}
				string XMLAsString = CoreUtilities.Constants.BLANK;

				BaseDatabase MyDatabase = CreateDatabase ();

				if (MyDatabase == null) {
					throw new Exception ("Unable to create database in SaveTo");
				}


				// we are storing a LIST of NoteDataXML objects
				//	CoreUtilities.General.Serialize(dataForThisLayout, CreateFileName());
				try {
				
					NoteDataXML[] ListAsDataObjectsOfType = new NoteDataXML[dataForThisLayout.Count-CountSystemNotes()];


				//	dataForThisLayout.CopyTo (ListAsDataObjectsOfType);
				

					// Remove System Notes (these should never end up in save data
					for (int i = dataForThisLayout.Count-1 ; i >= 0; i--)
					{
						if (dataForThisLayout[i].GetType () != typeof(NoteDataXML_SystemOnly))
						{
							ListAsDataObjectsOfType[i] = (NoteDataXML)dataForThisLayout[i];
						}
					}

					// doing some data tracking to try to detect save failures later
					if (ListAsDataObjectsOfType.Length < debug_ObjectCount) {
						lg.Instance.Line ("LayoutDatabase.SaveTo", ProblemType.WARNING, String.Format ("Less objects being saved than loaded. Loaded {0}. Saved {0}.",
					                                                                              ListAsDataObjectsOfType.Length, debug_ObjectCount));
					}

					foreach (NoteDataInterface note in ListAsDataObjectsOfType) {
						// saves the actual UI elements
						note.Save ();

					}
					debug_ObjectCount = ListAsDataObjectsOfType.Length;
			
					System.Xml.Serialization.XmlSerializer x3 = 
					new System.Xml.Serialization.XmlSerializer (typeof(NoteDataXML[]), 
					                                            LayoutDetails.Instance.ListOfTypesToStoreInXML ());

					/*This worked but would need to iterate and get a list of all potential types present, not just the first, else it will fail withmixed types
				System.Xml.Serialization.XmlSerializer x4 = 
					new System.Xml.Serialization.XmlSerializer (ListAsDataObjectsOfType.GetType(), 
					                                            new Type[1] {ListAsDataObjectsOfType[0].GetType()});
*/
					System.IO.StringWriter sw = new System.IO.StringWriter ();
					//sw.Encoding = "";
					x3.Serialize (sw, ListAsDataObjectsOfType);
					//x3.Serialize (sw, ListAsDataObjectsOfType,ns, "utf-8");
					XMLAsString = sw.ToString ();
					sw.Close ();


					// here is where we need to test whether the Data exists

					if (MyDatabase.Exists (tmpDatabaseConstants.table_name, tmpDatabaseConstants.GUID, LayoutGUID) == false) {
						lg.Instance.Line ("LayoutDatabase.SaveTo", ProblemType.MESSAGE, "We have new data. Adding it.");
						// IF NOT, Insert
						MyDatabase.InsertData (tmpDatabaseConstants.table_name, 
				                      tmpDatabaseConstants.Columns,
				                      new object[tmpDatabaseConstants.ColumnCount]
				                      {DBNull.Value,LayoutGUID, XMLAsString, Status,Name,ShowTabs, IsSubPanel});

					} else {
						//TODO: Still need to save all the object properties out. And existing data.

						lg.Instance.Line ("LayoutDatabase.SaveTo", ProblemType.MESSAGE, "We are UPDATING existing Row." + LayoutGUID);
						MyDatabase.UpdateSpecificColumnData (tmpDatabaseConstants.table_name, 
					                                    new string[tmpDatabaseConstants.ColumnCount - 1]
						                                     {tmpDatabaseConstants.GUID, tmpDatabaseConstants.XML, tmpDatabaseConstants.STATUS,
						tmpDatabaseConstants.NAME, tmpDatabaseConstants.SHOWTABS, tmpDatabaseConstants.SUBPANEL},
				                                    new object[tmpDatabaseConstants.ColumnCount - 1]
				                                    {
						LayoutGUID as string, 
						XMLAsString as string, 
						Status as string
					,Name,
						ShowTabs,
						IsSubPanel},
				tmpDatabaseConstants.GUID, LayoutGUID);
					}



				} catch (Exception ex) {
					AmSaving = false;
					lg.Instance.Line ("LayoutDatabase.SaveTo", ProblemType.EXCEPTION,"save exception happened " + ex.ToString());
					throw new Exception (String.Format ("Must call CreateParent before calling Save or Update!! Exception: {0}", ex.ToString ()));
					//lg.Instance.Line("LayoutDatabase.SaveTo", ProblemType.EXCEPTION, ex.ToString());
				}
				AmSaving = false;
				saveworked = true;
			} else {
				// we were already saving information when this was called again
				saveworked = false;
			}
			return saveworked;
		}
		public NoteDataXML_SystemOnly GetAvailableSystemNote (LayoutPanelBase LayoutPanel)
		{
			// called from MainForm to request a docking place for a new layout to be loaded
			NoteDataXML_SystemOnly system = new NoteDataXML_SystemOnly(600, 600);
			Add (system);
			system.CreateParent(LayoutPanel);
			return system;
			//return (System.Windows.Forms.Control)system.Parent;
			/*
			foreach (NoteDataInterface note in Notes.GetNotes ()) {
				if (note.GetType() == typeof(NoteDataXML_SystemOnly))
				{
					return (Control)note.Parent;
				}
			}
			return null;*/
		}
		/// <summary>
		/// Determines whether this instance is note exists in layout the specified NoteGUID.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance is note exists in layout the specified NoteGUID; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='NoteGUID'>
		/// Note GUI.
		/// </param>
		public bool IsNoteExistsInLayout (string NoteGUID)
		{
			NoteDataInterface NoteToMove = dataForThisLayout.Find (NoteDataInterface => NoteDataInterface.GuidForNote == NoteGUID);
			if (NoteToMove != null) {
				return true;
			}
			return false;

		}
		/// <summary>
		/// Does the specified GUID exist?
		/// </summary>
		/// <param name='GUID'>
		/// GUI.
		/// </param>
		public bool IsLayoutExists (string GUID)
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




		private static bool FindGUID (NoteDataInterface note, string guid)
		{
			if (note.GuidForNote == "") {
				return true;
			} else {
				return false;
			}
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
		/// <returns>The Layout where the Note was Added to</returns>
		public void RemoveNote (NoteDataInterface NoteToMove)
		{
			//NoteDataInterface NoteToMove = dataForThisLayout.Find (NoteDataInterface => NoteDataInterface.GuidForNote == GUIDOfNoteToMove);
			if (NoteToMove != null) {
				NoteToMove.Destroy();
				if (dataForThisLayout.Remove (NoteToMove) == false)
				{
					lg.Instance.Line("LayoutDatabase.MoveNote", ProblemType.WARNING,"Was unable to REmove this note");
				}
			}



			/*
			// Take 1 -- seemed too complicated
			string GUIDOfLayoutThatOwnsIt = this.LayoutGUID;

			SaveTo ();
			LayoutDatabase newLayout = null;

			// grab note out of the list
			NoteDataInterface NoteToMove = dataForThisLayout.Find (NoteDataInterface=>NoteDataInterface.GuidForNote == GUIDOfNoteToMove);
			if (NoteToMove != null) {
				Console.WriteLine ("moving " + NoteToMove.Caption);
				newLayout = new LayoutDatabase(GUIDOfLayoutToMoveItTo);
				newLayout.LoadFrom (null);
				newLayout.Add (NoteToMove);
				try
				{
					Console.WriteLine ("before save");
				newLayout.SaveTo();
					Console.WriteLine ("after save");
				}
				catch (Exception ex)
				{
					Console.WriteLine (ex.ToString());
				}

				// how to load new object
				if (dataForThisLayout.Remove (NoteToMove) == false)
				{
					lg.Instance.Line("LayoutDatabase.MoveNote", ProblemType.WARNING,"Was unable to REmove this note");
				}
				// We do the rest of these steps in the LayoutPanel that called this routine
				// need to reload the Layout We Added To First before saving this layotu
				//SaveTo();
			}
			Console.WriteLine(String.Format ("MOVE DONE {0} {1} {2}", GUIDOfNoteToMove, GUIDOfLayoutThatOwnsIt, GUIDOfLayoutToMoveItTo));
			return newLayout;*/
		
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
					(note as NoteDataXML_NoteList).UpdateListOfNotesOnLayout();
				}
			}


		}
	}
}

