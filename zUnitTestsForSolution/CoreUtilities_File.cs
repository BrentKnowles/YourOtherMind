using System;
using NUnit.Framework;
using CoreUtilities;
namespace Testing
{
	[TestFixture]
	public class CoreUtilities_File
	{
		public CoreUtilities_File ()
		{
		}

		[Test]
		public void TestForBlankFile()
		{
			Assert.True (CoreUtilities.File.DoesThisFileHaveErrors(@"C:\Users\Brent\Documents\Keeper\Files\brokenfiles2012\brokenrtf_dec2012.rtf"));
		}

		[Test]
		public void TestForBlankFileThatIsNotBlank()
		{
			Assert.False (CoreUtilities.File.DoesThisFileHaveErrors(@"C:\Users\Brent\Documents\Keeper\Files\brokenfiles2012\notbroken.txt"));
		}

		[Test]
		public void TestForHdriveFailure()
		{
			Assert.False (CoreUtilities.File.CheckForFileError());
		}
	}
}

