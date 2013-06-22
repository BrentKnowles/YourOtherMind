// lg.cs
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
//#define SHOWLOGS not working the way it should BUT if I uncomment, the Conditionals, below, it is removed. Just won't reapepar with the This on it sown
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;



namespace CoreUtilities
{

	/// <summary>
	/// Lg. New logging class. TODO: Refcator to Log. once old class removed
	/// </summary>
	public class lg : /*iLogging,*/ IDisposable
	{
	

		#region const
		// when to write out the log

		protected int bUFFER_LINES=20;

		public int BUFFER_LINES {
			get {
				return bUFFER_LINES;
			}
			set {
				bUFFER_LINES = value;
			}
		}

 
		public const string LOG_FILE = "yom2013log.txt";
		#endregion

		#region variables
		protected static volatile lg instance;
		protected static object syncRoot = new Object();
		protected List<string> LogLines = new List<string>();
		StreamWriter logWriter = null;

		private Loud _Loudness = Loud.BDEFAULT;// We show default and louder but not Trivial
		public Loud Loudness { get { return _Loudness; } set { _Loudness = value; }}
#endregion;
		public bool OutputToConstoleToo=false;
		public lg ()
		{
			logWriter = File.AppendText(LOG_FILE);
		}
		// if set to true then only TIMING messages will be seen
		public bool OnlyTime=false;

		/// <summary>
		/// Releases all resource used by the <see cref="CoreUtilities.lg"/> object.
		/// Will write the remainder of the log out
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the <see cref="CoreUtilities.lg"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="CoreUtilities.lg"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="CoreUtilities.lg"/> so the garbage
		/// collector can reclaim the memory that the <see cref="CoreUtilities.lg"/> was occupying.
		/// </remarks>
		public void Dispose()
		{
			WriteLog();


			logWriter.Close ();
			logWriter.Dispose();

			LogLines = null;
		}

		public static lg Instance
		{
			get
			{
				if (null == instance)
				{
					// only one instance is created and when needed
					lock (syncRoot)
					{
						if (null == instance)
						{

							if (File.Exists (LOG_FILE))
							{
								File.Delete (LOG_FILE);
							}
							instance = new lg();
						}
					}
				}
				return (lg)instance;
			}
		}
		bool writinglog = false;
		/// <summary>
		/// Writes the log. Called after the buffer reaches a certain size
		/// </summary>
		public void WriteLog ()
		{
			if (null == logWriter) {
				throw new Exception("LogWriter was never initialized");
			}
			if (true == writinglog) return;

			writinglog = true;
			// We copy this to an array to try to prevent any collection modified by writing errors
			foreach (string log in LogLines.ToArray ()) {
				//Console.WriteLine (log);
			try
				{
				logWriter.WriteLine(log);
				}
				catch (Exception ex)
				{
					throw new Exception(ex.ToString ());
					//NewMessage.Show ("Logwriter was closed while still writing the log");
				}
			}
			LogLines = new List<string>();
			writinglog = false;
		}
		//[Conditional("SHOWLOGS")]
		public void Line(string Routine, ProblemType Problem, string Details)
		{
		
		
			//return
				Line(Routine, Problem, Details, Loud.ACRITICAL);
		}

		// this allows us to override the console writing behavior
		// useful for unity
		public Action<string> ConsoleWriter = null;
		void ConsoleIt (string log)
		{
			if (ConsoleWriter == null) {
				Console.WriteLine (log);
			} else {
				ConsoleWriter(log);
			}
		}
	
	
		//[Conditional("SHOWLOGS")]
		public void Line (string Routine, ProblemType Problem, string Details, Loud myLoudness)
		{
			bool show = true;
			if (OnlyTime == true && Problem != ProblemType.TIMING) {
				show = false;
			}
		

			if ((this.Loudness == Loud.DOFF) || (this.Loudness < myLoudness)) {
				//NewMessage.Show("Not loud enough!");
				//Console.WriteLine ("exiting log");
				// if this message is not loud enough we don't post it
				//return false;
				show = false;
			} 

			if (true == show)
			{
				//NewMessage.Show("loud enough!");
				string log = (String.Format ("{0}, {1}, {2}, {3}", Routine, Problem.ToString (), Details, myLoudness));

				// this happens if we try to close but the close does not happen for some reason
				if (null == LogLines) LogLines = new List<string>();

				LogLines.Add (log);
				if (OutputToConstoleToo)
				{
					ConsoleIt(log);

				}

				if (LogLines.Count >= bUFFER_LINES) {
					WriteLog ();
				}
			}
			//return true;
		}
		// used for testing
		public bool IsThereALog()
		{
			bool result = false;
			// calls WriteLog
			if (LogLines.Count >0) result= true;
			
			// we empty the loglines so each test is neutral
		
			Reset ();
			
			return result;
		}
		// clears the buffer, generally should only be used for unit testing
		public void Reset ()
		{
			LogLines = new List<string>();
		}
	}
}

