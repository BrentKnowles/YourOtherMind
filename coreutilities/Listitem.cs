using System;

namespace CoreUtilities
{
	/// <summary>
	/// class for representing storage data in listbox (specifically the MiniIndex on StickItPages)
	/// </summary>
	public class Listitem : object
	{
		private string mTag;
		
		public string sTag
		{
			get { return mTag; }
			set { mTag = value; }
		}
		
		private string mItem;
		public string sItem
		{
			get { return mItem; }
			set { mItem = value; }
		}
		private string mType;
		public string PageType
		{
			get { return mType; }
			set { mType = value; }
		}
		public string FancyCaption
		{
			get { return String.Format("{0} ({1})", mItem, mType); }
		}
	}// listitem
}

