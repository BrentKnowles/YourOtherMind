// WordsSystem.cs
//
// Copyright (c) 2013 Brent Knowles (http://www.brentknowles.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Review documentation at http://www.yourothermind.com for updated implementation notes, license updates
// or other general information/
// 
// Author information available at http://www.brentknowles.com or http://www.amazon.com/Brent-Knowles/e/B0035WW7OW
// Full source code: https://github.com/BrentKnowles/YourOtherMind
//###
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
		public virtual string GetPartOfSpeech(string sWord)
		{
			// intended to overridden
			return "n/a";
		}
		public virtual bool IsVerb(string sWord)
		{
			return false;
		}
	}
}

