// TransactionWorkLog.cs
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
	public class TransactionWorkLog :  TransactionBase
	{
		public TransactionWorkLog (DateTime date, string LayoutGuid, string Note, int words, int minutes, string category) : base()
		{
			// I originally had Data2 and Data1 reversed to be consident with the old (broken) system
			// but this will complicate every query I ever write and it doesn't really make
			// sense. 
			RowData[TransactionsTable.TYPE.Index] = TransactionsTable.T_USER.ToString ();
			RowData[TransactionsTable.DATE.Index] = date;
			RowData[TransactionsTable.DATA2.Index] = Note;
				RowData[TransactionsTable.DATA1_LAYOUTGUID.Index] = LayoutGuid;

			RowData[TransactionsTable.DATA3.Index] = minutes;
			RowData[TransactionsTable.DATA4.Index] = words;
			RowData[TransactionsTable.DATA5.Index] = category;
		}
		public TransactionWorkLog(object[] items): base(items)
		{
			// all children need this form of the constructor
		}
		public override string Display {
			get {
				return Loc.Instance.GetStringFmt("Worked {0} minutes and wrote {1} words on {2} with Category {3}",
				                                 RowData[TransactionsTable.DATA3.Index].ToString (),RowData[TransactionsTable.DATA4.Index].ToString (),DateAsFriendlyString(),
				                                 RowData[TransactionsTable.DATA5.Index].ToString ());
			}
		}

		public int Words {
			get { return Int32.Parse (RowData[TransactionsTable.DATA4.Index].ToString ());}
			set{ RowData[TransactionsTable.DATA4.Index] = value;}
		}

		public int Minutes {
			get { return Int32.Parse (RowData[TransactionsTable.DATA3.Index].ToString ());}
			set{ RowData[TransactionsTable.DATA3.Index] = value;}
		}

		public string Category {
			get { return RowData[TransactionsTable.DATA5.Index].ToString ();}
			set{ RowData[TransactionsTable.DATA5.Index] = value;}
		}

		public string Notes {
			get { return RowData[TransactionsTable.DATA2.Index].ToString ();}
			set{ RowData[TransactionsTable.DATA2.Index] = value;}
		}
	}
}

