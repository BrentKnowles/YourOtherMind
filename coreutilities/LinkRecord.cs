// LinkRecord.cs
//
// Copyright (c) 2013 Brent Knowles (http://www.brentknowles.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Review documentation at http://www.yourothermind.com for updated implementation notes, license updates
// or other general information/
// 
// Author information available at http://www.brentknowles.com or http://www.amazon.com/Brent-Knowles/e/B0035WW7OW
// Full source code: https://github.com/BrentKnowles/YourOtherMind
//###
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

