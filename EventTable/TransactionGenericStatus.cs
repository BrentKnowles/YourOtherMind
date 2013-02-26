using System;
using CoreUtilities;
namespace Transactions
{
	public class TransactionGenericStatus : TransactionBase
	{
		public TransactionGenericStatus (DateTime date, string LayoutGuid, string Status) : base()
		{
			RowData[TransactionsTable.TYPE.Index] = TransactionsTable.T_GENERIC_STATUS_UPDATE.ToString ();
			RowData[TransactionsTable.DATE.Index] = date;
			RowData[TransactionsTable.DATA1_LAYOUTGUID.Index] = LayoutGuid;
			RowData[TransactionsTable.DATA2.Index] = Status;
		}
		public TransactionGenericStatus(object[] items): base(items)
		{
			// all children need this form of the constructor
		}
		// This will probably never be seen because the note is gone, but just in case
		public override string Display {
			get {
				return Loc.Instance.GetStringFmt("Status Update {0} on {1}",RowData[TransactionsTable.DATA2.Index] , DateAsFriendlyString());
			}
		}
	}
}

