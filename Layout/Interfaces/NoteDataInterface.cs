using System;

namespace Layout
{

	public interface NoteDataInterface
	{
		// Fields
		string Caption {get; set;}
		NotePanelInterface Parent {get;set;}


		// Methods
		void CreateParent();
	}
}

