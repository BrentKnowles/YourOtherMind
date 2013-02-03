using System;
using CoreUtilities;

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


		public TransactionImportLayout(object[] items): base(items)
		{
			// all children need this form of the constructor
		}

		public override string Display {
			get {
				return Loc.Instance.GetStringFmt("Layout {0} Imported on {1}", RowData[TransactionsTable.DATA2.Index].ToString (),DateAsFriendlyString());
			}
		}

	}
}

