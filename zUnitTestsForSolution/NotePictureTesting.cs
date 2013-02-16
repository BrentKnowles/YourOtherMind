using System;
using NUnit.Framework;
using Layout;
using NoteDataXML_Picture;
using System.IO;

namespace Testing
{
	[TestFixture]
	public class NotePictureTesting
	{
		public NotePictureTesting ()
		{
		}

		[Test]
		public void TestLinkTables()
		{

			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			LayoutDetails.Instance.AddToList(typeof(FAKE_NoteDataPicture),"pictures");



			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			form.Controls.Add (panel);
			form.Show ();
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);
			NoteDataXML basicNote = new NoteDataXML();
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			//basicNote.CreateParent(panel);
			
			
			
			
			//panel.MoveNote(

			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel();
			panelA.Caption = "PanelA";
			
			FAKE_NoteDataXML_Panel panelB = new FAKE_NoteDataXML_Panel();
			panelB.Caption = "PanelB";


			panel.AddNote(panelA);
			panelA.CreateParent(panel);

			// b is inside A
			panelA.AddNote(panelB);
			panelB.CreateParent(panelA.GetPanelsLayout());


			FAKE_NoteDataPicture picture = new FAKE_NoteDataPicture();

			panelB.AddNote(picture);
			picture.CreateParent(panelB.GetPanelsLayout());
			// add a few more pictures
			picture = new FAKE_NoteDataPicture();
			panelB.AddNote(picture);
			picture.CreateParent(panelB.GetPanelsLayout());


			picture = new FAKE_NoteDataPicture();
			panelB.AddNote(picture);
			picture.CreateParent(panelB.GetPanelsLayout());

		


			panel.SaveLayout();
			// basic note + LINKTABLE + Default Note + Panel A + Panel B + Picture1 + Picture2 + Picture3= 8
			Assert.AreEqual(8, panel.CountNotes());
			// create several pictures, including in a subpanel, and then count linktables=1
			CoreUtilities.Links.LinkTable panelLinkTable = panelB.GetPanelsLayout().GetLinkTable ();
			CoreUtilities.Links.LinkTable MainLinkTable = panel.GetLinkTable();

			Assert.AreEqual(panelLinkTable, MainLinkTable);

			CoreUtilities.Links.LinkTable dodgeball = new CoreUtilities.Links.LinkTable();
			Assert.AreNotEqual(MainLinkTable, dodgeball);
			//Assert.AreNotEqual(MainLinkTable, panelLinkTable);


			// moves one of the pictures out of the Folder B and into FolderA
			Assert.AreEqual (4, panelA.CountNotes());
			panelB.myLayoutPanel().MoveNote(picture.GuidForNote, "up");
			Assert.AreEqual (4, panelA.CountNotes()); // count should stay the same
			panelA.myLayoutPanel().MoveNote(picture.GuidForNote, "up");
			Assert.AreEqual (3, panelA.CountNotes()); // now that the note is out the count should go down
			Assert.AreEqual(8, panel.CountNotes());
		}
		[Test]
		public void FindFileSimple()
		{
			//TestingSingleton.GetImageFile
			// Tests to see if it can find the file
			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			LayoutDetails.Instance.AddToList(typeof(FAKE_NoteDataPicture),"pictures");
			
			
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			form.Controls.Add (panel);
			form.Show ();
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);
			NoteDataXML basicNote = new NoteDataXML();
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			//basicNote.CreateParent(panel);
			
			
			
			
			//panel.MoveNote(
			
			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel();
			panelA.Caption = "PanelA";
			
			FAKE_NoteDataXML_Panel panelB = new FAKE_NoteDataXML_Panel();
			panelB.Caption = "PanelB";
			
			
			panel.AddNote(panelA);
			panelA.CreateParent(panel);
			
			// b is inside A
			panelA.AddNote(panelB);
			panelB.CreateParent(panelA.GetPanelsLayout());
			
			
			FAKE_NoteDataPicture picture = new FAKE_NoteDataPicture();
			
			panelB.AddNote(picture);
			picture.CreateParent(panelB.GetPanelsLayout());
			Assert.IsNull (picture.GetPictureBox().Image);

