using System;
using CoreUtilities;

namespace Transactions
{
	// will be moved to an AddIn
	public class TransactionSubmission : TransactionBase
	{
		/* Mapping Musing - Submissions
		 * 
		 * date = date sent
		 * 
		 * data1 - Guid of Layout
		 * data2 - Guid of Market
		 * data3 - Priority
		 * data4 - WILL NOT BE USED
		 * Expenses     - Money1
		 * Earned       - Money2
		 * DateReplied  - Date2
		 * Notes - Notes
		 * 		 * Rights  - Data5
		 * DraftVersionUsed - Data6
		 * 
		 * ReplyType - Data7
		 * ReplyFeedback - Data8
		 * 
		 * 
		 * SubmissionType - Data9
		 */
	
		public TransactionSubmission (DateTime dateSubmitted, string ProjectGUID, string MarketGUID, int Priority,
		                             float Money1, float Money2, DateTime DataReplied,
		                             string Notes, string Rights, string Version,
		                             string ReplyType, string ReplyFeedback, string SubmissionType) : base()
		{
		
			RowData[TransactionsTable.TYPE.Index] = TransactionsTable.T_SUBMISSION.ToString ();


			RowData[TransactionsTable.DATE.Index] = dateSubmitted;
			RowData[TransactionsTable.DATA1_LAYOUTGUID.Index] = ProjectGUID;
			RowData[TransactionsTable.DATA2.Index] = MarketGUID;
			RowData[TransactionsTable.DATA3.Index] = Priority;

			RowData[TransactionsTable.MONEY1.Index] = Money1;
			RowData[TransactionsTable.MONEY2.Index] = Money2;

			RowData[TransactionsTable.DATE2.Index] = DataReplied;
			RowData[TransactionsTable.NOTES.Index] = Notes;
			RowData[TransactionsTable.DATA5.Index] = Rights;
			RowData[TransactionsTable.DATA6.Index] = Version;

			RowData[TransactionsTable.DATA7.Index] = ReplyType;
			RowData[TransactionsTable.DATA8.Index] = ReplyFeedback;

			RowData[TransactionsTable.DATA9.Index] = SubmissionType;

		}
		public TransactionSubmission(object[] items): base(items)
		{
			// all children need this form of the constructor
		}
				/* Mapping Musing - Submissions
+		 * 
+		 * date = date sent
+		 * 
+		 * data1 - Guid of Layout
+		 * data2 - Guid of Market
+		 * data3 - Priority
+		 * data4 - WILL NOT BE USED
+		 * Expenses     - Money1
+		 * Earned       - Money2
+		 * DateReplied  - Date2
+		 * Notes - Notes
+		 * 		 * Rights  - Data5
+		 * DraftVersionUsed - Data6
+		 * 
+		 * ReplyType - Data7
+		 * ReplyFeedback - Data8
+		 * 
+		 * 
+		 * SubmissionType - Data9
+		 */

		public override string Display {
			get {

				string dataToAdd = String.Format("Date = {0};GuidOfLayout={1};GuidOfMarket={2};Priority {3}; Expenses{4};Earned{5};datereplied {6}; notes{7};"+
				                                 "rights {8}; version {9}; reply type{10};reply feedback {11};sub type {12}", 

				                                 DateAsFriendlyString(), RowData[TransactionsTable.DATA1_LAYOUTGUID.Index], 
				                                 RowData[TransactionsTable.DATA2.Index], RowData[TransactionsTable.DATA3.Index], 
				                                 RowData[TransactionsTable.MONEY1.Index], RowData[TransactionsTable.MONEY2.Index],
				                                 RowData[TransactionsTable.DATE2.Index], RowData[TransactionsTable.NOTES.Index],
				                                 RowData[TransactionsTable.DATA5.Index], RowData[TransactionsTable.DATA6.Index], 
				                                 RowData[TransactionsTable.DATA7.Index], RowData[TransactionsTable.DATA8.Index], 
				                                 RowData[TransactionsTable.DATA9.Index]);


				return Loc.Instance.GetStringFmt("Submission {0}", dataToAdd);
			}
		}


	}
}

