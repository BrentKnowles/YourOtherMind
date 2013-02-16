using System;
using CoreUtilities;
namespace Transactions
{
	public class TransactionFinishedLayout: TransactionBase
	{
		public TransactionFinishedLayout (DateTime date, string LayoutGuid) : base()
		{
			RowData[TransactionsTable.TYPE.Index] = TransactionsTable.T_FINISHED.ToString ();
			RowData[TransactionsTable.DATE.Index] = date;
			RowData[TransactionsTable.DATA1_LAYOUTGUID.Index] = LayoutGuid;
		}
		public TransactionFinishedLayout(object[] items): base(items)
		{
			// all children need this form of the constructor
		}
		// This will probably never be seen because the note is gone, but just in case
		public override string Display {
			get {
				return Loc.Instance.GetStringFmt("Layout Finished on {0}",DateAsFriendlyString());
			}
		}
	}
}
