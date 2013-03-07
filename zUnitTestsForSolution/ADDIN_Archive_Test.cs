using System;
using NUnit.Framework;
using Layout;
using System.IO;
using CoreUtilities;
using System.Windows.Forms;
namespace Testing
{
	[TestFixture]
	public class ADDIN_Archive_Test
	{
		public ADDIN_Archive_Test ()
		{
		}
		[Test]
		public void ArchiveAllAndCount()
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
			
			LayoutDetails.Instance.CurrentLayout = panel;
			MefAddIns.Addin_Archive Archiver = new MefAddIns.Addin_Archive();

			// delete existing files
			string path = Archiver.BuildArchiveDepotPath();
			string[] files = Directory.GetFiles(path);
			foreach (string s in files)
			{
			File.Delete (s);
			}
			files = Directory.GetFiles(path);

			Assert.AreEqual (0, files.Length);


			Archiver.ArchiveAll(panel, "automatedtesting");
			files = Directory.GetFiles(path);
			Assert.AreEqual (3, files.Length);
		}
	}
}

