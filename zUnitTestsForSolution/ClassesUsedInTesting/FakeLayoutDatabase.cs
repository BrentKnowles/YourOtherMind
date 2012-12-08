using System;
using Layout;

namespace Testing
{
	public class FakeLayoutDatabase: LayoutDatabase
	{
		protected override string YOM_DATABASE {
			get { return "TEST_YOM_DATABASE.s3db";}
		}
		// we override this to prevent actually wrecking the real database.
		//string YOM_DATABASE = "YOM_TESTING_DATABASE";
		public FakeLayoutDatabase (string GUID) : base(GUID)
		{


		}
	}
}

