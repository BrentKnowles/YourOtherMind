using System;
using CoreUtilities;

namespace Transactions
{
	public class TransactionBase: IComparable
	{
		protected object[] RowData;
		// each type of note overrides this with the appropraite code
		/// <summary>
		/// Compares to.
		/// Implements the interface
		/// </summary>
		/// <returns>
		/// The to.
		/// </returns>
		/// <param name='obj'>
		/// Object.
		/// </param>
		public int CompareTo (object obj)
		{
			if (obj == null)
				return 1;
			TransactionBase otherNote = obj as TransactionBase;
			if (otherNote != null) {
				DateTime time1 = Date1;
				DateTime time2 = otherNote.Date1;
				return time1.CompareTo (time2);
			} else {
				throw new Exception("Object is a NoteDataXML");
			}
		}

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
			RowData[TransactionsTable.DATE2.Index] = DateTime.Now;
			RowData[TransactionsTable.TYPE_OF_OBJECT.Index] = this.GetType ().AssemblyQualifiedName;//this.GetType().ToString ();
		}


		public DateTime Date1 {
			get { return DateTime.Parse (this.RowData [TransactionsTable.DATE.Index].ToString ());}
		}

		public TransactionBase (object[] items) :base()
		{
			// populates the objectarray
			RowData = items;
		}
		public string LayoutGuid {
			get { return RowData [TransactionsTable.DATA1_LAYOUTGUID.Index].ToString ();}
		}
		public string ID {
			get { return RowData [TransactionsTable.ID.Index].ToString ();}
		}
		
		
		// This shows up in listbox

		public virtual string Display {
			get {

				int type = Int32.Parse (RowData [TransactionsTable.TYPE.Index].ToString ());
				string returnValue = "BaseNote.AfterDevShouldNeverSee**" + type.ToString ();

				return returnValue;
			}

		}

		protected string DateAsFriendlyString()
		{
			DateTime dateValue = DateTime.Parse (RowData[TransactionsTable.DATE.Index].ToString());
						string dateAsString = dateValue.ToShortDateString();
			return dateAsString;
		}

//		protected string DisplayName {
//			get {
//				int type = Int32.Parse (RowData[TransactionsTable.TYPE.Index].ToString ());
//				string returnValue= "BaseNote.AfterDevShouldNeverSee**"+type.ToString();
//
//
//				DateTime dateValue = DateTime.Parse (RowData[TransactionsTable.DATE.Index].ToString());
//				string dateAsString = dateValue.ToShortDateString();
//
//				try
//				{
//					
//					switch (type)
//					{
//					case TransactionsTable.T_ADDED:
//
//						returnValue = Loc.Instance.GetStringFmt("Layout Added on {0}", dateAsString);
//						break;
//
//					case TransactionsTable.T_IMPORTED:
//						returnValue = Loc.Instance.GetStringFmt("Layout {0} Imported on {1}", RowData[TransactionsTable.DATA2.Index].ToString (),dateAsString);
//						break;
//					}
//				}
//				catch (Exception ex)
//				{
//					//Console.WriteLine(ex.ToString());
//					lg.Instance.Line("EventRowBase->DisplayName", ProblemType.EXCEPTION, ex.ToString());
//					//NewMessage.Show (ex.ToString());
//				}
//				
//				return returnValue;
//			}
//		}
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

