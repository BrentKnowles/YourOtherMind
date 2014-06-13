// LayoutDatabaseTest.cs
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
using Layout.data;
using Layout;
using LayoutPanels;
// Use FakeLayoutDatabase, not the real one to avoid messing with the data files
namespace Testing
{
	[TestFixture]
	//[Ignore]
	public class LayoutDatabaseTest
	{
		public LayoutDatabaseTest ()
		{
		}


		#region general
		private void _setupforlayoutests()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
//			_w.output("boo");
//			LayoutDetails.Instance.YOM_DATABASE = "yom_test_database.s3db";
//			FakeLayoutDatabase layout = new FakeLayoutDatabase("testguid");
//			FAKE_SqlLiteDatabase db = new FAKE_SqlLiteDatabase(layout.GetDatabaseName ());
//			db.DropTableIfExists(dbConstants.table_name);
			// drop the table
		}
		#endregion

		[Test]
		public void EnsureThatDatabaseColumnsEqualsDefinedCount ()
		{
			// This unit test exists to make sure that when I fill in the constants for the table
			// being used that I make sure my column count matches the actual number of oclumns

			// The reas on they could not be the same is that I needed a constant for array initialization
			Assert.AreEqual(dbConstants.ColumnCount, dbConstants.Columns.Length);

		}

//		[Test]
//		public void TryToEditReadOnly ()
//		{
//			FakeLayoutDatabase layout = new FakeLayoutDatabase("testguid");
//			NoteDataXML note = new NoteDataXML();
//			note.Caption = "boo";
//			layout.Add(note);
//			for (int i = 0; i < layout.GetNotes().Count ; i++)
//			{
//				_w.output(layout.GetNotes()[i].Caption);
//				layout.GetNotes()[i].Caption = "snake";
//				_w.output(layout.GetNotes()[i].Caption);
//				_w.output("NOTE: I know this test will fail because I can make the list readonly but not the objects on the list. (and honestly, maybe this is how it needs to work!)");
//				Assert.AreEqual(layout.GetNotes()[i].Caption, "boo");
//			}
//			//System.Collections.Generic.List<NoteDataXML> list = layout.GetNotes();
//		}
		[Test]
		[ExpectedException]
		public void  SaveTo_BlankGUID ()
		{
			FakeLayoutDatabase layout = new FakeLayoutDatabase("");
			layout.SaveTo ();
		}

		[Test]
		public void TryToAddANullNote()
		{
			// no failure should happen. Simply should not be allowed to to do this.
			FakeLayoutDatabase layout = new FakeLayoutDatabase("testguid");
			NoteDataXML note = new NoteDataXML();
			note.Caption = "boo";
			layout.Add(note);
			// we should have one note in the list. Now add null
			layout.Add (null);
			Assert.AreEqual(1, layout.GetNotes().Count);
		}
		[Test]
		[ExpectedException]
		public void SaveAndLoadTest_FailsIfNoAppearanceDefined ()
		{
			// without appearances setup notes will fail, this is intentional
			// 
			_TestSingleTon.Instance._SetupForLayoutPanelTests(true);
			
			int linktable = 1;
			int extranodeadded = 1;
			int count = 25;
			///FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel(CoreUtilities.Constants.BLANK, false);
			layoutPanel.NewLayout("testguid", true, null);
			//layout.LoadFrom(layoutPanel);
			NoteDataXML note = new NoteDataXML ();
			for (int i = 0; i < count; i++) {
				note.Caption = "boo" + i.ToString();
				layoutPanel.AddNote (note);
			}
			
			layoutPanel.SaveLayout();
			_w.output("save worked");
			//layout = new FakeLayoutDatabase ("testguid");
			layoutPanel = new LayoutPanel(CoreUtilities.Constants.BLANK, false);
			layoutPanel.LoadLayout("testguid", false, null);
			//layout.LoadFrom(layoutPanel);
			
			//	_w.output (layout.Backup ());
			//_w.output(layout.GetNotes().Count.ToString());
			Assert.AreEqual(count+linktable +extranodeadded, layoutPanel.GetAllNotes().Count);
			
			
		}
		[Test]
		public void SaveAndLoadTest_Works ()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();

