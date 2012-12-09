using System;
using System.Drawing;

namespace Layout
{

	public interface NoteDataInterface
	{
		#region variables
		#region UserSet
		string Caption {get; set;}
		Point Location {get; set;}
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
		NotePanel Parent {get;set;}
		#endregion
		#endregion




		// Methods
		#region methods
		/// <summary>
		/// Creates the parent. Called when creating a new Note, or Loading a Note file
		/// </summary>
		/// <param name='Layout'>
		/// Layout.
		/// </param>
		void CreateParent(LayoutPanelBase Layout);
		void Save();
		void Update(LayoutPanelBase Layout);


		/// <summary>
		/// Updates the location, called when location is set outside actual movement.
		/// </summary>
		void UpdateLocation ();

		#endregion;
	}

}

