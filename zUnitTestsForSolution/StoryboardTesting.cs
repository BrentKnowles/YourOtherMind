using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Storyboards;

namespace Testing
{
	[TestFixture]
	public class StoryboardTesting
	{
		public StoryboardTesting ()
		{
		}
		[Test]
		[ExpectedException("System.Exception")]
		public void testExceptionAddItem()
		{
			Storyboard groupEm = new Storyboard();
		//	groupEm.AllowDrop = false;
			groupEm.AddItem("test", "test", 0);
			
		}
		[Test]
		[ExpectedException("System.Exception")]
		public void testExceptionEditItem()
		{
			Storyboard groupEm = new Storyboard();
			groupEm.EditItem(null);
			
		}
		
		[Test]
		[ExpectedException("System.Exception")]
		public void testExceptionDeleteItem()
		{
			Storyboard groupEm = new Storyboard();
			groupEm.DeleteItem(null);
			
		}
	}
}



