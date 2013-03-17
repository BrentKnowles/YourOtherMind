using System;
using NUnit.Framework;
using Layout;
using MefAddIns;
using Submissions;

namespace Testing
{
	[TestFixture]
	public class ADDIN_Submissions_Test
	{

		public ADDIN_Submissions_Test ()
		{
		}

		public void SetupForSubmissionTest()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			LayoutDetails.Instance.AddToList(typeof(NoteDataXML_Submissions), "submissionsfromtest");
		}

		[Test]
		public void BasicInvokeTest ()
		{

			SetupForSubmissionTest();
		


			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			form.Controls.Add (panel);
			// needed else DataGrid does not initialize
			form.Show ();
			//form.Visible = false;
			_w.output("boom");
			// March 2013 -- notelist relies on having this
			YOM2013.DefaultLayouts.CreateASystemLayout(form,null);
			
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			string panelname = System.Guid.NewGuid().ToString();
			panel.NewLayout (panelname,true, null);
			LayoutDetails.Instance.AddToList (typeof(FAKE_NoteDataXML_Panel), "testingpanel");
			_w.output ("herefirst");


			NoteDataXML_Submissions Submissions = new NoteDataXML_Submissions(250,250);
			Submissions.GuidForNote = "submissions";
			Submissions.Caption = "Submissions";

			panel.AddNote(Submissions);
			Submissions.CreateParent(panel);
			Submissions.UpdateAfterLoad();

			panel.SaveLayout();
			form.Dispose ();
		
		}
		[Test]
		public void MoveSubmissionToDestinationAndBackAgain()
		{
			Assert.True (false);

			// use submission master
		}
	}
}

