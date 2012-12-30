namespace MefAddIns
{
	using MefAddIns.Extensibility;
	using System.ComponentModel.Composition;
	using System;

	/// <summary>
	/// Provides an implementation of a supported language by implementing ISupportedLanguage. 
	/// Moreover it uses Export attribute to make it available thru MEF framework.
	/// </summary>
	[Export(typeof(mef_ISupportedLanguage))]
	public class EnglishLanguage : mef_ISupportedLanguage
	{
		public string Author
		{
			get { return @"Herberth Madrigal"; }
		}
		public string Version
		{
			get { return @"EN-US.1.0.0"; }
		}
		public string Description
		{
			get { return "This is the English language pack."; }
		}
		public string Name
		{
			get { return @"English"; }
		}

		public void Boom()
		{
			Console.WriteLine("Hello");
		}
		public string Tester(string s)
		{
			Console.WriteLine (s);
			return "Egghead!";
		}

	}
}
