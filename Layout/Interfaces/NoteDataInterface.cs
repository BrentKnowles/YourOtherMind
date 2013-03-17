using System;
using System.Drawing;
using System.Windows.Forms;

namespace Layout
{

	public interface NoteDataInterface
	{
		#region variables
#endregion
		#region UserSet
		string Caption {get; set;}
		Point Location {get; set;}
		int Height {get;set;}
		int Width{get;set;}
		bool Visible{get;set;}
		DockStyle Dock {get; set;}
		bool ReadOnly { get; set; }

		/// <summary>
		/// This field will be used to store RTF text in the TextBox, A Link to the picture file for Picture Notes, et cetera.
		/// </summary>
		/// <value>
		/// The data1.
		/// </value>
		string Data1 {get;set;}

		#endregion
		#region System
		string GuidForNote {get;set;}
		NotePanel ParentNotePanel {get;set;}

		// I did not want to do this but because panels are implemented "further down the chain" I need these two functions here
		bool IsPanel{get;} 
		// if true means a NoteLink can link to this
		bool IsLinkable {get;}
		bool IsSystemNote{get;}
		System.Collections.ArrayList   GetChildNotes();
		string GetStoryboardPreview{get;}
		#endregion

		void GetStoryboardData (out string sCaption, out string sValue, out int type, out string extraField);

		string GetLinkData ();




		// Methods
		#region methods
		/// <summary>
		/// Creates the parent. Called when creating a new Note, or Loading a Note file
		/// </summary>
		/// <param name='Layout'>
		/// Layout.
		/// </param>
		void CreateParent(LayoutPanelBase Layout);
		void Update(LayoutPanelBase Layout);
		void Destroy();

		Action<bool> SetSaveRequired { get; set; }
		void Save();
		 void BringToFrontAndShow();
		 void Flash();
		void Maximize (bool Maximize);
		void Minimize();
		string RegisterType();

		/// <summary>
		/// Updates the location, called when location is set outside actual movement.
		/// </summary>
		void UpdateLocation ();

		// returns the GUID of the master containing object for this form
		string GetAbsoluteParent();
		void EndDrag ();
		void UpdateAfterLoad();

		#endregion;
	}

}

