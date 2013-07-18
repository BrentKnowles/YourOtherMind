// TransactionSubmission.cs
//
// Copyright (c) 2013 Brent Knowles (http://www.brentknowles.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Review documentation at http://www.yourothermind.com for updated implementation notes, license updates
// or other general information/
// 
// Author information available at http://www.brentknowles.com or http://www.amazon.com/Brent-Knowles/e/B0035WW7OW
// Full source code: https://github.com/BrentKnowles/YourOtherMind
//###
using System;
using CoreUtilities;
using System.ComponentModel.Composition;
namespace Transactions
{
	//[Export(typeof(TransactionBase))]
	// will be moved to an AddIn
	public class TransactionSubmission : TransactionBase
	{
			public const int T_SUBMISSION = 1001; 
			public const int T_SUBMISSION_DESTINATION= 1005;
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
		// a lookup function to translate a GUId to a project name
		// optional.
		public delegate string getprojectbyguiddelegate(string guid);
		public getprojectbyguiddelegate GetProjectFromGUID = null;

		// using this so I can simply have destinations inherit TransactionSubmission
		protected virtual void SetType()
		{
			RowData[TransactionsTable.TYPE.Index] = TransactionSubmission.T_SUBMISSION.ToString ();
		}
	
		public TransactionSubmission (DateTime dateSubmitted, string ProjectGUID, string MarketGUID, int Priority,
		                             float Money1, float Money2, DateTime DataReplied,
		                             string Notes, string Rights, string Version,
		                             string ReplyType, string ReplyFeedback, string SubmissionType, string MarketName) : base()
		{
		
			SetType ();
		


			RowData[TransactionsTable.DATE.Index] = dateSubmitted;
			RowData[TransactionsTable.DATA1_LAYOUTGUID.Index] = ProjectGUID;
			RowData[TransactionsTable.DATA2.Index] = MarketGUID;
			RowData[TransactionsTable.DATA3.Index] = Priority;

			RowData[TransactionsTable.MONEY1.Index] = Money1; // March 2013 - am assuming this must be expenses
			RowData[TransactionsTable.MONEY2.Index] = Money2;

			RowData[TransactionsTable.DATE2.Index] = DataReplied;
			RowData[TransactionsTable.NOTES.Index] = Notes;
			RowData[TransactionsTable.DATA5.Index] = Rights;
			RowData[TransactionsTable.DATA6.Index] = Version;

			RowData[TransactionsTable.DATA7.Index] = ReplyType;
			RowData[TransactionsTable.DATA8.Index] = ReplyFeedback;

			RowData[TransactionsTable.DATA9.Index] = SubmissionType;
			RowData[TransactionsTable.DATA10.Index] = MarketName;

		}

		public void SetID (string iD)
		{
			RowData[TransactionsTable.ID.Index ] = iD;
		}
		public TransactionSubmission()
		{
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

		public string ProjectGUID
		{
			get {return RowData[TransactionsTable.DATA1_LAYOUTGUID.Index].ToString ();}
		}

		public DateTime SubmissionDate {
			get { return 	DateTime.Parse (RowData [TransactionsTable.DATE.Index].ToString ());}
			set { RowData [TransactionsTable.DATE.Index] = value;}
		}

		public DateTime ReplyDate {
			get {return 	DateTime.Parse (RowData[TransactionsTable.DATE2.Index].ToString());}
		}

		public string Draft {
			get { return RowData [TransactionsTable.DATA6.Index].ToString();}

		}

		public string Rights {
			get {return RowData[TransactionsTable.DATA5.Index].ToString ();}
		
		}

		public string Notes {
			get {return RowData[TransactionsTable.NOTES.Index].ToString ();}
		
		}

		public string Priority {
			get {return RowData[TransactionsTable.DATA3.Index].ToString ();}

		}

		public string Replyfeedback {
			get { return RowData [TransactionsTable.DATA8.Index].ToString ();}

		}

		public decimal Earned {
			get {
				decimal fEarned = 0;
				try {
					fEarned = Decimal.Parse (RowData [TransactionsTable.MONEY2.Index].ToString ());
				} catch (Exception) {
					fEarned = 0;
				}
				return fEarned;
				;
			}
			set{ ;}
		}

		public decimal Expenses {
			get {   decimal fPostage = 0;
				try
				{
					fPostage = Decimal.Parse(RowData[TransactionsTable.MONEY1.Index].ToString ());
				}
				catch (Exception)
				{
					fPostage = 0;
				}
				return fPostage; 
				;}

		}

		public string MarketName
		{
			get {return RowData[TransactionsTable.DATA10.Index].ToString ();}
		}
		public string MarketGuid
		{
			get {return RowData[TransactionsTable.DATA2.Index].ToString ();}
		}
		public string ReplyType
		{
			get  {return RowData[TransactionsTable.DATA7.Index].ToString ();}
		}

		public string SubmissionType
		{
			get  {return RowData[TransactionsTable.DATA9.Index].ToString ();}
		}
		public override string Display {
			get {

//				string dataToAdd = String.Format("Date = {0};GuidOfLayout={1};GuidOfMarket={2};Priority {3}; Expenses{4};Earned{5};datereplied {6}; notes{7};"+
//				                                 "rights {8}; version {9}; reply type{10};reply feedback {11};sub type {12}", 
//
//				                                 DateAsFriendlyString(), RowData[TransactionsTable.DATA1_LAYOUTGUID.Index], 
//				                                 RowData[TransactionsTable.DATA2.Index], RowData[TransactionsTable.DATA3.Index], 
//				                                 RowData[TransactionsTable.MONEY1.Index], RowData[TransactionsTable.MONEY2.Index],
//				                                 RowData[TransactionsTable.DATE2.Index], RowData[TransactionsTable.NOTES.Index],
//				                                 RowData[TransactionsTable.DATA5.Index], RowData[TransactionsTable.DATA6.Index], 
//				                                 RowData[TransactionsTable.DATA7.Index], RowData[TransactionsTable.DATA8.Index], 
//				                                 RowData[TransactionsTable.DATA9.Index]);


			//	string debug = String.Format ("DEBUGGUID :[{0}]", RowData[TransactionsTable.DATA2.Index].ToString());



				string dataToAdd =Loc.Instance.GetStringFmt 
					("Submitted to {0} on {1} [{2}] ",  RowData[TransactionsTable.DATA10.Index], GetFriendlyDateForMostRecentDate (), RowData[TransactionsTable.DATA7.Index]
					 );
				return Loc.Instance.GetStringFmt("Submission {0}", dataToAdd);
			}
		}

		// For when a market is viewing a project
		public override string DisplayVariant {
			get {
				string ProjectName = Loc.Instance.GetString ("Unknown Project");

				if (GetProjectFromGUID != null)
				{
					try
					{
					// if we have provided a lookup function then use it
					ProjectName = GetProjectFromGUID(ProjectGUID);
					}
					catch (Exception)
					{
						ProjectName = "error";
					}
				}
				string date = GetFriendlyDateForMostRecentDate ();
				string dataToAdd =Loc.Instance.GetStringFmt 
					("{0} was submitted here on  {1} [{2}]",  ProjectName, date, RowData[TransactionsTable.DATA7.Index]);

				int DaysBetween =  this.DaysInBetween();
				return Loc.Instance.GetStringFmt("Submission {0} ({1} days out)", dataToAdd, DaysBetween);
			}
		}





	}
}

