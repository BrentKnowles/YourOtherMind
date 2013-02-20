using System;
using System.Xml.Serialization;
using System.ComponentModel;

namespace CoreUtilities.Links
{
	public enum LinkType { PAGE, FILE, WEB, POPUP, DELETED };

	[XmlRootAttribute("LinkTableRecord", Namespace = "", IsNullable = false)]
	public class LinkTableRecord
	{
		public const string PageLinkFormatString = "{0}.{1}";
		public string sText;
		public string sFileName; // this has been CHANGED to GUID
		public LinkType linkType;
		public int nBookmarkKey; // this is the the key into the bookmark table to get the position
		public string sKey; // Don't really need this (this is the position). But just in case something gets messed up...
		// empty constructor for serialization
		public string sSource; // new: required for look-back lists of pages that are linked to me
		
		public string sExtra; // Added February 20 2009, needed an extra field for listview
		
		private bool Status; // if true, then file is still found, otherwises its location has changed

		public string ExtraField; // Feb 2013 for links to Pictures
		public LinkTableRecord()
		{
			bStatus = true;
		}
		[DisplayName("Link Still Valid?")]
		public bool bStatus
		{
			get { return Status; }
			set { Status = value; }
		}
	} // - class LinkTableRecord
}

