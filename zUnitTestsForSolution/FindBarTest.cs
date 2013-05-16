// FindBarTest.cs
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

namespace Testing
{
	[TestFixture]
	public class FindBarTest
	{
		public FindBarTest ()
		{
		}
		/// <summary>
		/// Tests the position not changing when it should not.
		/// 
		/// THE REASON for this test is a bug fix I made so that when you modify text during a search
		/// the Position is kept close to what the Position was before the text edit was made. This just 
		/// makes editing a bit easier
		/// Feb 23 2013
		/// </summary>
		[Test]
		public void TestPositionNotChangingWhenItShouldNot()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			
			
			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			
			
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			
			form.Controls.Add (panel);
			
			// needed else DataGrid does not initialize
			
			form.Show ();
			//form.Visible = false;
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			string panelname = System.Guid.NewGuid().ToString();
			panel.NewLayout (panelname,true, null);
			LayoutDetails.Instance.AddToList (typeof(FAKE_NoteDataXML_Panel), "testingpanel");


			// Won't actually add things properly because I need to override the findbar
			FAKE_FindBar findBar = new FAKE_FindBar();
			form.Controls.Add (findBar);
			RichTextExtended fakeText = new RichTextExtended();
			form.Controls.Add (fakeText);
			fakeText.Text = "We have a dog. His name is Jake." + Environment.NewLine + "We love him a lot. He is a nice dog. We all love dogs.";
			findBar.DoFind("dog", false,fakeText,0);
			Assert.AreEqual(3, findBar.PositionsFOUND(), "Found 3 dogs");
			findBar.GoToNext();
			findBar.GoToNext();
			Assert.AreEqual (2, findBar.zPosition());
			findBar.DoFindBuildLIstTesty(fakeText.Text);
			Assert.AreEqual (2, findBar.zPosition());
		}
		[Test]
		public void TestAsterix()
		{

			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			
			
			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			
			
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			
			form.Controls.Add (panel);
			
			// needed else DataGrid does not initialize
			
			form.Show ();
			//form.Visible = false;
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			string panelname = System.Guid.NewGuid().ToString();
			panel.NewLayout (panelname,true, null);
			LayoutDetails.Instance.AddToList (typeof(FAKE_NoteDataXML_Panel), "testingpanel");
			
			
			// Won't actually add things properly because I need to override the findbar
			FAKE_FindBar findBar = new FAKE_FindBar();
			form.Controls.Add (findBar);
			RichTextExtended fakeText = new RichTextExtended();
			form.Controls.Add (fakeText);
			fakeText.Text = "We have a dog. His name is Jake." + Environment.NewLine + "We love him a lot. He is a nice dog. We all love dogs.";
			findBar.DoFind("*dog", false,fakeText,0);
			Assert.AreEqual(0, findBar.PositionsFOUND(), "Found 0 *dogs");

			// I removed the code that prevented typing asertixes. Do not remember why I had it.
			// make sure it does not crash\

		}
		[Test]
		public void TestFindexact()
		{
			
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			
			
			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			
			
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			
			form.Controls.Add (panel);
			
			// needed else DataGrid does not initialize
			
			form.Show ();
			//form.Visible = false;
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			string panelname = System.Guid.NewGuid().ToString();
			panel.NewLayout (panelname,true, null);
			LayoutDetails.Instance.AddToList (typeof(FAKE_NoteDataXML_Panel), "testingpanel");
			
			
			// Won't actually add things properly because I need to override the findbar
			FAKE_FindBar findBar = new FAKE_FindBar();
			form.Controls.Add (findBar);
			RichTextExtended fakeText = new RichTextExtended();
			form.Controls.Add (fakeText);
			fakeText.Text = "We corndog and have a dog. His name is Jake." + Environment.NewLine + "We love him a lot. He is a nice dog. We all love dogs.";
			findBar.DoFind("dog", true,fakeText,0);

			Assert.AreEqual(2, findBar.PositionsFOUND(), "Found 2 *dogs");
			
			// I removed the code that prevented typing asertixes. Do not remember why I had it.
			// make sure it does not crash\
			
		}

		[Test]
		public void TestReplaceWorks()
		{
			
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			
			
			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			
			
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			
			form.Controls.Add (panel);
			
			// needed else DataGrid does not initialize
			
			form.Show ();
			//form.Visible = false;
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			string panelname = System.Guid.NewGuid().ToString();
			panel.NewLayout (panelname,true, null);
			LayoutDetails.Instance.AddToList (typeof(FAKE_NoteDataXML_Panel), "testingpanel");


			FAKE_NoteDataXML_Text fakeTextNote = new FAKE_NoteDataXML_Text();
			fakeTextNote.Caption ="Fake Text Note";
			panel.AddNote(fakeTextNote);
			fakeTextNote.CreateParent(panel);

			
			// Won't actually add things properly because I need to override the findbar
			FAKE_FindBar findBar = new FAKE_FindBar();
			form.Controls.Add (findBar);
			//RichTextExtended fakeText = new RichTextExtended();
			//form.Controls.Add (fakeText);


			LayoutDetails.Instance.CurrentLayout = panel;
			LayoutDetails.Instance.CurrentLayout.CurrentTextNote = fakeTextNote;
			panel.SetFindBar(findBar);
			findBar.SupressMode = true;
			//!= null && LayoutDetails.Instance.CurrentLayout.CurrentTextNote != null
			fakeTextNote.GetRichTextBox().Text = "We have a dog. His name is Jake." + Environment.NewLine + "We love him a lot. And that cat. He is a nice dog. We all love dogs. Dogs are neat.";
			findBar.SetLastRichText(fakeTextNote.GetRichTextBox());

			findBar.DoFind("dog", false,fakeTextNote.GetRichTextBox(),0);

			//need to rewrite 'find/repalce' to make accessible better to test? It crashes.

			Assert.AreEqual(4, findBar.PositionsFOUND(), "Found 4 dogs");
			findBar.Replace_Text("dog", "cat");
			findBar.DoFind("dog", false,fakeTextNote.GetRichTextBox(),0);
			Assert.AreEqual(0, findBar.PositionsFOUND(), "Found 0 dogs");
			findBar.DoFind("cat", false,fakeTextNote.GetRichTextBox(),0);
			Assert.AreEqual(5, findBar.PositionsFOUND(), "Found 5 cats. 4 replacements + 1 original");
		}


