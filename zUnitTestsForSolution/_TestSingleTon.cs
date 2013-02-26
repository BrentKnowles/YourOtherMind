using System;
using Layout;

namespace Testing
{
	public class _TestSingleTon
	{
		#region variables
		protected static volatile _TestSingleTon instance;
		protected static object syncRoot = new Object();
		

        #endregion;
		public _TestSingleTon ()
		{
		}
		public static _TestSingleTon Instance
		{
			get
			{
				if (null == instance)
				{
					// only one instance is created and when needed
					lock (syncRoot)
					{
						if (null == instance)
						{
							instance = new _TestSingleTon();
						}
					}
				}
				return (_TestSingleTon)instance;
			}
		}
		public const string ValidImageFile=@"C:\Users\Public\Pictures\PhotoStage\012.jpg";
		// the test will verify this file does not exist but then copy it in (and delete it afterwards) to the local directory
		// a second test will move it into a subdirectory
		public const string InvalidImageFile="012.jpg";
		public const string TESTDATABASE = "yom_test_database.s3db";
		private void SetupForAnyTest ()
		{
			CoreUtilities.lg.Instance.OnlyTime= false;
			CoreUtilities.lg.Instance.Loudness = CoreUtilities.Loud.CTRIVIAL;
		}
		Layout.AppearanceClass GetAppearanceFromStorage (string Key)
		{
			Layout.AppearanceClass fake = AppearanceClass.SetAsClassic();
			//fake.SetAsClassic();
			return fake;
		}
		public void _SetupForLayoutPanelTests ()
		{
			_SetupForLayoutPanelTests(false);
		}
		public void _SetupForLayoutPanelTests (bool BreakAppearanceOnPurpose)
		{
			SetupForAnyTest();
			LayoutDetails.Instance.YOM_DATABASE = TESTDATABASE;
			LayoutDetails.Instance.OverridePath = Environment.CurrentDirectory;

			if (false == BreakAppearanceOnPurpose) LayoutDetails.Instance.GetAppearanceFromStorage = GetAppearanceFromStorage;
				else LayoutDetails.Instance.GetAppearanceFromStorage = null;


			LayoutDetails.Instance.AddToList(typeof(FAKE_NoteDataXML_Panel),"testingpanel");
			LayoutDetails.Instance.AddToList(typeof(FAKE_NoteDataXML_Text),"testingtext");

			FakeLayoutDatabase layout = new FakeLayoutDatabase("testguid");
			FAKE_SqlLiteDatabase db = new FAKE_SqlLiteDatabase(layout.GetDatabaseName ());

			LayoutDetails.Instance.TransactionsList =new Transactions.TransactionsTable(MasterOfLayouts.GetDatabaseType(LayoutDetails.Instance.YOM_DATABASE));
			db.DropTableIfExists(Layout.data.dbConstants.table_name);
			_w.output ("dropping table " + Layout.data.dbConstants.table_name);
		}
	}
}

