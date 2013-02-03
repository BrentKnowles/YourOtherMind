using System;

namespace Transactions
{
	/// <summary>
	/// Event row_ return note. This is just for returnign from the GetExisting Function in the EventTable (to avoid confusion)
	/// </summary>
	public class TransactionReturnNote: TransactionBase
	{
		//		public override int dbType {
		//			get {
		//				return -1;
		//			}
		//		}
		public TransactionReturnNote (object[] items) : base()
		{
			// populates the objectarray
			RowData = items;
			Display = DisplayName;
		}
	}
}

