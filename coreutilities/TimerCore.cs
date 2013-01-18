using System;
using System.Diagnostics;
namespace CoreUtilities
{
	public class TimerCore
	{
		// must be set to true else no timing happens
		public static bool TimerOn;
		/// <summary>
		/// Time the specified action.
		/// 
		/// Example of usage
		/// 	time = Time (() => {
		///for (int i = 1 ; i < steps; i++)
		///{
		///	string Result = db.GetLongText ("pages", "rtf", "name", "");
		///	System.IO.StringReader reader = new System.IO.StringReader(Result);
		///	
		///	DeSerializedList = (System.Collections.Generic.List<NoteDataXML>)test.Deserialize(reader);
		///}
		///	});
		/// </summary>
		/// <param name='action'>
		/// Action.
		/// </param>
		public static TimeSpan Time (Action action)
		{
			if (TimerOn) {
				Stopwatch stopwatch = Stopwatch.StartNew ();
				action ();
				stopwatch.Stop ();
				return stopwatch.Elapsed;
			} else {
				// still need to run the action
				action ();
				return TimeSpan.Zero;
			}
		}

	}
}

