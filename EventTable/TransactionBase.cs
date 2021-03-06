// TransactionBase.cs
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
	public class TransactionBase: IComparable
	{
		protected object[] RowData;
		// each type of note overrides this with the appropraite code
		/// <summary>
		/// Compares to.
		/// Implements the interface
		/// </summary>
		/// <returns>
		/// The to.
		/// </returns>
		/// <param name='obj'>
		/// Object.
		/// </param>
		public int CompareTo (object obj)
		{
			if (obj == null)
				return 1;
			TransactionBase otherNote = obj as TransactionBase;
			if (otherNote != null) {
				DateTime time1 = Date1;
				DateTime time2 = otherNote.Date1;
				return time1.CompareTo (time2);
			} else {
				throw new Exception("Object is a NoteDataXML");
			}
		}

		public TransactionBase ()
		{
			RowData = new object[TransactionsTable.ColumnCount];
			for (int i = 0; i < RowData.Length; i++) {
				
				RowData[i] = Constants.BLANK;
			}
			// override non strings
			RowData[TransactionsTable.ID.Index ] =DBNull.Value;
			RowData[TransactionsTable.DATA3.Index] = 0;
			RowData[TransactionsTable.DATA4.Index] = 0;
			RowData[TransactionsTable.MONEY1.Index]=0.0;
			RowData[TransactionsTable.MONEY2.Index]=0.0;
			RowData[TransactionsTable.DATE2.Index] = DateTime.Now;
			RowData[TransactionsTable.TYPE_OF_OBJECT.Index] = this.GetType ().AssemblyQualifiedName;//this.GetType().ToString ();
		}

		/// <summary>
		/// Refreshs the type. Call this when Transforming one Transaction into a different kind of transaction to
		/// ensure the right type is associated with it
		/// </summary>
		public void RefreshType(int newTypeNumber)
		{
			RowData [TransactionsTable.TYPE.Index] = newTypeNumber;
			RowData[TransactionsTable.TYPE_OF_OBJECT.Index] = this.GetType ().AssemblyQualifiedName;
		}

		public string GetTypeCode {
			get { return RowData [TransactionsTable.TYPE.Index].ToString ();}
		}

		public DateTime Date1 {
			get { return DateTime.Parse (this.RowData [TransactionsTable.DATE.Index].ToString ());}
		}

		public TransactionBase (object[] items) :base()
		{
			// populates the objectarray
			RowData = items;
		}
		public string LayoutGuid {
			get { return RowData [TransactionsTable.DATA1_LAYOUTGUID.Index].ToString ();}
		}
		public string ID {
			get { return RowData [TransactionsTable.ID.Index].ToString ();}
		}
		
		
		// This shows up in listbox

		public virtual string Display {
			get {

				int type = Int32.Parse (RowData [TransactionsTable.TYPE.Index].ToString ());
				string returnValue = Loc.Instance.GetStringFmt("The ADDIN used to add this transaction has been removed. No data will be lost but this message won't display properly under it is restored. TYPE: {0}", type.ToString ());

				return returnValue;
			}

		}

		public virtual string ExpandedDescription ()
		{
			return string.Format ("[TransactionBase: GetTypeCode={0}, Date1={1}, LayoutGuid={2}, ID={3}, Display={4}, DisplayVariant={5}]", GetTypeCode, Date1, LayoutGuid, ID, Display, DisplayVariant);
		}

		/// <summary>
		/// Gets the friendly date for most recent date.
		/// 
		/// Picks between the most recent of dates and returns its strings 
		/// </summary>
		/// <returns>
		/// The friendly date for most recent date.
		/// </returns>
		protected string GetFriendlyDateForMostRecentDate ()
		{
		
			DateTime date1 = DateTime.Now;
			DateTime date2 = DateTime.Now;

			DateTime.TryParse (RowData [TransactionsTable.DATE.Index].ToString (), out date1);
			DateTime.TryParse (RowData [TransactionsTable.DATE2.Index].ToString (), out date2);

			if (date1 > date2) {
				return DateAsFriendlyString();
			}

			return Date2AsFriendlyString();
		}
		public virtual string DisplayVariant {
			get {
				return Loc.Instance.GetString ("undefined");
			}
		}
		protected string DateAsFriendlyString()
		{
			DateTime dateValue = DateTime.Parse (RowData[TransactionsTable.DATE.Index].ToString());
						string dateAsString = dateValue.ToShortDateString();
			return dateAsString;
		}

		protected int DaysInBetween()
		{
			DateTime dateValue = DateTime.Parse (RowData[TransactionsTable.DATE.Index].ToString());
			DateTime dateValue2 = DateTime.Parse (RowData[TransactionsTable.DATE2.Index].ToString());
			int Days = (dateValue2 - dateValue).Days;
			return Days;
		}

		protected string Date2AsFriendlyString()
		{
			DateTime dateValue = DateTime.Parse (RowData[TransactionsTable.DATE2.Index].ToString());
			string dateAsString = dateValue.ToShortDateString();
			return dateAsString;
		}

//		protected string DisplayName {
//			get {
//				int type = Int32.Parse (RowData[TransactionsTable.TYPE.Index].ToString ());
//				string returnValue= "BaseNote.AfterDevShouldNeverSee**"+type.ToString();
//
//
//				DateTime dateValue = DateTime.Parse (RowData[TransactionsTable.DATE.Index].ToString());
//				string dateAsString = dateValue.ToShortDateString();
//
//				try
//				{
//					
//					switch (type)
//					{
//					case TransactionsTable.T_ADDED:
//
//						returnValue = Loc.Instance.GetStringFmt("Layout Added on {0}", dateAsString);
//						break;
//
//					case TransactionsTable.T_IMPORTED:
//						returnValue = Loc.Instance.GetStringFmt("Layout {0} Imported on {1}", RowData[TransactionsTable.DATA2.Index].ToString (),dateAsString);
//						break;
//					}
//				}
//				catch (Exception ex)
//				{
//					//Console.WriteLine(ex.ToString());
//					lg.Instance.Line("EventRowBase->DisplayName", ProblemType.EXCEPTION, ex.ToString());
//					//NewMessage.Show (ex.ToString());
//				}
//				
//				return returnValue;
//			}
//		}
		public object[] GetRowData()
		{
			return RowData;
			
		}
		/// <summary>
		/// Updates the value for string based values in existing 
		/// </summary>
		/// <param name='location'>
		/// Location.
		/// </param>
		/// <param name='newValue'>
		/// New value.
		/// </param>
		public void UpdateValue (int location, string newValue)
		{
			RowData[location] = newValue;
		}

		public void UpdateDate(DateTime newDate)
		{
			RowData[TransactionsTable.DATE.Index] = newDate;
		}
		
	}

}

