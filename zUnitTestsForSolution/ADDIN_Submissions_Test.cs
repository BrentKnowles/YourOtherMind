// ADDIN_Submissions_Test.cs
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
			LayoutDetails.Instance.AddToList (typeof(FAKE_NoteDataXML_Panel), "testingpanelZZ");
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

