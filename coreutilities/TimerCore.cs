using System;
using System.Diagnostics;
namespace CoreUtilities
{
	public class TimerCore
	{
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
		public static TimeSpan Time(Action action)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			action();
			stopwatch.Stop();
			return stopwatch.Elapsed;
		}

	}
}

