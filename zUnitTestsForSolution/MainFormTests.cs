using System;
using NUnit.Framework;
using YOM2013;
using LayoutPanels;
using Layout;

namespace Testing
{
	public class MainFormTests
	{
		public MainFormTests ()
		{
		}
		[Test]
		public void CreateExampleAndSystemLayouts()
		{
			_TestSingleTon.Instance._SetupForLayoutPanelTests();

			System.Windows.Forms .Form form = new System.Windows.Forms.Form();
			form.Show();
			//form.Visible=false; NOPE Form has to be visible now!!!

			DefaultLayouts.CreateExampleLayout(form,null);
			DefaultLayouts.CreateASystemLayout(form,null);
			FAKE_SqlLiteDatabase db = new FAKE_SqlLiteDatabase(LayoutDetails.Instance.YOM_DATABASE);
			NoteDataXML_Table table =  (NoteDataXML_Table) LayoutDetails.Instance.TableLayout.FindNoteByGuid("systemqueries");
			Assert.NotNull(table);
			Assert.AreEqual ("name", table.Columns[1].ColumnName);


			string result = db.BackupDatabase();
			Assert.Greater(result.Length,29000);
			//Assert.AreEqual(29971, result.Length);
			db.Dispose();
			// creates the example and system layouts
			// to catch if any popups or other oddities introduced
			//Assert.True (false);

		}
	}
}

