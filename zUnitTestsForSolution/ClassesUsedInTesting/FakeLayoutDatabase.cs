using System;
using Layout;

namespace Testing
{
	public class FakeLayoutDatabase: LayoutDatabase
	{


		/// <summary>
		/// debug tracking to look for save failures.
		/// </summary>
		/// <returns>
		/// The loaded.
		/// </returns>
		public int ObjectsSaved ()
		{
			return debug_ObjectCount;
		}


		public string GetDatabaseName ()
		{
			return LayoutDetails.Instance.YOM_DATABASE;
		}

		// we override this to prevent actually wrecking the real database.
		//string YOM_DATABASE = "YOM_TESTING_DATABASE";
		public FakeLayoutDatabase (string GUID) : base(GUID)
		{


		}
	}
}

