using System;
using Layout;
using LayoutPanels;
namespace YOM2013
{
	public class LayoutsInMemory
	{
		bool maximized=false;
		public bool Maximized { get { return maximized; } set {maximized = value;}}
		public NoteDataXML_SystemOnly Container;
		public LayoutPanel LayoutPanel;
		public string GUID="";
		public LayoutsInMemory ()
		{
		}

	}
}

