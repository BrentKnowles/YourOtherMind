// Log_Test.cs
//
// Copyright (c) 2013 Brent Knowles (http://www.brentknowles.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Review documentation at http://www.yourothermind.com for updated implementation notes, license updates
// or other general information/
// 
// Author information available at http://www.brentknowles.com or http://www.amazon.com/Brent-Knowles/e/B0035WW7OW
// Full source code: https://github.com/BrentKnowles/YourOtherMind
//###
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

