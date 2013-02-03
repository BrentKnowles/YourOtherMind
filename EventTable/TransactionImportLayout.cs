using System;

namespace Transactions
{
	public class TransactionImportLayout : TransactionBase
	{
		public TransactionImportLayout (DateTime date, string LayoutGuid, string Caption) : base()
		{
			RowData[TransactionsTable.TYPE.Index] = TransactionsTable.T_IMPORTED.ToString ();
			RowData[TransactionsTable.DATE.Index] = date;
			RowData[TransactionsTable.DATA1_LAYOUTGUID.Index] = LayoutGuid;
			RowData[TransactionsTable.DATA2.Index] = Caption;
		}
	}
}

