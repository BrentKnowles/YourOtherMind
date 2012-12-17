namespace MefAddIns.Extensibility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	/// <summary>
	/// Defines the functionality that third parties have to implement for my language plug in
	/// </summary>
	public interface mef_ISupportedLanguage
	{
		string Author { get; }
		string Version { get; }
		string Description { get; }
		string Name { get; }
		void Boom();
		string Tester(string incoming);
	}
}