		[Test]
		public void TestReplaceWorks_WithSimiliarWORDS()
		{

			//NOTE: This will not work until I implement the Replace system improvements

			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			
			
			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			
			
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			
			form.Controls.Add (panel);
			
			// needed else DataGrid does not initialize
			
			form.Show ();
			//form.Visible = false;
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			string panelname = System.Guid.NewGuid().ToString();
			panel.NewLayout (panelname,true, null);
			LayoutDetails.Instance.AddToList (typeof(FAKE_NoteDataXML_Panel), "testingpanel");
			
			
			FAKE_NoteDataXML_Text fakeTextNote = new FAKE_NoteDataXML_Text();
			fakeTextNote.Caption ="Fake Text Note";
			panel.AddNote(fakeTextNote);
			fakeTextNote.CreateParent(panel);
			
			
			// Won't actually add things properly because I need to override the findbar
			FAKE_FindBar findBar = new FAKE_FindBar();
			form.Controls.Add (findBar);
			//RichTextExtended fakeText = new RichTextExtended();
			//form.Controls.Add (fakeText);
			
			
			LayoutDetails.Instance.CurrentLayout = panel;
			LayoutDetails.Instance.CurrentLayout.CurrentTextNote = fakeTextNote;
			panel.SetFindBar(findBar);
			findBar.SupressMode = true;
			//!= null && LayoutDetails.Instance.CurrentLayout.CurrentTextNote != null
			fakeTextNote.GetRichTextBox().Text = "We have a dog. His name is Jake." + Environment.NewLine + "We love him a lot. And that cat. He is a nice dog. We dog all love dogs. Dogs are neat.";
			findBar.SetLastRichText(fakeTextNote.GetRichTextBox());
			
			findBar.DoFind("dog", false,fakeTextNote.GetRichTextBox(),0);
			
			//need to rewrite 'find/repalce' to make accessible better to test? It crashes.
			
			Assert.AreEqual(5, findBar.PositionsFOUND(), "Found 5 dogs");
			findBar.Replace_Text("dog", "dog2");
			findBar.DoFind("dog", false,fakeTextNote.GetRichTextBox(),0);
			Assert.AreEqual(5, findBar.PositionsFOUND(), "Found 5 dogs because we are doing a partial search");

			findBar.DoFind("dog", true,fakeTextNote.GetRichTextBox(),0);
			Assert.AreEqual(0, findBar.PositionsFOUND(), "Found 0 dogs because we are doing an exact search NOTE: This will not work until I implement the Replace system improvements");

			findBar.DoFind("dog2", false,fakeTextNote.GetRichTextBox(),0);
			Assert.AreEqual(5, findBar.PositionsFOUND(), "Found 4 dog2. 4 replacements NOTE: This will not work until I implement the Replace system improvements ");
		}
		[Test]
		public void AlLFindBarsAreTheSame()
		{
			// children panels have the same findbar as a parent
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			
			
			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			
			
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			
			form.Controls.Add (panel);
			
			// needed else DataGrid does not initialize
			
			form.Show ();
			//form.Visible = false;
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			string panelname = System.Guid.NewGuid().ToString();
			panel.NewLayout (panelname,true, null);
			LayoutDetails.Instance.AddToList (typeof(FAKE_NoteDataXML_Panel), "testingpanel");
			
			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel();
			panelA.GuidForNote="panelA";
			panel.AddNote(panelA);


		
			//panel.AddNote(panelA); Intentionally do not add this because it should bea  failre

			// Won't actually add things properly because I need to override the findbar
			FAKE_FindBar findBar = new FAKE_FindBar();
			form.Controls.Add (findBar);
			RichTextExtended fakeText = new RichTextExtended();
			form.Controls.Add (fakeText);
			fakeText.Text = "We have a dog. His name is Jake." + Environment.NewLine + "We love him a lot. He is a nice dog. We all love dogs.";
			findBar.DoFind("dog", false,fakeText,0);

			Assert.AreEqual(panelA.GetPanelsLayout().GetFindbar(), panel.GetFindbar());
		//	Assert.AreEqual(panelB.GetPanelsLayout().GetFindbar(), panel.GetFindbar());

		}
	}
}

