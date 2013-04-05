// _TestSingleTon.cs
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

