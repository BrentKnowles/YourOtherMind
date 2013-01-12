namespace MefAddIns.Extensibility
{
	using System;
	using System.Collections.Generic;
	using System.Text;


 	



	/// <summary>
	/// Defines the functionality that third parties have to implement for my language plug in
	/// </summary>
	public interface mef_IBase
	{
		string Author { get; }
		string Version { get; }
		string Description { get; }
		string Name { get; }
		// used when loading AddIns to mark a second copy (a retained in memory only value)
		bool IsCopy {get;set;}

		// Where this plugin should appear
		PlugInAction CalledFrom { get; }
		// What happens when the menu is clicked or a hotkey called
		void RespondToCallToAction();
		// needed to modify the plugin in situation wherein we intentionally have a temporary copy
		void SetGuid(string s);
		//string Tester(string incoming);

		/// <summary>
		/// The hookups, where this addin has been added. This are disposed of when remove
		/// </summary>
		List<IDisposable> Hookups { get; set; }
		void RegisterType();
		object Storage {get;set;}
		// hooking up a database, usually via string
		void SetStorage(object storage);
	}
}