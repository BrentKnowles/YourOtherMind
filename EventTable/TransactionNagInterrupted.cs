using System;
using CoreUtilities;
namespace Transactions
{
	public class TransactionNagInterrupted: TransactionBase
	{
		// Note: The original version of this I did not store layout but figured more context was handy so now include it
		public TransactionNagInterrupted (DateTime date, string LayoutGuid) : base()
		{
			RowData[TransactionsTable.TYPE.Index] = TransactionsTable.T_NAGINTERRUPTED.ToString ();
			RowData[TransactionsTable.DATE.Index] = date;
			RowData[TransactionsTable.DATA1_LAYOUTGUID.Index] = LayoutGuid;
		}
		public TransactionNagInterrupted(object[] items): base(items)
		{
			// all children need this form of the constructor
		}
		// This will probably never be seen because the note is gone, but just in case
		public override string Display {
			get {
				return Loc.Instance.GetStringFmt("Nag Mode Interrupted at {0}",DateAsFriendlyString());
			}
		}
	}
}
