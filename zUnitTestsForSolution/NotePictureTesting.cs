using System;
using NUnit.Framework;

namespace Testing
{
	[TestFixture]
	public class NotePictureTesting
	{
		public NotePictureTesting ()
		{
		}

		[Test]
		public void TestLinkTables()
		{
			// create several pictures, including in a subpanel, and then count linktables=1
			Assert.True (false);
		}

		public void FindFileSimple()
		{
			//TestingSingleton.GetImageFile
			// Tests to see if it can find the file
		}
		public void FindFile_Missing1()
		{
			//TestingSingleton.GetImageFile
			// Tests to see if it can find the file, in CURRENT directory (not at location specifeid)
		}
		public void FindFile_MissingAdvanced()
		{
			//TestingSingleton.GetImageFile
			// Tests to see if it can find the file, in CURRENT directory (not at location specifeid and not at root level -- Searches subdirectores)
		}
	}
}

