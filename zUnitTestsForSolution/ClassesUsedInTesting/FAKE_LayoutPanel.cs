using System;
using Layout;
namespace Testing
{
	public class FAKE_LayoutPanel : LayoutPanel
	{
		public FAKE_LayoutPanel (string GUID, bool IsSubPanel) : base(GUID, IsSubPanel)
		{

		}
		public LayoutDatabase GetLayoutDatabase()
		{
			return (LayoutDatabase)Notes;
		}
	}
}

