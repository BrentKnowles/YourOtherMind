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
			db.CreateTableIfDoesNotExist (dbConstants.table_name, 
		                              dbConstants.Columns, 
		                              dbConstants.Types, String.Format ("{0}", dbConstants.ID)
			);
			return db;
		}



		public void Dispose()
		{
		}
		/// <summary>
		/// Exists the specified LayoutName.
		/// </summary>
		/// <param name='LayoutName'>
		/// Layout name.
		/// </param>
		public static bool ExistsByName(string LayoutName)
		{

			BaseDatabase MyDatabase = CreateDatabase ();
			bool result =  MyDatabase.Exists (dbConstants.table_name, dbConstants.NAME, LayoutName);
			MyDatabase.Dispose();
			return result;
		}

		/// <summary>
		/// Gets the name of the GUID from.
		/// </summary>
		/// <returns>
		/// The GUID from name.
		/// </returns>
		/// <param name='name'>
		/// Name.
		/// </param>
		public static string GetGuidFromName (string name)
		{
			BaseDatabase MyDatabase = CreateDatabase ();
			string guid=Constants.BLANK;
			List<object[]> result = MyDatabase.GetValues(dbConstants.table_name, new string[1] {dbConstants.GUID}, dbConstants.NAME, name);
			if (result != null && result.Count > 0)
			{
				if (result[0][0] != null)
				{
					guid = (result[0][0]).ToString();
				}
			}
			MyDatabase.Dispose();
			return guid;
		}
		public static bool ExistsByGUID(string GUID)
		{
			
			BaseDatabase MyDatabase = CreateDatabase ();
			bool result =  MyDatabase.Exists (dbConstants.table_name, dbConstants.GUID, GUID);
			MyDatabase.Dispose();
			return result;
		}
		public string Backup ()
		{
			SqlLiteDatabase db = new SqlLiteDatabase (LayoutDetails.Instance.YOM_DATABASE);
			string result = db.BackupDatabase();
			db.Dispose();
			return result;
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
			
			List<object[]> myList = MyDatabase.GetValues (dbConstants.table_name, new string[2] {dbConstants.GUID, dbConstants.NAME},
			dbConstants.SUBPANEL , 0,String.Format(" order by {0} COLLATE NOCASE", dbConstants.NAME));
			
			
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

