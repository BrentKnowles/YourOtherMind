using System;
using CoreUtilities;

namespace Transactions
{
	public class TransactionWorkLog :  TransactionBase
	{
		public TransactionWorkLog (DateTime date, string LayoutGuid, string Note, int words, int minutes, string category) : base()
		{
			// I originally had Data2 and Data1 reversed to be consident with the old (broken) system
			// but this will complicate every query I ever write and it doesn't really make
			// sense. 
			RowData[TransactionsTable.TYPE.Index] = TransactionsTable.T_USER.ToString ();
			RowData[TransactionsTable.DATE.Index] = date;
			RowData[TransactionsTable.DATA2.Index] = Note;
				RowData[TransactionsTable.DATA1_LAYOUTGUID.Index] = LayoutGuid;

			RowData[TransactionsTable.DATA3.Index] = minutes;
			RowData[TransactionsTable.DATA4.Index] = words;
			RowData[TransactionsTable.DATA5.Index] = category;
		}
		public TransactionWorkLog(object[] items): base(items)
		{
			// all children need this form of the constructor
		}
		public override string Display {
			get {
				return Loc.Instance.GetStringFmt("Worked {0} minutes and wrote {1} words on {2} with Category {3}",
				                                 RowData[TransactionsTable.DATA3.Index].ToString (),RowData[TransactionsTable.DATA4.Index].ToString (),DateAsFriendlyString(),
				                                 RowData[TransactionsTable.DATA5.Index].ToString ());
			}
		}

		public int Words {
			get { return Int32.Parse (RowData[TransactionsTable.DATA4.Index].ToString ());}
			set{ RowData[TransactionsTable.DATA4.Index] = value;}
		}

		public int Minutes {
			get { return Int32.Parse (RowData[TransactionsTable.DATA3.Index].ToString ());}
			set{ RowData[TransactionsTable.DATA3.Index] = value;}
		}

		public string Category {
			get { return RowData[TransactionsTable.DATA5.Index].ToString ();}
			set{ RowData[TransactionsTable.DATA5.Index] = value;}
		}

		public string Notes {
			get { return RowData[TransactionsTable.DATA2.Index].ToString ();}
			set{ RowData[TransactionsTable.DATA2.Index] = value;}
		}
	}
}

