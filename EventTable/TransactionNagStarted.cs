using System;
using CoreUtilities;
namespace Transactions
{
	public class TransactionNagStarted: TransactionBase
	{
		// Note: The original version of this I did not store layout but figured more context was handy so now include it
		public TransactionNagStarted (DateTime date, string LayoutGuid) : base()
		{
			RowData[TransactionsTable.TYPE.Index] = TransactionsTable.T_NAGSTARTED.ToString ();
			RowData[TransactionsTable.DATE.Index] = date;
			RowData[TransactionsTable.DATA1_LAYOUTGUID.Index] = LayoutGuid;
		}
		public TransactionNagStarted(object[] items): base(items)
		{
			// all children need this form of the constructor
		}
		// This will probably never be seen because the note is gone, but just in case
		public override string Display {
			get {
				return Loc.Instance.GetStringFmt("Nag Mode Entered on {0}",DateAsFriendlyString());
			}
		}
	}
}
