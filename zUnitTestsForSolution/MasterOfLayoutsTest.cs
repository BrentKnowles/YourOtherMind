using System;
using NUnit.Framework;
using Layout;
using System.IO;
using CoreUtilities;
using System.Windows.Forms;
using System.Collections.Generic;
namespace Testing
{
	[TestFixture]
	public class MasterOfLayoutsTest 
	{
		public MasterOfLayoutsTest ()
		{
		}
//		protected void _SetupForLayoutPanelTests ()
//		{
//			LayoutDetails.Instance.YOM_DATABASE = "yom_test_database.s3db";
//			LayoutDetails.Instance.AddToList(typeof(FAKE_NoteDataXML_Panel),"testingpanel");
//			LayoutDetails.Instance.AddToList(typeof(FAKE_NoteDataXML_Text),"testingtext");
//			
//			FakeLayoutDatabase layout = new FakeLayoutDatabase("testguid");
//			FAKE_SqlLiteDatabase db = new FAKE_SqlLiteDatabase(layout.GetDatabaseName ());
//			db.DropTableIfExists(Layout.data.dbConstants.table_name);
//			_w.output ("dropping table " + Layout.data.dbConstants.table_name);
//		}

		FAKE_LayoutPanel panelAutosave=null;
		[Test]
		public void AutosaveThrash()
		{
			// just spawna timer and see if I can make it fail

			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			
			
			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			
			
			
			
			panelAutosave = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			
			form.Controls.Add (panelAutosave);
			
			// needed else DataGrid does not initialize
			
			form.Show ();
			//form.Visible = false;
			_w.output("boom");
			// March 2013 -- notelist relies on having this
			YOM2013.DefaultLayouts.CreateASystemLayout(form,null);
			
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			string panelname = System.Guid.NewGuid().ToString();
			panelAutosave.NewLayout (panelname,true, null);
			LayoutDetails.Instance.AddToList (typeof(FAKE_NoteDataXML_Panel), "testingpanel");
			_w.output ("herefirst");


			Timer SaveTimer= new Timer();
			SaveTimer.Interval = 300;
			SaveTimer.Tick+= HandleSaveTimerTick;
			SaveTimer.Start ();

			// ADD 1 of each type
			foreach (Type t in LayoutDetails.Instance.ListOfTypesToStoreInXML()) {
				for (int i = 0; i < 2; i++) {
					NoteDataInterface note = (NoteDataInterface)Activator.CreateInstance (t);
					panelAutosave.AddNote (note);
					note.CreateParent(panelAutosave);
					
					note.UpdateAfterLoad();
				}
			}

			panelAutosave.SaveLayout();


			//
			// Second panel
			//

			string panelname2 = System.Guid.NewGuid().ToString();
			FAKE_LayoutPanel  PanelOtherGuy= new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			PanelOtherGuy.NewLayout (panelname2,true, null);
			PanelOtherGuy.SaveLayout();
			
			Assert.AreEqual( 2, PanelOtherGuy.CountNotes(), "count1");	
			
			// ADD 1 of each type
			//foreach (Type t in LayoutDetails.Instance.ListOfTypesToStoreInXML())
			{
				for (int i = 0; i < 10; i++) {

						NoteDataInterface note = new NoteDataXML_RichText();
					PanelOtherGuy.AddNote (note);
					note.CreateParent(PanelOtherGuy);
					
					note.UpdateAfterLoad();

				}
			}
			Assert.AreEqual( 12, PanelOtherGuy.CountNotes(), "count2");	
			PanelOtherGuy.SaveLayout();
			PanelOtherGuy = null;
			PanelOtherGuy= new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			PanelOtherGuy.LoadLayout(panelname2, false, null);
			Assert.AreEqual(12, PanelOtherGuy.CountNotes(), "count2");	
			// add another Layout and do something with it while autosave continues running


			SaveTimer.Stop();
			form.Dispose ();
		}

		void HandleSaveTimerTick (object sender, EventArgs e)
		{
			panelAutosave.SaveLayout();
		}

