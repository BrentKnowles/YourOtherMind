using System;
using NUnit.Framework;
using Layout.data;
using Layout;
// Use FakeLayoutDatabase, not the real one to avoid messing with the data files
namespace Testing
{
	[TestFixture]
	public class LayoutDatabaseTest
	{
		public LayoutDatabaseTest ()
		{
		}
		[Test]
		public void EnsureThatDatabaseColumnsEqualsDefinedCount ()
		{
			// This unit test exists to make sure that when I fill in the constants for the table
			// being used that I make sure my column count matches the actual number of oclumns

			// The reas on they could not be the same is that I needed a constant for array initialization
			Assert.AreEqual(tmpDatabaseConstants.ColumnCount, tmpDatabaseConstants.Columns.Length);

		}
		[Test]
		[ExpectedException]
		public void  SaveTo_BlankGUID ()
		{
			FakeLayoutDatabase layout = new FakeLayoutDatabase("");
			layout.SaveTo ();
		}
	}
}

