using System;

namespace CoreUtilities
{
	/// <summary>
	/// Lg. New logging class. TODO: Refcator to Log. once old class removed
	/// </summary>
	public class lg : BaseSingleton, iLogging
	{

		#region variables

		private Loud _Loudness;
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


		public void Line(string Routine, ProblemType Problem, string Details)
		{
		
			if (Loudness < this.Loudness)
			{
				// if this message is not loud enough we don't post it
				return;
			}
			Line(Routine, Problem, Details, Loud.CRITICAL);
		}

		public void Line (string Routine, ProblemType Problem, string Details, Loud Loudness)
		{

			if (Loudness < this.Loudness)
			{
				// if this message is not loud enough we don't post it
				return;
			}
			string log = (String.Format("{0}, Problem: {1}, {2}", Routine, Problem, Details));

			// *TODO: Change this text writing later
			Console.WriteLine(log);
		}

	}
}

