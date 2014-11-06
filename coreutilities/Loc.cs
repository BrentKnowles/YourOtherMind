// Loc.cs
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
using GNU.Gettext;
using  System.Globalization;

namespace CoreUtilities
{
	/// <summary>
	/// Wrapper singleton for Localization effort.
	/// 
	/// Did not want to implement like this but I was unable to get GetText.Net to create the dlls from the .PO files (it creates them
	/// but they crash). In case I need to move to a different solution I want to wrap it all, simply.
	/// </summary>
	public class Loc 
	{
		public GettextResourceManager Cat = null;
		protected static volatile Loc instance;
		protected static object syncRoot = new Object();

		public Loc()
		{
			Cat = new GettextResourceManager (); 
		}
		/// <summary>
		/// Wrapper for Cat.GetString for faster code entry
		/// </summary>
		/// <param name='msg'>
		/// Message.
		/// </param>
		public string GetString(string msg)
		{
			return Cat.GetString(msg);

		}
		/// <summary>
		/// An even faster short-cut so you don't have to type 'instance' each time
		/// </summary>
		/// <returns>
		/// The string.
		/// </returns>
		/// <param name='msg'>
		/// Message.
		/// </param>
		public static string GetStr (string msg)
		{
			return Loc.Instance.GetString(msg);
		}
		/// <summary>
		/// 
		/// </summary>
		public bool ChangeLocale (string Locale)
		{
			bool result = false;

			if (Locale != "") {

				string locale = Locale; //"en-US";//"fr-FR";
				System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo (locale);

				// I think we have to reinitialize this now
				Cat = new GettextResourceManager (); 
			} else {
				lg.Instance.Line("Loc.ChangeLocale", ProblemType.ERROR, String.Format ("{0} is an invalid Locale", Locale));

			}
			return result;
		}

		public string GetStringFmt (string msg, params object[] args)
		{
			return Cat.GetStringFmt(msg, args);
		}
		

		public static Loc Instance
		{
			get
			{
				if (null == instance)
				{
					// only one instance is created and when needed
					lock (syncRoot)
					{
						if (null == instance)
						{
							instance = new Loc();
						}
					}
				}
				return (Loc)instance;
			}
		}
	}
}

