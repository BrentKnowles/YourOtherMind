using System;
using NUnit.Framework;
using Layout;
// Will only finish writing tests for this if I end up using class
namespace NotUsed
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

