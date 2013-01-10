using System;
using NUnit.Framework;
using System.Collections.Generic;
using CoreUtilities;
using System.Drawing;
namespace Testing
{
	[TestFixture]
	public class CheckedListBoxFormTest
	{
		public CheckedListBoxFormTest ()
		{


		}

		[Test]
		public void TestAddingAndRetrievingValues()
		{
			List<string>All = new List<string>();
			All.Add ("add");
			All.Add ("snakes");
			All.Add ("boomers");
			All.Add ("firefox");
			All.Add ("alien");

			List<string>Selected = new List<string>();
			Selected.Add ("snakes");
			Selected.Add ("alien");



			CheckBoxForm checkers = new CheckBoxForm(All, Selected,"boo", null);
			// should not be equal because list comes back sorted
			Assert.AreNotEqual(checkers.GetItems(), Selected);
			Selected.Sort();
			Assert.AreEqual(checkers.GetItems(), Selected);
		}
	}
}

