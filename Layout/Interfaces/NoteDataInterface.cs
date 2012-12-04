using System;

namespace Layout
{

	public interface NoteDataInterface
	{
		#region variables
		string Caption {get; set;}
		string GuidForNote {get;set;}
		#endregion

		NotePanelInterface Parent {get;set;}


		// Methods
		void CreateParent();
	}
}

