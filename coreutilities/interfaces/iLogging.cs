using System;

namespace CoreUtilities
{


	public enum Loud {ACRITICAL=0, BDEFAULT=1,CTRIVIAL=2, DOFF=3};
	public enum ProblemType {MESSAGE, ERROR, TEMPORARY,EXCEPTION, WARNING, TIMING};
	public interface iLogging
	{


		Loud Loudness {get; set;}
		bool Line(string Routine, ProblemType Problem, string Details);
		bool Line(string Routine, ProblemType Problem, string Details, Loud Loudness);
	}
}

