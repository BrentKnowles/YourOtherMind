// TransactionNewLayout.cs
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
using CoreUtilities;

namespace Transactions
{
	// depends on the Type of Event.
	// For a New Note, there should only be one but for a Submission uniqueness would be ID only
	
	public class TransactionNewLayout  : TransactionBase
	{
		public TransactionNewLayout (DateTime date, string LayoutGuid) : base()
		{
			// The trick is that the definition of whether an eventrow is UNIQUe

			RowData[TransactionsTable.TYPE.Index] = TransactionsTable.T_ADDED.ToString ();
			RowData[TransactionsTable.DATE.Index] = date;
			RowData[TransactionsTable.DATA1_LAYOUTGUID.Index] = LayoutGuid;
//			RowData[TransactionsTable.DATA2.Index] ="0";
//			RowData[TransactionsTable.DATA3.Index] =0; 
//			RowData[TransactionsTable.DATA4.Index]  =0;
		}
		public TransactionNewLayout ()
		{
		}

		public TransactionNewLayout(object[] items): base(items)
		{
			// all children need this form of the constructor
		}
	
		public override string Display {
			get {
				return Loc.Instance.GetStringFmt("Layout Added on {0}", DateAsFriendlyString());
			}
		}



	public override string ToString ()
		{
			return string.Format ("[EventRowNewNote]");
		}
//		public override int dbType {
//			get {
//				return EventTABLE.T_ADDED;
//			}
//		}
/* CONVERSION NOTES
		 * 
		 ********** Storing GUID instead of Name (will expect imported data to have swapped Name for GUID
		 **| Not storing Data2 = note {this seems pointless to me}
		 **| 
		 **|
		 **|
		 *********
		 *********
		 */

	}
}

