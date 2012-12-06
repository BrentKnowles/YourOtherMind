using System;
using NUnit.Framework;
using Layout;

namespace Testing
{
	[TestFixture]
	public class LayoutXMLTest
	{
		public LayoutXMLTest ()
		{
		}




		[Test]
		[ExpectedException]
		public void  SaveTo_BlankGUID ()
		{
			LayoutXML layout = new LayoutXML("");
			layout.SaveTo ();
		}
	}
}

