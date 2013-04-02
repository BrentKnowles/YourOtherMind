using System;
using CoreUtilities;

namespace Transactions
{
	// for destinations (future submissions -- places you want to send a story if rejected at current place)
	public class TransactionSubmissionDestination : TransactionSubmission
	{
		public TransactionSubmissionDestination (DateTime dateSubmitted, string ProjectGUID, string MarketGUID, int Priority,
		                              float Money1, float Money2, DateTime DataReplied,
		                              string Notes, string Rights, string Version,
		                                         string ReplyType, string ReplyFeedback, string SubmissionType, string MarketName) : base(dateSubmitted, ProjectGUID
		                                                                      ,MarketGUID,Priority,  Money1,  Money2,  DataReplied,
		                                                                       Notes,  Rights,  Version,
		                                                                       ReplyType,  ReplyFeedback,  SubmissionType, MarketName
		                                                                      )
		{
		}

		public TransactionSubmissionDestination(object[] items): base(items)
		{
			// all children need this form of the constructor
		}
		protected override void SetType ()
		{
			RowData[TransactionsTable.TYPE.Index] = TransactionsTable.T_SUBMISSION_DESTINATION.ToString ();
		}

		public override string Display {
			get {
				return Loc.Instance.GetString("DESTINATION: ") + base.Display;
			}
		}
		public override string DisplayVariant {
			get {
				return Loc.Instance.GetString ("DESTINATION: ") + base.DisplayVariant;
			}
		}
	}
}