			Assert.True (System.IO.File.Exists(_TestSingleTon.ValidImageFile));
			picture._SetImage(_TestSingleTon.ValidImageFile);
			Assert.IsNotNull (picture.GetPictureBox().Image);


		}
		[Test]
		public void FindFile_Missing1()
		{
			//TestingSingleton.GetImageFile
			// Tests to see if it can find the file, in CURRENT directory (not at location specifeid)

			//TestingSingleton.GetImageFile
			// Tests to see if it can find the file
			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			LayoutDetails.Instance.AddToList(typeof(FAKE_NoteDataPicture),"pictures");
			
			
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			form.Controls.Add (panel);
			form.Show ();
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);
			NoteDataXML basicNote = new NoteDataXML();
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			//basicNote.CreateParent(panel);
			
			
			
			
			//panel.MoveNote(
			
			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel();
			panelA.Caption = "PanelA";
			
			FAKE_NoteDataXML_Panel panelB = new FAKE_NoteDataXML_Panel();
			panelB.Caption = "PanelB";
			
			
			panel.AddNote(panelA);
			panelA.CreateParent(panel);
			
			// b is inside A
			panelA.AddNote(panelB);
			panelB.CreateParent(panelA.GetPanelsLayout());
			
			
			FAKE_NoteDataPicture picture = new FAKE_NoteDataPicture();
			
			panelB.AddNote(picture);
			picture.CreateParent(panelB.GetPanelsLayout());
			Assert.IsNull (picture.GetPictureBox().Image);

			string NewLocation = Path.Combine (Environment.CurrentDirectory, _TestSingleTon.InvalidImageFile);
			_w.output("here");
			File.Delete (NewLocation);
			Assert.False (System.IO.File.Exists(_TestSingleTon.InvalidImageFile), "File ALready Exists!");
			// copy it into local directory

			File.Copy (_TestSingleTon.ValidImageFile, NewLocation);

			picture._SetImage(_TestSingleTon.InvalidImageFile);
			Assert.IsNotNull (picture.GetPictureBox().Image);
			picture.Dispose();
			//File.Delete (NewLocation);
		}
		[Test]
		public void FindFile_MissingAdvanced()
		{
			//TestingSingleton.GetImageFile
			// Tests to see if it can find the file, in CURRENT directory (not at location specifeid and not at root level -- Searches subdirectores)
			//TestingSingleton.GetImageFile
			// Tests to see if it can find the file, in CURRENT directory (not at location specifeid)
			
			//TestingSingleton.GetImageFile
			// Tests to see if it can find the file
			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			_TestSingleTon.Instance._SetupForLayoutPanelTests();
			LayoutDetails.Instance.AddToList(typeof(FAKE_NoteDataPicture),"pictures");
			
			
			
			FAKE_LayoutPanel panel = new FAKE_LayoutPanel(CoreUtilities.Constants.BLANK, false);
			form.Controls.Add (panel);
			form.Show ();
			
			//NOTE: For now remember that htis ADDS 1 Extra notes
			panel.NewLayout("mynewpanel", true, null);
			NoteDataXML basicNote = new NoteDataXML();
			basicNote.Caption = "note1";
			
			panel.AddNote(basicNote);
			//basicNote.CreateParent(panel);
			
			
			
			
			//panel.MoveNote(
			
			FAKE_NoteDataXML_Panel panelA = new FAKE_NoteDataXML_Panel();
			panelA.Caption = "PanelA";
			
			FAKE_NoteDataXML_Panel panelB = new FAKE_NoteDataXML_Panel();
			panelB.Caption = "PanelB";
			
			
			panel.AddNote(panelA);
			panelA.CreateParent(panel);
			
			// b is inside A
			panelA.AddNote(panelB);
			panelB.CreateParent(panelA.GetPanelsLayout());
			
			
			FAKE_NoteDataPicture picture = new FAKE_NoteDataPicture();
			
			panelB.AddNote(picture);
			picture.CreateParent(panelB.GetPanelsLayout());
			Assert.IsNull (picture.GetPictureBox().Image);




			string NewDirectory = Path.Combine (Environment.CurrentDirectory, @"temppicdirectory");
			//CoreUtilities.NewMessage.Show("trying to create: "+ NewDirectory);
			Directory.CreateDirectory(NewDirectory);
			//string NewLocation = Path.Combine (NewDirectory, _TestSingleTon.InvalidImageFile);
			
		

			string NewLocation = Path.Combine (NewDirectory, _TestSingleTon.InvalidImageFile);
			_w.output("here");

			File.Delete (NewLocation);

			//File.Copy (_TestSingleTon.ValidImageFile, NewLocation, true);

			File.Delete (_TestSingleTon.InvalidImageFile); // in case leftover froma nother test
		


			Assert.False (File.Exists(_TestSingleTon.InvalidImageFile), "File ALready Exists!");
			// copy it into local directory
			_w.output("here2");

			File.Copy (_TestSingleTon.ValidImageFile, NewLocation,true);
			
			picture._SetImage(_TestSingleTon.InvalidImageFile);
			// THis should work, it works LIVE??
			Assert.IsNotNull (picture.GetPictureBox().Image);
			picture.Dispose();
		}
		[Test]
		public void TestCreatingCaptureFolderIfNeeded()
		{
			// using fcapture direct (need a fake class) protected string CaptureToFile()

			string imagedir = Path.Combine(new string[3]{Environment.CurrentDirectory, "files","captures"});
			Directory.Delete (imagedir);
			Assert.False (Directory.Exists (imagedir),"directory not there at start");
			fCapture capture = new fCapture(imagedir, "capture", true, new System.Windows.Forms.Form().Icon, CoreUtilities.FormUtils.FontSize.Normal);
			Assert.True (Directory.Exists (imagedir), "directory now there");
			// now remove the directory
		}

	}
}

