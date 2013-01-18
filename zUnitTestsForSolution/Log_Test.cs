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
			lg.Instance.Reset();

			lg.Instance.OnlyTime = true;
			CoreUtilities.lg.Instance.Line("test", CoreUtilities.ProblemType.ERROR, "test details");
			Assert.AreEqual (false,lg.Instance.IsThereALog(),"time1");

			CoreUtilities.lg.Instance.Line("test", CoreUtilities.ProblemType.TIMING, "test details");
			Assert.AreEqual (true,lg.Instance.IsThereALog(),"time2");


			lg.Instance.OnlyTime = false;



			CoreUtilities.Loud currentLoud = lg.Instance.Loudness;
			lg.Instance.Loudness = Loud.BDEFAULT;
	
			CoreUtilities.lg.Instance.Line("test", CoreUtilities.ProblemType.ERROR, "test details");
			Assert.AreEqual (true,lg.Instance.IsThereALog(),"1");

			CoreUtilities.lg.Instance.Line("test", CoreUtilities.ProblemType.WARNING, "default", Loud.BDEFAULT);
			Assert.AreEqual (true, lg.Instance.IsThereALog(),"2"); // should 


			CoreUtilities.lg.Instance.Line("test", CoreUtilities.ProblemType.WARNING, "test details", Loud.CTRIVIAL);
			Assert.AreEqual (false, lg.Instance.IsThereALog(),"3"); // should not


			lg.Instance.Loudness = Loud.ACRITICAL;
			CoreUtilities.lg.Instance.Line("test", CoreUtilities.ProblemType.WARNING, "default", Loud.BDEFAULT);
			Assert.AreEqual (false, lg.Instance.IsThereALog()); // should not

			CoreUtilities.lg.Instance.Line("test",CoreUtilities.ProblemType.WARNING, "critical", Loud.ACRITICAL);
			Assert.AreEqual (true,  lg.Instance.IsThereALog()); // should 


			lg.Instance.Loudness = Loud.DOFF;
			                 CoreUtilities.lg.Instance.Line("test", CoreUtilities.ProblemType.WARNING, "critical", Loud.ACRITICAL);
			                 Assert.AreEqual (false, lg.Instance.IsThereALog()); // should NOT


//			//string s = Console.ReadLine();
			//Console.Writ\eLine(s);


			// set loudness back to what it should be
			lg.Instance.Loudness = currentLoud;


		}
	}
}

