namespace MefAddIns
{
	using MefAddIns.Extensibility;
	using System.ComponentModel.Composition;
	using System;

	using System.Collections.Generic;

	/// <summary>
	/// Provides an implementation of a supported language by implementing ISupportedLanguage. 
	/// Moreover it uses Export attribute to make it available thru MEF framework.
	/// </summary>
	[Export(typeof(mef_INotes))]
	public class Addin_Note_Picture : mef_INotes
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
		public void RespondToCallToAction ()
		{
			// do nothing. This is not called for mef_Inotes
			return;
		}

		bool iscopy = false;
		
		public bool IsCopy {
			get{ return iscopy;}
			set{   iscopy = value;}
		}

		string GUID = "notedataxml_picture";
		public PlugInAction CalledFrom { 
			get
			{
				PlugInAction action = new PlugInAction();
				action.HotkeyNumber = -1;
				action.MyMenuName = "Picture Notes";
				action.ParentMenuName = "";
				action.IsANote = true;
				action.IsOnAMenu = true;
				action.IsOnAToolbar = false;
				action.GUID = GUID;
				return action;
			} 
		}
		public void SetGuid(string s)
		{
			GUID = s;
		}

		List<IDisposable> hookups = null;
		public List<IDisposable>  Hookups {
			get {
				if (null == hookups)
					hookups = new List<IDisposable> ();
				return hookups;
			}
			set { hookups = value;}
			
		}

	}
}