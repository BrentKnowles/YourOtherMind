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
			Assert.True (CoreUtilities.FileUtils.DoesThisFileHaveErrors(@"C:\Users\BrentK\Documents\Keeper\Files\brokenfiles2012\brokenrtf_dec2012.rtf"));
		}

		[Test]
		public void TestForBlankFileThatIsNotBlank()
		{
			Assert.False (CoreUtilities.FileUtils.DoesThisFileHaveErrors(@"C:\Users\BrentK\Documents\Keeper\Files\brokenfiles2012\notbroken.txt"));
		}

		[Test]
		public void TestForHdriveFailure()
		{
			Assert.False (CoreUtilities.FileUtils.CheckForFileError());
		}
	}
}

