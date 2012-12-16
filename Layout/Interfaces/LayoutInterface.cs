using System;
using System.Collections.Generic;

namespace Layout
{
	/// <summary>
	/// This is the FILE/OR EQUIVALENT that is opened for the ENTIRE Layout
	/// </summary>
	public interface LayoutInterface
	{



		#region variables
		string Status {get;set;}
		string Name { get; set; }
		bool ShowTabs { get; set; }
		#endregion


		// The list of Notes
		//List<NoteDataInterface> DataForThisLayout {get; set;}
		void Add(NoteDataInterface note);


		// Methods


		/// <summary>
		/// Loads from the appropriate Data Location.
		/// </summary>
		/// <param name='GUID'>
		/// The unique identifier representing this Layout
		/// </param>
		bool LoadFrom(LayoutPanelBase parent);

		bool SaveTo ();

		string Backup();
		bool IsLayoutExists (string GUID);
		/// <summary>
		/// Gets the notes.
		/// </summary>
		/// <returns>
		/// The notes.
		/// </returns>
		System.Collections.ObjectModel.ReadOnlyCollection<NoteDataInterface>  GetNotes();
		System.Collections.ArrayList GetAllNotes ();
		NoteDataInterface GetNoteByGUID(string GUID);
		System.Collections.Generic.List<NoteDataInterface> GetAvailableFolders ();
		bool IsNoteExistsInLayout(string NoteGUID);
		void RemoveNote (NoteDataInterface NoteToMove);
		void UpdateListOfNotes();
		//List<LayoutDetails.NameAndGuid> GetListOfLayouts (string filter);

	}
}

