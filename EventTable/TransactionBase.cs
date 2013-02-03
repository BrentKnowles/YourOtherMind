using System;
using CoreUtilities;

namespace Transactions
{
	public abstract class TransactionBase
	{
		protected object[] RowData;
		// each type of note overrides this with the appropraite code
		
		public TransactionBase ()
		{
			RowData = new object[TransactionsTable.ColumnCount];
			for (int i = 0; i < RowData.Length; i++) {
				
				RowData[i] = Constants.BLANK;
			}
			// override non strings
			RowData[TransactionsTable.ID.Index ] =DBNull.Value;
			RowData[TransactionsTable.DATA3.Index] = 0;
			RowData[TransactionsTable.DATA4.Index] = 0;
			RowData[TransactionsTable.MONEY1.Index]=0.0;
			RowData[TransactionsTable.MONEY2.Index]=0.0;
			RowData[TransactionsTable.DATE2.Index] = DateTime.Today;
		}
		
		public string LayoutGuid {
			get { return RowData [TransactionsTable.DATA1_LAYOUTGUID.Index].ToString ();}
		}
		public string ID {
			get { return RowData [TransactionsTable.ID.Index].ToString ();}
		}
		
		
		// This shows up in listbox
		string display = "*";
		public string Display {
			get { return display;}
			set { display = value;}
		}
		
		protected string DisplayName {
			get {
				int type = Int32.Parse (RowData[TransactionsTable.TYPE.Index].ToString ());
				string returnValue= "*"+type.ToString();


				DateTime dateValue = DateTime.Parse (RowData[TransactionsTable.DATE.Index].ToString());
				string dateAsString = dateValue.ToShortDateString();

				try
				{
					
					switch (type)
					{
					case TransactionsTable.T_ADDED:

						returnValue = Loc.Instance.GetStringFmt("Layout Added on {0}", dateAsString);
						break;

					case TransactionsTable.T_IMPORTED:
						returnValue = Loc.Instance.GetStringFmt("Layout {0} Imported on {1}", RowData[TransactionsTable.DATA2.Index].ToString (),dateAsString);
						break;
					}
				}
				catch (Exception ex)
				{
					//Console.WriteLine(ex.ToString());
					lg.Instance.Line("EventRowBase->DisplayName", ProblemType.EXCEPTION, ex.ToString());
					//NewMessage.Show (ex.ToString());
				}
				
				return returnValue;
			}
		}
		public object[] GetRowData()
		{
			return RowData;
			
		}
		/// <summary>
		/// Updates the value for string based values in existing 
		/// </summary>
		/// <param name='location'>
		/// Location.
		/// </param>
		/// <param name='newValue'>
		/// New value.
		/// </param>
		public void UpdateValue (int location, string newValue)
		{
			RowData[location] = newValue;
		}
		
		
	}

}