		[Test]
		public void TestExistsByName()
		{
			//_SetupForLayoutPanelTests ();
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel3", true, null);
			panel.SetName("frank");
			NoteDataXML basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();
			Assert.False (MasterOfLayouts.ExistsByName("dasdfasd"));
			Assert.True (MasterOfLayouts.ExistsByName("frank"));
		}
		[Test]
		public void TestExistsByGUID()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			//	_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);
			NoteDataXML basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();
			Assert.False (MasterOfLayouts.ExistsByGUID("mynewpanel2"));
			Assert.True (MasterOfLayouts.ExistsByGUID("mynewpanel"));
		}
		[Test]
		public void DeleteTests()
		{
				
				// nest several panels
				//_SetupForLayoutPanelTests ();
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
				FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
				
				//NOTE: For now remember that htis ADDS 1 Extra notes
				panel.NewLayout("mynewpanel", true, null);
				NoteDataXML basicNote = new NoteDataXML();
				basicNote.GuidForNote = "thisguid1";
				basicNote.Caption = "note1";
				
				panel.AddNote(basicNote);
				basicNote.CreateParent(panel);
				panel.SaveLayout();

			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel ();
			panelA.Caption = "PanelA";
			panelA.GuidForNote = "panela";
			panel.AddNote (panelA);
			panelA.CreateParent (panel);
			
			
			FAKE_NoteDataXML_Text textNote = new FAKE_NoteDataXML_Text ();
			
			panelA.AddNote (textNote);
			textNote.CreateParent (panelA.GetPanelsLayout ());
			textNote.Caption = "My text";
			textNote.GetRichTextBox ().Text = "Hello there.";
			panel.SaveLayout ();
			Assert.AreEqual (2, MasterOfLayouts.Count (true));
			Assert.True (MasterOfLayouts.ExistsByGUID("mynewpanel"));
			MasterOfLayouts.DeleteLayout("mynewpanel");
			Assert.False (MasterOfLayouts.ExistsByGUID("mynewpanel"));
			Assert.AreEqual (0, MasterOfLayouts.Count (true));
				
				

		}
		[Test]
		public void BuildReciprocalLinks()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			//	_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);

			NoteDataXML basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();

			// #2



			panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			panel.NewLayout("mynewpanel2", true, null);
			basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();

			NoteDataXML_LinkNote note = new NoteDataXML_LinkNote();




			panel.AddNote (note);
			note.CreateParent(panel);
			note.SetLink("mynewpanel.thisguid1");
			panel.SaveLayout();


		

