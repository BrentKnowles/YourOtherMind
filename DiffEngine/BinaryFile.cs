// BinaryFile.cs
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
	public class DiffList_BinaryFile : IDiffList
	{
		private byte[] _byteList;

		public DiffList_BinaryFile(string fileName)
		{
			FileStream fs = null;
			BinaryReader br = null;
			try
			{
				fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
				int len = (int)fs.Length;
				br = new BinaryReader(fs);
				_byteList = br.ReadBytes(len);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (br != null) br.Close();
				if (fs != null) fs.Close();
			}

		}
		#region IDiffList Members

		public int Count()
		{
			return _byteList.Length;
		}

		public IComparable GetByIndex(int index)
		{
			return _byteList[index];
		}

		#endregion
	}
}