// ColumnConstant.cs
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

namespace database
{
	/// <summary>
	/// Column constant. Represents a COLUMN in a database, used to clean up the constants used for table definitions
	/// 
	/// usage: static public ColumnConstant SUBPANEL = new ColumnConstant("subpanel", 6, "boolean", 3);
	/// </summary>
	public class ColumnConstant
		{
		    // the name of the column in the database
			public string Name;
			// the type in the database
			public string Type; 
		    // Index of column in database
			public int Index;
		    // index in internal storage once loaded into memory (may or may not be needed)
			public int LayoutIndex;
			public ColumnConstant(string name, int index, string type, int layoutIndex)
			{
				Name = name;
				Index = index;
				Type = type;
				LayoutIndex = layoutIndex;
				
			}
			public static implicit operator string(ColumnConstant x) {
				return x.ToString();}
			public override string ToString ()
			{
				return Name;
			}
			
		}
}

