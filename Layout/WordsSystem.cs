using System;
using System.Text.RegularExpressions;
using CoreUtilities;
using System.Collections.Generic;
namespace Layout
{
	// A generic framework class, to calculate Word
	// Can be overridden in plugins (i.e., it is set by application on load in LayoutDeatils but last-in takes precendence
	// Current value will appear in options
	public class WordsSystem
	{
		/// <summary>
		/// Count words with Regex.
		/// </summary>
		private static int CountWords1(string s)
		{
			MatchCollection collection = Regex.Matches(s, @"[\S]+");
			return collection.Count;
		}
		public virtual int CountWords(string Text)
		{
			return CountWords1 (Text);
		}

		public override string ToString ()
		{
			return  (Loc.Instance.GetString ("Default Word System"));
		}

		public virtual string[] SpellingSuggestions(string sFoundWord)
		{
			string[] returnvalue = null;
			//new string[1];
			//returnvalue[0] =  (Loc.Instance.GetString ("Feature not supported"));
			return returnvalue;
		}
		public virtual bool AddWordToDictionary(string newWord, bool EditMode)
		{
			NewMessage.Show (Loc.Instance.GetString("Feature not implemented for default word system"));
			return false;
		}
	}
}

