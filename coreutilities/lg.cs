#define ShowLogs
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
		// TODO: Log also needs to write when app closes
		protected const int BUFFER_LINES=20; 
		protected const string LOG_FILE = "yom2013log.txt";
		#endregion

		#region variables
		protected static volatile lg instance;
		protected static object syncRoot = new Object();
		protected List<string> LogLines = new List<string>();
		StreamWriter logWriter = null;

		private Loud _Loudness = Loud.BDEFAULT;// We show default and louder but not Trivial
		public Loud Loudness { get { return _Loudness; } set { _Loudness = value; }}
#endregion;

		public lg ()
		{
			logWriter = File.AppendText(LOG_FILE);
		}

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


		public void Line(string Routine, ProblemType Problem, string Details)
		{
		
		
			//return
				Line(Routine, Problem, Details, Loud.ACRITICAL);
		}

		/// <summary>
		/// Writes the log. Called after the buffer reaches a certain size
		/// </summary>
		protected void WriteLog ()
		{
			if (null == logWriter) {
				throw new Exception("LogWriter was never initialized");
			}
			// *TODO: Change this text writing later
			foreach (string log in LogLines) {
				//Console.WriteLine (log);
				logWriter.WriteLine(log);
			}
			LogLines = new List<string>();
		}


		[Conditional("ShowLogs")]
		public void Line (string Routine, ProblemType Problem, string Details, Loud myLoudness)
		{
		

			if ((this.Loudness == Loud.DOFF) || (this.Loudness < myLoudness)) {
				//Console.WriteLine ("exiting log");
				// if this message is not loud enough we don't post it
				//return false;
			} else {

				string log = (String.Format ("{0}, {1}, {2}, {3}", Routine, Problem.ToString (), Details, myLoudness));
				LogLines.Add (log);

				if (LogLines.Count >= BUFFER_LINES) {
					WriteLog ();
				}
			}
			//return true;
		}

	}
}