			int linktable = 1;
			int extranodeadded = 1;
			int count = 25;
			///FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel(CoreUtilities.Constants.BLANK, false);
			layoutPanel.NewLayout("testguid", true, null);
			//layout.LoadFrom(layoutPanel);
			NoteDataXML note = new NoteDataXML ();
			for (int i = 0; i < count; i++) {
				note.Caption = "boo" + i.ToString();
				layoutPanel.AddNote (note);
			}

			layoutPanel.SaveLayout();
			_w.output("save worked");
			//layout = new FakeLayoutDatabase ("testguid");
			layoutPanel = new LayoutPanel(CoreUtilities.Constants.BLANK, false);
			layoutPanel.LoadLayout("testguid", false, null);
			//layout.LoadFrom(layoutPanel);

		//	_w.output (layout.Backup ());
			//_w.output(layout.GetNotes().Count.ToString());
			Assert.AreEqual(count+linktable +extranodeadded, layoutPanel.GetAllNotes().Count);


		}
		[Test]

		public void LoadSomethingNotThere()
		{
			_setupforlayoutests();

			// this one needs to create the database

			FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
		//	LayoutPanel layoutPanel = new LayoutPanel(CoreUtilities.Constants.BLANK, false);
			//layoutPanel.NewLayout("testguid", false, null);
		//	layoutPanel.LoadLayout("testguid", false, null);
			bool result = layout.LoadFrom(null);
			_w.output("made it");
			Assert.AreEqual(false, result);
		}




		/// <summary>
		/// Saves the and load stress load.
		/// 
		/// Does several larger saves and loads lookign to see if any objects get lost
		/// </summary>
		[Test]
		public void SaveAndLoadStressLoad()
		{
			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			int linktable = 1;
			int extranodeadded = 1;
			_w.output("this is a slow test. 6 minutes?");
			int count = 200;
			//FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel(CoreUtilities.Constants.BLANK, false);
			layoutPanel.NewLayout("testguid", true, null);
			form.Controls.Add (layoutPanel);
			form.Show ();

			NoteDataXML note = new NoteDataXML ();
			for (int i = 0; i < count; i++) {
				note.Caption = "boo" + i.ToString();
				layoutPanel.AddNote (note);
				note.CreateParent(layoutPanel);
			}
			//_w.output(String.Format ("{0} Notes in Layout before save", layout.GetNotes().Count.ToString()));
			layoutPanel.SaveLayout();

			//_w.output(String.Format ("{0} Objects Saved", layout.ObjectsSaved().ToString()));
			//layoutPanel = new FakeLayoutDatabase ("testguid");
			 layoutPanel = new LayoutPanel(CoreUtilities.Constants.BLANK, false);
			layoutPanel.LoadLayout("testguid", false, null);
			//layoutPanel.LoadFrom(layoutPanel);

			//_w.output(String.Format ("{0} Objects Loaded", layout.GetNotes().Count));
			//NOT DONE YET
			Assert.AreEqual (count + linktable + extranodeadded, layoutPanel.GetAllNotes().Count); 
		}

		/*// I made the decision to suppress Errors (hence not required CreateParent, for the purpose of MOVING notes
		// This violated an earlier decision I had made and I had to disable the TryToSaveWithoutCreating a Parent Exception
		[Test]
		[ExpectedException]
		public void TryToSaveWithoutCreatingAParent()
		{
			Type TypeToTest = typeof(NoteDataXML_RichText);
			// 1. write data to notes
			_setupforlayoutests ();
			int count = 25;
			FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel ();
			
			NoteDataInterface note = null; 
			for (int i = 0; i < count; i++) {
				note = (NoteDataInterface)Activator.CreateInstance(TypeToTest);//new NoteDataXML ();
			//	note.CreateParent(layoutPanel);
				note.Caption = "boo" + i.ToString ();
				layout.Add (note);
			}
			
			
			note = (NoteDataInterface)Activator.CreateInstance(TypeToTest);
			note.Caption = "snake";
			string guid = note.GuidForNote;
			//note.CreateParent(layoutPanel);
			_w.output("new guid" + guid);
			layout.Add (note);
			layout.SaveTo ();
			_w.output ("save worked");
		}*/

		/// <summary>
		/// Creates the lots of notes to test different types and return proper caption.
		/// 
		/// Testing NOTE: You must CreateParent when building items. Else the save cannot work.
		/// 
		/// </summary>
		/// <returns>
		/// The lots of notes to test different types and return proper caption.
		/// </returns>
		/// <param name='TestCaption'>
		/// Test caption.
		/// </param>
		/// <param name='TypeToTest'>
		/// Type to test.
		/// </param>
		private string CreateLotsOfNotesToTestDifferentTypesAndReturnProperCaption (string TestCaption, Type TypeToTest)
		{

			// 1. write data to notes
			//System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			_TestSingleTon.Instance._SetupForLayoutPanelTests();


			int count = 25;
		//	FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel (CoreUtilities.Constants.BLANK, false);
			layoutPanel.NewLayout("testguid", true, null);

			NoteDataInterface note = null; 
			for (int i = 0; i < count; i++) {
				note = (NoteDataInterface)Activator.CreateInstance(TypeToTest);//new NoteDataXML ();

				note.Caption = "boo" + i.ToString ();
				layoutPanel.AddNote (note);
				note.CreateParent(layoutPanel);
			}
			
			
			note = (NoteDataInterface)Activator.CreateInstance(TypeToTest);
			note.Caption = TestCaption;
			string guid = note.GuidForNote;
			note.CreateParent(layoutPanel);
			_w.output("new guid" + guid);
			layoutPanel.AddNote (note);
			layoutPanel.SaveLayout ();
			_w.output ("save worked");
			
			// 2. Now we pretend that later one, elsewhere in code, we need to get access to this (i.e., a Random Table)
			layoutPanel = new LayoutPanel (CoreUtilities.Constants.BLANK, false);
			//layout = new FakeLayoutDatabase ("testguid");
			layoutPanel.LoadLayout("testguid",false, null);
			
			//layoutPanel.LoadLayout( (null);
			//	_w.output (layout.Backup ());
			//_w.output(layoutPanel.GetNotes().Count.ToString());
			foreach (NoteDataXML _note in layoutPanel.GetAllNotes()) {
				
				if( _note.GuidForNote == guid)
				{
					_w.output(_note.Caption);
					return note.Caption;
				}
				
			}
			return "<error>";
		}

		[Test]
		public void CreateAllCoreNoteTypesViaActivator()
		{

			foreach (Type t in LayoutDetails.Instance.ListOfTypesToStoreInXML())
			{
				_w.output("creating " + t.ToString());
			NoteDataInterface note = null;
						
				note = (NoteDataInterface)Activator.CreateInstance (t, -1, -1);
				Assert.NotNull(note);
			}
		}
		[Test]
		public void CreateAllCoreNoteTypesWithAValidGUID()
		{
			
			foreach (Type t in LayoutDetails.Instance.ListOfTypesToStoreInXML())
			{

				NoteDataInterface note = null;
				
				note = (NoteDataInterface)Activator.CreateInstance (t, -1, -1);
				Assert.NotNull(note);
				_w.output(String.Format ("creating {0} with GUID {1}", t.ToString(), note.GuidForNote));
				Assert.AreNotEqual(CoreUtilities.Constants.BLANK, note.GuidForNote);

			}
		}
		[Test]
		public void TryMixedTypeSave()
		{
			// 1. write data to notes
			_setupforlayoutests ();
			int count = 25;
		//	FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel (CoreUtilities.Constants.BLANK, false);
			layoutPanel.NewLayout("testguid", true, null);
			NoteDataInterface note = null; 
			for (int i = 0; i < count; i++) {
				note = (NoteDataInterface)Activator.CreateInstance(typeof(NoteDataXML));//new NoteDataXML ();

				note.Caption = "boo" + i.ToString ();
				layoutPanel.AddNote (note);
				note.CreateParent(layoutPanel);
			}
			
			// store a SECOND TYPE into the mix
			note = (NoteDataInterface)Activator.CreateInstance(typeof(NoteDataXML_RichText));
			note.Caption = "textnote";
			string guid = note.GuidForNote;

			_w.output("new guid" + guid);
			layoutPanel.AddNote (note);
			note.CreateParent(layoutPanel);
			layoutPanel.SaveLayout ();
		}
		[Test]
		public void RemoteGrabNote_base ()
		{

			string result = CreateLotsOfNotesToTestDifferentTypesAndReturnProperCaption("fishy", typeof(NoteDataXML));

			Assert.AreEqual("fishy", result);
		}
		[Test]
		public void RemoteGrabNote_text ()
		{
			
			string result = CreateLotsOfNotesToTestDifferentTypesAndReturnProperCaption("fishytexts", typeof(NoteDataXML_RichText));
			
			Assert.AreEqual("fishytexts", result);
			
		}

		[Test]
		public void TestSavingRTFTextWorks()
		{
			Type TypeToTest = typeof(NoteDataXML_RichText);
			// 1. write data to notes
			_setupforlayoutests ();
			int count = 15;
			//FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			FAKE_LayoutPanel layoutPanel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			layoutPanel.NewLayout("testguid", true, null);
			NoteDataInterface note = null; 
			for (int i = 0; i < count; i++) {
				note = (NoteDataInterface)Activator.CreateInstance(TypeToTest);//new NoteDataXML ();

				note.Caption = "boo" + i.ToString ();
				layoutPanel.AddNote (note);
				note.CreateParent(layoutPanel);
			}
			
			
			note = (NoteDataInterface)Activator.CreateInstance(TypeToTest);
			note.Caption = "snake";
			string guid = note.GuidForNote;
			string teststring = "\nthe test is this ";
			

			note.Data1=(@"{\rtf1\ansi\ansicpg1252\deff0\deflang4105{\fonttbl{\f0\fnil\fcharset0 Microsoft Sans Serif;}}"+
			                         @"\viewkind4\uc1\pard\f0\fs17\par the test is this }");
			
				
		//	note.Data1=String.Format (@"{\rtf1\ansi\ansicpg1252\deff0\deflang4105{\fonttbl{\f0\fnil\fcharset0 Microsoft Sans Serif;}}\viewkind4\uc1\pard\f0\fs17\par {0} \par}", teststring);

				//"This is the story of the bird.";
			note.CreateParent(layoutPanel);
			_w.output("new guid" + guid);

			layoutPanel.AddNote (note);
			//layout.SaveTo ();
			layoutPanel.SaveLayout();
			//layout = new FakeLayoutDatabase ("testguid");
			layoutPanel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			layoutPanel.LoadLayout("testguid", false, null);
			//layout.LoadFrom(layoutPanel);
			
			//_w.output(String.Format ("{0} Objects Loaded", layout.GetNotes().Count));

			NoteDataInterface result = layoutPanel.GetLayoutDatabase().GetNoteByGUID(guid);
			string astest = ((NoteDataXML_RichText)result).GetAsText();
			_w.output(result.Data1);
			_w.output (astest);
			Assert.AreEqual(teststring, astest);

		}

		[Test]
		public void CountSpecificSubType ()
		{
			//-- do unit tests counting store 6 textboxes and know this (countbytype)
			
			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			_TestSingleTon.Instance._SetupForLayoutPanelTests();


			int count = 25;
		//	FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			FAKE_LayoutPanel layoutPanel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			layoutPanel.NewLayout("testguid", true, null);

			// jan152013 - tweak to allow this to work with new system without rewriting all the tstings in LayoutDatabasetest
			LayoutDatabase layout = layoutPanel.GetLayoutDatabase();

			form.Controls.Add (layoutPanel);
			form.Show ();

			NoteDataXML note = new NoteDataXML ();
			for (int i = 0; i < count; i++) {

				note.Caption = "boo" + i.ToString ();
				layout.Add (note);
				note.CreateParent(layoutPanel);
			}
			_w.output (String.Format ("{0} Notes in Layout before save", layout.GetNotes ().Count.ToString ()));

			for (int i = 0; i < 6; i++) {
				note = new NoteDataXML_RichText ();
			
				note.Caption = "richText";
				layout.Add (note);
				note.CreateParent(layoutPanel);
			}

			layout.SaveTo();
			
		//	_w.output(String.Format ("{0} Objects Saved", layout.ObjectsSaved().ToString()));
			layout = new FakeLayoutDatabase ("testguid");
			
			layout.LoadFrom(layoutPanel);

			// now count RichText notes
			int count2 = 0;
			foreach (NoteDataInterface _note in layout.GetNotes ())
			{
				if (_note.GetType() == typeof(NoteDataXML_RichText))
				    {
					count2++;
				}

				}

			_w.output(String.Format ("{0} Objects Loaded", layout.GetNotes().Count));


			// added linktable
			Assert.AreEqual (7, count2); 
		}

		[Test]
		public void CountPanelsSubType ()
		{

			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			
			//FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			Assert.False (MasterOfLayouts.ExistsByGUID("testlayout"));


			_w.output("here");
			//-- do unit tests counting store 6 textboxes and know this (countbytype)
			//_setupforlayoutests ();

			_w.output("here");
			int count = 25;

		//	FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			FAKE_LayoutPanel layoutPanel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			layoutPanel.NewLayout("testlayout", false, null);
			form.Controls.Add (layoutPanel);
			form.Show ();
		


			NoteDataXML note = new NoteDataXML ();
			for (int i = 0; i < count; i++) {

				note.Caption = "boo" + i.ToString ();
				layoutPanel.AddNote (note);
				note.CreateParent(layoutPanel);
			}
			_w.output (String.Format ("{0} Notes in Layout before save", layoutPanel.GetAllNotes ().Count.ToString ()));
			
			for (int i = 0; i < 6; i++) {
				note = new NoteDataXML_Panel ();
			

				note.Caption = "Panel";
				layoutPanel.AddNote (note);
				note.CreateParent(layoutPanel);
				((NoteDataXML_Panel)note).Add10TestNotes();
			}



			
			layoutPanel.SaveLayout();
			layoutPanel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			//_w.output(String.Format ("{0} Objects Saved", layoutPanel.ObjectsSaved().ToString()));
			layoutPanel.LoadLayout("testlayout",false, null);

//			layout = new FakeLayoutDatabase ("testguid");
//			
//			layout.LoadFrom(layoutPanel);
			
			// now count RichText notes
			int count2 = 0;
			int subnotecount = 0;
			foreach (NoteDataInterface _note in layoutPanel.GetAllNotes ())
			{
				if (_note.GetType() == typeof(NoteDataXML_Panel))
				{
					count2++;
					subnotecount = subnotecount + ((NoteDataXML_Panel)_note).GetChildNotes().Count;
				}
				
			}


			// total note count should be (once I get GetNotes working on Child Notes = 60 + 6 + 25 = 91

			_w.output(String.Format ("{0} Objects Loaded", layoutPanel.GetAllNotes().Count));
			//NOT DONE YET
			Assert.AreEqual (6, count2); 
			Assert.AreEqual (60, subnotecount); 
			Assert.AreEqual (count2, layoutPanel.GetAvailableFolders().Count);

			// had to change because a linktable makes 91 become 92
			Assert.AreEqual (92, layoutPanel.GetAllNotes().Count); 
		}
		[Test]
		public void LayoutShouldNotExist()
		{
			_setupforlayoutests ();
			int count = 25;
			//FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel (CoreUtilities.Constants.BLANK, false);
			layoutPanel.NewLayout("testguid", true, null);
			NoteDataXML note = new NoteDataXML ();
			for (int i = 0; i < count; i++) {

				note.Caption = "boo" + i.ToString ();
				layoutPanel.AddNote (note);
				note.CreateParent(layoutPanel);
			}
		//	layout.SaveTo(); no save, so note should not exist
			
			Assert.False (MasterOfLayouts.ExistsByGUID("testguid"));
		}


		[Test]
		public void LayoutExists()
		{
			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			_w.output("1");
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			_w.output("2");
			int count = 25;
			//FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel (CoreUtilities.Constants.BLANK, false);
			layoutPanel.NewLayout("testguid", true, null);

			NoteDataXML note = new NoteDataXML ();
			for (int i = 0; i < count; i++) {

				note.Caption = "boo" + i.ToString ();
				layoutPanel.AddNote (note);
				note.CreateParent(layoutPanel);
			}
			layoutPanel.SaveLayout();
			layoutPanel.Dispose ();
			Assert.True (MasterOfLayouts.ExistsByGUID("testguid"));
		}
		[Test]
		public void SaveNotequired()
		{	_setupforlayoutests ();
			int count = 25;
			//FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel (CoreUtilities.Constants.BLANK, false);
			layoutPanel.NewLayout ("testguid", true, null);
			NoteDataXML_RichText note = new NoteDataXML_RichText ();
			for (int i = 0; i < count; i++) {

				note.Caption = "boo" + i.ToString ();
				//note.UpdateLocation();
				layoutPanel.AddNote (note);
				note.CreateParent(layoutPanel);
			}
			// cannot actually save becaue LayoutPanel is just fake for this test
			// but we check that the flag was set correclty -- i..e, nothing improtant changes, nothing needs tob e saved
			//	layout.SaveTo(); 
			//layoutPanel.SaveLayout();

			// April 2013 - This test was flagged as Assert.False but I don't understand
			// We WANT the flag to say we need a save, no?
			Assert.True (layoutPanel.GetSaveRequired);	
			
		}

		[Test]
		public void SaveRequired()
		{	_setupforlayoutests ();
			int count = 25;
			//FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel (CoreUtilities.Constants.BLANK, false);
			layoutPanel.NewLayout("testguid", false, null);
			NoteDataXML_RichText note = new NoteDataXML_RichText ();
			for (int i = 0; i < count; i++) {

				note.Caption = "boo" + i.ToString ();

				layoutPanel.AddNote (note);
				note.CreateParent(layoutPanel);
				note.UpdateLocation();
			}
			//layout.SaveTo(); 
			Assert.True (layoutPanel.GetSaveRequired);	

		}
		[Test]
		public void TestDeleteNote()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			// add a note with specific label
			FAKE_LayoutPanel layoutPanel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			//FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			layoutPanel.NewLayout("testguid", true, null);
			NoteDataXML_RichText note = new NoteDataXML_RichText ();
			string guid2find = "";
			NoteDataInterface mynotetogo = null;
			for (int i = 0; i < 1; i++) {
				note.CreateParent(layoutPanel);
				note.Caption = "boo" + i.ToString ();
				note.UpdateLocation();
				guid2find = note.GuidForNote;
				layoutPanel.AddNote (note);
				mynotetogo = note;
			}
		//	layout.SaveTo(); 
			layoutPanel.SaveLayout();
			_w.output (guid2find);
			Assert.True (layoutPanel.GetLayoutDatabase().IsNoteExistsInLayout (guid2find));
			_w.output("here");
			// then delete it

			layoutPanel.GetLayoutDatabase().RemoveNote(mynotetogo);
			Assert.False (layoutPanel.GetLayoutDatabase().IsNoteExistsInLayout (guid2find));
		}

		[Test]
		//[Ignore]
		public void NoCrashIfSaveEmptyPanel()
		{	_setupforlayoutests ();

			//FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel (CoreUtilities.Constants.BLANK, false);
			layoutPanel.NewLayout("testguid", true, null);
			NoteDataXML_Panel note = new NoteDataXML_Panel ();
			layoutPanel.AddNote (note);
			note.CreateParent(layoutPanel);
			layoutPanel.SaveLayout();

			_w.output("This often fails if we have added a new Layout Variable and it has not been set to a default value in constructor");
			
		}
		[Test]
		//[Ignore]
		public void DeleteTest()
		{

			_setupforlayoutests ();
			Assert.False (MasterOfLayouts.ExistsByGUID("DeleteMe"));

			//FakeLayoutDatabase layout = new FakeLayoutDatabase ("DeleteMe");
			FAKE_LayoutPanel layoutPanel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			layoutPanel.NewLayout("DeleteMe",true,  null);
			//LayoutDatabase layout = layoutPanel.GetLayoutDatabase();
			//layout.SaveTo ();
			layoutPanel.SaveLayout ();


			Assert.True (MasterOfLayouts.ExistsByGUID("DeleteMe"));
			MasterOfLayouts.DeleteLayout ("DeleteMe");
			Assert.False (MasterOfLayouts.ExistsByGUID("DeleteMe"));
			layoutPanel.Dispose();
		}
		[Test]
		[ExpectedException]
		public void TestInvalidAddToStart()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			FakeLayoutDatabase layout = new FakeLayoutDatabase ("DeleteMe");
			//LayoutPanel layoutPanel = new LayoutPanel (CoreUtilities.Constants.BLANK, false);
			//layoutPanel.NewLayout("testguid", true, null);
			layout.SaveTo ();
			NoteDataXML test = new NoteDataXML();
			test.GuidForNote = "boo";
			layout.AddToStart(test);
		}
		[Test]
		public void TestAddToStart()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			FakeLayoutDatabase layout = new FakeLayoutDatabase ("DeleteMe");
		//	LayoutPanel layoutPanel = new LayoutPanel (CoreUtilities.Constants.BLANK, false);
			layout.SaveTo ();
			NoteDataXML test = new NoteDataXML();
			test.GuidForNote = CoreUtilities.Links.LinkTable.STICKY_TABLE;
			layout.AddToStart(test);
		}

	}
}

