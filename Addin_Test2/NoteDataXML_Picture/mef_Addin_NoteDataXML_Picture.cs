namespace MefAddIns
{
	using MefAddIns.Extensibility;
	using System.ComponentModel.Composition;
	using System;
	//using System.Windows.Forms;
	using System.Diagnostics;
	
	/// <summary>
	/// Provides an implementation of a supported language by implementing ISupportedLanguage. 
	/// Moreover it uses Export attribute to make it available thru MEF framework.
	/// </summary>
	[Export(typeof(mef_INotes))]
	public class Addin_MemoryTrack : mef_INotes
	{
		public string Author
		{
			get { return @"Brent Knowles"; }
		}
		public string Version
		{
			get { return @"1.0.0.0"; }
		}
		public string Description
		{
			get { return "Allows pictures to be added to a Layout."; }
		}
		public string Name
		{
			get { return @"Layout"; }
		}
		public void RegisterType()
		{
			Layout.LayoutDetails.Instance.AddToList(typeof(NoteDataXML_Picture.NoteDataXML_Pictures), "Picture");
		}


	}
}