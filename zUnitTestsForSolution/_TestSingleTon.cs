using System;
using Layout;
using System.Collections.Generic;
namespace Testing
{
	public class _TestSingleTon
	{
		#region variables

		public List<string> ListOfTablesToDrop = new List<string>();

		protected static volatile _TestSingleTon instance;
		protected static object syncRoot = new Object();
		public static string PathToSendAwayEPUBTemplates = @"C:\Users\BrentK\Documents\Keeper\SendTextAwayControlFiles\epubfiles";
		public static string BlankFileTest2 = @"C:\Users\BrentK\Documents\YourOtherMind\YourOtherMind\Files\brokenfiles2012\notbroken.txt";
		public static string BlankFileTest1 = @"C:\Users\BrentK\Documents\YourOtherMind\YourOtherMind\Files\brokenfiles2012\brokenrtf_dec2012.rtf";
		public static string Zipper = @"C:\Program Files\7-Zip\";
        #endregion;
		public _TestSingleTon ()
		{
		}

		System.Windows.Forms.Form form = null;
		public System.Windows.Forms.Form FORM {
			get {
				if (null == form)
				{
					form = new System.Windows.Forms.Form();
				}
				return form;
			}
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
							instance.ListOfTablesToDrop.Add (Layout.data.dbConstants.table_name);
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
		public void SetupForAnyTest ()
		{
			CoreUtilities.lg.Instance.OnlyTime= false;
			CoreUtilities.lg.Instance.Loudness = CoreUtilities.Loud.CTRIVIAL;
		}
		public Layout.AppearanceClass GetAppearanceFromStorage (string Key)
		{
			Layout.AppearanceClass fake = AppearanceClass.SetAsClassic();
			//fake.SetAsClassic();
			return fake;
		}
		public void _SetupForLayoutPanelTests ()
		{
			_SetupForLayoutPanelTests(false,"");
		}
		public void _SetupForLayoutPanelTests (bool BreakAppearanceOnPurpose)
		{
			_SetupForLayoutPanelTests(BreakAppearanceOnPurpose,"");
		}
		public void _SetupForLayoutPanelTests (bool BreakAppearanceOnPurpose, string OverrideDatabaseName)
		{
			LayoutDetails.Instance.UpdateAfterLoadList.Clear ();
			SetupForAnyTest ();
			LayoutDetails.Instance.YOM_DATABASE = TESTDATABASE;
			if ("" != OverrideDatabaseName) {
				LayoutDetails.Instance.YOM_DATABASE = OverrideDatabaseName;
			}
			LayoutDetails.Instance.OverridePath = Environment.CurrentDirectory;
			_w.output (LayoutDetails.Instance.YOM_DATABASE);
			if (false == BreakAppearanceOnPurpose)
				LayoutDetails.Instance.GetAppearanceFromStorage = GetAppearanceFromStorage;
			else
				LayoutDetails.Instance.GetAppearanceFromStorage = null;


			LayoutDetails.Instance.AddToList (typeof(FAKE_NoteDataXML_Panel), "testingpanel");
			LayoutDetails.Instance.AddToList (typeof(FAKE_NoteDataXML_Text), "testingtext");

			FakeLayoutDatabase layout = new FakeLayoutDatabase ("testguid");
			FAKE_SqlLiteDatabase db = new FAKE_SqlLiteDatabase (layout.GetDatabaseName ());

			LayoutDetails.Instance.TransactionsList = new Transactions.TransactionsTable (MasterOfLayouts.GetDatabaseType (LayoutDetails.Instance.YOM_DATABASE));
			foreach (string s in ListOfTablesToDrop) {
				db.DropTableIfExists (s);
				//	db.DropTableIfExists("system");
				_w.output ("dropping table " + s);
			}
		}
	}
}

