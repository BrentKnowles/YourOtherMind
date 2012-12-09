using System;
using NUnit.Framework;
using CoreUtilities;
namespace Testing
{
	[TestFixture]
	public class Log_Test
	{
		public Log_Test ()
		{
		}
		[Test]
		public void TestLoudness()
		{
			CoreUtilities.Loud currentLoud = lg.Instance.Loudness;
			lg.Instance.Loudness = Loud.BDEFAULT;
			Assert.AreEqual (true, CoreUtilities.lg.Instance.Line("test", CoreUtilities.ProblemType.ERROR, "test details"));
			Assert.AreEqual (true, CoreUtilities.lg.Instance.Line("test", CoreUtilities.ProblemType.WARNING, "default", Loud.BDEFAULT)); // should 
			Assert.AreEqual (false, CoreUtilities.lg.Instance.Line("test", CoreUtilities.ProblemType.WARNING, "test details", Loud.CTRIVIAL)); // should not
			lg.Instance.Loudness = Loud.ACRITICAL;
			Assert.AreEqual (false, CoreUtilities.lg.Instance.Line("test", CoreUtilities.ProblemType.WARNING, "default", Loud.BDEFAULT)); // should not
			Assert.AreEqual (true, CoreUtilities.lg.Instance.Line("test", CoreUtilities.ProblemType.WARNING, "critical", Loud.ACRITICAL)); // should 
			lg.Instance.Loudness = Loud.DOFF;
			Assert.AreEqual (false, CoreUtilities.lg.Instance.Line("test", CoreUtilities.ProblemType.WARNING, "critical", Loud.ACRITICAL)); // should NOT
			//string s = Console.ReadLine();
			//Console.Writ\eLine(s);


			// set loudness back to what it should be
			lg.Instance.Loudness = currentLoud;


		}
	}
}

