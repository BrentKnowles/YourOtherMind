using System;
using Layout;

namespace Testing
{
	public class FakeLayoutDatabase: LayoutDatabase
	{
		protected override string YOM_DATABASE {
			get { return "TEST_YOM_DATABASE.s3db";}
		}

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
			return YOM_DATABASE;
		}

		// we override this to prevent actually wrecking the real database.
		//string YOM_DATABASE = "YOM_TESTING_DATABASE";
		public FakeLayoutDatabase (string GUID) : base(GUID)
		{


		}
	}
}

