// MarkupLanguageNone.cs
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
using CoreUtilities;
using System.Windows.Forms;
using System.Collections;

namespace Layout
{
	public class MarkupLanguageNone : iMarkupLanguage
	{
		public MarkupLanguageNone ()
		{
		}

		public override string ToString ()
		{
			return nameAndIdentifier;
		}
		private string nameAndIdentifier= Loc.Instance.GetString("None");

		public string NameAndIdentifier {
			get {
				return nameAndIdentifier;
			}
			set {
				nameAndIdentifier = value;
			}
		}
		public bool IsIndex (string incoming)
		{
			return false;
		}
		public ArrayList GetListOfPages (string s, ref bool b)
		{
			b = false;
			return null;
		}
		public void DoPaint(PaintEventArgs e, int Start, int End, RichTextBox RichText)
		{
			// do nothing
		}
		public bool IsWordRequest(string incoming)
		{
			return false;
		}
		public string CleanWordRequest(string incoming)
		{
			return incoming;
		}
		
		public bool IsGroupRequest(string incoming)
		{
			return false;
		}
	}
}

