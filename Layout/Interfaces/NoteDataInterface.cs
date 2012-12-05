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

		#endregion
		#region System
		string GuidForNote {get;set;}
		NotePanel Parent {get;set;}
		#endregion
		#endregion




		// Methods

		/// <summary>
		/// Creates the parent. Called when creating a new Note, or Loading a Note file
		/// </summary>
		/// <param name='Layout'>
		/// Layout.
		/// </param>
		void CreateParent(LayoutPanel Layout);

		/// <summary>
		/// Updates the location, called when location is set outside actual movement.
		/// </summary>
		void UpdateLocation ();
	}

}

