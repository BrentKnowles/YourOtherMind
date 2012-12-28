using System;
using database;
using System.Collections.Generic;
using Layout.data;
using CoreUtilities;

namespace Layout
{
	/// <summary>
	/// List of layouts try1.  (This is effectively ALL THE PAGES
	/// </summary>
	public class MasterOfLayouts : IDisposable
	{
		public MasterOfLayouts ()
		{
		}
		#region structs
		public struct NameAndGuid
		{
			private string _guid;
			private string _caption;
			public string Guid {get {return _guid;}  set {_guid = value;}}
			public string Caption {get {return _caption;}  set {_caption = value;}}
		}
		#endregion
		/// <summary>
		/// Creates the database. (MOVED from LayoutDatabase());
		/// </summary>
		public static BaseDatabase CreateDatabase ()
		{
			SqlLiteDatabase db = new SqlLiteDatabase (LayoutDetails.Instance.YOM_DATABASE);
			db.CreateTableIfDoesNotExist (tmpDatabaseConstants.table_name, 
		                              tmpDatabaseConstants.Columns, 
		                              tmpDatabaseConstants.Types, String.Format ("{0}", tmpDatabaseConstants.ID)
			);
			return db;
		}



		public void Dispose()
		{
		}
		public string Backup ()
		{
			SqlLiteDatabase db = new SqlLiteDatabase (LayoutDetails.Instance.YOM_DATABASE);
			return db.BackupDatabase();
		}

		/// <summary>
		/// Gets the list of layouts.
		/// </summary>
		/// <returns>
		/// The list of layouts.
		/// </returns>
		/// <param name='filter'>
		/// Filter.
		/// </param>
			/// 
		public List<NameAndGuid> GetListOfLayouts (string filter)
		{
			
			BaseDatabase MyDatabase = CreateDatabase ();
			List<NameAndGuid>result = new List<NameAndGuid>();
			if (MyDatabase == null) {
				throw new Exception ("Unable to create database in LoadFrom");
			}
			
			List<object[]> myList = MyDatabase.GetValues (tmpDatabaseConstants.table_name, new string[2] {tmpDatabaseConstants.GUID, tmpDatabaseConstants.NAME},
			tmpDatabaseConstants.SUBPANEL , 0,String.Format(" order by {0} COLLATE NOCASE", tmpDatabaseConstants.NAME));
			
			
			if (myList != null && myList.Count > 0) {
				
				foreach (object[] o in myList)
				{
					NameAndGuid record = new NameAndGuid();
					record.Guid = o[0].ToString();
					record.Caption = o [1].ToString();
					lg.Instance.Line("MasterOfLayouts->GetListOfLayouts", ProblemType.MESSAGE, "adding to ListOfLayouts " + record.Caption);
					result.Add (record);
				}
			}
			MyDatabase.Dispose();
			return result;
		}
	}
}

