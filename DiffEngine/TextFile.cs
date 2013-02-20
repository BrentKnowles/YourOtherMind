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
                        string.Format("File contains a line greater than {0} characters.",
                        MaxLineLength.ToString()));
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
							string.Format("File contains a line greater than {0} characters.",
							MaxLineLength.ToString()));
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