using System;
using NUnit.Framework;
using Layout.data;
using Layout;
using LayoutPanels;
// Use FakeLayoutDatabase, not the real one to avoid messing with the data files
namespace Testing
{
	[TestFixture]
	public class LayoutDatabaseTest
	{
		public LayoutDatabaseTest ()
		{
		}


		#region general
		private void _setupforlayoutests()
		{
			FakeLayoutDatabase layout = new FakeLayoutDatabase("testguid");
			FAKE_SqlLiteDatabase db = new FAKE_SqlLiteDatabase(layout.GetDatabaseName ());
			db.DropTableIfExists(tmpDatabaseConstants.table_name);
			// drop the table
		}
		#endregion

		[Test]
		public void EnsureThatDatabaseColumnsEqualsDefinedCount ()
		{
			// This unit test exists to make sure that when I fill in the constants for the table
			// being used that I make sure my column count matches the actual number of oclumns

			// The reas on they could not be the same is that I needed a constant for array initialization
			Assert.AreEqual(tmpDatabaseConstants.ColumnCount, tmpDatabaseConstants.Columns.Length);

		}

		[Test]
		public void TryToEditReadOnly ()
		{
			FakeLayoutDatabase layout = new FakeLayoutDatabase("testguid");
			NoteDataXML note = new NoteDataXML();
			note.Caption = "boo";
			layout.Add(note);
			for (int i = 0; i < layout.GetNotes().Count ; i++)
			{
				_w.output(layout.GetNotes()[i].Caption);
				layout.GetNotes()[i].Caption = "snake";
				_w.output(layout.GetNotes()[i].Caption);
				_w.output("NOTE: I know this test will fail because I can make the list readonly but not the objects on the list. (and honestly, maybe this is how it needs to work!)");
				Assert.AreEqual(layout.GetNotes()[i].Caption, "boo");
			}
			//System.Collections.Generic.List<NoteDataXML> list = layout.GetNotes();
		}
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
		public void SaveAndLoadTest_Works ()
		{
			_setupforlayoutests();
			int count = 25;
			FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel();
			layout.LoadFrom(layoutPanel);
			NoteDataXML note = new NoteDataXML ();
			for (int i = 0; i < count; i++) {
				note.Caption = "boo" + i.ToString();
				layout.Add (note);
			}

			layout.SaveTo();
			_w.output("save worked");
			layout = new FakeLayoutDatabase ("testguid");

			layout.LoadFrom(layoutPanel);

		//	_w.output (layout.Backup ());
			_w.output(layout.GetNotes().Count.ToString());
			Assert.AreEqual(count, layout.GetNotes().Count);


		}
		[Test]
		public void LoadSomethingNotThere()
		{
			_setupforlayoutests();

			FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel();
			bool result = layout.LoadFrom(layoutPanel);
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
			_setupforlayoutests();
			int count = 200;
			FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel();

			NoteDataXML note = new NoteDataXML ();
			for (int i = 0; i < count; i++) {
				note.Caption = "boo" + i.ToString();
				layout.Add (note);
			}
			_w.output(String.Format ("{0} Notes in Layout before save", layout.GetNotes().Count.ToString()));
			layout.SaveTo();

			_w.output(String.Format ("{0} Objects Saved", layout.ObjectsSaved().ToString()));
			layout = new FakeLayoutDatabase ("testguid");
			
			layout.LoadFrom(layoutPanel);

			_w.output(String.Format ("{0} Objects Loaded", layout.GetNotes().Count));
			//NOT DONE YET
			Assert.AreEqual (200, layout.GetNotes().Count); 
		}


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
		}

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
			_setupforlayoutests ();
			int count = 25;
			FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel ();
			
