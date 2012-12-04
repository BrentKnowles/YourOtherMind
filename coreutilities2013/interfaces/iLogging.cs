using System;

namespace CoreUtilities
{


	public enum Loud{CRITICAL, TRIVIAL, DEFAULT,OFF};
	public enum ProblemType {MESSAGE, ERROR, TEMPORARY,EXCEPTION};
	public interface iLogging
	{


		Loud Loudness {get; set;}
		void Line(string Routine, ProblemType Problem, string Details);
		void Line(string Routine, ProblemType Problem, string Details, Loud Loudness);
	}
}

