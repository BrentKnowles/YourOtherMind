namespace MefAddIns.Terminal
{
	using MefAddIns.Extensibility;
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	
	public class Bootstrapper
	{
		/// <summary>
		/// Holds a list of all the valid addins for this application
		/// </summary>
		[ImportMany]
		public IEnumerable<mef_IBase> Base { get; set; }

		
	}
}