			NoteDataInterface note = null; 
			for (int i = 0; i < count; i++) {
				note = (NoteDataInterface)Activator.CreateInstance(TypeToTest);//new NoteDataXML ();
				note.CreateParent(layoutPanel);
				note.Caption = "boo" + i.ToString ();
				layout.Add (note);
			}
			
			
			note = (NoteDataInterface)Activator.CreateInstance(TypeToTest);
			note.Caption = TestCaption;
			string guid = note.GuidForNote;
			note.CreateParent(layoutPanel);
			_w.output("new guid" + guid);
			layout.Add (note);
			layout.SaveTo ();
			_w.output ("save worked");
			
			// 2. Now we pretend that later one, elsewhere in code, we need to get access to this (i.e., a Random Table)
			
			layout = new FakeLayoutDatabase ("testguid");
			
			layout.LoadFrom (null);
			//	_w.output (layout.Backup ());
			_w.output(layout.GetNotes().Count.ToString());
			foreach (NoteDataXML _note in layout.GetNotes()) {
				
				if( _note.GuidForNote == guid)
				{
					_w.output(_note.Caption);
					return note.Caption;
				}
				
			}
			return "<error>";
		}
		[Test]
		public void TryMixedTypeSave()
		{
			// 1. write data to notes
			_setupforlayoutests ();
			int count = 25;
			FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel ();
			
			NoteDataInterface note = null; 
			for (int i = 0; i < count; i++) {
				note = (NoteDataInterface)Activator.CreateInstance(typeof(NoteDataXML));//new NoteDataXML ();
				note.CreateParent(layoutPanel);
				note.Caption = "boo" + i.ToString ();
				layout.Add (note);
			}
			
			// store a SECOND TYPE into the mix
			note = (NoteDataInterface)Activator.CreateInstance(typeof(NoteDataXML_RichText));
			note.Caption = "textnote";
			string guid = note.GuidForNote;
			note.CreateParent(layoutPanel);
			_w.output("new guid" + guid);
			layout.Add (note);
			layout.SaveTo ();
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
			FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel ();
			
			NoteDataInterface note = null; 
			for (int i = 0; i < count; i++) {
				note = (NoteDataInterface)Activator.CreateInstance(TypeToTest);//new NoteDataXML ();
					note.CreateParent(layoutPanel);
				note.Caption = "boo" + i.ToString ();
				layout.Add (note);
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

			layout.Add (note);
			layout.SaveTo ();

			layout = new FakeLayoutDatabase ("testguid");
			
			layout.LoadFrom(layoutPanel);
			
			_w.output(String.Format ("{0} Objects Loaded", layout.GetNotes().Count));

			NoteDataInterface result = layout.GetNoteByGUID(guid);
			string astest = ((NoteDataXML_RichText)result).GetAsText();
			_w.output(result.Data1);
			_w.output (astest);
			Assert.AreEqual(teststring, astest);

		}

		[Test]
		public void CountSpecificSubType ()
		{
			//-- do unit tests counting store 6 textboxes and know this (countbytype)
			_setupforlayoutests ();
			int count = 25;
			FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel ();
			
			NoteDataXML note = new NoteDataXML ();
			for (int i = 0; i < count; i++) {
				note.CreateParent(layoutPanel);
				note.Caption = "boo" + i.ToString ();
				layout.Add (note);
			}
			_w.output (String.Format ("{0} Notes in Layout before save", layout.GetNotes ().Count.ToString ()));

			for (int i = 0; i < 6; i++) {
				note = new NoteDataXML_RichText ();
				note.CreateParent(layoutPanel);
				note.Caption = "richText";
				layout.Add (note);
			}

			layout.SaveTo();
			
			_w.output(String.Format ("{0} Objects Saved", layout.ObjectsSaved().ToString()));
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
			//NOT DONE YET
			Assert.AreEqual (6, count2); 
		}

		[Test]
		public void CountPanelsSubType ()
		{
			//-- do unit tests counting store 6 textboxes and know this (countbytype)
			_setupforlayoutests ();
			int count = 25;
			FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel ();
			
			NoteDataXML note = new NoteDataXML ();
			for (int i = 0; i < count; i++) {
				note.CreateParent(layoutPanel);
				note.Caption = "boo" + i.ToString ();
				layout.Add (note);
			}
			_w.output (String.Format ("{0} Notes in Layout before save", layout.GetNotes ().Count.ToString ()));
			
			for (int i = 0; i < 6; i++) {
				note = new NoteDataXML_Panel ();
				note.CreateParent(layoutPanel);

				note.Caption = "Panel";
				layout.Add (note);
				((NoteDataXML_Panel)note).Add10TestNotes();
			}



			
			layout.SaveTo();
			
			_w.output(String.Format ("{0} Objects Saved", layout.ObjectsSaved().ToString()));
			layout = new FakeLayoutDatabase ("testguid");
			
			layout.LoadFrom(layoutPanel);
			
			// now count RichText notes
			int count2 = 0;
			int subnotecount = 0;
			foreach (NoteDataInterface _note in layout.GetNotes ())
			{
				if (_note.GetType() == typeof(NoteDataXML_Panel))
				{
					count2++;
					subnotecount = subnotecount + ((NoteDataXML_Panel)_note).GetChildNotes().Count;
				}
				
			}


			// total note count should be (once I get GetNotes working on Child Notes = 60 + 6 + 25 = 91

			_w.output(String.Format ("{0} Objects Loaded", layout.GetAllNotes().Count));
			//NOT DONE YET
			Assert.AreEqual (6, count2); 
			Assert.AreEqual (60, subnotecount); 
			Assert.AreEqual (count2, layout.GetAvailableFolders().Count);
			Assert.AreEqual (91, layout.GetAllNotes().Count); 
		}
		[Test]
		public void NoteShouldNotExist()
		{
			_setupforlayoutests ();
			int count = 25;
			FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel ();
			
			NoteDataXML note = new NoteDataXML ();
			for (int i = 0; i < count; i++) {
				note.CreateParent(layoutPanel);
				note.Caption = "boo" + i.ToString ();
				layout.Add (note);
			}
		//	layout.SaveTo(); no save, so note should not exist
			
			Assert.False (layout.Exists("testguid"));
		}


		[Test]
		public void NoteExists()
		{
			_setupforlayoutests ();
			int count = 25;
			FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel ();
			
			NoteDataXML note = new NoteDataXML ();
			for (int i = 0; i < count; i++) {
				note.CreateParent(layoutPanel);
				note.Caption = "boo" + i.ToString ();
				layout.Add (note);
			}
			layout.SaveTo();

			Assert.True (layout.Exists("testguid"));
		}
		[Test]
		public void SaveNotequired()
		{	_setupforlayoutests ();
			int count = 25;
			FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel ();
			
			NoteDataXML_RichText note = new NoteDataXML_RichText ();
			for (int i = 0; i < count; i++) {
				note.CreateParent(layoutPanel);
				note.Caption = "boo" + i.ToString ();
				//note.UpdateLocation();
				layout.Add (note);
			}
			// cannot actually save becaue LayoutPanel is just fake for this test
			// but we check that the flag was set correclty -- i..e, nothing improtant changes, nothing needs tob e saved
			//	layout.SaveTo(); 
			//layoutPanel.SaveLayout();
			Assert.False (layoutPanel.GetSaveRequired);	
			
		}

		[Test]
		public void SaveRequired()
		{	_setupforlayoutests ();
			int count = 25;
			FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			LayoutPanel layoutPanel = new LayoutPanel ();
			
			NoteDataXML_RichText note = new NoteDataXML_RichText ();
			for (int i = 0; i < count; i++) {
				note.CreateParent(layoutPanel);
				note.Caption = "boo" + i.ToString ();
				note.UpdateLocation();
				layout.Add (note);
			}
			//layout.SaveTo(); 
			Assert.True (layoutPanel.GetSaveRequired);	

		}

		[Test]
		public void MoveNoteTest()
		{
			Assert.True (false);
		}
	}
}

