using System;
using System.Collections.Generic;

namespace Layout
{
	/// <summary>
	/// This is the FILE/OR EQUIVALENT that is opened for the ENTIRE Layout
	/// </summary>
	public interface LayoutInterface: IDisposable
	{



		#region variables
		// the name of the Layout
		string Name { get; set; }
		// A status (user defined strings) which represents the state of work on the project (retried, first draft, et cetera)
		string Status {get;set;}
		// Whether to show the tabs bar which is fast acces between notes
		bool ShowTabs { get; set; }
		// if true when the Tabs are clicked on the Layout Toolbar, the note will be displayed maximized
		bool MaximizeTabs { get; set; }
		// true if a subpanel layout (i..e, contained in another panel/layout)
		bool IsSubPanel { get; set; }
		// a rating from 1 to 5
		int Stars {get;set;}
		// how many times this layout has been loaded
		int Hits {get;set;}
		DateTime DateCreated {get;set;}
		DateTime DateEdited{ get; set; }
		string Notebook { get; set; }
		string Section { get; set; }
		string Subtype { get; set; }
		string Source {get;set;}
		int Words {get; set;}
		string Keywords {get;set;}
		System.Drawing.Color BackgroundColor { get; set; }
		string Blurb {get;set;}
		string ParentGuid {get;set;}
		#endregion


		// The list of Notes
		//List<NoteDataInterface> DataForThisLayout {get; set;}
		void Add(NoteDataInterface note);
		void AddToStart(NoteDataInterface note);

		// Methods


		/// <summary>
		/// Loads from the appropriate Data Location.
		/// </summary>
		/// <param name='GUID'>
		/// The unique identifier representing this Layout
		/// </param>
		bool LoadFrom(LayoutPanelBase parent);

		bool SaveTo ();
		int CountNotes ();
		bool Count_Warning_ThereAreLessNotesNowThanWhenWeLoaded ();
		//string Backup();

		bool IsLayoutExists (string GUID);
		/// <summary>
		/// Gets the notes.
		/// </summary>
		/// <returns>
		/// The notes.
		/// </returns>
		System.Collections.ObjectModel.ReadOnlyCollection<NoteDataInterface>  GetNotes();
		NoteDataInterface[] GetNotesSorted ();
		System.Collections.ArrayList GetAllNotes ();
		NoteDataInterface GetNoteByGUID(string GUID);
		System.Collections.Generic.List<NoteDataInterface> GetAvailableFolders ();
		bool IsNoteExistsInLayout(string NoteGUID);
		void RemoveNote (NoteDataInterface NoteToMove);
		void UpdateListOfNotes();

		CoreUtilities.Links.LinkTable GetLinkTable ();
		void CreateLinkTableIfNecessary(NoteDataXML_Table table, LayoutPanelBase LayoutPanelToLoadNoteOnto);
		NoteDataXML_SystemOnly GetAvailableSystemNote (LayoutPanelBase LayoutPanel);
		//List<LayoutDetails.NameAndGuid> GetListOfLayouts (string filter);

	}
}

