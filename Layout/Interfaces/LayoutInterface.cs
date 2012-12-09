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
		bool LoadFrom(LayoutPanel parent);

		void SaveTo ();

		string Backup();

		/// <summary>
		/// Gets the notes.
		/// </summary>
		/// <returns>
		/// The notes.
		/// </returns>
		System.Collections.ObjectModel.ReadOnlyCollection<NoteDataInterface>  GetNotes();

	}
}

