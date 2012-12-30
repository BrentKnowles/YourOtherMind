using System;

namespace CoreUtilities
{
	/// <summary>
	/// Lg. New logging class. TODO: Refcator to Log. once old class removed
	/// </summary>
	public class lg : iLogging
	{

		#region variables
		protected static volatile lg instance;
		protected static object syncRoot = new Object();

		private Loud _Loudness = Loud.BDEFAULT;// We show default and louder but not Trivial
		public Loud Loudness { get { return _Loudness; } set { _Loudness = value; }}
#endregion;

		public lg ()
		{
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
							instance = new lg();
						}
					}
				}
				return (lg)instance;
			}
		}


		public bool Line(string Routine, ProblemType Problem, string Details)
		{
		
		
			return Line(Routine, Problem, Details, Loud.ACRITICAL);
		}

		public bool Line (string Routine, ProblemType Problem, string Details, Loud myLoudness)
		{
		

			if ((this.Loudness == Loud.DOFF) || (this.Loudness < myLoudness))
			{
				//Console.WriteLine ("exiting log");
				// if this message is not loud enough we don't post it
				return false;
			}
			string log = (String.Format("{0}, TYPE: {1}, {2}", Routine, Problem.ToString (), Details));

			// *TODO: Change this text writing later
			Console.WriteLine(log);
			return true;
		}

	}
}

