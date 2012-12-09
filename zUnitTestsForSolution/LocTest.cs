using System;
using NUnit.Framework;

namespace Testing
{
	[TestFixture]
	public class LocTest
	{
		public LocTest ()
		{
		}

		[Test]
		public void NoLocaleSpecified()
		{
			Assert.AreEqual(false, CoreUtilities.Loc.Instance.ChangeLocale(""));
		}
	}
}

