using System;

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
		public TransactionSubmission ()
		{
		}

		public TransactionSubmission(object[] items): base(items)
		{
			// all children need this form of the constructor
		}
	}
}