			System.Collections.Generic.List<string> linkstome = MasterOfLayouts.ReciprocalLinks("mynewpanel2");
			Assert.AreEqual(0, linkstome.Count);
			linkstome = MasterOfLayouts.ReciprocalLinks("mynewpanel");
			Assert.AreEqual(1, linkstome.Count);

		}

		[Test]
		public void CountSizeOfCombinedTableAfterReciprocalLinks()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			//	_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);
			
			NoteDataXML basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();
			
			// #2
			
			
			
			panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			panel.NewLayout("mynewpanel2", true, null);
			basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();
			
			NoteDataXML_LinkNote note = new NoteDataXML_LinkNote();
			
			
			
			
			panel.AddNote (note);
			note.CreateParent(panel);
			note.SetLink("mynewpanel.thisguid1");
			panel.SaveLayout();
			
			


			
			// #3
			
			
			
			panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			panel.NewLayout("mynewpanel3", true, null);
			basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();
			
			note = new NoteDataXML_LinkNote();
			
			
			
			
			panel.AddNote (note);
			note.CreateParent(panel);
			note.SetLink("mynewpanel2.thisguid1");
			panel.SaveLayout();


			Assert.AreEqual(2, Fake_MasterOfLayouts.GetRowCountOfCombinedLinkTableAfterCallToReciprocalLinks());



			
		}
		[Test]
		public void AddLinkToSubpanel()
		{
			// get the link into a subpanel and make the counts are as expected
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			//	_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);
			
			NoteDataXML basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();


			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel ();
			panelA.Caption = "PanelA";
			panel.AddNote(panelA);
			panelA.CreateParent(panel);

			panel.SaveLayout();
			System.Collections.Generic.List<string> linkstome = MasterOfLayouts.ReciprocalLinks("mynewpanel");
			Assert.AreEqual(0, linkstome.Count);


			NoteDataXML_LinkNote link = new NoteDataXML_LinkNote();

			panelA.AddNote (link);
			link.CreateParent(panelA.GetPanelsLayout());
			link.SetLink("mynewpanel.thisguid1");
			panel.SaveLayout();
	

			linkstome = MasterOfLayouts.ReciprocalLinks("mynewpanel");
			Assert.AreEqual(1, linkstome.Count);

		}

		[Test]
		public void AddAndThenEditLinks()
		{
			// add a couple links, take count
			// then edit one of the early links a few times
			// get the link into a subpanel and make the counts are as expected
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			//	_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);
			
			NoteDataXML basicNote = new NoteDataXML();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			basicNote.CreateParent(panel);
			panel.SaveLayout();
			
			// in this test we don't use the subpanel but just have it here for fun
			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel ();
			panelA.Caption = "PanelA";
			panel.AddNote(panelA);
			panelA.CreateParent(panel);
			
			panel.SaveLayout();
			System.Collections.Generic.List<string> linkstome = MasterOfLayouts.ReciprocalLinks("mynewpanel");
			Assert.AreEqual(0, linkstome.Count);
			
			
			NoteDataXML_LinkNote link = new NoteDataXML_LinkNote();
			
			panel.AddNote (link);
			link.CreateParent(panel);
			link.SetLink("mynewpanel.thisguid1");
			panel.SaveLayout();
			
			
			linkstome = MasterOfLayouts.ReciprocalLinks("mynewpanel");
			Assert.AreEqual(1, linkstome.Count);

			// now edit the existing link
			link.SetLink("mynewpanel.thisguid2");
			panel.SaveLayout();
			Assert.AreEqual(1, linkstome.Count);
			// the counts should remain the same


		}
		[Test]
		public void IdentifySubpanels ()
		{
		
			// get the link into a subpanel and make the counts are as expected
			_TestSingleTon.Instance._SetupForLayoutPanelTests ();
			//	_SetupForLayoutPanelTests ();
				
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
				
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout ("mynewpanel", true, null);
				
			NoteDataXML basicNote = new NoteDataXML ();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
				
			panel.AddNote (basicNote);
			basicNote.CreateParent (panel);
			panel.SaveLayout ();
				
				
			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel ();
			panelA.Caption = "PanelA";
			panelA.GuidForNote = "panela";
			panel.AddNote (panelA);
			panelA.CreateParent (panel);
				
			panel.SaveLayout ();
			System.Collections.Generic.List<string> linkstome = MasterOfLayouts.ReciprocalLinks ("mynewpanel");
			Assert.AreEqual (0, linkstome.Count);
				
				
			NoteDataXML_LinkNote link = new NoteDataXML_LinkNote ();
				
			panelA.AddNote (link);
			link.CreateParent (panelA.GetPanelsLayout ());
			link.SetLink ("mynewpanel.thisguid1");
			panel.SaveLayout ();
				
				
			linkstome = MasterOfLayouts.ReciprocalLinks ("mynewpanel");

			Assert.AreEqual (true,MasterOfLayouts.ExistsByGUID("panela"),"1");
			Assert.AreEqual (true,MasterOfLayouts.IsSubpanel("panela"),"2");
			Assert.AreEqual (true,MasterOfLayouts.ExistsByGUID("mynewpanel"),"3");
			Assert.AreEqual (false,MasterOfLayouts.IsSubpanel("mynewpanel"),"4");
				

		}
		/// <summary>
		/// Tests to string. Just in case I have messed up the string format
		/// </summary>
		[Test]
		public void TestToString()
		{
			NoteDataXML note = new NoteDataXML();
			note.Caption = "boo";
			Assert.AreNotEqual(0, note.ToString().Length);
		}

		/// <summary>
		/// Iterates the big database. Runs through the DEBUG IMPORET of old data (140mb file)
		/// Only run this periodically. It worked the last time I used it (March 19 2013)
	
		/// </summary>
		[Test]
		[Ignore]
		public void IterateBigDatabase()
		{

			_TestSingleTon.Instance.SetupForAnyTest ();
			LayoutDetails.Instance.YOM_DATABASE =@"C:\Users\BrentK\Documents\Projects\Utilities\yom2013B\zUnitTestsForSolution\bin\Debug\yomBIGGY.s3db";
			LayoutDetails.Instance.OverridePath = Environment.CurrentDirectory;
			
			LayoutDetails.Instance.GetAppearanceFromStorage = _TestSingleTon.Instance.GetAppearanceFromStorage;

			
			
			LayoutDetails.Instance.AddToList(typeof(FAKE_NoteDataXML_Panel),"testingpanel");
			LayoutDetails.Instance.AddToList(typeof(FAKE_NoteDataXML_Text),"testingtext");
			
			FakeLayoutDatabase layout = new FakeLayoutDatabase("testguid");
			FAKE_SqlLiteDatabase db = new FAKE_SqlLiteDatabase(layout.GetDatabaseName ());

			LayoutDetails.Instance.SuppressWarnings = true; /// want image missing popups not to bug us

			LayoutDetails.Instance.AddToList(typeof(NoteDataXML_Picture.NoteDataXML_Pictures), "picture");
			LayoutDetails.Instance.AddToList(typeof(MefAddIns.NoteDataXML_SendIndex), "index");
			LayoutDetails.Instance.AddToList(typeof(MefAddIns.NoteDataXML_Submissions), "index");

			LayoutDetails.Instance.TransactionsList =new Transactions.TransactionsTable(MasterOfLayouts.GetDatabaseType(LayoutDetails.Instance.YOM_DATABASE));
		

			Form form = new Form();

			// system panel
//			LayoutPanel panel = new LayoutPanel("", false);
//			panel.LoadLayout("system", false, null);


			FAKE_LayoutPanel system = new FAKE_LayoutPanel("", false);
			form.Controls.Add(system);
			system.LoadLayout("system", false, null);

			LayoutDetails.Instance.SystemLayout = system;

			FAKE_LayoutPanel tablelay = new FAKE_LayoutPanel("", false);
			form.Controls.Add(tablelay);
			tablelay.LoadLayout ("tables", false, null);

			LayoutDetails.Instance.TableLayout = tablelay;
		//	YOM2013.DefaultLayouts.CreateASystemLayout(form,null);
		//	string ThisLayoutGUID = "mynewpanelXA";
			// create a layout
		//	_TestSingleTon.Instance._SetupForLayoutPanelTests (false, @"C:\Users\BrentK\Documents\Projects\Utilities\yom2013B\zUnitTestsForSolution\bin\Debug\yomBIGGY.s3db");
			//	_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			form.Controls.Add (panel);
			//NOTE: For now remember that htis ADDS 1 Extra notes
		//	panel.NewLayout (ThisLayoutGUID, true, null);




			// tmp: goto all notes
			System.Collections.Generic.List<MasterOfLayouts.NameAndGuid> ss = MasterOfLayouts.GetListOfLayouts ("");
			Assert.AreEqual (3651, ss.Count);
			//Console.WriteLine(ss.Count);
		//	NewMessage.Show (ss.Count.ToString ());
			int count = 0;
			foreach (MasterOfLayouts.NameAndGuid name in ss) {
				panel.Dispose ();
				panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
				form.Controls.Add (panel);
				count++;
				panel.LoadLayout(name.Guid,false, null);
			//	MDIHOST.DoCloseNote(false);
			}
			Assert.AreEqual (3651, count);
			form.Dispose ();
		}
		[Test]
		public void AnEmptyPanelStillHasAParent()
		{
			lg.Instance.OutputToConstoleToo = true;
			Form form = new Form();
			string ThisLayoutGUID = "mynewpanelXA";
			// create a layout
			_TestSingleTon.Instance._SetupForLayoutPanelTests ();
			//	_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			form.Controls.Add (panel);
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout (ThisLayoutGUID, true, null);
			
			NoteDataXML basicNote = new NoteDataXML ();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote (basicNote);
			//basicNote.CreateParent (panel);
			panel.SaveLayout ();
			
			
			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel ();
			panelA.Caption = "PanelA";
			panelA.GuidForNote = "panela";
			panel.AddNote (panelA);
			panelA.CreateParent (panel);
			
			
			FAKE_NoteDataXML_Panel panelB = new FAKE_NoteDataXML_Panel ();
			panelB.Caption = "PanelBBBBB";
			panelB.GuidForNote = "panelBB";
			panelA.AddNote (panelB);
			panelB.CreateParent (panelA.GetPanelsLayout());
		
			
			
			
			FAKE_NoteDataXML_Text textNote = new FAKE_NoteDataXML_Text ();
			
			panelA.AddNote (textNote);
			textNote.CreateParent (panelA.GetPanelsLayout ());
			textNote.Caption = "My text";
			textNote.GetRichTextBox ().Text = "Hello there." +Environment.NewLine +"I am still here, are you?" + Environment.NewLine+"Yep!";
			panel.SaveLayout ();
			Assert.AreEqual (6, panel.CountNotes ());
			_w.output(panelB.GetPanelsLayout().ParentGUID);

			// get parent GUID directlyf rom database
			FAKE_LayoutPanel SubPanelB = new FAKE_LayoutPanel ("panelBB", true);
			form.Controls.Add (SubPanelB);
			SubPanelB.LoadLayout("panelBB", true, null);

			Assert.AreNotEqual(Constants.BLANK, SubPanelB.GetLayoutDatabase().ParentGuid);
		}

		[Test]
		public void ExportImport ()
		{	lg.Instance.OutputToConstoleToo = true;
			Form form = new Form();
			string ThisLayoutGUID = "mynewpanelXA";
			// create a layout
			_TestSingleTon.Instance._SetupForLayoutPanelTests ();
			//	_SetupForLayoutPanelTests ();
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			form.Controls.Add (panel);
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout (ThisLayoutGUID, true, null);
			
			NoteDataXML basicNote = new NoteDataXML ();
			basicNote.GuidForNote = "thisguid1";
			basicNote.Caption = "note1";
			
			panel.AddNote (basicNote);
			//basicNote.CreateParent (panel);
			panel.SaveLayout ();

			
			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel ();
			panelA.Caption = "PanelA";
			panelA.GuidForNote = "panela";
			panel.AddNote (panelA);
			panelA.CreateParent (panel);


			FAKE_NoteDataXML_Panel panelB = new FAKE_NoteDataXML_Panel ();
			panelB.Caption = "PanelBBBBB2";
			panelB.GuidForNote = "panelBB";


			panelA.AddNote (panelB);
			panelB.CreateParent (panelA.GetPanelsLayout());
			FAKE_NoteDataXML_Text textNoteA = new FAKE_NoteDataXML_Text ();

			panelB.AddNote (textNoteA);
			textNoteA.GuidForNote = "My Text Note For the B Panel";
			textNoteA.CreateParent (panelB.GetPanelsLayout ());
			textNoteA.Caption = "My text B";



			FAKE_NoteDataXML_Text textNote = new FAKE_NoteDataXML_Text ();

			panelA.AddNote (textNote);
			textNote.GuidForNote = "Text Note For Panel A";
			textNote.CreateParent (panelA.GetPanelsLayout ());
			textNote.Caption = "My text A";
			textNote.GetRichTextBox ().Text = "Hello there." +Environment.NewLine +"I am still here, are you?" + Environment.NewLine+"Yep!";
			panel.SaveLayout ();
			Assert.AreEqual (7, panel.CountNotes (), "Count1");

			// Note count: Default Note + BasicNote+ PanelA + LinkTable + MyText  + PanbelB + My Text B  =7


			//test existence
			Assert.True (MasterOfLayouts.ExistsByGUID (ThisLayoutGUID));
			// counting our subpanel we have 2
			Assert.AreEqual (3, MasterOfLayouts.Count (true));
			// NOT counting our subpanel we have 1
			Assert.AreEqual (1, MasterOfLayouts.Count (false));

			// export
			string file = Path.Combine (Environment.CurrentDirectory, "exportest.txt");
			if (File.Exists (file)) {
				File.Delete (file);
			}
			string subfile = file+"__child0.txt";
			if (File.Exists (subfile)) {
				File.Delete (subfile);
			}
			string subfile2 = file+"__child1.txt";
			if (File.Exists (subfile2)) {
				File.Delete (subfile2);
			}
			
			Assert.False (File.Exists (file), file + " does not exist");

			MasterOfLayouts.ExportLayout(ThisLayoutGUID, file);
			// test exportfile existence 	// count 2 files exported
			Assert.True (File.Exists(file),"main file exists");
			Assert.True (File.Exists(subfile),"subfile exists");
			Assert.True (File.Exists(subfile2),"subfile2 exists");
		
		


			// delete original note
			MasterOfLayouts.DeleteLayout(ThisLayoutGUID);


			// test existence
			Assert.False (MasterOfLayouts.ExistsByGUID (ThisLayoutGUID));
			// counting our subpanel we have ZERO
			Assert.AreEqual (0, MasterOfLayouts.Count (true) ,"Nothing should be left");
			// Import as New (but won't be duplicate because old is gone)

			int errorcode = MasterOfLayouts.ImportLayout(file);
			Console.WriteLine(errorcode);
			Assert.True (MasterOfLayouts.ExistsByGUID (ThisLayoutGUID));
			// test existence
			 
			// confirm all notes laoded into layout
			panel = null;
			panel = new FAKE_LayoutPanel(ThisLayoutGUID, false);
			form.Controls.Add (panel);
			panel.LoadLayout (ThisLayoutGUID,false, null);
		//	panel.SaveLayout();

			Assert.AreEqual (7, panel.CountNotes (), "Count2");
			Assert.AreEqual (1, MasterOfLayouts.Count (false));
			Assert.AreEqual (3, MasterOfLayouts.Count (true));
	


			// Import as Overwrite
			if (File.Exists (file)) {
				File.Delete (file);
			}
			subfile = file+"__child0.txt";
			if (File.Exists (subfile)) {
				File.Delete (subfile);
			}
			Assert.True (MasterOfLayouts.ExistsByGUID (ThisLayoutGUID));
			MasterOfLayouts.ExportLayout(ThisLayoutGUID, file);

			panel = null;
			errorcode = MasterOfLayouts.ImportLayout(file);
			Assert.AreEqual (0, errorcode);
			// test existences
			panel = new FAKE_LayoutPanel(ThisLayoutGUID, false);
			form.Controls.Add (panel);
			panel.LoadLayout (ThisLayoutGUID,false, null);
			
			Assert.AreEqual (7, panel.CountNotes ());
			Assert.AreEqual (1, MasterOfLayouts.Count (false));
			Assert.AreEqual (3, MasterOfLayouts.Count (true));


			lg.Instance.OutputToConstoleToo = false;
		}
		[Test]
		public void TableSearch()
		{
			// the way filters work for tables is weird
			// and becaue it is weird I might tweak it later and this is a bad idea
			// so I'm writing this test to stop me from doing that

			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			
			
			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			
			
			
		
			
		
			
			// needed else DataGrid does not initialize
			
			form.Show ();
			//form.Visible = false;
			_w.output("boom");
			// March 2013 -- notelist relies on having this
			YOM2013.DefaultLayouts.CreateASystemLayout(form,null);
			
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
		

			string panelname2 = System.Guid.NewGuid().ToString();
			FAKE_LayoutPanel  PanelOtherGuy= new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			PanelOtherGuy.NewLayout (panelname2,true, null);
			form.Controls.Add (PanelOtherGuy);
			PanelOtherGuy.SaveLayout();
			
			Assert.AreEqual( 2, PanelOtherGuy.CountNotes(), "count1");	
			
			// ADD 1 of each type
			//foreach (Type t in LayoutDetails.Instance.ListOfTypesToStoreInXML())

					
			NoteDataXML_Table table =  new NoteDataXML_Table(100, 100,new appframe.ColumnDetails[3]{new appframe.ColumnDetails("id",100), 
				new appframe.ColumnDetails("tables",100),
				new appframe.ColumnDetails("values",220)} );
			table.GuidForNote="thetable";
			table.Caption="thettable2";




			PanelOtherGuy.AddNote (table);
			table.CreateParent(PanelOtherGuy);
					
		//	table.UpdateAfterLoad();
					
			table.AddRow(new object[3] {"0", "table1", "value1"});
			table.AddRow(new object[3] {"0", "table2", "value2"});
			table.AddRow(new object[3] {"0", "table3", "value3"});
			table.AddRow(new object[3] {"0", "table4", "value4"});

		
			PanelOtherGuy.SaveLayout();
			Assert.AreEqual( 3, PanelOtherGuy.CountNotes(), "count2");	
			string ToSearchFor = "table3";

			// looks in row 1 for the value and will return the value from row 2
			List<string> results  = PanelOtherGuy.GetListOfStringsFromSystemTable("thettable2", 2, String.Format ("1|{0}", ToSearchFor));
			Assert.NotNull(results);
			Assert.AreEqual (1, results.Count);
			Assert.AreEqual ("value3", results[0]);

			ToSearchFor ="bacon";
			results  = PanelOtherGuy.GetListOfStringsFromSystemTable("thettable2", 2, String.Format ("1|{0}", ToSearchFor));
			Assert.NotNull(results);
			Assert.AreEqual (0, results.Count);

			ToSearchFor ="0";
			results  = PanelOtherGuy.GetListOfStringsFromSystemTable("thettable2", 1, String.Format ("0|{0}", ToSearchFor));
			Assert.NotNull(results);
			Assert.AreEqual (4, results.Count);




		}
		[Test]
		public void GetListOfLayoutTests()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			form.Show ();
			// March 2013 -- notelist relies on having this
			YOM2013.DefaultLayouts.CreateASystemLayout(form,null);
			
			string panelname2 = System.Guid.NewGuid().ToString();
			FAKE_LayoutPanel  PanelOtherGuy= new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			PanelOtherGuy.NewLayout (panelname2,true, null);
			form.Controls.Add (PanelOtherGuy);
			PanelOtherGuy.SaveLayout();
			
			Assert.AreEqual( 2, PanelOtherGuy.CountNotes(), "count1");	

			List<MasterOfLayouts.NameAndGuid> names =  MasterOfLayouts.GetListOfLayouts("WritingProjects");

			Assert.AreEqual (0, names.Count);

			// 1 prtoject

			PanelOtherGuy= new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			PanelOtherGuy.NewLayout ("NextLayout",true, null);
			PanelOtherGuy.SetCaption("booler");
			PanelOtherGuy.SetNotebookSection("Writing","Projects");
			form.Controls.Add (PanelOtherGuy);
			PanelOtherGuy.SaveLayout();

			names =  MasterOfLayouts.GetListOfLayouts("WritingProjects");
			
			Assert.AreEqual (1, names.Count);

			// search LIKE NAME

			PanelOtherGuy= new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			PanelOtherGuy.NewLayout ("NextLayout22",true, null);
			PanelOtherGuy.SetCaption("boolAt");

			LayoutDetails.Instance.AddToList (typeof(FAKE_NoteDataXML_Text), "textfake");

			FAKE_NoteDataXML_Text richy = new FAKE_NoteDataXML_Text();
		

			PanelOtherGuy.AddNote(richy);
			richy.CreateParent(PanelOtherGuy);
			richy.GetRichTextBox().Text="Hello there";

			PanelOtherGuy.SetNotebookSection("Writing","Projects");
			form.Controls.Add (PanelOtherGuy);
			PanelOtherGuy.SaveLayout();

			PanelOtherGuy= new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			PanelOtherGuy.NewLayout ("NextLayout33",true, null);
			PanelOtherGuy.SetCaption("bolzzz");
			PanelOtherGuy.SetNotebookSection("Writing","Projects");
			form.Controls.Add (PanelOtherGuy);
			PanelOtherGuy.SaveLayout();


			names =  MasterOfLayouts.GetListOfLayouts("WritingProjects");
			
			Assert.AreEqual (3, names.Count);

			names =  MasterOfLayouts.GetListOfLayouts("WritingProjects","bool",false, null);
			
			Assert.AreEqual (2, names.Count);


			//
			// text searching
			//

			
			PanelOtherGuy= new FAKE_LayoutPanel (CoreUtilities.Constants.BLANK, false);
			PanelOtherGuy.NewLayout ("NextLayout55",true, null);
			PanelOtherGuy.SetCaption("bolzzz222");
			PanelOtherGuy.SetNotebookSection("Writing","Projects");
			form.Controls.Add (PanelOtherGuy);
			PanelOtherGuy.SaveLayout();


			richy = new FAKE_NoteDataXML_Text();
		
			PanelOtherGuy.AddNote(richy);
			richy.CreateParent(PanelOtherGuy);
			richy.GetRichTextBox().Text="Hello there again!";
			PanelOtherGuy.SaveLayout();
			richy = new FAKE_NoteDataXML_Text();
		
			
			PanelOtherGuy.AddNote(richy);
			richy.CreateParent(PanelOtherGuy);
			richy.GetRichTextBox().Text="Hello the fish are good there";
			PanelOtherGuy.SaveLayout();


			names =  MasterOfLayouts.GetListOfLayouts("All", "fish", true, null);
			Assert.AreEqual (1, names.Count);
			// FINAL TEST now count all


			names =  MasterOfLayouts.GetListOfLayouts("All");
			Assert.AreEqual (6, names.Count);


		}
	}
}

