using System;
using CoreUtilities;

namespace Transactions
{
	public class TransactionDeleteLayout  : TransactionBase
	{
		public TransactionDeleteLayout (DateTime date, string LayoutGuid) : base()
		{
			RowData[TransactionsTable.TYPE.Index] = TransactionsTable.T_RETIRED.ToString ();
			RowData[TransactionsTable.DATE.Index] = date;
			RowData[TransactionsTable.DATA1_LAYOUTGUID.Index] = LayoutGuid;
		}
		public TransactionDeleteLayout(object[] items): base(items)
		{
			// all children need this form of the constructor
		}
		// This will probably never be seen because the note is gone, but just in case
		public override string Display {
			get {
				return Loc.Instance.GetStringFmt("Layout {0} Deleted on {1}", RowData[TransactionsTable.DATA2.Index].ToString (),DateAsFriendlyString());
			}
		}
	}
}

