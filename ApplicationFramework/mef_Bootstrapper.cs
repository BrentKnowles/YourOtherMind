namespace MefAddIns.Terminal
{
	using MefAddIns.Extensibility;
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	
	public class Bootstrapper
	{
		/// <summary>
		/// Holds a list of all the valid languages for this application
		/// </summary>
		[ImportMany]
		public IEnumerable<mef_IBase> Languages { get; set; }
		[ImportMany]
		public IEnumerable<mef_IShowFormBasic> FormBasic { get; set; }
		
	}
}
