// TextFile.cs
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
using System.IO;
using System.Collections;

namespace DiffEngine
{
	public class TextLine : IComparable
	{
		public string Line;
		public int _hash;

		public TextLine(string str)
		{
			Line = str.Replace("\t","    ");
			_hash = str.GetHashCode();
		}
		#region IComparable Members

		public int CompareTo(object obj)
		{
			return _hash.CompareTo(((TextLine)obj)._hash);
		}

		#endregion
	}


	public class DiffList_TextFile : IDiffList
	{
		private const int MaxLineLength = 2048;
		private ArrayList _lines;
        public int words = 0;
        /// <summary>
        /// jan 2010 - ability to compare just two textboxes?
        /// splits it on paragraph
        ///
        /// </summary>
        /// <param name="fileName"></param>
        public DiffList_TextFile(string sString, bool bString)
        {
            _lines = new ArrayList();
            string[] lines = sString.Split(new char[3] { '.', '!', '?' }, StringSplitOptions.None);
            foreach (string s in lines)
            {
                if (s.Length > MaxLineLength)
                {
                    throw new InvalidOperationException(
						string.Format("File contains a line greater than {0} characters. That line is {1}",
                        MaxLineLength.ToString(), s));
                }
                _lines.Add(new TextLine(s));
            }
        }
        /// <summary>
        /// http://www.dotnetperls.com/word-count
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int CountWords1(string s)
        {
            System.Text.RegularExpressions.MatchCollection collection = System.Text.RegularExpressions.Regex.Matches(s, @"[\S]+");
            return collection.Count;
        }
		public DiffList_TextFile(string fileName)
		{
			_lines = new ArrayList();
            words = 0; 
			using (StreamReader sr = new StreamReader(fileName)) 
			{
				String line;
				// Read and display lines from the file until the end of 
				// the file is reached.
				while ((line = sr.ReadLine()) != null) 
				{
					if (line.Length > MaxLineLength)
					{
						throw new InvalidOperationException(
							string.Format("File contains a line greater than {0} characters. That line is {1}",
						              MaxLineLength.ToString(), line));
					}
                    words = words + CountWords1(line);
					_lines.Add(new TextLine(line));
				}
			}
		}
		#region IDiffList Members

		public int Count()
		{
			return _lines.Count;
		}

		public IComparable GetByIndex(int index)
		{
			return (TextLine)_lines[index];
		}

		#endregion
	
	}
}