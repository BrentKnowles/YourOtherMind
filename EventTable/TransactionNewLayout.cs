using System;

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

